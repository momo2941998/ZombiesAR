using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissleButton : MonoBehaviour
{
    public GameObject misslePrefab;
    private Image percentImage;
    public float timeToFill;
    public float lastTimeFill;
    public Camera mainCam;
    public float pushForce;


    // Start is called before the first frame update
    void Start()
    {
        lastTimeFill = timeToFill;
        percentImage = GetComponent<Image>();
        UpdateFillAmount();
        GetComponent<Button>().onClick.AddListener(LauchMissle);
    }

    // Update is called once per frame
    void Update()
    {
        RefillTime(Time.deltaTime);
    }

    private void RefillTime(float timeAdd)
    {
        float x = lastTimeFill + timeAdd;
        if (x > timeToFill) lastTimeFill = timeToFill;
        else
        {
            lastTimeFill = x;
        }
        UpdateFillAmount();
    }

    private void UpdateFillAmount()
    {
        percentImage.fillAmount = lastTimeFill / timeToFill;
    }

    public void LauchMissle()
    {
        if (lastTimeFill == timeToFill)
        {
            GameObject missle = Instantiate(misslePrefab, mainCam.transform.position, mainCam.transform.rotation) as GameObject;
            missle.GetComponent<Rigidbody>().AddForce(pushForce * Vector3.forward, ForceMode.Impulse);
            
            ResetMissle();
        }

    }

    private void ResetMissle()
    {
        lastTimeFill = 0;
        UpdateFillAmount();
    }
}
