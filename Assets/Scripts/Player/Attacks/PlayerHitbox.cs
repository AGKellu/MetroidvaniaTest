using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerAttack playerAttack  = gameObject.transform.parent.gameObject.GetComponent<PlayerAttack>();
        if (collider.gameObject.CompareTag("Hurtbox"))
        {
            //Debug.Log("Please");
            if (playerAttack.Mana < playerAttack.ManaMax)
            {
                
            playerAttack.Mana += playerAttack.currentAttack.ManaGain;
            playerAttack.ManaContainer.fillAmount = playerAttack.Mana/100;
            }
            //collider.gameObject.transform.parent.gameObject.GetComponent<EnemyAttack>().TakeDamage()
            /*if (playervelocityY < 0)
            {
              PlayerRB.AddForce(transform.up * 3, ForceMode2D.Impulse)  
            }
            */


            //Debug.Log(collider.transform.parent.gameObject.name);
            collider.transform.parent.gameObject.GetComponent<EnemyAttack>().TakeDamage(playerAttack.currentAttack.Damage, playerAttack.currentAttack.KnockBack, playerAttack.currentAttack.HitDirection);
        }
        else if (collider.gameObject.CompareTag("Terrain"))
        {
            //Below is Pogo logic (Hopefully)
            /*if (playervelocityY < 0)
            {
              PlayerRB.AddForce(transform.up * 3, ForceMode2D.Impulse)  
            }
            */
        }
        else if (collider.gameObject.CompareTag("Door"))
        {
            if (collider.gameObject.GetComponent<DoorScript>().NormalDoor)
            {

            collider.gameObject.GetComponent<DoorScript>().OpenDoor();
            }
            
        }
    }
}
