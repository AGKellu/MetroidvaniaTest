using UnityEngine;

public class EnemySight : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.transform.parent.gameObject.GetComponent<EnemyAttack>().shooting = true;
        }
    }
}
