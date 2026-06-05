using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;
public class PlayerAttack : MonoBehaviour
{
    [Header ("Attack Actions")]    
    private InputAction NormalAttack;

    [Header("PlayerComponents")]
    private Animator PlayerAnim;
    public  ScriptableObjectScript currentAttack;

    [Header ("PlayerAttributes")]
    [SerializeField] private float timeBetweenAttack = 0;
    private float timeSinceAttack;
    public  bool attacking = false;
    public int Health;
    public int Mana;
    private float InvulFrames = 0;
    private bool invuln= false;
    private bool ableToAttack = true;

    [Header("Misc")]
    
    [SerializeField]private Material trueMaterial;
    [SerializeField]private Material flashMaterial;
    [SerializeField] private Image ManaContainer;
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject[] HealthMasks;
    private int MaskInt = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerAnim = gameObject.GetComponent<Animator>();
        NormalAttack = InputSystem.actions.FindAction("Attacks/NormalAttack");
        NormalAttack.performed += ctx => Attack();
    }

    public void TakeDamage(int AttackDamage)
    {
        if (!invuln)
        {
            gameObject.GetComponent<PlayerMovement>().ableToMove = false;
            ableToAttack = false;
            Camera.GetComponent<CameraFollow>().shaking = true;
            Camera.GetComponent<CameraFollow>().Shake();
            HealthMasks[MaskInt].GetComponent<Animator>().SetTrigger("Broken");
            MaskInt++;
            //Camera.GetComponent<CameraFollow>().Shake(60, 100);
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
            
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 0;
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }
    // Update is called once per frame
    void Update()
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

    }
    void Attack()
    {
        if (!attacking && ableToAttack)
        {
            //busy = true;
            attacking = true;
            PlayerAnim.SetBool("Attacking", true);
            PlayerAnim.SetBool("Idle", false);
            //Debug.Log("Starting Attack");
          //when you shoot, set manacontainer.fillamount to Mana/10;
          //create a int of "Mana Gain" for when you hit an enemy
        }
    }
    void EndAttack()
    {
        timeSinceAttack = 0;
            //Debug.Log("Attack should be done");
            PlayerAnim.SetBool("Attacking", false);
            if (!gameObject.GetComponent<PlayerMovement>().MovingLeft && !gameObject.GetComponent<PlayerMovement>().MovingRight)
            {
                PlayerAnim.SetBool("Idle", true);
            }
            else 
            {
                PlayerAnim.SetBool("Running", true);
            }
            //look at other xcript and check if moving and do idle

            //busy = false;
            attacking = false;
    }
    void Fireball()
    {

    }
    void EndInvuln()
    {
        InvulFrames = 0;
        invuln = false;
        gameObject.GetComponent<PlayerMovement>().ableToMove = true;
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
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        //PlayerAnim.SetBool("Damaged", false);
    }
}
