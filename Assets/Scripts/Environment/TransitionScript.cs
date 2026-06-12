using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;
//using System.ComponentModel;
//using System.ComponentModel;
public class TransitionScript : MonoBehaviour
{
    [SerializeField] private string RoomToLoad;
    //[SerializeField] private GameObject RealPlayer;
    //[SerializeField] private PlayerSOScript Values;
    public Vector3 nowPosition;
    //private bool changing = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        //if (!changing)
        //{
            if (other.gameObject.CompareTag("Player"))
        {
            GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
            if (!Cam.GetComponent<CameraFollow>().changing)
            {
                other.gameObject.GetComponent<PlayerAttack>().Transition();
                other.gameObject.GetComponent<PlayerMovement>().Transition();
                Destroy(other.gameObject, 0);
            StartCoroutine(Cam.GetComponent<CameraFollow>().Transition(RoomToLoad));
            }
        }
        //}
        
    }
}
