using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//using System.ComponentModel;
//using System.ComponentModel;
public class TransitionScript : MonoBehaviour
{
    public enum DoorToSpawnAt
    {
        None,
        One,
        Two,
        Three,
        Four,
    }
    [Header("Spawn TO")]
    [SerializeField] private DoorToSpawnAt DoorToSpawnTo;
    [SerializeField] private SceneField _sceneToLoad;
    [Space(10)]
    [Header("This Door")]
    public DoorToSpawnAt CurrentDoorPosition;
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
            //GameObject Cam = GameObject.FindGameObjectWithTag("MainCamera");
            //if (!Cam.GetComponent<CameraFollow>().changing)
            //{
            other.gameObject.GetComponent<PlayerAttack>().Transition();
            other.gameObject.GetComponent<PlayerMovement>().Transition();
            StartCoroutine(Transition());
            //Debug.Log("This should only trigger once");
        }
        //}

    }
    IEnumerator Transition()
    {
        //SceneManager.LoadScene(RoomToLoad);
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(RoomToLoad, LoadSceneMode.Additive);
        while (!loadScene.isDone)
        {
            yield return null;
        }
        //GameObject[] Spawners = GameObject.FindGameObjectsWithTag("Spawner");
        //foreach(GameObject Spawner in Spawners)
        //{
          //  Debug.Log(Spawner.scene);
        //}
        
        //Debug.Log(SceneManager.GetActiveScene().name);
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(RoomToLoad));
        while (!unloadScene.isDone)
        {
            yield return null;
        }
    }

}
