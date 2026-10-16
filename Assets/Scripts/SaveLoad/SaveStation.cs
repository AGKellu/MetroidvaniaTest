using UnityEngine;

public class SaveStation : MonoBehaviour
{
    [SerializeField] private GameObject SaveCanvas;
    public Vector3 nowPosition;
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
            SaveCanvas.SetActive(true);
            PlayerMovement.instance.AvailableJumps = 0;
            PlayerMovement.instance.OnSaveStation = true;
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            SaveCanvas.SetActive(false);
            if (PlayerMovement.instance.Unlockables[2])
            {
                PlayerMovement.instance.AvailableJumps = 2;
            }
            else
            {

                PlayerMovement.instance.AvailableJumps = 1;
            }
            PlayerMovement.instance.OnSaveStation = false;
        }
    }
}
