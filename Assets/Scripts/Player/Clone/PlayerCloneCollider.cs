using UnityEngine;

public class PlayerCloneCollider : MonoBehaviour
{
    BoxCollider2D boxColl;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxColl = gameObject.GetComponent<BoxCollider2D>();
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
