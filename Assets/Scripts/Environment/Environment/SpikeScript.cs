//using System.Numerics;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    [SerializeField] private Vector3 SafePosition;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerAttack>().TakeDamage(1);
            TPToSafety(other.gameObject);
        }
    }
    void TPToSafety(GameObject player)
    {
        player.transform.position = SafePosition;
    }
}
