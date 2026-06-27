//using System.Numerics;
using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int Health;
    public EnemyAttackSOScript EnemyCurrentAttack;
    public GameObject Fireball;

    //public Projectile Fireball;
    public bool attacking = false;
    public bool shooting = false;
    public float timeSinceAttack;
    private Rigidbody2D RB2D;
    private Animator Anim;
    private float invulnFrames = 0;
    private bool recoiling;
    [SerializeField] private bool pushable;
    private bool chasing = false;
    //[SerializeField] private BoxCollider2D hitboxCollider;
    [SerializeField] private GameObject FreezeBlock;
    public bool Frozen;

    void Start()
    {
        Anim = gameObject.GetComponent<Animator>();
        RB2D = gameObject.GetComponent<Rigidbody2D>();
    }
    IEnumerator FreezeTime()
    {
        //Debug.Log(Time.time);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(.5f);
        //Debug.Log(Time.time);
        Time.timeScale = 1;
    }
    public void TakeShatterDamage()
    {
        Anim.SetTrigger("Dead");
        //CameraManager.instance.Shake(new Vector3(-.2f, -.2f, 0));
        StartCoroutine(FreezeTime());
        Destroy(gameObject, 1);
    }

    public void TakeDamage()
    {
        if (!recoiling)
        {
            FreezeBlock.SetActive(true);
            Frozen = true;
            //Health -= AttackDamage;
            //if (Health <= 0)
            //{
                //StartCoroutine(FreezeTime());
               // Anim.SetTrigger("Dead");
                RB2D.gravityScale = 0;
                //RB2D.linearVelocity = new Vector2(0, 0);
                RB2D.mass = 0;
              //  Destroy(gameObject, 1);
            //}
            recoiling = true;
            Anim.SetTrigger("Damaged");
            Anim.SetBool("Idle", false);
           /* else
            {
                if (pushable)
                {
                    if (GameObject.FindGameObjectWithTag("Player").transform.position.x < gameObject.transform.position.x)
                    {
                        //Hollow knight does not do recoil, but metroid and castlevania does 
                        //RB2D.linearVelocity = KnockBackForce * new Vector2(hitDirection.x, hitDirection.y);
                        
                    }
                    else
                    {
                        //RB2D.linearVelocity = KnockBackForce * new Vector2(-hitDirection.x, -hitDirection.y);
                        
                    }
                }
                //StartCoroutine(FreezeTime());
                recoiling = true;
                //hitboxCollider.enabled = false;
                Anim.SetTrigger("Damaged");
                Anim.SetBool("Idle", false);
            }*/
            


        }

    }

    void Update()
    {
        if (recoiling)
        {
            invulnFrames++;
            if (invulnFrames >= 120)
            {

                EndRecoil();
            }
        }
        if (attacking)
        {
            timeSinceAttack++;
            if (timeSinceAttack == 4)
            {
                Melee();
            }
            if (timeSinceAttack >= EnemyCurrentAttack.AttackFrames)
            {
                EndAttack();
            }
        }
        else if (shooting)
        {
            timeSinceAttack++;
            if (timeSinceAttack == 4)
            {
                Shoot();
            }
            if (timeSinceAttack >= EnemyCurrentAttack.AttackFrames)
            {
                EndShoot();
            }
        }
        else if (chasing)
        {

        }
    }

    void MoveTowardsPlayer()
    {

    }

    void EndAttack()
    {
        timeSinceAttack = 0;
        attacking = false;
        Anim.SetBool("Attacking", false);
        Anim.SetBool("Idle", true);
    }

    void EndShoot()
    {
        timeSinceAttack = 0;
        shooting = false;
        Anim.SetBool("Shooting", false);
        Anim.SetBool("Idle", true);

    }

    void EndRecoil()
    {
        invulnFrames = 0;
        recoiling = false;
        RB2D.linearVelocity = new Vector2(0, 0);
        Anim.SetBool("Damaged", false);
        Anim.SetBool("Idle", true);
       // hitboxCollider.enabled = true;
        if (!chasing)
        {
            Anim.SetBool("Idle", true);
        }
        FreezeBlock.SetActive(false);
        Frozen = false;
    }
    
    

    void Shoot()
    {
        Anim.SetBool("Shooting", true);
        int i = 0;

        while (i < 45)
        {
            i++;
        }
        if (GameObject.FindGameObjectWithTag("Player").transform.position.x < gameObject.transform.position.x)
        {
            if (i == 45)
            {
                GameObject Projectile = Instantiate(Fireball, transform.position, transform.rotation);
                Projectile.GetComponent<ProjectileScript>().BelongsTo = gameObject;
                Projectile.transform.localScale = new Vector3(Projectile.transform.localScale.x * -1, Projectile.transform.localScale.y, Projectile.transform.localScale.z);

                Projectile.GetComponent<Rigidbody2D>().AddForce(1 * new Vector2(-45, -45));
            }
        }
        if (GameObject.FindGameObjectWithTag("Player").transform.position.x > gameObject.transform.position.x)
        {
            if (i == 45)
            {
                GameObject Projectile = Instantiate(Fireball, transform.position, transform.rotation);
                Projectile.GetComponent<ProjectileScript>().BelongsTo = gameObject;
                Projectile.transform.localScale = new Vector3(Projectile.transform.localScale.x * 1, Projectile.transform.localScale.y, Projectile.transform.localScale.z);
                Projectile.GetComponent<Rigidbody2D>().AddForce(1 * new Vector2(45, -45));
            }
        }
        //Fireball.GetComponent<Rigidbody2D>().linearVelocityX = 5;
        //Fireball.GetComponent<Rigidbody2D>().linearVelocityY = 5;
    }

    void Melee()
    {

    }
}
