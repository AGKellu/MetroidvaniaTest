using UnityEngine;

public class GroundScript : MonoBehaviour
{
    //private BoxCollider2D GroundCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
//
        if (collider.gameObject.CompareTag("Player"))
        {
            //PlayerMovement.instance.EndJump();
            //collider.GetComponent<Rigidbody2D>().gravityScale = 0;
            //Debug.Log("Player is grounded");
        }
    }
   // void OnTriggerExit2D(Collider2D collider)
   // {
       // if (collider.gameObject.CompareTag("Player"))
       // {
        //    collider.gameObject.GetComponent<PlayerMovement>().Grounded = false;
      //  }
    //}
}
