using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerAttack playerAttack  = gameObject.transform.parent.gameObject.GetComponent<PlayerAttack>();
        if (collider.gameObject.CompareTag("Hurtbox"))
        {
            if (playerAttack.Mana < playerAttack.ManaMax)
            {
                
            playerAttack.Mana += playerAttack.currentAttack.ManaGain;
            playerAttack.ManaContainer.fillAmount = playerAttack.Mana/100;
            }
            /*if (playervelocityY < 0)
            {
              PlayerRB.AddForce(transform.up * 3, ForceMode2D.Impulse)  
            }
            */
            
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
    }
}
