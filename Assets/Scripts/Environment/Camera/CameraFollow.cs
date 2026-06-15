//using System.Numerics;
using UnityEngine;
using System.Collections;
//using System.Numerics;
using UnityEngine.SceneManagement;
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
    public bool movingDown;
    public bool sliding;
    public bool changing;
    [SerializeField] private GameObject Spawner;
    //[SerializeField] private GameObject TransitionPanel;
    int i = 0;
    public Vector3 LeftCamPos;
    public Vector3 RightCamPos;
    private string CurrentScene;
    //public bool FromLeft;
    //public bool FromRight;
    //public bool LeftDoor;
    //public bool RightDoor;
    //public bool UpDoor;
    //public bool DownDoor;
    //[SerializeField] private float minCamera;
    //[SerializeField] private float maxCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Player = GameObject.FindWithTag("Player");
        //if (FromLeft)
        //{

        //}
        //else if (FromRight)
        //{

        //}
        /*
        if (Player.transform.localScale == new Vector3(-1, 1, 1))
        {
            Debug.Log("Coming from right");
        }
        else 
        {
            Debug.Log("Coming from left");
        }
        */
        Player = GameObject.Find("Player");
        Debug.Log(Player.name);
        Player.GetComponent<PlayerAttack>().Camera = gameObject;
        Player.GetComponent<PlayerMovement>().Camera = gameObject;
        Spawner.GetComponent<SpawnerScript>().PlayerCheck(Player);
        //Player.instance.gameObject.GetComponent<PlayerAttack>().Camera = gameObject; 
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
        //shaking  = true;

        //strength = Mathf.Max(strength, value);
    }
   // public void Switch()
    //{
      //  transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        //transform.localScale.x = transform.localScale.x * -1;
    //}
    /*public IEnumerator Transition(string RoomToLoad, GameObject PlayerCharacter)
    {
        changing = true;
        Debug.Log("Transition to next Scene");
        string ActiveScene = SceneManager.GetActiveScene().name;
        Debug.Log(ActiveScene);
        yield return null;
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(RoomToLoad, LoadSceneMode.Additive);
        while (!loadScene.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(RoomToLoad));
        GameObject UI = GameObject.FindGameObjectWithTag("Canvas");
        SceneManager.MoveGameObjectToScene(PlayerCharacter, SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(UI, SceneManager.GetActiveScene());

        GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnerScript>().PlayerCheck();
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync(ActiveScene);
        while (!unloadScene.isDone)
        {
            yield return null;
        }
        Debug.Log(RoomToLoad + " has been loaded!\nSend player in front of leave trigger depending on what direction theyre looking");

    }*/
}
