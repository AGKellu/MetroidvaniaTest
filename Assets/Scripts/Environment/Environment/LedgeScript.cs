using UnityEngine;

public class LedgeScript : MonoBehaviour
{
   void OnTriggerEnter2D(Collider2D coll)
   {
    if (coll.gameObject.name == "PlayerTop")
    {
        PlayerMovement.instance.GrabLedge();
    }
   }
}
