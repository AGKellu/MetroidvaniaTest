using UnityEngine;
using System.Collections;
public class DoorScript : MonoBehaviour
{
    //public bool hit;
    public bool NormalDoor;
    public bool FireDoor;
    public bool IceBool;
    public void OpenDoor()
    {
        gameObject.GetComponent<Animator>().SetTrigger("Open");
        //hit = true;
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        
    }

    public void Close()
    {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        //hit = false;
        gameObject.GetComponent<Animator>().SetTrigger("Close");
    }
}
