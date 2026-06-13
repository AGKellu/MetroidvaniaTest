using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SpawnerScript : MonoBehaviour
{
    public GameObject[] GOsInScene;
    public Vector3[] GOPositions;
    public PlayerSOScript Vals;
    public GameObject LeftDoor;
    public GameObject LeftTrans;
    public GameObject RightDoor;
    public GameObject RightTrans;
    //public GameObject Player;
    //public GameObject EyeBat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Spawn();
        //SpawnPlayerOneTime();
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            Debug.Log("There is another pplayer jere\nYOu should never see this!");
        }
        Spawn();
    }
    public void Spawn()
    {
        /*int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];
        for (int i= 0; i<countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
            if (loadedScenes[i] !=gameObject.scene)
        {
            Debug.Log("Unload " + loadedScenes[i].name);
        }
        }*/
        //GetArray of scenes, if gameObject doesnt belong to scene x, unload scene x
        
        for (int i = 0; i< GOsInScene.Length; i++)
        {
            GameObject GO = Instantiate(GOsInScene[i], GOPositions[i], Quaternion.identity);
            if (GO.scene != gameObject.scene)
            {
                SceneManager.MoveGameObjectToScene(GO, gameObject.scene);
            }
            if (GO.CompareTag("Player"))
            {
                
                GO.GetComponent<PlayerMovement>().ableToMove = true;
               // GO.GetComponent<PlayerAttack>().Camera = GameObject.FindGameObjectWithTag("MainCamera");
                GO.GetComponent<PlayerAttack>().ManaContainer = GameObject.FindGameObjectWithTag("Container").GetComponent<Image>();
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().Player = GO;
                GameObject[] Cameras = GameObject.FindGameObjectsWithTag("MainCamera");
                foreach(GameObject cam in Cameras)
                {
                    Debug.Log(cam.scene);
                }
                GO.GetComponent<PlayerAttack>().Camera = GameObject.FindGameObjectWithTag("MainCamera");
                GO.transform.localScale = Vals.currentRotation;
                
                if (GO.transform.localScale == new Vector3(-1, 1, 1))
                {
                    GO.transform.position = LeftTrans.GetComponent<TransitionScript>().nowPosition;
                    LeftDoor.GetComponent<DoorScript>().Close();
                }
                else if (GO.transform.localScale == new Vector3(1, 1, 1))
                {
                    GO.transform.position = RightTrans.GetComponent<TransitionScript>().nowPosition;
                    RightDoor.GetComponent<DoorScript>().Close();
                }
        

            }
        }
    }
    public void SpawnPlayerOneTime()
    {
        for (int i = 0; i < GOsInScene.Length; i++)
        {
            GameObject GO = Instantiate(GOsInScene[i], GOPositions[i], Quaternion.identity);
             GO.GetComponent<PlayerMovement>().ableToMove = true;
                GO.GetComponent<PlayerAttack>().Camera = GameObject.FindGameObjectWithTag("MainCamera");
                GO.GetComponent<PlayerAttack>().ManaContainer = GameObject.FindGameObjectWithTag("Container").GetComponent<Image>();
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().Player = GO;

        }
        //Destroy(gameObject, 0);
    }
}
