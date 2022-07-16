using System.Collections;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private int _choice = Player.modeChoice;
    public float indexSpeed; 
    public string colTag;

    public GameObject coin;
    public GameObject trigger;
    public GameObject triggerForTheMe;
    public GameObject playerTrigger;

    public AudioSource signal;
    public bool playSound;

    /*private void Awake()
    {
        DontDestroyOnLoad(Signal);

        if (GameObject.FindGameObjectsWithTag("music").Length == 2) Destroy(soundBG);
    }*/

    private void Start()
    {
        trigger.SetActive(true);
        triggerForTheMe.SetActive(true);
        coin.SetActive(true);

        colTag = trigger.tag;
        
        _choice = transform.position.x switch
        {
            < 0 when _choice == 1 => 3,
            > 0 when _choice == 1 => 2,
            _ => _choice
        };
    }

    private void Update()
    {
        var speed = Player.speed;
        
        if (Player.distance > 350)
        {
            speed *= 1.2f;
        }
        else if (Player.distance > 600)
        {
            speed *= 1.3f;
        }
        
        switch (_choice)
        {
            case 2: transform.Translate(Vector3.forward * ((speed * -1.5f + indexSpeed) * Time.deltaTime)); 
                break;
            case 3: transform.Translate(Vector3.forward * ((speed * 0.5f + indexSpeed * 0.8f) * Time.deltaTime)); 
                break;
        }
        
        if (transform.position.z is > 45f or < -3f && !playSound)
            Destroy(gameObject);
    }

    private IEnumerator PlayOnDestroy()
    {            
        yield return new WaitForSeconds(signal.clip.length);
        if (transform.position.z is > 45f or < -3f)
            Destroy(gameObject);
    }
    
    public void OnTriggerEnter(Collider collider1)
    {
        switch (collider1.tag)
        {
            case "Car": if (indexSpeed >= 15) 
                {
                    indexSpeed = 14.7f;
                    trigger.tag = "Car";
                }
                break;
            
            case "SportCar": if (indexSpeed >= 25)
                {
                    indexSpeed = 24.7f;
                    trigger.tag = "SportCar";
                }
                break;

            case "Player":
                var num = Random.Range(0, 3);
                if (num == 0)
                {
                    playSound = true;
                    signal.Play();
                    StartCoroutine(PlayOnDestroy());
                }
                break;
        }
    }
}