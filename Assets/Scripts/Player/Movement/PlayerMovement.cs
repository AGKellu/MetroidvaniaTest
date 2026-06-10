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

    [SerializeField] private PlayerSOScript Values; 
    /*
    Unlockables are:
    Slide
    Dash
    Wall Jump
    Double Jump
    */

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
        //Jump.canceled += ctx => EndJump();
        Slide = InputSystem.actions.FindAction("Move/Slide");
        Slide.performed += ctx => StartSlide();
        PlayerRB = gameObject.GetComponent<Rigidbody2D>();
        PlayerAnim = gameObject.GetComponent<Animator>();
        PlayerSprite = gameObject.GetComponent<SpriteRenderer>();
        Speed = Values.speed;
        JumpForce = Values.JumpForce;
        JumpSpeed = Values.JumpSpeed;
        JumpCount = Values.JumpCount;
        transform.localScale = Values.currentRotation;
        //moveActionRight.canceled += ctx => StopCurrentMoveInput();
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
            gameObject.GetComponent<PlayerAttack>().Camera.GetComponent<CameraFollow>().sliding  = true;
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
        if (PlayerRB.linearVelocityY < 0)
        {
            fallFrames++;
            // = true;
            //Debug.Log(fallFrames);
            if (fallFrames >= 15 && !Grounded && !gameObject.GetComponent<PlayerAttack>().attacking)
            {
                PlayerAnim.SetBool("Falling", true);
            }
            gameObject.GetComponent<PlayerAttack>().Camera.GetComponent<CameraFollow>().movingUp = false;
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
            if (gameObject.GetComponent<PlayerAttack>().attacking)
            {
                gameObject.GetComponent<PlayerAttack>().QueueRightTurn = true;
            }
            else
            {
                //gameObject.GetComponent<PlayerAttack>().Camera.transform.localScale = new Vector3(Mathf.Lerp(-1, 1, 30f), 1, 1);
                //gameObject.GetComponent<PlayerAttack>().Camera.transform.localScale = new Vector3();
                if (gameObject.GetComponent<PlayerAttack>().Camera.transform.localScale.x == transform.localScale.x)
                {

                }
                else
                {
                    
                gameObject.GetComponent<PlayerAttack>().Camera.GetComponent<CameraFollow>().Switch();
                }
                gameObject
                    .GetComponent<PlayerAttack>().Camera.GetComponent<CameraFollow>().offset.x = .2f;
                transform.localScale = new Vector3(1, 1, 1);
            }

            // Vector3 startLocalScale = gameObject.GetComponent<PlayerAttack>().Camera.transform.localScale;
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

                gameObject
                    .GetComponent<PlayerAttack>()
                    .Camera.GetComponent<CameraFollow>()
                    .offset.x = -.1f;
                    if (gameObject.GetComponent<PlayerAttack>().Camera.transform.localScale.x == transform.localScale.x)
                 {
                    
                }
                else
                {
                    gameObject
                        .GetComponent<PlayerAttack>()
                        .Camera.GetComponent<CameraFollow>()
                        .Switch();
                }
                transform.localScale = new Vector3(-1, 1, 1);
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
            gameObject.GetComponent<PlayerAttack>().Camera.GetComponent<CameraFollow>().movingUp = true;
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
    }
}
