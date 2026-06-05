using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Movement Actions")]
    private InputAction moveActionRight;
    private InputAction moveActionLeft;
    private InputAction Jump;

    

    [Header ("Player Components")]
    private Animator PlayerAnim;
    private Rigidbody2D PlayerRB;
    private SpriteRenderer PlayerSprite;
 //   public InputAction MoveRight;

    [Header ("Player Attributes")]
    public float Speed;
    public float JumpForce;
    public float JumpSpeed;
    public bool MovingLeft;
    public bool MovingRight;
    public  bool CurrentlyJumping = false;
    public bool Grounded = true;
    public  bool ableToMove = true;
    //private bool busy;

    [Header ("Misc")]
    private int trueFrames;
    private int fallFrames = 0;
    //private int JumpStartFrames = 0;
    //private int JumpEndFrames = 0;
    //private bool jumpStarted = false;
    
    
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
        PlayerRB = gameObject.GetComponent<Rigidbody2D>();
        PlayerAnim = gameObject.GetComponent<Animator>();
        PlayerSprite = gameObject.GetComponent<SpriteRenderer>();
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
        
        if (PlayerRB.linearVelocityY <0 && !gameObject.GetComponent<PlayerAttack>().attacking && !Grounded)
        {

            PlayerAnim.SetBool("Falling", true);
        }
       
        if (moveActionRight.IsPressed() && !gameObject.GetComponent<PlayerAttack>().attacking && MovingRight)
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
        
        if (moveActionLeft.IsPressed() && !gameObject.GetComponent<PlayerAttack>().attacking && MovingLeft)
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

    void StartMovingRight()
    {
        if (ableToMove)
        {
        MovingRight = true;
        MovingLeft= false;
       
        transform.localScale = new Vector3(1, 1, 1);
        
        if (Grounded && !gameObject.GetComponent<PlayerAttack>().attacking)
        {

        PlayerAnim.SetBool("Running", true);
        PlayerAnim.SetBool("Idle", false);
        }
        }
        
      
    }
    void StartMovingLeft()
    {
        if (ableToMove)
        {
        MovingLeft = true;
        MovingRight = false;
        transform.localScale = new Vector3(-1, 1, 1);
        
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
            if (Grounded && !gameObject.GetComponent<PlayerAttack>().attacking)
        {
           
            PlayerRB.AddForce(transform.up * JumpForce, ForceMode2D.Impulse);
            PlayerAnim.SetBool("Jumping", true);
            Grounded = false;
            CurrentlyJumping = true;
            

        }
        }
        
    }
    
    
    public void EndJump()
    {
        //if (PlayerAnim.GetBool("Falling"))
        //{
        PlayerAnim.SetBool("Falling", false);
        PlayerAnim.SetBool("Jumping", false);
        //}
        if (!MovingLeft && !MovingRight)
        {
            PlayerAnim.SetBool("Idle", true);
        }
        else if (MovingLeft || MovingRight)
        {
            PlayerAnim.SetBool("Running", true);
        }
        //if (CurrentlyJumping)
        //{
            
        PlayerRB.linearVelocityX = 0;
        //PlayerRB.linearVelocityY = 0;
        //}
        //PlayerAnim.SetBool("Idle", true);
        CurrentlyJumping = false;
        Grounded = true;
        //busy = false;
    }
    
    
}
