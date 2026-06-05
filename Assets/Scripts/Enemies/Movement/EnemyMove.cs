using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private GameObject Player;
    private SpriteRenderer SR;
    //private Rigidbody2D RB2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        SR = gameObject.GetComponent<SpriteRenderer>();
        //RB2D = gameObject.GetComponent<Rigidbody2D>();
        //RB2D.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.transform.position.x < gameObject.transform.position.x)
        {
            //SR.flipX = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Player.transform.position.x > gameObject.transform.position.x)
        {
            //SR.flipX = false;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
