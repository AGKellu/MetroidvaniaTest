using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using System.Collections;
using UnityEngine.UI;
//using System.Numerics;
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Actions")]
    private InputAction Shoot;
    //private InputAction FirstSpell;
    private InputAction Melee;


    [Header("PlayerComponents")]
    private Animator PlayerAnim;
    public ScriptableObjectScript currentAttack;
    public ScriptableObjectScript MeleeAttackSO;
    public ScriptableObjectScript Normal;
    [SerializeField] private int SpellHeldFrames = 0;
    [SerializeField]private bool healing;
    //[SerializeField] private int healingFrames = 0;

    [Header("PlayerAttributes")]
    [SerializeField] private float timeBetweenAttack = 0;
    private float timeSinceAttack;
    public bool attacking = false;
    //public bool shooting = false;
    public bool casting = false;
    public int Health;
    public int maxHealth;
    public float Mana;
    public float ManaStartFloat;
    //public float ManaEndFloat;
    public float ManaMax;
    private float InvulFrames = 0;
    private bool invuln = false;
    private bool ableToAttack = true;
    [SerializeField] private float ManaDrainSpeed;
    [SerializeField] private float TimeToNextHealthTick;

    //This was originally 33f


    [Header("Misc")]

    [SerializeField] private Material trueMaterial;
    [SerializeField] private Material flashMaterial;
    public Image ManaContainer;
    //public GameObject Camera;
    //set camera
    //[SerializeField] private GameObject[] HealthMasks;
    [SerializeField] private GameObject FirstMask;
    [SerializeField] private GameObject SecondMask;
    [SerializeField] private GameObject ThirdMask;
    [SerializeField] private GameObject FourthMask;
    [SerializeField] private GameObject FifthMask;
    [SerializeField] private GameObject Fireball;
    [SerializeField] private GameObject Shot;
    [SerializeField] private bool[] Unlockables;
    /*public bool QueueRightTurn = false;
    public bool QueueLeftTurn = false;
    */

    //public string currentAnimationName;
    //private bool sequentialHealing;

    //[SerializeField] private InputAction Heal;
    //[SerializeField] private bool healing = false;
    // [SerializeField] private int healingFrames = 0;
    /*
    Unlockables are:
    Fireball
    Iceball
    */



    //when getting a new health mask, HealthMasks.Add(newMask);
    //private int MaskInt = 0;
    //private int HealthInt = 0;
    [SerializeField] PlayerSOScript Values;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerAnim = gameObject.GetComponent<Animator>();

        Shoot = InputSystem.actions.FindAction("Attacks/NormalAttack");
        Shoot.performed += ctx => Attack();

        Melee = InputSystem.actions.FindAction("Attacks/Melee");
        Melee.performed += ctx => MeleeAttack();
       // Melee.performed += ctx => StartFireSpell1();
       // Melee.canceled += ctx => SpellCheck();
        //Heal = InputSystem.actions.FindAction("Heal");
        //Heal.performed += ctx => StartHeal();
        Health = Values.Health;
        maxHealth = Values.maxHealth;
        Mana = Values.Mana;
        ManaMax = Values.ManaMax;
        //currentAttack = Values.currentAttack;
        //Spell1 = Values.Spell1;
        Normal = Values.Normal;
        //GameObject Spawner = GameObject.FindGameObjectWithTag("Spawner");
        //Camera = Spawner.GetComponent<SpawnerScript>().Camera;
        //Camera = GameObject.FindGameObjectWithTag("MainCamera");

    }

    public void TakeDamage(int AttackDamage)
    {
        if (!invuln)
        {
            gameObject.GetComponent<PlayerMovement>().ableToMove = false;
            ableToAttack = false;
            
            
            //Camera.GetComponent<CameraFollow>().shaking = true;
            //Camera.GetComponent<CameraFollow>().Shake();
            //Use cinemachine noise
            //Camera.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            CameraManager.instance.Shake();
            //HealthMasks[MaskInt].GetComponent<Animator>().SetTrigger("Broken");
            //MaskInt++;
            Health -= AttackDamage;
            if (Health == 4)
            {
                
                FirstMask.GetComponent<Animator>().SetBool("Healed", false);
                FirstMask.GetComponent<Animator>().SetTrigger("Broken");
            }
            else if (Health == 3)
            {
                
                SecondMask.GetComponent<Animator>().SetBool("Healed", false);
                SecondMask.GetComponent<Animator>().SetTrigger("Broken");
            }
            else if (Health == 2)
            {
                
                ThirdMask.GetComponent<Animator>().SetBool("Healed", false);
                ThirdMask.GetComponent<Animator>().SetTrigger("Broken");
            }
            else if (Health == 1)
            {
                
                FourthMask.GetComponent<Animator>().SetBool("Healed", false);
                FourthMask.GetComponent<Animator>().SetTrigger("Broken");
            }
            if (Health <= 0)
            {
                PlayerAnim.SetTrigger("Dead");
                FifthMask.GetComponent<Animator>().SetTrigger("Broken");
                Destroy(gameObject, 5);
            }
            else
            {
                invuln = true;

                PlayerAnim.SetTrigger("Damaged");
                PlayerAnim.SetBool("Running", false);



            }

            gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (attacking)
        {
            timeSinceAttack++;
            //if (currentAttack.holdable)
            //{
            //  if (timeSinceAttack > )
            //}
            if (timeSinceAttack >= currentAttack.AttackFrames)
            {
                EndAttack();

            }
        }
        else if (invuln)
        {
            InvulFrames++;
            if (InvulFrames >= 60)
            {
                EndInvuln();
            }
        }
        else if (casting)
        {
            timeSinceAttack++;
            /*if (currentAttack.holdable)
            {
                if (FirstSpell.IsPressed())
                {
                    SpellHeldFrames++;
                    if (SpellHeldFrames >=10 && Health < maxHealth)
                    {
                        StartHeal();
                    }
                    
                }
            }*/
            
            /*if (timeSinceAttack >= currentAttack.AttackFrames)
            {
                EndSpell();
            }*/

        }
        /*
        if (healing)
        {
            if (FirstSpell.IsPressed())
            {
                gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 0; 
                Heal();
            
            }
            
            
        }*/

    }

    void Attack()
    {
        if (!attacking && ableToAttack)
        {
            currentAttack = Normal;
            attacking = true;
            AnimatorClipInfo[] clipInfo = PlayerAnim.GetCurrentAnimatorClipInfo(0);
            string currentClipName = clipInfo[0].clip.name;
            //Debug.Log(currentClipName);
            if (PlayerAnim.GetBool("Crouching"))
            {
                attacking = true;
                GameObject Projectile = Instantiate(Shot, new Vector2(transform.position.x, transform.position.y + 0.05f), transform.rotation);
                Destroy(Projectile, 0.5f);
                if (transform.rotation != Quaternion.Euler(0f, 0f, 0f))
                {
                    Projectile.GetComponent<Rigidbody2D>().linearVelocityX = -5;
                    Debug.Log(Projectile.GetComponent<Rigidbody2D>().linearVelocityX);
                }
                if (transform.rotation == Quaternion.Euler(0f, 0f, 0f))
                {
                    Projectile.GetComponent<Rigidbody2D>().linearVelocityX = 5;
                }
                Projectile.GetComponent<ProjectileScript>().BelongsTo = gameObject;
            }
            else
            {
                attacking = true;
                GameObject Projectile = Instantiate(Shot, new Vector2(transform.position.x, transform.position.y + .1f), transform.rotation);
                Destroy(Projectile, 0.5f);
                if (transform.rotation != Quaternion.Euler(0f, 0f, 0f))
                {
                    Projectile.GetComponent<Rigidbody2D>().linearVelocityX = -5;
                    Debug.Log(Projectile.GetComponent<Rigidbody2D>().linearVelocityX);
                }
                if (transform.rotation == Quaternion.Euler(0f, 0f, 0f))
                {
                    Projectile.GetComponent<Rigidbody2D>().linearVelocityX = 5;
                }
                Projectile.GetComponent<ProjectileScript>().BelongsTo = gameObject;
                PlayerAnim.Play("Shoot");
                PlayerAnim.SetBool(currentClipName, false);
            }
        }
    }
    void MeleeAttack()
    {
        if (!attacking && ableToAttack)
        {
            currentAttack = MeleeAttackSO;
            attacking = true;
            AnimatorClipInfo[] clipInfo = PlayerAnim.GetCurrentAnimatorClipInfo(0);
            string currentClipName = clipInfo[0].clip.name;
            //Debug.Log(currentClipName);
            if (PlayerAnim.GetBool("Crouching"))
            {
                //attacking = true;
            }
            else
            {
                attacking = true;
                PlayerAnim.SetBool(currentClipName, false);
                //make hella hitstop here, like a second or half second
                StartCoroutine(FreezeTime());
            }
        }
    }
    IEnumerator FreezeTime()
    {
        Debug.Log(Time.time);
        PlayerAnim.Play("ShootMelee");
        
        yield return new WaitForSecondsRealtime(2);
        Debug.Log(Time.time);
    }
    void FireSpell()
    {
       // currentAttack = Spell1;
       /* if (!attacking && ableToAttack && (Mana >= currentAttack.ManaGain) && Unlockables[0] == true)
        {
            gameObject.GetComponent<PlayerMovement>().ableToMove = false;
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
            casting = true;
            GameObject Projectile = Instantiate(Fireball, transform.position, transform.rotation);
            Destroy(Projectile, 1);
            if (transform.rotation == Quaternion.Euler(0f, 180f, 0f))
            {
                //Projectile.transform.localScale = new Vector3(-1, 1, 1);
                Projectile.GetComponent<Rigidbody2D>().linearVelocityX = -5;
            }
            else if (transform.rotation == Quaternion.Euler(0f, 0f,0f))
            {
              //  Projectile.transform.localScale = new Vector3(1, 1, 1);
                Projectile.GetComponent<Rigidbody2D>().linearVelocityX = 5;
            }
            Projectile.GetComponent<ProjectileScript>().BelongsTo = gameObject;
            attacking = true;
            Mana -= currentAttack.ManaGain;
            ManaContainer.fillAmount = Mana / 100;
            PlayerAnim.SetBool("Casting", true);
            PlayerAnim.SetBool("Idle", false);
        }*/
        if (!attacking && ableToAttack)
        {
            gameObject.GetComponent<PlayerMovement>().ableToMove = false;
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
            casting = true;
            //for dash, keep the linearvelocity = new Vector2(0, 0) and abletomove = false;
            //Instantiate a clone of the player that has trigger collider, make the alpha slightly transparent, and move them forward while the button is held
            //maybe stop time???
            //make no rigidbody so it doesnt have gravity
            //when the button is let go, set the player position to the clone position, destroy the clone, allow yourself to cast again 

            //for trap, make sure the player is grounded, instantiate trap that when projectiles contact it, destroy the projectile, set collider to trigger, make no rigidbody so it doesnt move
            
            //for clone, make sure the player is grounded, instantiate player clone, make player invisible to enemies (most likely change a tag)
            //after 3 seconds and/or when the player attacks return tag to player, delete clone
            
            //casting is Y or f for keyboard, changing casts is LT/RT or scroll wheel for keyboard

        }
    }
    void EndAttack()
    {
        timeSinceAttack = 0;
        //gameObject.GetComponent<PlayerMovement>().ableToMove = true;
        //PlayerAnim.SetTrigger("Attacking");
        if (!gameObject.GetComponent<PlayerMovement>().MovingLeft && !gameObject.GetComponent<PlayerMovement>().MovingRight)
        {
            PlayerAnim.SetBool("Idle", true);
        }
        else
        {
            PlayerAnim.SetBool("Running", true);
        }
        attacking = false;
        /*if (QueueLeftTurn)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            QueueLeftTurn = false;
        }
        else if (QueueRightTurn)
        {
            transform.localScale = new Vector3(1, 1, 1);
            QueueRightTurn = false;
        }*/
    }
    void EndSpell()
    {
        gameObject.GetComponent<PlayerMovement>().ableToMove = true;
        timeSinceAttack = 0;
        //PlayerAnim.SetBool("Casting", false);
        if (!gameObject.GetComponent<PlayerMovement>().MovingLeft && !gameObject.GetComponent<PlayerMovement>().MovingRight)
        {
            PlayerAnim.SetBool("Idle", true);
        }
        else
        {
            PlayerAnim.SetBool("Running", true);
        }
        casting = false;
        /*if (QueueLeftTurn)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            QueueLeftTurn = false;
        }
        else if (QueueRightTurn)
        {
            transform.localScale = new Vector3(1, 1, 1);
            QueueRightTurn = false;
        }*/
    }
    void EndInvuln()
    {
        InvulFrames = 0;
        invuln = false;
        //gameObject.GetComponent<BoxCollider2D>().enabled = true;
        //gameObject.GetComponent<Rigidbody2D>().WakeUp();
        gameObject.GetComponent<PlayerMovement>().ableToMove = true;
        ableToAttack = true;
        if (!gameObject.GetComponent<PlayerMovement>().MovingRight && !gameObject.GetComponent<PlayerMovement>().MovingLeft)
        {
            PlayerAnim.SetBool("Idle", true);
        }
        else if (gameObject.GetComponent<Rigidbody2D>().linearVelocityY < -0.01)
        {
            PlayerAnim.SetBool("Falling", true);
        }
        else if (gameObject.GetComponent<Rigidbody2D>().linearVelocityY > 0)
        {
            PlayerAnim.SetBool("Jumping", true);
        }
       /* if (QueueLeftTurn)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            QueueLeftTurn = false;
        }
        else if (QueueRightTurn)
        {
            transform.localScale = new Vector3(1, 1, 1);
            QueueRightTurn = false;
        }*/
        //Camera.transform.localEulerAngles = new Vector3(0, 0, 0);
        //Camera.GetComponent<CameraFollow>().shaking = false;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
    }
    /*void StartHeal()
    {
        healing = true;
        casting = false;
        ManaStartFloat = Mana;
        timeSinceAttack = 0;
        //if (Health < (Health - 1))
        //{
          //  sequentialHealing = true;
        //}
       // if (sequentialHealing)
        //{

        //}
        //else
        //{
            if (Health == 4)
            {
                FirstMask.GetComponent<Animator>().SetTrigger("Heal");
            }
            else if (Health == 3)
            {
                SecondMask.GetComponent<Animator>().SetTrigger("Heal");
            }
            else if (Health == 2)
            {
                ThirdMask.GetComponent<Animator>().SetTrigger("Heal");
            }
            else if (Health == 1)
            {
                FourthMask.GetComponent<Animator>().SetTrigger("Heal");
            }
        //HealthMasks[HealthInt].GetComponent<Animator>().SetTrigger("Heal");
        //}
    }*/
    
    void Heal()
    {
        /*SpellHeldFrames = 0;
        if (Mana > 0)
        {
            if (Mana < (ManaStartFloat - TimeToNextHealthTick))
            {
                ManaStartFloat = Mana;
                if (Health == 4)
            {
                FirstMask.GetComponent<Animator>().SetBool("Healed", true);
            }
            else if (Health == 3)
            {
                SecondMask.GetComponent<Animator>().SetBool("Healed", true);
            }
            else if (Health == 2)
            {
                ThirdMask.GetComponent<Animator>().SetBool("Healed", true);
            }
            else if (Health == 1)
            {
                FourthMask.GetComponent<Animator>().SetBool("Healed", true);
            }
                Health++;

                //HealthMasks[HealthInt].GetComponent<Animator>().SetBool("Healed", true);
                //MaskInt--;
               // HealthInt++;
                //HealthMasks[HealthInt].GetComponent<Animator>().SetTrigger("Heal");
                //HealthInt--;
                Debug.Log("Healed one mask!");
            }
        Mana -= ManaDrainSpeed* Time.deltaTime;
        ManaContainer.fillAmount = Mana/100;
        }*/
    }
    void SpellCheck()
    {
        //if (healing)
        //{
          //  CancelHeal();
        //}
        /*else 
        {
            if (SpellHeldFrames >= 10)
        {
            
        }
          //  StartHeal();
        //}
        else
        {
            FireSpell1();
        }
        }
        */
        SpellHeldFrames = 0;
        //healingFrames = 0;
    }
    void CancelHeal()
    {
        /*healing = false;
        //sequentialHealing = false;
        if ((Mana - ManaStartFloat) < TimeToNextHealthTick)
        {
            if (Health == 4)
            {
                
                FirstMask.GetComponent<Animator>().SetBool("Healed", false);
                FirstMask.GetComponent<Animator>().SetTrigger("Broken");
            }
            else if (Health == 3)
            {
                
                SecondMask.GetComponent<Animator>().SetBool("Healed", false);
                SecondMask.GetComponent<Animator>().SetTrigger("Broken");
            }
            else if (Health == 2)
            {
                
                ThirdMask.GetComponent<Animator>().SetBool("Healed", false);
                ThirdMask.GetComponent<Animator>().SetTrigger("Broken");
            }
            else if (Health == 1)
            {
                
                FourthMask.GetComponent<Animator>().SetBool("Healed", false);
                FourthMask.GetComponent<Animator>().SetTrigger("Broken");
            }
            //Debug.Log("Heal Canceled! Mask at " + HealthMasks[HealthInt].name + "will be broken again");
        }
        //Debug.Log(ManaStartFloat -= Mana);
        //if (Mana - ManaStartFloat < 33)
        //{
          //  Debug.Log("Interrupted! \nMask Canceled!");
           // Debug.Log("The mask at " + HealthMasks[HealthInt].name + " will be broken again");
            //HealthMasks[HealthInt].GetComponent<Animator>().SetTrigger("Broken");
        //}
        //if ((ManaStartFloat - Mana) < TimeToNextHealthTick)
        //{
          //  Debug.Log("Interrupted!\nMask Canceled!\nThe mask at " + HealthMasks[HealthInt].name + " will be broken again");
          //  HealthMasks[HealthInt].GetComponent<Animator>().SetTrigger("Broken");
          //  HealthInt++;
       // }
        /*if (QueueLeftTurn)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            QueueLeftTurn = false;
        }
        else if (QueueRightTurn)
        {
            transform.localScale = new Vector3(1, 1, 1);
            QueueRightTurn = false;
        }*/
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("Please");
        if (other.gameObject.CompareTag("Enemy"))
        {
            //Debug.Log("Ran into the enemy");

            TakeDamage(1);

            //gameObject.GetComponent<Rigidbody2D>().Sleep();
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;

        }
        else if (other.gameObject.CompareTag("Spike"))
        {
            TakeDamage(1);
            //TPToSafety();
        }



    }
    public void Transition()
    {
        Values.Health = Health;
    Values.maxHealth = maxHealth;
    Values.Mana = Mana;
        Values.ManaMax = ManaMax;
    //Values.currentAttack = currentAttack;
    //Values.Spell1 = Spell1;
    //hfeiuheuihfe
    Values.Normal = Normal;
        //Values.currentTransform = transform.position;
        //Destroy(gameObject, 0);
        //Debug.Log(Values.currentTransform);
    }
    
}
