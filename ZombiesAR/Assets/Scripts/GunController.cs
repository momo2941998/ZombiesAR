using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    public float damageGun = 10;
    public float range = 100f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlashPartical;
    public GameObject impact;
    public float hitForce = 300;
    public float powerGunMax = 50;
    public float powerGun;
    public float refillTime = 0.1f;
    GameManagerController gameManagerController;
    public Vector3 movePos = new Vector3(0,0,0.2f);
    public float backTime = 0.05f;
    private AudioSource fireSound;

    public Button fire;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerController = GameObject.FindWithTag("GameManager").GetComponent<GameManagerController>();
        fireSound = GetComponent<AudioSource>();
        powerGun = powerGunMax;
        fpsCam = Camera.main;
        fire.onClick.AddListener(Shoot);
        muzzleFlashPartical.gameObject.SetActive(true);
        impact.gameObject.SetActive(true);
        InvokeRepeating("RefillPower", 0.5f , 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Shoot(){
        if (powerGun <= 0) return;
        fireSound.Play();
        powerGun -= 1;
        ShootEffect();
        fpsCam.GetComponent<ShakeCam>().Shake(0.2f);
        gameManagerController.UpdatePowerGun(powerGun);
        StopAllCoroutines();
        muzzleFlashPartical.Play();
        StartCoroutine(GapShoot());
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position,fpsCam.transform.forward, out hit, range))
        {
            ZombieController zombieController = hit.transform.GetComponent<ZombieController>();
            if(zombieController != null)
            {
                zombieController.TakeDamage(damageGun);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(hit.normal * hitForce,ForceMode.Impulse);
            }
            GameObject impactGO = Instantiate(impact,hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2);

        }
        
    }

    IEnumerator GapShoot()
    {
        yield return new WaitForSeconds(0.2f);
        muzzleFlashPartical.Stop();
    }
    
    void RefillPower()
    {
        if (powerGun +1 <= powerGunMax)
        {
            powerGun += 1;
        }
        gameManagerController.UpdatePowerGun(powerGun);
    }

    void ShootEffect()
    {
        MoveBackward();
        Invoke("MoveForward",backTime);
    }
    void MoveBackward()
    {
        transform.position -= movePos;
    }
    void MoveForward()
    {
        transform.position += movePos;
    }

}
