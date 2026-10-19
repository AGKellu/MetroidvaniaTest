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
        Five,
    }

    [Header("Spawn TO")]
    [SerializeField] private DoorToSpawnAt DoorToSpawnTo;
    [SerializeField] private SceneField _sceneToLoad;
   // [SerializeField] private bool InRoomTrans;
    //public bool BigRoomLoad;
    [Space(10)]
    [Header("This Door")]
    public DoorToSpawnAt CurrentDoorPosition;
    //[SerializeField] private string RoomToLoad;
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
            //StartCoroutine(Transition());
            //if (InRoomTrans)
           // {
                //RoomTrans();
          // }
           // else
            //{
                
            CoolTrans();
           // }
        }
        //}

    }
    /*IEnumerator Transition()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(RoomToLoad, LoadSceneMode.Additive);
        while (!loadScene.isDone)
        {
            yield return null;
        }
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(RoomToLoad));
        while (!unloadScene.isDone)
        {
            yield return null;
        }
        //THIS WORKS!!!
        //if the new stuff doesnt work (below code), COME BACK to this
    }*/
    public void CoolTrans()
    {
        SceneSwapManager.SwapSceneFromDoorUse(_sceneToLoad, DoorToSpawnTo);
    }
    //public void RoomTrans()
    //{
      //  gameObject.GetComponent<InRoomSceneTransition>().SwapSceneFromDoorUse();
   // }
}
