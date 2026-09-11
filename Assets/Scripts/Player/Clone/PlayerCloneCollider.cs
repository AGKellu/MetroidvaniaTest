using UnityEngine;

public class PlayerCloneCollider : MonoBehaviour
{
    BoxCollider2D boxColl;
    public bool dashing;
    //public GameObject Player;
    public Vector3 direction;
    public Vector3 RealDir;
    [SerializeField] private float maxCloneDistance;
    private Vector2 playerTransformPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransformPos = transform.position;
        Debug.Log(playerTransformPos);
        if (direction.x < 0)
        {
            RealDir = -transform.right;
        }
        else if (direction.x > 0)
        {
            RealDir = transform.right;
        }
        //RealDir
        boxColl = gameObject.GetComponent<BoxCollider2D>();
        //Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (dashing)
        {
            if (Mathf.Abs(Vector2.Distance(playerTransformPos, transform.position)) < maxCloneDistance)
            {
                if (direction.x < 0)
                {

                    transform.position -= RealDir * 1 / 100;
                }
                else if (direction.x > 0)
                {

                    transform.position += RealDir * 1 / 100;
                }
            }

        }
    }
    //void
    //check if trigger enter anything, stop moving (dashing = false) 
}
