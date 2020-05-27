//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google LLC. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class HelloARController : MonoBehaviour
    {
        public Button placeGroundButton;
        public Button placeTombButton;
        public Button startGameButton;

        public GameObject dummyPrefab;
        public GameObject groundPlanePrefab;
        public GameObject tombPrefab;

        private GameObject groundPlaneGO;
        private GameObject tombGO;

        private bool isPrefabInitialized;
        private bool isGroundPlaced;
        private bool isTombPlaced;
        private bool hasGameStarted;
        private int numOfTombsPlaced = 0;
        public float distance = 3;

        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR
        /// background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab to place when a raycast from a user touch hits a vertical plane.
        /// </summary>
        public GameObject GameObjectVerticalPlanePrefab;

        /// <summary>
        /// A prefab to place when a raycast from a user touch hits a horizontal plane.
        /// </summary>
        public GameObject GameObjectHorizontalPlanePrefab;

        /// <summary>
        /// A prefab to place when a raycast from a user touch hits a feature point.
        /// </summary>
        public GameObject GameObjectPointPrefab;

        /// <summary>
        /// The rotation in degrees need to apply to prefab when it is placed.
        /// </summary>
        private const float k_PrefabRotation = 180.0f;

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error,
        /// otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;
        private GameManagerController gameManagerController;
        GameObject prefab;
        GameObject currentGO;

        /// <summary>
        /// The Unity Awake() method.
        /// </summary>
        public void Awake()
        {
            // Enable ARCore to target 60fps camera capture frame rate on supported devices.
            // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            currentGO = Instantiate(dummyPrefab) as GameObject;
            isGroundPlaced = false;
            isTombPlaced = false;
            gameManagerController = GameObject.FindWithTag("GameManager").GetComponent<GameManagerController>();
            if (gameManagerController.isGameSetup == false)
            {
                if (!isGroundPlaced)
                {
                    AllowPlaceGroundButton();
                }
            }
        }


        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            if (gameManagerController.isGameSetup) return;
            _UpdateApplicationLifecycle();

            // If the player has not touched the screen, we are done with this update.
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;

            }

            // Should not handle input if the player is pointing on UI.
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }

            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    // Choose the prefab based on the Trackable that got hit.

                    if (hit.Trackable is FeaturePoint)
                    {
                        prefab = GameObjectPointPrefab;
                    }
                    else if (hit.Trackable is DetectedPlane)
                    {
                        DetectedPlane detectedPlane = hit.Trackable as DetectedPlane;
                        if (detectedPlane.PlaneType == DetectedPlaneType.Vertical)
                        {
                            prefab = GameObjectVerticalPlanePrefab;
                        }
                        else
                        {
                            prefab = GameObjectHorizontalPlanePrefab;
                        }
                    }
                    else
                    {
                        prefab = GameObjectHorizontalPlanePrefab;
                    }

                    // Instantiate prefab at the hit pose.
                    var gameObject = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);
                    Destroy(gameObject,5);

                    // Compensate for the hitPose rotation facing away from the raycast (i.e.
                    // camera).
                    gameObject.transform.Rotate(0, k_PrefabRotation, 0, Space.Self);

                    // Create an anchor to allow ARCore to track the hitpoint as understanding of
                    // the physical world evolves.
                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                    // Make game object a child of the anchor.
                    gameObject.transform.parent = anchor.transform;
                    currentGO.transform.position = hit.Pose.position;
                    currentGO.transform.rotation = hit.Pose.rotation;
                    isPrefabInitialized = true;
                }
            }
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            if (gameManagerController.isGameSetup) return;
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to
            // appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage(
                    "ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity =
                unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>(
                            "makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }
        void AllowPlaceGroundButton()
        {
            //placeGroundButton.gameObject.SetActive(true);
            //placeTombButton.gameObject.SetActive(false);
            //startGameButton.gameObject.SetActive(false);
            //placeGroundButton.onClick.AddListener(PlaceGround);
            Camera mainCam = Camera.main;

            //Init Ground
            GameObject ground = Instantiate(groundPlanePrefab, Vector3.zero, mainCam.transform.rotation) as GameObject;


            Level level = LevelGame.GetInstance().GetLevel();
            float levelGame = level.GetDifficult();
            if (levelGame == 0)
                numOfTombsPlaced = 1;
            else
            {
                numOfTombsPlaced = (int)levelGame * 2; 
            }
            // init tomb
            for (int i = 0; i < numOfTombsPlaced; i++)
            {
                Vector3 ranPos = new Vector3(Random.Range(-3, 3), 0, Random.Range(5, 7));
                GameObject tomb = Instantiate(tombPrefab, ranPos, mainCam.transform.rotation) as GameObject;
                tomb.transform.Rotate(Vector3.up * 180);
            }
            AllowStartButton();

        }
        void AllowPlaceTombButton()
        {
            placeGroundButton.gameObject.SetActive(false);
            placeTombButton.gameObject.SetActive(true);
            startGameButton.gameObject.SetActive(false);
            
            placeTombButton.onClick.AddListener(PlaceTomb);
        }
        void AllowStartButton()
        {
            placeGroundButton.gameObject.SetActive(false);
            placeTombButton.gameObject.SetActive(false);
            startGameButton.gameObject.SetActive(true);
            startGameButton.onClick.AddListener(StartGame);
            // Time.timeScale =1;
        }

        
        void PlaceGround()
        {
            if (isPrefabInitialized)
            {
                Instantiate(groundPlanePrefab,currentGO.transform.position, currentGO.transform.rotation);
                isPrefabInitialized = false;
                AllowPlaceTombButton();
            }
        }

        void PlaceTomb()
        {
            if (isPrefabInitialized)
            {
                Instantiate(tombPrefab,currentGO.transform.position, currentGO.transform.rotation);
                isPrefabInitialized = false;
                AllowStartButton();
            }
        }

        void StartGame()
        {
            startGameButton.gameObject.SetActive(false);
            
            gameManagerController.isGameSetup = true;
        }

        void assignTransform(GameObject _gameObject, GameObject _prefab)
        {
            _gameObject.transform.position = _prefab.transform.position;
            _gameObject.transform.rotation = _prefab.transform.rotation;
        }
    }

}

