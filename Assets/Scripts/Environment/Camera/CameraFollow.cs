//using System.Numerics;
using UnityEngine;
//using System.Collections;
//using System.Numerics;

public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    public float followSpeed = 1f;
    public Vector3 offset;
    public bool shaking;
    public float smoothTime;
    public float strength;
    public float strengthVelocity;
    public bool movingUp;
    public bool sliding;
    [SerializeField] private GameObject TransitionPanel;
    int i = 0;
    //public bool LeftDoor;
    //public bool RightDoor;
    //public bool UpDoor;
    //public bool DownDoor;
    //[SerializeField] private float minCamera;
    //[SerializeField] private float maxCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Player.GetComponent<PlayerMovement>().ableToMove = true;
        Player.GetComponent<PlayerAttack>().Camera = gameObject;
        TransitionPanel.GetComponent<Animator>().SetTrigger("End");
    }

    // Update is called once per frame
    void Update()
    {
        if (!shaking)
        {
            //mathf.clamp (min, max), make min a little further than the transition
            if (sliding || movingUp)
            {
                transform.position = Vector3.Lerp(transform.position, Player.transform.position + offset, followSpeed / 2);
            }
            else
            {
                
            transform.position = Vector3.Lerp(transform.position, Player.transform.position + offset, followSpeed);
            }
        }
        if (shaking)
        {
            //shaking = false;
            float randomX = Random.value - 0.5f;
            float randomY = Random.value - 0.5f;
            float randomZ = Random.value - 0.5f;

            transform.localEulerAngles = new Vector3(randomX, randomY, randomZ) * strength;
            i++;
            if (i == 30)
            {
                shaking = false;
                i = 0;
            }
            
        }

        
    }

    public void Shake()
    {
        // distance dynamite to camera
        //float distance = Vector3.Distance(explosionLocation, transform.position);
        // map the distance to a [0-1] value based on chosen maximum distance
        //float maxAffectDistance = 10f;
        //float value = Mathf.Clamp01((maxAffectDistance - distance) / maxAffectDistance);
        // set the camera shake strength to this value,
        // or keep it at current value if it is already higher
        strength = Mathf.SmoothDamp(strength, 0f, ref strengthVelocity, smoothTime);


        //strength = Mathf.Max(strength, value);
    }
    public void Switch()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        //transform.localScale.x = transform.localScale.x * -1;
    }
}
