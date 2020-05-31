using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Missle : MonoBehaviour
{
    public float damage;
    private ParticleSystem boom;
    private AudioSource audio;
    public AudioClip launchSound;
    public AudioClip explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        boom = GetComponent<ParticleSystem>();
        audio = GetComponent<AudioSource>();
        audio.PlayOneShot(launchSound);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Enemy")|| collision.gameObject.CompareTag("Ground"))
        {
            Explosion();
            
            //collision.gameObject.GetComponent<ZombieController>().TakeDamage(damage);
        }

    }

    private void Explosion()
    {

        boom.Play();
        audio.PlayOneShot(explosionSound);
        Collider[] overlap = Physics.OverlapSphere(transform.position, 10);
        foreach (Collider x in overlap)
        {
            if (x.gameObject.GetComponent<ZombieController>() != null)
            {
                x.gameObject.GetComponent<ZombieController>().TakeDamage(damage);
                x.gameObject.GetComponent<Rigidbody>().AddExplosionForce(0, transform.position , 10);
            }
        }
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        gameObject.layer = 2;
        Destroy(gameObject, 5);
    }
}
