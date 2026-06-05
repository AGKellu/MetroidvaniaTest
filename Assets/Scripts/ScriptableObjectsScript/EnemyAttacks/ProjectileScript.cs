using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public  GameObject BelongsTo;

    
    void OnTriggerEnter2D(Collider2D attackHitbox)
    {
        /*if (attackHitbox.gameObject.CompareTag("Player") && !BelongsTo == attackHitbox.gameObject)
        {
            attackHitbox.gameObject.GetComponent<PlayerAttack>().TakeDamage(BelongsTo.GetComponent<EnemyAttack>().EnemyCurrentAttack.Damage);
            Destroy(gameObject, 0);
        }*/
        if (attackHitbox.gameObject.CompareTag("Player"))
        {
            attackHitbox.gameObject.GetComponent<PlayerAttack>().TakeDamage(BelongsTo.GetComponent<EnemyAttack>().EnemyCurrentAttack.Damage);
            Destroy(gameObject, 0);
        }
    }
}
