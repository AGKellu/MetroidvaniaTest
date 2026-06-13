//using System.ComponentModel.DataAnnotations.Schema;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    //[Header("Movement Actions")]
    private InputAction moveActionRight;
    private InputAction moveActionLeft;
    private InputAction Jump;
    private InputAction Slide;
    //[Header("Attack Actions")]
    private InputAction NormalAttack;
    private InputAction FirstSpell;
    //Movement Components
    private Animator PlayerAnim;
    private Rigidbody2D PlayerRB;
    private SpriteRenderer PlayerSprite;
    [Header("Player Components")]
    public ScriptableObjectScript currentAttack;
    public ScriptableObjectScript Spell1;
    public ScriptableObjectScript Normal;

    //Frame Times
    private int SpellHeldFrames = 0;
    private float timeBetweenAttack = 0;
    private float timeSinceAttack = 0;
    private float InvulFrames = 0;
    private int slideFrames = 0;
    private int fallFrames = 0;
    private int trueFrames = 0;
    [Header("Player Attributes")]
    public float Speed;
    public float JumpForce;
    public float JumpSpeed;
    public bool MovingLeft;
    public bool MovingRight;
    public bool Grounded = true;
    public bool ableToMove = true;
    [SerializeField] private int JumpCount;
    public bool sliding = false;
    [SerializeField] private bool healing;
    public bool attacking = false;
    public bool casting = false;
    public int Health;
    public int maxHealth;
    public float Mana;
    public float ManaStartFloat;
    public float ManaMax;
    private bool invuln = false;
    private bool ableToAttack = true;
    [SerializeField] private float ManaDrainSpeed;
    [SerializeField] private float TimeToNextHealthTick;

    [Header("UI")]
    public Image ManaContainer;
    public GameObject Camera;
    [SerializeField] private GameObject[] HealthMasks;
    [SerializeField] private GameObject Fireball;
    [SerializeField] private bool[] Unlockables;
    public bool QueueRightTurn = false;
    public bool QueueLeftTurn = false;
    private int MaskInt = 0;
    private int HealthInt = 0;

    [Header("Value Container")]
    [SerializeField] private PlayerSOScript Values;



    void Start()
    {
        NormalAttack = InputSystem.actions.FindAction("Attacks/NormalAttack");
        NormalAttack.performed += ctx => Attack();
        FirstSpell = InputSystem.actions.FindAction("Attacks/Spell1");
        FirstSpell.performed += ctx => StartFireSpell1();
        FirstSpell.canceled += ctx => SpellCheck();
        moveActionRight = InputSystem.actions.FindAction("Move/WalkRight");
        moveActionRight.performed += ctx => StartMovingRight();
        moveActionLeft = InputSystem.actions.FindAction("Move/WalkLeft");
        moveActionLeft.performed += ctx => StartMovingLeft();
        Jump = InputSystem.actions.FindAction("Move/Jump");
        Jump.performed += ctx => StartJump();
        Slide = InputSystem.actions.FindAction("Move/Slide");
        Slide.performed += ctx => StartSlide();
        PlayerRB = gameObject.GetComponent<Rigidbody2D>();
        PlayerAnim = gameObject.GetComponent<Animator>();
        PlayerSprite = gameObject.GetComponent<SpriteRenderer>();
        Health = Values.Health;
        maxHealth = Values.maxHealth;
        Mana = Values.Mana;
        ManaMax = Values.ManaMax;
        currentAttack = Values.currentAttack;
        Spell1 = Values.Spell1;
        Normal = Values.Normal;
        Speed = Values.speed;
        JumpForce = Values.JumpForce;
        JumpSpeed = Values.JumpSpeed;
        JumpCount = Values.JumpCount;
        transform.localScale = Values.currentRotation;
    }
    void Update()
    {
        if (ableToMove)
        {
            CheckForMovement();
        }
        if (ableToAttack)
        {
            if (attacking)
            {
                timeSinceAttack++;
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
                        if (SpellHeldFrames >= 10 && Health < maxHealth)
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
        }
        if (sliding)
        {
            Camera.GetComponent<CameraFollow>().sliding = true;
            slideFrames++;
            ableToMove = false;
            if (slideFrames == 30)
            {
                EndSlide();
            }
        }
        

        if (healing)
        {
            if (FirstSpell.IsPressed())
            {
                Heal();
            }
        }

        CheckForRelease();
    }
    void CheckForRelease()
    {
        if (moveActionRight.WasReleasedThisFrame())
        {
            if (!MovingLeft)
            {
                PlayerAnim.SetBool("Running", false);
                PlayerRB.linearVelocityX = 0;
            }

            MovingRight = false;
        }
        else if (moveActionLeft.WasReleasedThisFrame())
        {
            if (!MovingRight)
            {
                PlayerAnim.SetBool("Running", false);
                PlayerRB.linearVelocityX = 0;
            }
            MovingLeft = false;
        }
    }
    void EndSlide()
    {
        ableToMove = true;
        sliding = false;
        PlayerAnim.SetBool("Sliding", false);
        if (!MovingLeft && !MovingRight)
        {
            Debug.Log("Stop");
            PlayerAnim.SetBool("Idle", true);
            PlayerRB.linearVelocityX = 0;
        }
        else if (MovingLeft || MovingRight)
        {
            PlayerAnim.SetBool("Running", true);
            PlayerRB.linearVelocityX = Speed;
        }
        slideFrames = 0;
    }
    void CheckForMovement()
    {
        if (PlayerRB.linearVelocityY < 0)
        {
            fallFrames++;
            // = true;
            //Debug.Log(fallFrames);
            if (fallFrames >= 15 && !Grounded && !attacking)
            {
                PlayerAnim.SetBool("Falling", true);
            }
            Camera.GetComponent<CameraFollow>().movingUp =
                false;
        }
        if (!sliding)
        {
            //dont stop movement while attacking as well as \/\/\/
            //after attacking, check queued turn, once atacking finished, turn
            if (moveActionRight.IsPressed() && MovingRight)
            {
                if (PlayerAnim.GetBool("Falling"))
                {
                    PlayerRB.linearVelocityX = JumpSpeed;
                }
                else
                {
                    PlayerRB.linearVelocityX = Speed;
                }
            }

            if (moveActionLeft.IsPressed() && MovingLeft)
            {
                if (PlayerAnim.GetBool("Falling"))
                {
                    PlayerRB.linearVelocityX = -JumpSpeed;
                }
                else
                {
                    PlayerRB.linearVelocityX = -Speed;
                }
            }
        }
    }
    void StartMovingRight()
    {
        if (ableToMove)
        {
            MovingRight = true;
            MovingLeft = false;
            if (attacking)
            {
                QueueRightTurn = true;
            }
            else
            {
                //gameObject.GetComponent<PlayerAttack>().Camera.transform.localScale = new Vector3(Mathf.Lerp(-1, 1, 30f), 1, 1);
                //gameObject.GetComponent<PlayerAttack>().Camera.transform.localScale = new Vector3();
                if (
                    Camera.transform.localScale.x
                    == transform.localScale.x
                ) { }
                else
                {
                    Camera.GetComponent<CameraFollow>()
                        .Switch();
                }
                Camera.GetComponent<CameraFollow>()
                    .offset.x = .2f;
                transform.localScale = new Vector3(1, 1, 1);
            }

            // Vector3 startLocalScale = gameObject.GetComponent<PlayerAttack>().Camera.transform.localScale;
            if (Grounded && !attacking)
            {
                PlayerAnim.SetBool("Running", true);
                PlayerAnim.SetBool("Idle", false);
            }
            //if youre in the sky for a speific amount of time, fall
            //when you fall, change the movement things to -fallspeed
        }
    }
    void StartMovingLeft()
    {
        if (ableToMove)
        {
            MovingLeft = true;
            MovingRight = false;
            if (attacking)
            {
                QueueLeftTurn = true;
            }
            else
            {
                Camera.GetComponent<CameraFollow>()
                    .offset.x = -.1f;
                if (
                    Camera.transform.localScale.x
                    == transform.localScale.x
                ) { }
                else
                {
                    Camera.GetComponent<CameraFollow>()
                         .Switch();
                }
                transform.localScale = new Vector3(-1, 1, 1);
            }

            if (Grounded && !attacking)
            {
                PlayerAnim.SetBool("Running", true);
                PlayerAnim.SetBool("Idle", false);
            }
        }
    }
    void StartJump()
    {
        if (ableToMove)
        {
            //if (!gameObject.GetComponent<PlayerAttack>().attacking)
            //{
            if (Grounded)
            {
                PlayerRB.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
                PlayerAnim.SetBool("Jumping", true);
                JumpCount++;
                //Grounded = false;
            }
            else if (Unlockables[3] == true && JumpCount < 2)
            {
                PlayerRB.AddForce(transform.up * (JumpForce * 1.5f), ForceMode2D.Impulse);
                PlayerAnim.Play("jump", -1, 0f);
                JumpCount++;
            }
            /*make another else if once you do wall logic
            else if (Unlockables[2] == true))
            {
            
            PlayerRB.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
            PlayerAnim.SetBool("Jumping", true);
            
            CurrentlyJumping = true;
            }
            */
            //}
            Camera.GetComponent<CameraFollow>().movingUp =
                true;
        }
        //CREATE COYOTE TIME and jump buffering
        //create a timer when youre falling, if the jump button is pressed
        //and the player touches the ground before the timer ends(5 frames), jump when getting on the ground

        //put this code in jump script
        //create a timer after leaving the ground (5 frames)
        // if (playervelocityY < 0  && (playervelocityX > || <0) && timer < ground leave timer)
        //Jump
        fallFrames = 0;

    }
    void StartSlide()
    {
        if (Grounded && !attacking)
        {
            if (transform.localScale.x == 1)
            {
                PlayerRB.AddForce(transform.right * new Vector2(1, 0), ForceMode2D.Impulse);
            }
            else if (transform.localScale.x == -1)
            {
                PlayerRB.AddForce(transform.right * new Vector2(-1, 0), ForceMode2D.Impulse);
            }
            sliding = true;
            PlayerAnim.SetBool("Sliding", true);
            ableToMove = false;
        }
    }
    void Attack()
    {
        if (!attacking && ableToAttack)
        {
            ableToMove = false;
            PlayerRB.linearVelocityX = 0;
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
            else
            {
                FireSpell1();
            }
        }
        SpellHeldFrames = 0;
    }
    public void EndJump()
    {
        PlayerAnim.SetBool("Falling", false);
        PlayerAnim.SetBool("Jumping", false);

        if (!MovingLeft && !MovingRight)
        {
            PlayerAnim.SetBool("Idle", true);
        }
        else if (MovingLeft || MovingRight)
        {
            PlayerAnim.SetBool("Running", true);
        }

        PlayerRB.linearVelocityX = 0;
        fallFrames = 0;
        //CurrentlyJumping = false;
        Grounded = true;
        JumpCount = 0;
        //falling = false;
    }

    public void Transition()
    {
        Values.speed = Speed;
        Values.JumpForce = JumpForce;
        Values.JumpSpeed = JumpSpeed;
        Values.JumpCount = JumpCount;
        Values.Health = Health;
        Values.maxHealth = maxHealth;
        Values.Mana = Mana;
        Values.ManaMax = ManaMax;
        Values.currentAttack = currentAttack;
        Values.Spell1 = Spell1;
        Values.Normal = Normal;
        Values.currentTransform = transform.position;
        Values.currentRotation = transform.localScale;
        //TransitionPanel.GetComponent<Animator>().SetTrigger("Start");
    }
    public void TakeDamage(int AttackDamage)
    {
        if (!invuln)
        {
            ableToMove = false;
            ableToAttack = false;
            Camera.GetComponent<CameraFollow>().shaking = true;
            Camera.GetComponent<CameraFollow>().Shake();
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

            PlayerRB.linearVelocity = new Vector2(0, 0);
            PlayerRB.gravityScale = 0;
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    void FireSpell1()
    {
        currentAttack = Spell1;
        if (
            !attacking
            && ableToAttack
            && (Mana >= currentAttack.ManaGain)
            && Unlockables[0] == true
        )
        {
            ableToMove = false;
            PlayerRB.linearVelocity = new Vector2(0, 0);
            casting = true;
            GameObject Projectile = Instantiate(Fireball, transform.position, transform.rotation);
            Destroy(Projectile, 1);
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
        ableToMove = true;
        PlayerAnim.SetBool("Attacking", false);
        if (
            !MovingLeft
            && !MovingRight
        )
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
        ableToMove = true;
        timeSinceAttack = 0;
        PlayerAnim.SetBool("Casting", false);
        if (
            !MovingLeft
            && !MovingRight
        )
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
        ableToMove = true;
        ableToAttack = true;
        if (
            !MovingRight
            && !MovingLeft
        )
        {
            PlayerAnim.SetBool("Idle", true);
        }
        else if (PlayerRB.linearVelocityY < -0.01)
        {
            PlayerAnim.SetBool("Falling", true);
        }
        else if (PlayerRB.linearVelocityY > 0)
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
        Camera.transform.localEulerAngles = new Vector3(0, 0, 0);
        Camera.GetComponent<CameraFollow>().shaking = false;
        PlayerRB.gravityScale = 1;
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
            Mana -= ManaDrainSpeed * Time.deltaTime;
            ManaContainer.fillAmount = Mana / 100;
        }
    }
    void CancelHeal()
    {
        healing = false;
        if ((ManaStartFloat - Mana) < TimeToNextHealthTick)
        {
            Debug.Log(
                "Interrupted!\nMask Canceled!\nThe mask at "
                    + HealthMasks[HealthInt].name
                    + " will be broken again"
            );
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
        else if (other.gameObject.CompareTag("Spike"))
        {
            TakeDamage(1);
            //TPToSafety();
        }
    }
    public void SetVals()
    {
        Speed = Values.speed;
        JumpForce = Values.JumpForce;
        JumpSpeed = Values.JumpSpeed;
        JumpCount = Values.JumpCount;
        Health = Values.Health;
        maxHealth = Values.maxHealth;
        Mana = Values.Mana;
        ManaMax = Values.ManaMax;
        currentAttack = Values.currentAttack;
        Spell1 = Values.Spell1;
        Normal = Values.Normal;
        transform.position = Values.currentTransform;
        transform.localScale = Values.currentRotation;
    }
}
