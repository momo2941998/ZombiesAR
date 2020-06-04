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
    public GameObject heathBar;
    private HeathController heathController;
    bool isInit;

    void Start()
    {
        player = GameObject.FindWithTag("MainCamera");
        gameManagerController = GameObject.FindWithTag("GameManager").GetComponent<GameManagerController>();
        heathController = heathBar.GetComponent<HeathController>();
        playerController = player.GetComponent<PlayerController>();
        //InitZombie();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerController.isGameSetup)
        {
            if (!isInit)
            {
                InitZombie();
                isInit = true;
            }
        }
        if (isZombieAttacking) return;
        if (zombieHp > 0)
        {
            anim.SetFloat("speed", speedMove);
            Move();
            attackCooldownTimer -= Time.deltaTime;
            if (attackCooldownTimer <= 0 && isZombieClose)
            {
                Attack();
                attackCooldownTimer = timeBetweenAttacks;
            }
        }

    }

    void Move()
    {
        Vector3 target = player.transform.position;
        target -= new Vector3(0, target.y, 0);
        transform.LookAt(target);
        
        transform.Translate(Vector3.forward * Time.deltaTime * speedMove * 0.5f);
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
        heathController.ChangeHeath(-_damage);
        if (heathController.currentHeath <= 0)
        {
            anim.SetTrigger("isDeath");
            //zombieRb.detectCollisions = false;
            gameObject.layer = 2;
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
        zombieHp = zombieHpMax = 50;
        heathController.GetParentInfor();
        heathController.ChangeHeath(0);
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
        gameObject.layer = 0;

        zombieRb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        LoadSound();
        ResetHp();
        anim.ResetTrigger("isDeath");
        gameManagerController.ZombieFinishAttack();
        ReturnMoving(1);
        GameObject tombstones = GameObject.FindWithTag("TombStones");
        transform.position = tombstones.transform.position;
        transform.rotation = tombstones.transform.rotation;
        StopAllCoroutines();
        isZombieClose = false;
        isZombieAttacking = false;
        attackCooldownTimer = timeBetweenAttacks;
        ReturnMoving(Random.Range(0.5f, 2.5f));
        anim.SetFloat("speed", speedMove);
        //zombieRb.detectCollisions = true;
    }
}
