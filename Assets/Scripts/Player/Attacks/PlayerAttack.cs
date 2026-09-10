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
    private InputAction Aim;
    private InputAction Cast;
    private InputAction ShootSpell;


    [Header("PlayerComponents")]
    public Animator PlayerAnim;
    public ScriptableObjectScript currentAttack;
    public ScriptableObjectScript MeleeAttackSO;
    public ScriptableObjectScript Normal;
    [SerializeField] private int SpellHeldFrames = 0;
   // [SerializeField] private bool healing;
    [SerializeField] private string[] SpellBook;
    [SerializeField] private string currentSpell;
    private int SpellInt;
    //[SerializeField] private int healingFrames = 0;

    [Header("PlayerAttributes")]
    //[SerializeField] private float timeBetweenAttack = 0;
    private float timeSinceAttack;
    public bool attacking = false;
    //public bool shooting = false;
    public bool casting = false;
    public int Health;
    public int maxHealth;
    public float ammo;
    //start ammo is .3f
    //private float maxAmmo;
    private bool reloading;
    //public float Mana;
    //public float ManaStartFloat;
    //public float ManaEndFloat;
    //public float ManaMax;
    private float InvulFrames = 0;
    //[SerializeField]private bool Aiming;
    [SerializeField] private bool invuln = false;
    public bool ableToAttack;
    [SerializeField] private float ManaDrainSpeed;
  //  [SerializeField] private float TimeToNextHealthTick;
    private bool inCooldown;
    public bool invisible;
    [SerializeField] private float EffectiveRange;
    [SerializeField] private GameObject RangeFinder; 

    //This was originally 33f


    [Header("Misc")]

    [SerializeField] private Material trueMaterial;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float duration;
    private SpriteRenderer PlayerRenderer;
    public Image ShotBarImage;
    //public GameObject Camera;
    //set camera
    //[SerializeField] private GameObject[] HealthMasks;
    [SerializeField] private GameObject FirstMask;
    [SerializeField] private GameObject SecondMask;
    [SerializeField] private GameObject ThirdMask;
    [SerializeField] private GameObject FourthMask;
    [SerializeField] private GameObject FifthMask;
    [SerializeField] private GameObject Fireball;
   // [SerializeField] private GameObject Shot;
    [SerializeField] private bool[] Unlockables;
    [SerializeField] private GameObject MeleeHB;
    [SerializeField] private GameObject[] AbilityUIBoxes;
    public static PlayerAttack instance;
    [SerializeField] private AimScript AimingScript;
    [SerializeField] private GameObject ClonePlayer;
    
    [SerializeField] private GameObject KEF;
    //[SerializeField] private GameObject AimRay;
    //[SerializeField] private GameObject UpperBody;
    //[SerializeField] private GameObject LowerBody;
    //private float amgle;
    //private Vector3 mousePos;
    //[SerializeField] Material FlashMaterial;
    //[SerializeField] Material NormalMaterial;
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
    Clone
    Dash
    KEF
    Long Beam (just put effective range to 1 or 1.25)
    Normal Shot (just put Able to attack to true)
    */



    //when getting a new health mask, HealthMasks.Add(newMask);
    //private int MaskInt = 0;
    //private int HealthInt = 0;
    [SerializeField] PlayerSOScript Values;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerAnim = gameObject.GetComponent<Animator>();
        PlayerRenderer = gameObject.GetComponent<SpriteRenderer>();
        Shoot = InputSystem.actions.FindAction("Attacks/NormalAttack");
        Shoot.performed += ctx => Attack();

        Melee = InputSystem.actions.FindAction("Attacks/Melee");
        //Melee.performed += ctx => MeleeAttack();
        Aim = InputSystem.actions.FindAction("Attacks/Aim");
        Aim.performed += ctx => StartAim();
        Aim.canceled += ctx => EndAiming();
        Cast = InputSystem.actions.FindAction("Attacks/Cast");
        Cast.performed += ctx => FireSpell();
        Cast.canceled += ctx => CancelCast();
        ShootSpell = InputSystem.actions.FindAction("Attacks/FireSpell");
        ShootSpell.performed += ctx => CastSpell();
        ShootSpell.canceled += ctx => EndCast();
        //Cast.canceled += ctx => EndSpell();
       // Melee.performed += ctx => StartFireSpell1();
       // Melee.canceled += ctx => SpellCheck();
        //Heal = InputSystem.actions.FindAction("Heal");
        //Heal.performed += ctx => StartHeal();
        Health = Values.Health;
        maxHealth = Values.maxHealth;
        //Mana = Values.Mana;
        //ManaMax = Values.ManaMax;
        //currentAttack = Values.currentAttack;
        //Spell1 = Values.Spell1;
        Normal = Values.Normal;
        //ammo = .3f;
        //maxAmmo = 1.0f;
        reloading = false;
        SpellInt = 0;
        inCooldown = false;
        invisible = false;
        RangeFinder.GetComponent<CircleCollider2D>().radius = EffectiveRange;
       // Aiming = false;
        //GameObject Spawner = GameObject.FindGameObjectWithTag("Spawner");
        //Camera = Spawner.GetComponent<SpawnerScript>().Camera;
        //Camera = GameObject.FindGameObjectWithTag("MainCamera");

    }

    public void TakeDamage(int AttackDamage)
    {
        if (!invuln)
        {
            PlayerMovement.instance.ableToMove = false;
            ableToAttack = false;
            if (AimingScript.Aiming)
            {
                AimingScript.Aiming = false;
            }
            
            //Camera.GetComponent<CameraFollow>().shaking = true;
            //Camera.GetComponent<CameraFollow>().Shake();
            //Use cinemachine noise
            //Camera.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            CameraManager.instance.Shake(new Vector3(-.2f, -.2f,0));
            //HealthMasks[MaskInt].GetComponent<Animator>().SetTrigger("Broken");
            //MaskInt++;
            Health -= AttackDamage;
            
            AnimatorClipInfo[] clipInfo = PlayerAnim.GetCurrentAnimatorClipInfo(0);
            string currentClipName = clipInfo[0].clip.name;
            PlayerAnim.SetBool(currentClipName, false);
            if (Health == 4)
            {
                
                FirstMask.GetComponent<Animator>().SetBool("Healed", false);
                FirstMask.GetComponent<Animator>().SetTrigger("Broken");
                //invuln = true;

            PlayerAnim.SetTrigger("Damaged");
            
            
            }
            else if (Health == 3)
            {
                
                SecondMask.GetComponent<Animator>().SetBool("Healed", false);
                SecondMask.GetComponent<Animator>().SetTrigger("Broken");
                //invuln = true;

            PlayerAnim.SetTrigger("Damaged");
            //PlayerAnim.SetBool("Running", false);
            
            }
            else if (Health == 2)
            {
                
                ThirdMask.GetComponent<Animator>().SetBool("Healed", false);
                ThirdMask.GetComponent<Animator>().SetTrigger("Broken");
                //invuln = true;

            PlayerAnim.SetTrigger("Damaged");
            //PlayerAnim.SetBool("Running", false);
            
            }
            else if (Health == 1)
            {
                
                FourthMask.GetComponent<Animator>().SetBool("Healed", false);
                FourthMask.GetComponent<Animator>().SetTrigger("Broken");
                //invuln = true;

            PlayerAnim.SetTrigger("Damaged");
            //PlayerAnim.SetBool("Running", false);
            
            }
            else if (Health <= 0)
            {
                PlayerAnim.SetTrigger("Dead");
                FifthMask.GetComponent<Animator>().SetTrigger("Broken");
                Destroy(gameObject, 5);
            }
            
            invuln = true;                
            StartCoroutine(Flash());


            
            //gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
            //gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    void StartAim()
    {
        if (PlayerMovement.instance.Grounded)
        {
            //Aiming = true;
            //AimRay.SetActive(true);
            //PlayerAnim.speed = 0;
            PlayerAnim.SetBool("Idle", false);
            PlayerAnim.Play("Aim");
            AimingScript.Aim();
            //UpperBody.GetComponent<SpriteRenderer>().enabled = true;
            //LowerBody.GetComponent<SpriteRenderer>().enabled = true;
            //PlayerRenderer.enabled = false;
          //  PlayerAnim.SetBool("Aiming", true);
        PlayerMovement.instance.ableToMove = false;
        }
    }
    void EndAiming()
    {
        //Aiming = false;
        //AimRay.SetActive(false);
        //PlayerRenderer.enabled = true;
        PlayerAnim.SetBool("Idle", true);
        AimingScript.EndAim();
        //PlayerAnim.speed = 1;
        //UpperBody.GetComponent<SpriteRenderer>().enabled = false;
        //LowerBody.GetComponent<SpriteRenderer>().enabled = false;
        //PlayerAnim.SetBool("")
        PlayerMovement.instance.ableToMove = true;
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
                StartCoroutine(EndAttack());

            }

        }

        if (!attacking && ShotBarImage.fillAmount <= 1.0f && reloading)
        {

            ShotBarImage.fillAmount += .01f;
        }

        else if (invuln)
        {
            InvulFrames++;
            if (InvulFrames >= 15)
            {
                ableToAttack = true;
                PlayerMovement.instance.ableToMove = true;
            }
            if (InvulFrames >= 60)
            {
                EndInvuln();
            }
        }
        if (casting)
        {

            //if (Cast.ReadValue<float>() > 0)
            //{
            //  Debug.Log("Scroll wheel went down");
            //}
            if (Mouse.current.scroll.ReadValue().y < -0.1)
            {
                if (SpellInt < SpellBook.Length - 1)
                {
                    AbilityUIBoxes[SpellInt].SetActive(false);
                    SpellInt++;
                    Debug.Log(SpellInt + "\n" + SpellBook.Length);
                    AbilityUIBoxes[SpellInt].SetActive(true);
                }
                currentSpell = SpellBook[SpellInt];
                //Debug.Log(currentSpell);
            }
            else if (Mouse.current.scroll.ReadValue().y > 0.1)
            {
                if (SpellInt > 0)
                {
                    AbilityUIBoxes[SpellInt].SetActive(false);
                    SpellInt--;
                    AbilityUIBoxes[SpellInt].SetActive(true);
                }
                currentSpell = SpellBook[SpellInt];
                //Debug.Log(currentSpell);
            }
            //Debug.Log(Mouse.current.scroll.ReadValue().y);
        }
        
        /*else if (casting)
        {
            timeSinceAttack++;
            
            if (timeSinceAttack >= currentAttack.AttackFrames)
            {
                EndSpell();
            }

        }*/
        
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
            if (ShotBarImage.fillAmount >= ammo)
            {
            ShotBarImage.fillAmount -= ammo;
            reloading = false;
            //}
            PlayerMovement.instance.ableToMove = false;
            if (PlayerMovement.instance.Grounded)
            {
                gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 0;   
            }
            currentAttack = Normal;
            attacking = true;
            if (!PlayerMovement.instance.MovingLeft && !PlayerMovement.instance.MovingRight)
        {
            PlayerAnim.SetBool("Idle", false);
        }
        else
        {
            PlayerAnim.SetBool("Running", false);
        }
            //PlayerAnim.SetBool("Idle", false);
            //AnimatorClipInfo[] clipInfo = PlayerAnim.GetCurrentAnimatorClipInfo(0);
            //string currentClipName = clipInfo[0].clip.name;
            if (PlayerAnim.GetBool("Crouching"))
            {
                attacking = true;
                //GameObject Projectile = Instantiate(Shot, new Vector2(transform.position.x, transform.position.y + 0.05f), transform.rotation);
                   // Destroy(Projectile, 0.5f);
                
                /*if (transform.rotation != Quaternion.Euler(0f, 0f, 0f))
                {
                    Projectile.GetComponent<Rigidbody2D>().linearVelocityX = -5;
                    Debug.Log(Projectile.GetComponent<Rigidbody2D>().linearVelocityX);
                }
                if (transform.rotation == Quaternion.Euler(0f, 0f, 0f))
                {
                    Projectile.GetComponent<Rigidbody2D>().linearVelocityX = 5;
                }*/
                //Projectile.GetComponent<ProjectileScript>().BelongsTo = gameObject;
            }
            else
            {
                    attacking = true;
                    // GameObject Projectile = Instantiate(Shot, new Vector2(transform.position.x, transform.position.y + .1f), transform.rotation);
                    //Destroy(Projectile, 0.5f);
                    /*if (transform.rotation != Quaternion.Euler(0f, 0f, 0f))
                    {
                        Projectile.GetComponent<Rigidbody2D>().linearVelocityX = -5;
                        //Debug.Log(Projectile.GetComponent<Rigidbody2D>().linearVelocityX);
                    }
                    if (transform.rotation == Quaternion.Euler(0f, 0f, 0f))
                    {
                        Projectile.GetComponent<Rigidbody2D>().linearVelocityX = 5;
                    }*/
                    AimingScript.Shoot();
                //Projectile.GetComponent<ProjectileScript>().BelongsTo = gameObject;
                //PlayerAnim.Play("Shoot");
                //ManaContainer.fillAmount -= 30;
                //PlayerAnim.SetBool(currentClipName, false);
            }
            }
        }
    }
    void MeleeAttack()
    {
        if (!attacking && ableToAttack)
        {
            PlayerMovement.instance.ableToMove = false;
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
                PlayerAnim.Play("ShootMelee");
                MeleeHB.SetActive(true);
                //make hella hitstop here, like a second or half second
                //StartCoroutine(FreezeTime());
            }
        }
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
            PlayerMovement.instance.ableToMove = false;
            gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
            casting = true;
            PlayerAnim.SetBool("Idle", false);
            PlayerAnim.Play("Cast");
            PlayerAnim.SetBool("Casting", true);
            //for (int i = 0; i < AbilityUIBoxes.Length; i++)
            //{

            //AbilityUIBoxes[i].SetActive(true);
            //}
            AbilityUIBoxes[SpellInt].SetActive(true);
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
    void CancelCast()
    {
        PlayerMovement.instance.ableToMove = true;
        casting = false;
        PlayerAnim.SetBool("Casting", false);
        for (int i = 0; i < AbilityUIBoxes.Length; i++)
        {

            AbilityUIBoxes[i].SetActive(false);
        }
    }
    void EndCast()
    {
        PlayerMovement.instance.ableToMove = true;
        if (SpellInt == 1)
        {
            GameObject ClonePlayer = GameObject.FindWithTag("Clone");
            if (ClonePlayer)
            {
                if (ClonePlayer.GetComponent<PlayerCloneCollider>().dashing)
                {
                    
            transform.position = ClonePlayer.transform.position;
            Destroy(ClonePlayer);
                }
            }
        }
    }
    IEnumerator EndAttack()
    {
        timeSinceAttack = 0;
        //gameObject.GetComponent<PlayerMovement>().ableToMove = true;
        //PlayerAnim.SetTrigger("Attacking");
        PlayerMovement.instance.ableToMove = true;
        MeleeHB.SetActive(false);
       // Debug.Log(Aim.IsPressed());
        if (!PlayerMovement.instance.MovingLeft && !PlayerMovement.instance.MovingRight && !Aim.IsPressed())
        {
            PlayerAnim.SetBool("Idle", true);
        }
        else
        {
            PlayerAnim.SetBool("Running", true);
        }
        attacking = false;
        yield return new WaitForSeconds(2);

        reloading= true;
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
        PlayerMovement.instance.ableToMove = true;
        timeSinceAttack = 0;
        //PlayerAnim.SetBool("Casting", false);
        if (!PlayerMovement.instance.MovingLeft && !PlayerMovement.instance.MovingRight)
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
    IEnumerator Flash()
    {
        if (invuln)
        {
            //yield return new WaitForSeconds(duration);
            PlayerRenderer.material = flashMaterial;
          //  for (int i = 0; i< 5; i++)
            //{
                
            //yield return null;
           // }
           yield return new WaitForSeconds(duration);
           Debug.Log("Hello");
            PlayerRenderer.material = trueMaterial;
            yield return new WaitForSeconds(duration);
            StartCoroutine(Flash());
        }
    }
    void EndInvuln()
    {
        InvulFrames = 0;
        StopCoroutine(Flash());
        invuln = false;
        //gameObject.GetComponent<BoxCollider2D>().enabled = true;
        //gameObject.GetComponent<Rigidbody2D>().WakeUp();
        PlayerMovement.instance.ableToMove = true;
        //PlayerRenderer.material = trueMaterial;
        //Debug.Log(gameObject.GetComponent<PlayerMovement>().ableToMove);
        //Debug.Log("What the fuck");
        ableToAttack = true;
        if (!PlayerMovement.instance.MovingRight && !PlayerMovement.instance.MovingLeft)
        {
            PlayerAnim.SetBool("Idle", true);
            //Debug.Log("Huh");
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
        //gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    void CastSpell()
    {
        if (!attacking && ableToAttack && !inCooldown)
        {
            //Debug.Log(SpellBook[SpellInt]);
            if (Unlockables[SpellInt] == true)
            {
                if (SpellInt == 0)
                {
                    if (Unlockables[SpellInt] == true)
                    {
                        GameObject PlayerClone = Instantiate(ClonePlayer, transform.position, Quaternion.identity);
                        PlayerClone.transform.rotation = transform.rotation;
                        Destroy(PlayerClone, 3);
                    }

                    //make player invisible to enemies 
                    /*if (transform.rotation.y == 0)
                    {
                        PlayerClone.GetComponent<SpriteRenderer>().flipX = false;
                        //!PlayerClone.GetComponent<SpriteRenderer>().flipX;
                    }
                    else if (transform.rotation.y == 180)
                    {
                        PlayerClone.GetComponent<SpriteRenderer>().flipX = true;
                    }*/


                }
                else if (SpellInt == 1)
                {
                    PlayerMovement.instance.ableToMove = false;
                    GameObject PlayerClone = Instantiate(ClonePlayer, transform.position, Quaternion.identity);
                    if (transform.rotation == Quaternion.Euler(0, 180, 0))
                    {
                        PlayerClone.GetComponent<PlayerCloneCollider>().direction = transform.right;
                    }
                    else if (transform.rotation == Quaternion.Euler(0, 0, 0))
                    {
                        PlayerClone.GetComponent<PlayerCloneCollider>().direction = transform.right;
                    }
                    PlayerClone.transform.rotation = transform.rotation;
                    gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
                    PlayerClone.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 145);
                    PlayerClone.GetComponent<PlayerCloneCollider>().dashing = true;


                    //GameObject.Findwithtag("Enemy")
                    //foreach gameObject enemy, pause animator 
                    //foreach gameobject movingplatform, pause movement 
                }
                else if (SpellInt == 2)
                {
                    GameObject KEFDrop = Instantiate(KEF, new Vector3(transform.position.x, transform.position.y - .155f, transform.position.z), Quaternion.identity);

                }
                inCooldown = true;
                //Debug.Log(inCooldown);
                StartCoroutine(Timer(3));
                //inCooldown = false;
                //Debug.Log(inCooldown);
            }
        }
    }
    IEnumerator Timer(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        inCooldown = false;
        //Debug.Log(inCooldown);
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


    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("Please");
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!other.gameObject.GetComponent<EnemyAttack>().Frozen)
            {
                
            //Debug.Log("Ran into the enemy");

            TakeDamage(1);
            PlayerMovement.instance.Recoil();
            }

            
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
        //Values.Mana = Mana;
        //Values.ManaMax = ManaMax;
        //Values.currentAttack = currentAttack;
        //Values.Spell1 = Spell1;
        //hfeiuheuihfe
        Values.Normal = Normal;
        Values.ableToAttack = ableToAttack;
        //Values.currentTransform = transform.position;
        //Destroy(gameObject, 0);
        //Debug.Log(Values.currentTransform);
    }
    
}
