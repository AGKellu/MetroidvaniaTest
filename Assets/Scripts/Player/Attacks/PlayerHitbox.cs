using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerAttack playerAttack  = gameObject.transform.parent.gameObject.GetComponent<PlayerAttack>();
        if (collider.gameObject.CompareTag("Hurtbox"))
        {
            //Debug.Log("Please");
            //Debug.Log(collider.transform.parent.gameObject.name);
            collider.transform.parent.gameObject.GetComponent<EnemyAttack>().TakeShatterDamage();
        }
        /*else if (collider.gameObject.CompareTag("Door"))
        {
            if (collider.gameObject.GetComponent<DoorScript>().NormalDoor)
            {

            collider.gameObject.GetComponent<DoorScript>().OpenDoor();
            }
            
        }*/
    }
}
