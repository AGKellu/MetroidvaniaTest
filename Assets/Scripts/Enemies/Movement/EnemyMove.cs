using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private GameObject Player;
    private SpriteRenderer SR;
    public bool Crawler;
    public float speed, rotation;
    public Transform front, back, belly, wall;
    private Collider2D frontBump, backBump, bellyBump, wallBump;
   // [SerializeField] private Vector3[] positions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        SR = gameObject.GetComponent<SpriteRenderer>();
        //create a 
    }

    // Update is called once per frame
    void Update()
    {
       /* if (Player.transform.position.x < gameObject.transform.position.x)
        {
            //SR.flipX = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Player.transform.position.x > gameObject.transform.position.x)
        {
            //SR.flipX = false;
            transform.localScale = new Vector3(1, 1, 1);
        }*/
        if (Crawler)
        {
            frontBump = Physics2D.OverlapPoint(front.position, LayerMask.GetMask("Ground"));
            backBump = Physics2D.OverlapPoint(back.position, LayerMask.GetMask("Ground"));
            bellyBump = Physics2D.OverlapPoint(belly.position, LayerMask.GetMask("Ground"));
            wallBump = Physics2D.OverlapPoint(wall.position, LayerMask.GetMask("Ground"));
            if (frontBump == null && bellyBump == null && backBump == null && wallBump == null)
            {
               // Debug.Log("In the air");
                transform.Translate(new Vector3(speed, -1.5f * speed, 0f) * Time.deltaTime);
            }
            else
            {
                if (wallBump != null)
                {
                    transform.Rotate(0.0f, 0.0f, .8f * rotation, Space.Self);
                }
                else if (bellyBump == null && frontBump == null)
                {
                    transform.Rotate(0.0f, 0.0f, -1.5f * rotation, Space.Self);
                }
                else if (frontBump != null && bellyBump == null && backBump == null)
                {
                    transform.Rotate(0.0f, 0.0f, 1.5f * rotation, Space.Self);
                }

                if (bellyBump == null && wallBump == null)
                {
                    transform.Translate(new Vector3(speed, 0f, 0f) * Time.deltaTime);
                }
                else if (bellyBump == null && wallBump != null)
                {
                    transform.Translate(new Vector3(-.2f * speed, -.2f * speed, 0f) * Time.deltaTime);
                }
                else if (bellyBump != null && wallBump == null)
                {
                    transform.Translate(new Vector3(1.25f * speed, speed, 0f) * Time.deltaTime);
                }
            }

            /*RaycastHit2D hitDown = Physics2D.Raycast(new Vector3(transform.position.x + .2f, transform.position.y, transform.position.z), -Vector2.up, 0.35f, LayerMask.GetMask("Ground"));
            if (hitDown)
            {
                if (hitDown.collider.gameObject.name.Contains("Floor"))
                {

                }
            }
            RaycastHit2D hitDown2 = Physics2D.Raycast(new Vector3(transform.position.x - .2f, transform.position.y, transform.position.z), -Vector2.up, .35f, LayerMask.GetMask("Ground"));
            if (hitDown2)
            {
                if (hitDown2.collider.gameObject.name.Contains("Floor"))
                {

                }
            }
            if (!hitDown && hitDown2)
            {

            }*/
        }

        //transform.position = MOveT
        //
    }

}
