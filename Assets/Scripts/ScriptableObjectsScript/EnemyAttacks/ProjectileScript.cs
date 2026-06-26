using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public GameObject BelongsTo;
    public bool OpensFire;
    public bool OpensIce;
    void Start()
    {
        Destroy(gameObject, 5);
    }
    
    void OnTriggerEnter2D(Collider2D attackHitbox)
    {
        //GameObject Player = GameObject.FindGameObjectWithTag("Player");
        /*if (attackHitbox.gameObject.CompareTag("Player") && !BelongsTo == attackHitbox.gameObject)
        {
            attackHitbox.gameObject.GetComponent<PlayerAttack>().TakeDamage(BelongsTo.GetComponent<EnemyAttack>().EnemyCurrentAttack.Damage);
            Destroy(gameObject, 0);
        }*/
        if (attackHitbox.gameObject.CompareTag("Player") && !BelongsTo.CompareTag("Player"))
        {
            attackHitbox.gameObject.GetComponent<PlayerAttack>().TakeDamage(BelongsTo.GetComponent<EnemyAttack>().EnemyCurrentAttack.Damage);
            Destroy(gameObject, 0);
        }
        else if (attackHitbox.gameObject.CompareTag("Hurtbox") && !BelongsTo.CompareTag("Enemy"))
        {
            attackHitbox.gameObject.transform.parent.gameObject.GetComponent<EnemyAttack>().TakeDamage(BelongsTo.GetComponent<PlayerAttack>().currentAttack.Damage, BelongsTo.GetComponent<PlayerAttack>().currentAttack.KnockBack, BelongsTo.GetComponent<PlayerAttack>().currentAttack.HitDirection);
        }
        else if (attackHitbox.gameObject.CompareTag("Door") && BelongsTo.CompareTag("Player"))
        {
            
            attackHitbox.gameObject.GetComponent<DoorScript>().OpenDoor(OpensFire, OpensIce);
            Destroy(gameObject, 0);
        }
    }
}
