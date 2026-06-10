using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;
//using System.Numerics;
public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Actions")]
    private InputAction NormalAttack;
    private InputAction FirstSpell;



    [Header("PlayerComponents")]
    private Animator PlayerAnim;
    public ScriptableObjectScript currentAttack;
    public ScriptableObjectScript Spell1;
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
    public GameObject Camera;
    [SerializeField] private GameObject[] HealthMasks;
    [SerializeField] private GameObject Fireball;
    [SerializeField] private bool[] Unlockables;
    public bool QueueRightTurn = false;
    public bool QueueLeftTurn = false;

    //[SerializeField] private InputAction Heal;
    //[SerializeField] private bool healing = false;
    // [SerializeField] private int healingFrames = 0;
    /*
    Unlockables are:
    Fireball
    Iceball
    */



    //when getting a new health mask, HealthMasks.Add(newMask);
    private int MaskInt = 0;
    private int HealthInt = 0;
    [SerializeField] PlayerSOScript Values;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerAnim = gameObject.GetComponent<Animator>();

        NormalAttack = InputSystem.actions.FindAction("Attacks/NormalAttack");
        NormalAttack.performed += ctx => Attack();

        FirstSpell = InputSystem.actions.FindAction("Attacks/Spell1");
        FirstSpell.performed += ctx => StartFireSpell1();
        FirstSpell.canceled += ctx => SpellCheck();
        //Heal = InputSystem.actions.FindAction("Heal");
        //Heal.performed += ctx => StartHeal();
        Health = Values.Health;
        maxHealth = Values.maxHealth;
        Mana = Values.Mana;
        ManaMax = Values.ManaMax;
        currentAttack = Values.currentAttack;
        Spell1 = Values.Spell1;
        Normal = Values.Normal;

    }

    public void TakeDamage(int AttackDamage)
    {
        if (!invuln)
        {
            gameObject.GetComponent<PlayerMovement>().ableToMove = false;
            ableToAttack = false;
            //Camera.GetComponent<CameraFollow>().shaking = true;
            Camera.GetComponent<CameraFollow>().Shake();
            //Camera.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            HealthMasks[MaskInt].GetComponent<Animator>().SetTrigger("Broken");
            MaskInt++;
            Health -= AttackDamage;
            if (Health <= 0)
            {
                PlayerAnim.SetTrigger("Dead");
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
            if (currentAttack.holdable)
            {
                if (FirstSpell.IsPressed())
                {
                    SpellHeldFrames++;
                    if (SpellHeldFrames >=10 && Health < maxHealth)
                    {
                        StartHeal();
                    }
                    
                }
            }
            else if (!currentAttack.holdable)
            {
                FireSpell1();
            }
            if (timeSinceAttack >= currentAttack.AttackFrames)
            {
                EndSpell();
            }
        }
        
        if (healing)
        {
            if (FirstSpell.IsPressed())
            {
                Heal();
            
            }
            
            
        }

    }

    void Attack()
    {
        if (!attacking && ableToAttack)
        {
            //gameObject.GetComponent<PlayerMovement>().ableToMove = false;
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 0;
            currentAttack = Normal;
            attacking = true;
            PlayerAnim.SetBool("Attacking", true);
            PlayerAnim.SetBool("Idle", false);
        }
    }
    void StartFireSpell1()
    {
        currentAttack = Spell1;
        if (!attacking && !casting && (Mana >= currentAttack.ManaGain) && Unlockables[0] == true)
        {
            casting = true;
        }
    }
    void FireSpell1()
    {
        currentAttack = Spell1;
        if (!attacking && ableToAttack && (Mana >= currentAttack.ManaGain) && Unlockables[0] == true)
        {
            gameObject.GetComponent<PlayerMovement>().ableToMove = false;
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
            casting = true;
            GameObject Projectile = Instantiate(Fireball, transform.position, transform.rotation);
            if (transform.localScale.x == 1)
            {
                Projectile.transform.localScale = new Vector3(-1, 1, 1);
                Projectile.GetComponent<Rigidbody2D>().linearVelocityX = 5;
            }
            else if (transform.localScale.x == -1)
            {
                Projectile.transform.localScale = new Vector3(1, 1, 1);
                Projectile.GetComponent<Rigidbody2D>().linearVelocityX = -5;
            }
            Projectile.GetComponent<ProjectileScript>().BelongsTo = gameObject;
            attacking = true;
            Mana -= currentAttack.ManaGain;
            ManaContainer.fillAmount = Mana / 100;
            PlayerAnim.SetBool("Casting", true);
            PlayerAnim.SetBool("Idle", false);
        }
    }
    void EndAttack()
    {
        timeSinceAttack = 0;
        gameObject.GetComponent<PlayerMovement>().ableToMove = true;
        PlayerAnim.SetBool("Attacking", false);
        if (!gameObject.GetComponent<PlayerMovement>().MovingLeft && !gameObject.GetComponent<PlayerMovement>().MovingRight)
        {
            PlayerAnim.SetBool("Idle", true);
        }
        else
        {
            PlayerAnim.SetBool("Running", true);
        }
        attacking = false;
        if (QueueLeftTurn)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            QueueLeftTurn = false;
        }
        else if (QueueRightTurn)
        {
            transform.localScale = new Vector3(1, 1, 1);
            QueueRightTurn = false;
        }
    }
    void EndSpell()
    {
        gameObject.GetComponent<PlayerMovement>().ableToMove = true;
        timeSinceAttack = 0;
        PlayerAnim.SetBool("Casting", false);
        if (!gameObject.GetComponent<PlayerMovement>().MovingLeft && !gameObject.GetComponent<PlayerMovement>().MovingRight)
        {
            PlayerAnim.SetBool("Idle", true);
        }
        else
        {
            PlayerAnim.SetBool("Running", true);
        }
        casting = false;
        if (QueueLeftTurn)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            QueueLeftTurn = false;
        }
        else if (QueueRightTurn)
        {
            transform.localScale = new Vector3(1, 1, 1);
            QueueRightTurn = false;
        }
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
        if (QueueLeftTurn)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            QueueLeftTurn = false;
        }
        else if (QueueRightTurn)
        {
            transform.localScale = new Vector3(1, 1, 1);
            QueueRightTurn = false;
        }
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
    }
    void StartHeal()
    {
        healing = true;
        casting = false;
        ManaStartFloat = Mana;
        timeSinceAttack = 0;
        HealthMasks[HealthInt].GetComponent<Animator>().SetTrigger("Heal");
    }
    
    void Heal()
    {
        SpellHeldFrames = 0;
        if (Mana > 0)
        {
            if (Mana < (ManaStartFloat - TimeToNextHealthTick))
            {
                ManaStartFloat = Mana;
                Health++;
                HealthMasks[HealthInt].GetComponent<Animator>().SetBool("Healed", true);
                MaskInt--;
            }
        Mana -= ManaDrainSpeed* Time.deltaTime;
        ManaContainer.fillAmount = Mana/100;
        }
    }
    void SpellCheck()
    {
        if (healing)
        {
            CancelHeal();
        }
        else 
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
        
        SpellHeldFrames = 0;
        //healingFrames = 0;
    }
    void CancelHeal()
    {
        healing = false;
        Debug.Log(ManaStartFloat -= Mana);
        //if (Mana - ManaStartFloat < 33)
        //{
          //  Debug.Log("Interrupted! \nMask Canceled!");
           // Debug.Log("The mask at " + HealthMasks[HealthInt].name + " will be broken again");
            //HealthMasks[HealthInt].GetComponent<Animator>().SetTrigger("Broken");
        //}
        if ((ManaStartFloat - Mana) < TimeToNextHealthTick)
        {
            Debug.Log("Interrupted!\nMask Canceled!\nThe mask at " + HealthMasks[HealthInt].name + " will be broken again");
            HealthMasks[HealthInt].GetComponent<Animator>().SetTrigger("Broken");
            HealthInt++;
        }
        if (QueueLeftTurn)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            QueueLeftTurn = false;
        }
        else if (QueueRightTurn)
        {
            transform.localScale = new Vector3(1, 1, 1);
            QueueRightTurn = false;
        }
        
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
        /*else if (other.gameObject.CompareTag("Spike"))
        {
            TakeDamage(1);
            //TPToSafety();
        }*/



    }
    public void Transition()
    {
        Values.Health = Health;
    Values.maxHealth = maxHealth;
    Values.Mana = Mana;
    Values.ManaMax = ManaMax;
    Values.currentAttack = currentAttack;
    Values.Spell1 = Spell1;
    Values.Normal = Normal;
        Values.currentTransform = transform.position;
        Debug.Log(Values.currentTransform);
    }
    
}
