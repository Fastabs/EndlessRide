using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject title;
    public GameObject panelChoice;
    public GameObject panelMenu;
    public GameObject soundBG;
    public AudioSource soundButton;

    public static int money;
    private bool _status;

    public AudioMixerGroup mixerMusic;
    public AudioMixerGroup mixerSounds;

    public GameObject panelSettings;
    public Toggle togMusic;
    public static float musVolume;

    public Toggle togSounds;
    public static float musSounds;

    private void Awake()
    {        
        DontDestroyOnLoad(soundBG);
        if (GameObject.FindGameObjectsWithTag("music").Length == 2) Destroy(soundBG);
    }

    private void Start()
    {
        money = !PlayerPrefs.HasKey("money") ? 0 : PlayerPrefs.GetInt("money");
        
        musVolume = !PlayerPrefs.HasKey("musVolume") ? 0 : PlayerPrefs.GetFloat("musVolume");
        mixerMusic.audioMixer.SetFloat("MusicVolume", musVolume);

        togMusic.isOn = musVolume == 0;
        
        musSounds = !PlayerPrefs.HasKey("musSounds") ? 0 : PlayerPrefs.GetFloat("musSounds");
        mixerSounds.audioMixer.SetFloat("MusicSounds", musSounds);

        togSounds.isOn = musSounds == 0;
        
        if (!PlayerPrefs.HasKey("NumCar")) PlayerPrefs.SetInt("NumCar", 0);
        
        panelMenu.SetActive(true);
        panelChoice.SetActive(false);

        if (!Garage.btnPlayOfGarage) return;
        panelMenu.SetActive(false);
        panelChoice.SetActive(true);
    }

    private void Update()
    {
        if (panelSettings.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            panelSettings.SetActive(false);
            panelMenu.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !panelChoice.activeSelf) 
            OnClickExit();
        else if (panelChoice.activeSelf && Input.GetKeyDown(KeyCode.Escape)) 
            OnClickBack();
    }

    public void OnSettings()
    {
        soundButton.Play();
        panelSettings.SetActive(true);
        panelMenu.SetActive(false);
    }

    public void Back()
    {
        soundButton.Play();
        panelSettings.SetActive(false);
        panelMenu.SetActive(true);
    }
    public void OnClickPlay()
    {
        soundButton.Play();
        panelChoice.SetActive(true);
        panelMenu.SetActive(false);
        title.SetActive(false);
    }

    public void OnClickBack()
    {
        soundButton.Play();
        panelMenu.SetActive(true);
        title.SetActive(true);
        panelChoice.SetActive(false);        
    }

    public void OnClickGarage()
    {
        soundButton.Play();
        SceneManager.LoadScene(4);
    }

    public void OnClickStart(int mode)
    {
        soundButton.Play();
        switch (mode)
        {
            case 1: SceneManager.LoadScene(1);
                Player.modeChoice = 1;
                break;

            case 2: SceneManager.LoadScene(2);
                Player.modeChoice = 2;
                break;

            case 3: SceneManager.LoadScene(3);
                Player.modeChoice = 3;
                break;
        }        
        panelChoice.SetActive(false);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void ToggleMusic(Toggle toggle)
    {
        switch (toggle.isOn)
        {
            case true:
                soundButton.Play();
                mixerMusic.audioMixer.SetFloat("MusicVolume", 0);
                PlayerPrefs.SetFloat("musVolume", 0);
                musVolume = 0;
                break;

            case false:
                soundButton.Play();
                mixerMusic.audioMixer.SetFloat("MusicVolume", -80);
                PlayerPrefs.SetFloat("musVolume", -80);
                musVolume = -80;
                break;
        }
    }

    public void ToggleSounds(Toggle toggle)
    {
        switch (toggle.isOn)
        {
            case true:
                mixerSounds.audioMixer.SetFloat("MusicSounds", 0);
                PlayerPrefs.SetFloat("musSounds", 0);
                musSounds = 0;
                break;

            case false:
                mixerSounds.audioMixer.SetFloat("MusicSounds", -80);
                PlayerPrefs.SetFloat("musSounds", -80);
                musSounds = -80;
                break;
        }
    }

    public void AddMoney()
    {
        money += 100000;
        PlayerPrefs.SetInt("money", money);
    }

    public void Status()
    {
        if (!PlayerPrefs.HasKey("statusControl")) _status = true;
        
        if (PlayerPrefs.GetFloat("statusControl") == 1) _status = true;
        else if (PlayerPrefs.GetFloat("statusControl") == 0) _status = false;
        _status = !_status;
        PlayerPrefs.SetFloat("statusControl", _status ? 1 : 0);
    }
}