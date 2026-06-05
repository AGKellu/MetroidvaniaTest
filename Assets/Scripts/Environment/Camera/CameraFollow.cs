using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    private GameObject Player;
    [SerializeField] private float followSpeed = 1f;
    public Vector3 offset;
    public bool shaking;
    public float smoothTime;
    public float strength;
    public float strengthVelocity;
    int i = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!shaking)
        {
            transform.position = Vector3.Lerp(transform.position, Player.transform.position + offset, followSpeed);
        }
        else if (shaking)
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
}
