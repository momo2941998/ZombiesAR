using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject player;
    private PlayerController playerController;
    [SerializeField] private bool isZombieClose = false;
    [SerializeField] private float timeBetweenAttacks = 3;
    [SerializeField] private float attackCooldownTimer;

    [SerializeField] private bool isZombieAttacking;
    private AudioSource audioSource;
    private Animator anim;
    private GameManagerController gameManagerController;
    [SerializeField] private float damage = 50;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        LoadSound();
        attackCooldownTimer = timeBetweenAttacks;
        gameManagerController =GameObject.FindWithTag("GameManager").GetComponent<GameManagerController>();
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isZombieAttacking) return;
        Move();
        attackCooldownTimer -= Time.deltaTime;
        if (attackCooldownTimer <= 0 && isZombieClose)
        {
            Attack();
            attackCooldownTimer = timeBetweenAttacks;
        }

    }

    void Move()
    {
        transform.LookAt(player.transform.position);
        transform.Translate(Vector3.forward * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            isZombieClose = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            isZombieClose = false;
        }
    }

    void Attack()
    {
        isZombieAttacking = true;
        gameManagerController.ZombieAttack(damage);
        anim.Play("Zombie Attack");
        audioSource.Play();
        StartCoroutine(FinishAttacking());
    }

    private IEnumerator FinishAttacking()
    {
        yield return new WaitForSeconds(1.2f);
        isZombieAttacking = false;
        gameManagerController.ZombieFinishAttack();
	}
    

    private void LoadSound()
    {
        audioSource = GetComponent<AudioSource>();
    }

}
