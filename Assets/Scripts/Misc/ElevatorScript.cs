using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {

            PlayerMovement.instance.OnElevator = true;

            PlayerMovement.instance.AvailableJumps = 0;
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            PlayerMovement.instance.OnElevator = false;

            if (PlayerMovement.instance.Unlockables[2])
            {
                PlayerMovement.instance.AvailableJumps = 2;
            }
            else
            {

                PlayerMovement.instance.AvailableJumps = 1;
            }

        }
    }
}
