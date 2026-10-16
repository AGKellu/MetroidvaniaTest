using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        //No idea what this is for;
        //PlayerAttack.instance;
        if (collider.gameObject.CompareTag("Hurtbox"))
        {
            //Debug.Log("Please");
            //Debug.Log(collider.transform.parent.gameObject.name);
            collider.transform.parent.gameObject.GetComponent<EnemyAttack>().TakeShatterDamage();
        }
        if (collider.gameObject.CompareTag("Door"))
        {
            if (collider.gameObject.GetComponent<DoorScript>().NormalDoor)
            {
                //Debug.Log(collider.gameObject.name);
                collider.gameObject.GetComponent<DoorScript>().OpenDoor(false, false, true);
            }
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
