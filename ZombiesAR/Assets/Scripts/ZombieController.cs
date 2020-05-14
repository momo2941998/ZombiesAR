using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float zombieScore;
    [SerializeField] private GameObject player;
    private PlayerController playerController;
    [SerializeField] private bool isZombieClose = false;
    [SerializeField] private float timeBetweenAttacks = 3;
    [SerializeField] private float attackCooldownTimer;
    private Rigidbody zombieRb;

    [SerializeField] private bool isZombieAttacking;
    private AudioSource audioSource;
    private Animator anim;
    private GameManagerController gameManagerController;
    [SerializeField] private float damage = 50;
    public float zombieHp;
    public float zombieHpMax = 50;
    public float speedMove = 1;
    void Start()
    {

        InitZombie();
    }

    // Update is called once per frame
    void Update()
    {
        if (isZombieAttacking) return;
        anim.SetFloat("speed", speedMove);
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
        transform.Translate(Vector3.forward * Time.deltaTime * speedMove);
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

    public void TakeDamage(float _damage)
    {
        zombieHp -= _damage;
        if (zombieHp <= 0)
        {
            anim.SetTrigger("isDeath");
            zombieRb.detectCollisions = false;
            StopMoving();
            gameManagerController.IncreaseScore(zombieScore);
            StopCoroutine(FinishAttacking());
            Invoke("Die", 2);
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        InitZombie();
    }
    private void ResetHp()
    {
        zombieHp = zombieHpMax;
    }

    private void StopMoving()
    {
        speedMove = 0;
    }

    private void ReturnMoving(float _speed)
    {
        speedMove = 0;
        speedMove = _speed;
    }

    private void InitZombie()
    {
        player = GameObject.FindWithTag("Player");
        zombieRb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        LoadSound();
        gameManagerController = GameObject.FindWithTag("GameManager").GetComponent<GameManagerController>();
        playerController = player.GetComponent<PlayerController>();
        ResetHp();
        anim.ResetTrigger("isDeath");
        gameManagerController.ZombieFinishAttack();
        ReturnMoving(1);
        GameObject tombstones = GameObject.Find("Tombstones");
        transform.position = tombstones.transform.position;
        transform.rotation = tombstones.transform.rotation;
        StopAllCoroutines();
        isZombieClose = false;
        isZombieAttacking = false;
        attackCooldownTimer = timeBetweenAttacks;
        ReturnMoving(Random.Range(0.5f, 3f));
        // anim.SetFloat("speed", speedMove);
        zombieRb.detectCollisions = true;
    }
}
