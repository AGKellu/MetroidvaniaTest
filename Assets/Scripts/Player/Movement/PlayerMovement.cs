//using System.Numerics;
//using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Actions")]
    private InputAction moveActionRight;
    private InputAction moveActionLeft;
    private InputAction moveActionCrouch;
    private InputAction Jump;
    private InputAction Slide;
    private InputAction CamToggle;

    [Header("Player Components")]
    private Animator PlayerAnim;
    private Rigidbody2D PlayerRB;
    private SpriteRenderer PlayerSprite;

    [Header("Player Attributes")]
    public float Speed;
    public float JumpForce;
    public float JumpSpeed;
    public bool MovingLeft;
    public bool MovingRight;

    // public bool CurrentlyJumping = false;
    public bool Grounded = true;
    public bool ableToMove = true;
    private float jumpGoodFrames;
    [SerializeField] private int JumpCount;
    public bool sliding = false;
    [SerializeField] private bool gripping;
    //public bool crouching;
    //private int slideFrames = 0;
    [SerializeField]private bool canWalk;

    //public bool slideUnlocked = false;
    //private bool busy;

    [Header("Misc")]
    private int trueFrames;
    //private int fallFrames = 0;
    //private bool falling;

    [SerializeField]
    private bool[] Unlockables;
    public static PlayerMovement instance;
    private GameObject Spawner;

    [SerializeField] private PlayerSOScript Values;
    [SerializeField] private GameObject CrouchHB;
    //public GameObject Camera;

    [Header("Camera Stuff")]
    private CameraFollowObject cameraFollowObject;
    [SerializeField] private GameObject cameraFollowGO;
    private float fallSpeedYDampingChangeThreshold;
    public float thingy;
    public bool CanMoveCam;
    //[SerializeField] private GameObject TransitionPanel;
    /*
    Unlockables are:
    Slide
    Dash
    Wall Jump
    Double Jump
    Ledge Grab
    */
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
        //CanMoveCam = false;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        moveActionRight = InputSystem.actions.FindAction("Move/WalkRight");
        moveActionRight.performed += ctx => StartMovingRight();
        moveActionLeft = InputSystem.actions.FindAction("Move/WalkLeft");
        moveActionLeft.performed += ctx => StartMovingLeft();
        moveActionCrouch = InputSystem.actions.FindAction("Move/Crouch");
        moveActionCrouch.performed += ctx => StartCrouching();
        moveActionCrouch.canceled += ctx => EndCrouch();
        Jump = InputSystem.actions.FindAction("Move/Jump");
        Jump.performed += ctx => StartJump();
        Jump.canceled += ctx => EndUpwardMomentum();
        Slide = InputSystem.actions.FindAction("Move/Slide");
        Slide.performed += ctx => StartSlide();
        Slide.canceled += ctx => EndSlide();
        CamToggle = InputSystem.actions.FindAction("Camera/Toggle");
        CamToggle.performed += ctx => ToggleCamera();
        CamToggle.canceled += ctx => EndToggleCamera();
        PlayerRB = gameObject.GetComponent<Rigidbody2D>();
        PlayerAnim = gameObject.GetComponent<Animator>();
        PlayerSprite = gameObject.GetComponent<SpriteRenderer>();
       // Speed = Values.speed;
        //JumpForce = Values.JumpForce;
        //JumpSpeed = Values.JumpSpeed;
        //JumpCount = Values.JumpCount;
        cameraFollowObject = cameraFollowGO.GetComponent<CameraFollowObject>();
        //GameObject Spawner()
        // transform.localScale = Values.currentRotation;
        CameraManager.instance.enabled = true;
        fallSpeedYDampingChangeThreshold = CameraManager.instance.fallSpeedYDampingChangeThreshold;
        canWalk = true;
        gripping = false;
        Grounded = true;
        PlayerAnim.SetBool("Idle", true);
    }
    public void Recoil()
    {
        Grounded = false;
        PlayerRB.gravityScale = 1;
        if (transform.rotation != Quaternion.Euler(0f, 0f, 0f))

        {
            PlayerRB.linearVelocity = 0.25f * new Vector2(25, 25);
        }
        else if (transform.rotation == Quaternion.Euler(0f, 0f, 0f))
        {
            PlayerRB.linearVelocity = 0.25f * new Vector2(-25, -25);
        }
    }
    //void
    // Update is called once per frame
    void Update()
    {
        if (ableToMove)
        {
            CheckForMovement();
        }
       /* if (sliding)
        {
            if (moveActionRight.WasReleasedThisFrame())
            {
                if (!MovingLeft && !sliding)
                {
                    PlayerAnim.SetBool("Running", false);
                    PlayerAnim.SetBool("Idle", true);
                    //PlayerRB.linearVelocityX = 0;
                }

                MovingRight = false;
            }
            else if (moveActionLeft.WasReleasedThisFrame())
            {
                if (!MovingRight && !sliding)
                {
                    PlayerAnim.SetBool("Running", false);
                    PlayerAnim.SetBool("Idle", true);

                    //PlayerRB.linearVelocityX = 0;
                }

                MovingLeft = false;
            }
        }*/
               RaycastHit2D hitDown = Physics2D.Raycast(transform.position, -Vector2.up, thingy, LayerMask.GetMask("Ground"));
            Debug.DrawRay(transform.position, -Vector2.up * thingy, Color.red);
        if (hitDown)
        {
            if (hitDown.collider.gameObject.name.Contains("Floor"))
            {

                Grounded = true;

            }
            
        }
        else
        {
            Grounded = false;
            if (PlayerRB.linearVelocityY < 0 && !PlayerAttack.instance.attacking)
            {
                PlayerAnim.SetBool("Falling", true);
                PlayerRB.gravityScale = 1.5f;
            }
        }
        /*if (sliding)
        {
           // Camera.GetComponent<CameraFollow>().sliding  = true;
            slideFrames++;
            ableToMove = false;
            if (slideFrames == 30)
            {
                EndSlide();
            }
        }*/
        /*if (Grounded)
        {
            PlayerRB.linearVelocityY = 0;
            PlayerRB.gravityScale = 0;
        }
        else 
        {
            if (gripping)
            {
                PlayerRB.linearVelocity = new Vector2(0, 0);
                PlayerRB.gravityScale = 0;
            
            }
            else 
            {
                PlayerRB.gravityScale = 1;
            }
        }*/
        
        /*if (Grounded)
        {
            PlayerRB.gravityScale = 0;
        }
        else 
        {
            PlayerRB.gravityScale = 1;
        }*/
        
            
        
        CheckForRelease();
    }

    void CheckForRelease()
    {
        if (moveActionRight.WasReleasedThisFrame())
        {
            if (!MovingLeft && !sliding)
            {
                PlayerAnim.SetBool("Running", false);
                PlayerAnim.SetBool("Idle", true);
                PlayerRB.linearVelocityX = 0;
            }
            if (sliding)
            {
                PlayerAnim.SetBool("Running", false);
            }

            MovingRight = false;
        }
        else if (moveActionLeft.WasReleasedThisFrame())
        {
            if (!MovingRight && !sliding)
            {
                PlayerAnim.SetBool("Running", false);
                PlayerAnim.SetBool("Idle", true);

                PlayerRB.linearVelocityX = 0;
            }

            MovingLeft = false;
            if (sliding)
            {
                PlayerAnim.SetBool("Running", false);
            }
            
        }
        /*if (QueueReleaseLeft)
        {
            if (!MovingRight)
            {
                PlayerAnim.SetBool("Running", false);
                PlayerAnim.SetBool("Idle", true);
                
            }
        }
        else if (QueueReleaseRight)
        {
            if (!MovingLeft)
            {
                
            PlayerAnim.SetBool("Running", false);
            PlayerAnim.SetBool("Idle", true);
            }

        }*/
        //else if (moveActionCrouch.WasReleasedThisFrame())
        //{
            //PlayerAnim.SetBool()
        //}
    }

    void CheckForMovement()
    {
        if (PlayerRB.linearVelocityY < fallSpeedYDampingChangeThreshold && !CameraManager.instance.isLerpingYDamping && !CameraManager.instance.lerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
           // fallFrames++;
            //if (fallFrames >= 15 && !Grounded && !gameObject.GetComponent<PlayerAttack>().attacking)
            //{
              //  PlayerAnim.SetBool("Falling", true);
              //  PlayerRB.gravityScale = 1.5f;

            //}
        }
        //if (PlayerRB.linearVelocityY <0 && PlayerRB.linearVelocityY > 0)
        //{
               RaycastHit2D hitDown = Physics2D.Raycast(transform.position, -Vector2.up, thingy, LayerMask.GetMask("Ground"));
            Debug.DrawRay(transform.position, -Vector2.up * thingy, Color.red);
        if (hitDown)
        {
            if (hitDown.collider.gameObject.name.Contains("Floor"))
            {

                if (PlayerRB.linearVelocityY < 0.01)
                {
                    if (JumpCount > 0)
                    {

                      EndJump();
                     }
                }

            }
            
        }
        else
        {
            Grounded = false;
            if (PlayerRB.linearVelocityY < 0 && !PlayerAttack.instance.attacking)
            {
                PlayerAnim.SetBool("Falling", true);
                PlayerRB.gravityScale = 1.5f;
            }
        }
        //}
        if (PlayerRB.linearVelocityY >= 0f && !CameraManager.instance.isLerpingYDamping && CameraManager.instance.lerpedFromPlayerFalling)
        {
            
            CameraManager.instance.lerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
        if (!sliding)
        {
            if (moveActionRight.IsPressed() && MovingRight)
            {
                if (PlayerAnim.GetBool("Falling"))
                {
                    //PlayerRB.linearVelocityX = JumpSpeed;
                }
                else
                {
                    if (PlayerAnim.GetBool("Crouching"))
                    {

                    }
                    else
                    {
                        
                    PlayerRB.linearVelocityX = Speed;
                    }
                }
            }

            if (moveActionLeft.IsPressed() && MovingLeft)
            {
                if (PlayerAnim.GetBool("Falling"))
                {
                    //PlayerRB.linearVelocityX = -JumpSpeed;
                }
                else
                {
                    if (PlayerAnim.GetBool("Crouching"))
                    {

                    }
                    else
                    {
                        
                    PlayerRB.linearVelocityX = -Speed;
                    }
                   // Debug.Log("Moving left");

                }
            }
            /*RaycastHit2D hitDown = Physics2D.Raycast(transform.position, -Vector2.up, .5f, LayerMask.GetMask("Ground"));
            Debug.DrawRay(transform.position, -Vector2.up *.5f, Color.red);
            if (hitDown)
            {
                //if hitdown gameObject.tag 
                if (hitDown.collider.gameObject.name.Contains("Floor"))
                {
                    //Grounded = true;
                    //PlayerRB.linearVelocityY = 0;
                    PlayerRB.gravityScale = 0;
                }
                else 
                {
                    Grounded = false;
                    PlayerRB.gravityScale = 1;
                }
                Debug.Log(hitDown.collider.gameObject.name);
            }*/
            
            //RaycastHit2D hitup 
            //if (moveActionCrouch.IsPressed() && !MovingRight && !MovingLeft)
            //{
                //Debug.Log("I should be Crouching");
                
            //}
        }
    }

    void StartMovingRight()
    {
        if (ableToMove && canWalk)
        {
            MovingRight = true;
            MovingLeft = false;
            //if (gameObject.GetComponent<PlayerAttack>().attacking)
            //{
                //gameObject.GetComponent<PlayerAttack>().QueueRightTurn = true;
            //}
            //else
           // {
                cameraFollowObject.CallTurn();
                if (transform.rotation != Quaternion.Euler(0f, 0f, 0f))
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);   
               // transform.Rotate(new Vector3(0f, 180f, 0f));
                }
           // }
            if (Grounded && !PlayerAttack.instance.attacking)
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
        if (ableToMove && canWalk)
        {
            MovingLeft = true;
            MovingRight = false;
            //if (gameObject.GetComponent<PlayerAttack>().attacking)
            //{
                //gameObject.GetComponent<PlayerAttack>().QueueLeftTurn = true;
            //}
            //else
            //{
                cameraFollowObject.CallTurn();
                if (transform.rotation == Quaternion.Euler(0f, 0f, 0f))
                {
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);  
                //transform.Rotate(new Vector3(0f, 180f, 0f));
                }
            //}

            if (Grounded && !PlayerAttack.instance.attacking)
            {
                PlayerAnim.SetBool("Running", true);
                PlayerAnim.SetBool("Idle", false);
            }
        }
    }
    void StartCrouching()
    {
        if (ableToMove)
        {
            if (Grounded)
            {
                //ableToMove = false;
                //transform.GetChild(1).gameObject.SetActive(true);
            CrouchHB.SetActive(true);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
                //PlayerRB.enabled = false;
                //gameObject.GetComponent<Rigidbody2D>().enabled = false;
                //PlayerAnim.SetBool("Crouching", true);
                //PlayerAnim.SetBool("Idle", false);
                if (MovingLeft || MovingRight)
                {
                    PlayerAnim.SetBool("Running", false);
                    PlayerAnim.SetBool("Crouching", true);
                }
                else
                {
                    PlayerAnim.SetBool("Idle", false);
                    PlayerAnim.SetBool("Crouching", true);
                }
            
            MovingLeft = false;
            MovingRight = false;
                canWalk = false;
                PlayerRB.linearVelocityX = 0;
            }
            else if (gripping)
            {

                PlayerAnim.SetBool("Falling", false);
                gripping = false;
                PlayerRB.gravityScale = 1;
            }
            //transform.GetChild(1).gameObject.SetActive(true);
            //CrouchHB.SetActive(true);
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
            //PlayerRB.enabled = false;
            //gameObject.GetComponent<Rigidbody2D>().enabled = false;
                //PlayerAnim.SetBool("Crouching", true);
                //if (MovingLeft || MovingRight)
                //{
                  //  PlayerAnim.SetBool("Running", false);
                   // PlayerAnim.SetBool("Crouching", true);
                //}
            
            //MovingLeft = false;
            //MovingRight = false;
            //canWalk = false;
        }
    }
    void StartJump()
    {
        if (ableToMove)
        {
            if (Grounded)
            {
                PlayerRB.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
                PlayerAnim.SetTrigger("Jumping");
                //Grounded = false;
                //Jumping = true;
                PlayerRB.gravityScale = 1;
                JumpCount++;
            }
            else if (Unlockables[3] == true && JumpCount < 2)
            {
                PlayerRB.AddForce(transform.up * (JumpForce * 1.5f), ForceMode2D.Impulse);
                PlayerAnim.Play("jump", -1, 0f);
               // JumpCount++;
            }
            canWalk = false;
            //CrouchHB.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            /*make another else if once you do wall logic
            else if (Unlockables[2] == true))
            {
            
            PlayerRB.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
            PlayerAnim.SetBool("Jumping", true);
            
            CurrentlyJumping = true;
            }
            */
            //}
            //Camera.GetComponent<CameraFollow>().movingUp = true;
        }
        
        //CREATE COYOTE TIME and jump buffering
        //create a timer when youre falling, if the jump button is pressed
        //and the player touches the ground before the timer ends(5 frames), jump when getting on the ground

        //put this code in jump script
        //create a timer after leaving the ground (5 frames)
        // if (playervelocityY < 0  && (playervelocityX > || <0) && timer < ground leave timer)
        //Jump
       //fallFrames = 0;
        
    }
    void EndUpwardMomentum()
    {
        if (PlayerRB.linearVelocityY >=0.01f)
        {
            PlayerRB.linearVelocityY = 0;
        }
    }

    void StartSlide()
    {
        if (Grounded && !PlayerAttack.instance.attacking)
        {
            if (transform.localScale.x == 1)
            {
                PlayerRB.AddForce(transform.right * new Vector2(1, 0), ForceMode2D.Impulse);
            }
            else if (transform.localScale.x == -1)
            {
                PlayerRB.AddForce(transform.right * new Vector2(-1, 0), ForceMode2D.Impulse);
            }
            CrouchHB.SetActive(true);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            sliding = true;
            PlayerAnim.SetBool("Sliding", true);
            PlayerAnim.SetBool("Idle", false);
            ableToMove = false;
            canWalk = false;
            RaycastHit2D hitDown = Physics2D.Raycast(transform.position, -Vector2.up);
            RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up);
            //ableToMove = false;
       }
    }

    void EndSlide()
    {
        ableToMove = true;
        sliding = false;
        PlayerAnim.SetBool("Sliding", false);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        CrouchHB.SetActive(false);
        if (!MovingLeft && !MovingRight)
        {
            PlayerAnim.SetBool("Idle", true);
            PlayerRB.linearVelocityX = 0;
        }
        else if (MovingLeft || MovingRight)
        {
            PlayerAnim.SetBool("Running", true);
            //PlayerRB.linearVelocityX = Speed;
        }
       // slideFrames = 0;
        canWalk = true;
    }

    public void EndJump()
    {
        PlayerAnim.SetBool("Falling", false);
        //PlayerAnim.SetBool("Jumping", false);
        //PlayerAnim.SetTrigger("Jumping");

        if (!MovingLeft && !MovingRight)
        {
            PlayerAnim.SetBool("Idle", true);
        }
        else if (MovingLeft || MovingRight)
        {
            PlayerAnim.SetBool("Running", true);
        }
        //Camera.GetComponent<CameraFollow>().movingUp = false;
        //PlayerRB.linearVelocityX = 0;
       // fallFrames = 0;
        PlayerRB.linearVelocityY = 0;
        PlayerRB.gravityScale = 0;
        //CurrentlyJumping = false;
        Grounded = true;
        JumpCount = 0;
        canWalk = true;
        CrouchHB.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);

        //falling = false;
    }
    public void EndCrouch()
    {
        if (ableToMove)
        {

            PlayerAnim.SetBool("Crouching", false);
            if (MovingRight || MovingLeft)
            {
                PlayerAnim.SetBool("Running", true);
            }
            else if (!MovingRight && !MovingLeft)
            {
                PlayerAnim.SetBool("Idle", true);
            }
            //gameObject.GetComponent<Rigidbody2D>().enabled = true;
            //PlayerRB.enabled = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            CrouchHB.SetActive(false);
            //transform.GetChild(1).gameObject.SetActive(false);
            canWalk = true;
            //ableToMove = true;
        }
    }
    void ToggleCamera()
    {
        CanMoveCam = true;
    }
    void EndToggleCamera()
    {
        CanMoveCam = false;
    }
    public void Transition()
    {
       // Values.speed = Speed;
        //Values.JumpForce = JumpForce;
        //Values.JumpSpeed = JumpSpeed;
        //Values.JumpCount = JumpCount;
        Values.currentRotation = transform.localScale;
        //TransitionPanel.GetComponent<Animator>().SetTrigger("Start");
    }
    public void GrabLedge()
    {
        gripping= true;
    }
    
}
