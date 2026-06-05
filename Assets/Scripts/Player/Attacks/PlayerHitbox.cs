using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerAttack playerAttack  = gameObject.transform.parent.gameObject.GetComponent<PlayerAttack>();
        if (collider.gameObject.CompareTag("Hurtbox"))
        {
            collider.transform.parent.gameObject.GetComponent<EnemyAttack>().TakeDamage(playerAttack.currentAttack.Damage, playerAttack.currentAttack.KnockBack, playerAttack.currentAttack.HitDirection);
        }
    }
}
