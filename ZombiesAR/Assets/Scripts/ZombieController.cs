using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private bool isZombieClose = false;
    [SerializeField] private float timeBetweenAttacks = 3;
    [SerializeField] private float attackCooldownTimer;

    [SerializeField] private bool isZombieAttacking;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        attackCooldownTimer = timeBetweenAttacks;
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
        transform.LookAt(Camera.main.transform.position);
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
        anim.Play("Zombie Attack");
        StartCoroutine(FinishAttacking());
    }

    private IEnumerator FinishAttacking()
    {
        yield return new WaitForSeconds(1.2f);
        isZombieAttacking = false;
    }

}
