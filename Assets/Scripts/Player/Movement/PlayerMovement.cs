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
    private InputAction Jump;
    private InputAction Slide;

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

    [SerializeField]
    private int JumpCount;
    public bool sliding = false;
    private int slideFrames = 0;

    //public bool slideUnlocked = false;
    //private bool busy;

    [Header("Misc")]
    private int trueFrames;
    private int fallFrames = 0;
    //private bool falling;

    [SerializeField]
    private bool[] Unlockables;
    public static PlayerMovement instance;
    private GameObject Spawner;

    [SerializeField] private PlayerSOScript Values;
    //public GameObject Camera;

    [Header("Camera Stuff")]
    private CameraFollowObject cameraFollowObject;
    [SerializeField] private GameObject cameraFollowGO;
    private float fallSpeedYDampingChangeThreshold;
    //[SerializeField] private GameObject TransitionPanel;
    /*
    Unlockables are:
    Slide
    Dash
    Wall Jump
    Double Jump
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
        
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
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
        Speed = Values.speed;
        JumpForce = Values.JumpForce;
        JumpSpeed = Values.JumpSpeed;
        JumpCount = Values.JumpCount;
        cameraFollowObject = cameraFollowGO.GetComponent<CameraFollowObject>();
        //GameObject Spawner()
        // transform.localScale = Values.currentRotation;
        CameraManager.instance.enabled = true;
        fallSpeedYDampingChangeThreshold = CameraManager.instance.fallSpeedYDampingChangeThreshold;
        
    }

    //void
    // Update is called once per frame
    void Update()
    {
        if (ableToMove)
        {
            CheckForMovement();
        }
        if (sliding)
        {
           // Camera.GetComponent<CameraFollow>().sliding  = true;
            slideFrames++;
            ableToMove = false;
            if (slideFrames == 30)
            {
                EndSlide();
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

    void CheckForMovement()
    {
        if (PlayerRB.linearVelocityY < fallSpeedYDampingChangeThreshold && !CameraManager.instance.isLerpingYDamping && !CameraManager.instance.lerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
            fallFrames++;
            if (fallFrames >= 15 && !Grounded && !gameObject.GetComponent<PlayerAttack>().attacking)
            {
                PlayerAnim.SetBool("Falling", true);
            }
        }
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
            if (gameObject.GetComponent<PlayerAttack>().attacking)
            {
                gameObject.GetComponent<PlayerAttack>().QueueRightTurn = true;
            }
            else
            {
                cameraFollowObject.CallTurn();
                if (transform.rotation != Quaternion.Euler(0f, 0f, 0f))
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);   
               // transform.Rotate(new Vector3(0f, 180f, 0f));
                }
            }
            if (Grounded && !gameObject.GetComponent<PlayerAttack>().attacking)
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
            if (gameObject.GetComponent<PlayerAttack>().attacking)
            {
                gameObject.GetComponent<PlayerAttack>().QueueLeftTurn = true;
            }
            else
            {
                cameraFollowObject.CallTurn();
                if (transform.rotation == Quaternion.Euler(0f, 0f, 0f))
                {
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);  
                //transform.Rotate(new Vector3(0f, 180f, 0f));
                }
            }

            if (Grounded && !gameObject.GetComponent<PlayerAttack>().attacking)
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
            if (Grounded)
            {
                PlayerRB.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
                PlayerAnim.SetBool("Jumping", true);
                JumpCount++;
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
            //Camera.GetComponent<CameraFollow>().movingUp = true;
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
        if (Grounded && !gameObject.GetComponent<PlayerAttack>().attacking)
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
        //Camera.GetComponent<CameraFollow>().movingUp = false;
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
        Values.currentRotation = transform.localScale;
        //TransitionPanel.GetComponent<Animator>().SetTrigger("Start");
    }
    
}
