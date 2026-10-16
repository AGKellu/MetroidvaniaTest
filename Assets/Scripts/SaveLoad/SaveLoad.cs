using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
public class SaveLoad : MonoBehaviour
{
    public bool SavedInRoom;
    //[SerializeField] private GameObject SaveCanvas;
    public bool ableToAttack;
    public int SavedHealth, SavedMaxHealth;
    public float SavedAmmo;
    public string SavedRoomName;
    //public double TimeSinceLoad;
    public bool[] AttackUnlockables, MovementUnlockables;
    //public Vector3 nowPosition;
    private string saveSeparator = "#SAVE-VALUE#";
    public static SaveLoad instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Save()
    {
        
        AttackUnlockables = PlayerAttack.instance.Unlockables;
        MovementUnlockables = PlayerMovement.instance.Unlockables;
        SavedHealth = PlayerAttack.instance.Health;
        SavedAmmo = PlayerAttack.instance.ammo;
        SavedMaxHealth = PlayerAttack.instance.maxHealth;
        //PlayerMovement.instance.Values.lastRoomSaved = SceneManager.GetActiveScene().name;
        //Debug.Log(PlayerMovement.instance.Values.lastRoomSaved);
        //Disable inputmap UI, Attack, Movement
        //while the game is running,and not at a menu/paused, add time to the in game timer
        //when saving, add that value to the save slot's file
        //when at the main menu, show that value on the save slot

        //string saveSeparator = "#SAVE-VALUE#";
        /* string[] contents = new string[]
         {
             "" + SavedHealth,
             "" + SavedAmmo,
             "" + SavedMaxHealth,
             SceneManager.GetActiveScene().name
         };*/
        SavedRoomName = SceneManager.GetActiveScene().name;
        SaveObject saveObject = new SaveObject
        {
            SavedHealth = SavedHealth,
            SavedAmmo = SavedAmmo,
            SavedMaxHealth = SavedMaxHealth,
            SavedRoomName = SavedRoomName,
            //SceneName = SceneManager.GetActiveScene().name
        };
        string json = JsonUtility.ToJson(saveObject);
        //string saveString = "" + string.Join(saveSeparator, contents);
        File.WriteAllText(Application.dataPath + "/save.txt", json);
        Debug.Log("Saved");
        
    }
    public void Load()
    {
        //put this in the main menu script
        //Instantiate the player, set all of the Playerinstance values to these values 
        //change the position to the save room to load nowposition
        //Enable inputmap UI, attack, movement
        if (File.Exists(Application.dataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);
            //string[]contents = saveString.Split(new[] { saveSeparator }, System.StringSplitOptions.None);
            //PlayerAttack.instance.Health = int.Parse(contents[0]);
            //PlayerAttack.instance.ammo = float.Parse(contents[1]);
            // PlayerAttack.instance.maxHealth = int.Parse(contents[2]);
            PlayerAttack.instance.Health = saveObject.SavedHealth;
            PlayerAttack.instance.ammo = saveObject.SavedAmmo;
            PlayerAttack.instance.maxHealth = saveObject.SavedMaxHealth;
        }
         else
        {
          Debug.Log("No save Found!");
        }



    }
    public string SetString()
    {
        if (File.Exists(Application.dataPath + "/save.txt"))
        {

            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            return saveObject.SavedRoomName;
        }
        else
        {
            return null;
        }
    }
    
    private class SaveObject
    {
        public int SavedHealth, SavedMaxHealth;
        public float SavedAmmo;
        public string SavedRoomName;
        //public string SceneName;
    }
}
