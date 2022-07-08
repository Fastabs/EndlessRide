using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Interface : MonoBehaviour {

    public GameObject panelPause;
    public GameObject panBrake;
    public GameObject panSettings;

    public static int click;
    public static float acceleration = 1.1f;
    private static readonly float aBrake_ = -5f;
    public static bool clickBrake;
    public bool rightAct, leftAct;

    public AudioSource sound;
    public AudioSource carAccel;
    public AudioSource carUsual;

    [Header("Audio")]
    public AudioMixerGroup mixerMusic;
    public Toggle togMusic;
    [Space]
    public AudioMixerGroup mixerSounds;
    public Toggle togSounds;
    private bool _status;
    public GameObject control1;
    public GameObject control2;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("statusControl")) _status = true;
        if (PlayerPrefs.GetFloat("statusControl") == 1) _status = true;
        else if (PlayerPrefs.GetFloat("statusControl") == 0) _status = false;
        control1.SetActive(_status);
        control2.SetActive(!_status);
       //if (status) PlayerPrefs.SetFloat("statusControl", 1);
       // else PlayerPrefs.SetFloat("statusControl", 0);
       
        carAccel.enabled = true;
        carUsual.enabled = false;

        switch(Menu.musVolume){
            case 0:  mixerMusic.audioMixer.SetFloat("MusicVolume", -12); break;
            case -80: mixerMusic.audioMixer.SetFloat("MusicVolume", Menu.musVolume); break;
        }
        
        togMusic.isOn = Menu.musVolume == 0;
        mixerSounds.audioMixer.SetFloat("MusicSounds", Menu.musSounds);
        togSounds.isOn = Menu.musSounds == 0;
    }

    private void Update()
    {
        sound.enabled = false;
        if (panelPause.activeInHierarchy == false && Input.GetKeyDown(KeyCode.Escape))
            OnPaused();
        if (Input.GetKeyDown(KeyCode.Space)) OnBrakeDown();
        if (Input.GetKeyUp(KeyCode.Space)) OnBrakeUp();
    }
    
    public void OnPaused()
    {
        carAccel.Stop();
        Time.timeScale = 0;
        panBrake.SetActive(false);
        panelPause.SetActive(true);
    }

    public void Back()
    {
        panSettings.SetActive(false);
        panelPause.SetActive(true);
    }

    public void ToggleMusic(Toggle toggle)
    {        
        switch (toggle.isOn)
        {
            case true:
                mixerMusic.audioMixer.SetFloat("MusicVolume", -12);                
                PlayerPrefs.SetFloat("musVolume", 0);
                Menu.musVolume = 0;
                break;

            case false:
                mixerMusic.audioMixer.SetFloat("MusicVolume", -80);
                Menu.musVolume = -80;                
                PlayerPrefs.SetFloat("musVolume", -80); 
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
                Menu.musSounds = 0;
                break;

            case false:
                mixerSounds.audioMixer.SetFloat("MusicSounds", -80);
                PlayerPrefs.SetFloat("musSounds", -80);
                Menu.musSounds = -80;
                break;
        }
    }
    
    public void OnSettings()
    {
        panSettings.SetActive(true);        
        panelPause.SetActive(false);
    }

    public void Continue()
    {
        carAccel.Play();
        sound.Play();
        Time.timeScale = 1;        
        panBrake.SetActive(true);       
        panelPause.SetActive(false);
    }

    public void MainMenu()
    {
        Garage.btnPlayOfGarage = false;
        Time.timeScale = 1;
        sound.Play();
        panBrake.SetActive(true);
        SceneManager.LoadScene(0);        
    }

    public void Restart()
    {
        Time.timeScale = 1; 
        sound.Play();

        switch (Player.modeChoice)
        {
            case 1: SceneManager.LoadScene(1); break;
            case 2: SceneManager.LoadScene(2); break;
            case 3: SceneManager.LoadScene(3); break;
        }        

        Player.speed = 0;
        Player.distance = 0;

        panBrake.SetActive(true);
    }
    
    public void OnMouseEnter(GameObject gm)
    {
        switch (gm.tag)
        {
            case "Left":
                click = -1;           
                leftAct = true;
                break;
            case "Right":
                click = 1;
                rightAct = true;
                break;
        }
    }

    public void OnMouseExit(GameObject gm)
    {
        switch (gm.tag)
        {
            case "Left":
                leftAct = false;
                break;
            case "Right":
                rightAct = false;
                break;
        }

        if ((gm.CompareTag("Right") || gm.CompareTag("Left")) && !leftAct && !rightAct) click = 0;
    }
    
    public void OnBrakeUp()
    {
        carAccel.enabled = true;
        carUsual.enabled = false;
        acceleration = Mathf.Abs(acceleration) ;              
        clickBrake = false;
        GameObject.Find("Player").GetComponent<Player>().indexSpeed = 
            GameObject.Find("Player").GetComponent<Player>().indexSpeedCount;        
    }
    
    private void OnBrakeDown()
    {
        carAccel.enabled = false;
        carUsual.enabled = true;
        clickBrake = true;
        acceleration = aBrake_;
        GameObject.Find("Player").GetComponent<Player>().indexSpeed *= 3;
    }

    public void OffCar()
    {
        _status = !_status;
        control1.SetActive(_status);
        control2.SetActive(!_status);

        PlayerPrefs.SetFloat("statusControl", _status ? 1 : 0);
    }
}
