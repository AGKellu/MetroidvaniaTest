using UnityEngine;

public class LedgeScript : MonoBehaviour
{
    public Vector2 climbPosition;
   void OnTriggerEnter2D(Collider2D coll)
   {
    if (coll.gameObject.name == "PlayerTop")
    {
            PlayerMovement.instance.GrabLedge();
            PlayerMovement.instance.climbPosition = climbPosition;
    }
   }
}
