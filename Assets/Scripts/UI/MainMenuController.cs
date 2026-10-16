using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Collections;

using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    [Header("VolumeSetting")]
    [SerializeField] private TMP_Text volumeTextVolume = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = .5f;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text ControllerSensTextValue = null;
    [SerializeField] private Slider controllerSensSlider = null;
    [SerializeField] private float defaultSen = .5f;
    public float mainControllerSens;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    [Header("ConfirmationPrompt")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Levels To Load")]
    public string _newGameLevel;
    //public Scene loadGameLevel;

    public string loadGameLevel;
    //[SerializeField] private GameObject noSavedGameDialog = null;

    //[Header("Resolution Dropdowns")]


    [Header("LoadingScreen")]
    public GameObject loadingScreen;
    public Image loadingBarFill;
    [Header("Player")]
    [SerializeField] private PlayerSOScript Values;
    [SerializeField] private GameObject Player;


    //[Header("Hate")]
    //public GameObject PlayerFighter;
    //public SaveLoad SaveLoad;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void NewGameDialogYes()
    {
        StartCoroutine(LoadNewGameScene());

    }
    IEnumerator LoadNewGameScene()
    {
        //IEnumerator LoadSceneAsync()
        //{

        //SceneSwapManager.instance.DisablePlayer();
        Scene currentScene = SceneManager.GetActiveScene();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_newGameLevel, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            loadingScreen.SetActive(true);
            loadingBarFill.fillAmount = asyncLoad.progress;
            yield return null;

        }
        Player.SetActive(true);
        SceneManager.MoveGameObjectToScene(Player, SceneManager.GetSceneByName(_newGameLevel));
        GameObject[] Spawners = GameObject.FindGameObjectsWithTag("Spawner");
        for (int i = 0; i < Spawners.Length; i++)
        {
            Debug.Log(Spawners[i].scene.name);
            if (Spawners[i].scene.name == _newGameLevel)
            {
                Spawners[i].GetComponent<SpawnerScript>().PlayerCheck();
            }
        }
        //PlayerMovement.instance.Spawn(false);
        //SceneSwapManager.instance.EnablePlayer();
        SceneManager.UnloadSceneAsync(currentScene);

    }
    public void LoadGameDialogYes()
    {
        //loadGamelevel = Saveslot number SceneManager.GetSceneByName(loadgamelevel)
        StartCoroutine(LoadGameScene());
    }
    IEnumerator LoadGameScene()
    {
        //SceneSwapManager.instance.DisablePlayer();
        Scene currentScene = SceneManager.GetActiveScene();
        //string saveSeparator = "#SAVE-VALUE#";
        //string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
        //string[] contents = saveString.Split(new[] { saveSeparator }, System.StringSplitOptions.None);
        //loadGameLevel = contents[3];
        if (File.Exists(Application.dataPath + "/save.txt"))
        {

            //for below to work, you have to spawn the player in the main menu, then when playing, save, then set that string somewhere 
            // loadGameLevel = PlayerMovement.instance.lastRoomSaved;
            loadGameLevel = Values.lastRoomSaved;
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(loadGameLevel, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                loadingScreen.SetActive(true);
                loadingBarFill.fillAmount = asyncLoad.progress;
                yield return null;

            }
            Player.SetActive(true);
            SceneManager.MoveGameObjectToScene(Player, SceneManager.GetSceneByName(loadGameLevel));
            //SceneSwapManager.instance.EnablePlayer();
            //GameObject PO = GameObject.Find("PERSISTOBJECTS");
            //PlayerMovement.instance.Spawn(true);
            GameObject SaveStation = GameObject.Find("SaveStation");
            if (SaveStation)
            {
                SaveStation.GetComponent<SaveLoad>().Load();
            }
            SceneManager.UnloadSceneAsync(currentScene);

        }
        else
        {
            Debug.Log("No save found!");
        }
        //ReadText from Application.dataPath + "/save.txt" to find name of scene to load
        //loadGameLevel = string above
        //The number in contents[x] needs to be changed when adding more things to save/load

    }
    /* public void Load()
     {
         string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
         Debug.Log(saveString);
     }*/
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Exiting Game...");
    }
    public void SetVolume(float volume)
    {

        float AudioFloat = Mathf.Round(volume * 10f) / 10f;
        //_brightnessLevel = brightnessFloat;
        //brightnessTextValue.text = _brightnessLevel.ToString("0.0");
        AudioListener.volume = AudioFloat;
        volumeTextVolume.text = AudioListener.volume.ToString("0.0");
    }
    public void VolumeApply()
    {
        // Debug.Log(volumeSlider.value);
        // SetVolume(volumeSlider.value);
        // PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }
    public void GameplayApply()
    {
        if (invertYToggle.isOn)
        {
            // PlayerPrefs.SetInt("masterInvertY", 1);
        }
        else
        {
            // PlayerPrefs.SetInt("masterInvertY", 0);
        }
        //PlayerPrefs.SetFloat("masterSen", mainControllerSens);
        StartCoroutine(ConfirmationBox());
    }
    public void SetBrightness(float brightness)
    {
        float brightnessFloat = Mathf.Round(brightness * 10f) / 10f;
        _brightnessLevel = brightnessFloat;
        brightnessTextValue.text = _brightnessLevel.ToString("0.0");
    }
    public void SetFullscreen(bool isfullscreen)
    {
        _isFullScreen = isfullscreen;
    }
    public void SetQuality(int QualityIndex)
    {
        _qualityLevel = QualityIndex;
    }
    public void GraphicsApply()
    {
        //PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        // PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);
        // PlayerPrefs.SetInt("masterFullscreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;
        StartCoroutine(ConfirmationBox());
    }
    public void SetControllerSens(float sensitivity)
    {
        float sensFloat = Mathf.Round(sensitivity * 10f) / 10f;
        mainControllerSens = sensFloat;
        ControllerSensTextValue.text = mainControllerSens.ToString("F1");
    }
    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextVolume.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
        if (MenuType == "Gameplay")
        {
            ControllerSensTextValue.text = defaultSen.ToString("0");
            controllerSensSlider.value = defaultSen;
            mainControllerSens = defaultSen;
            invertYToggle.isOn = false;
            GameplayApply();
        }
        if (MenuType == "Graphics")
        {
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");
            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);
            fullscreenToggle.isOn = false;
            Screen.fullScreen = false;
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            //resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }

    }
    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
