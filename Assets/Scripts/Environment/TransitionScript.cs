using UnityEngine;

public class TransitionScript : MonoBehaviour
{

    public void Transition(GameObject player)
    {
        Debug.Log("Transition to next Scene");
        //abletomove = false, fade scene to black, load, send player in front of leave trigger, unload, unfade from black, abletomove = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Transition(other.gameObject);
        }
    }
}
