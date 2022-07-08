using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject panelFail;
    public GameObject playerCar;
    
    private GameObject[] _wheels = { };
    public GameObject[] cars = { };

    public static int modeChoice = 0;

    private const float SpeedAngular = 5f;
    public static float distance;
    public float indexSpeed;
    public float indexSpeedCount;
    private static float _score;
    public int money;

    private float _hor1;
    public static float speed;                            
    public static float aBrake;  

    public Text distanceText;
    public Text speedText;   
    public Text scoreText;
    public Text recordScore;
    public Text moneyText;

    public TextMeshProUGUI resultDistance;
    public TextMeshProUGUI resultScore;
    public TextMeshProUGUI newRecord;
    
    public AudioMixerGroup mixer;
    public AudioSource soundAcc;


    private void Start()
    {
        if (Garage.numCar == -1) Garage.numCar = PlayerPrefs.GetInt("NumCar");
        playerCar = Instantiate(cars[Garage.numCar], transform, true);
        
        _wheels = GameObject.FindGameObjectsWithTag("FWheel");
        foreach (var t in _wheels)
            t.tag = "FirstWheel";

        playerCar.gameObject.GetComponent<CarController>().enabled = false;
        playerCar.gameObject.GetComponent<BoxCollider>().enabled = false;
                
        Destroy(playerCar.gameObject.GetComponent<CarController>().coin);
        Destroy(playerCar.gameObject.GetComponent<CarController>().trigger);
        Destroy(playerCar.gameObject.GetComponent<CarController>().triggerForTheMe);

        playerCar.gameObject.GetComponent<CarController>().playerTrigger.SetActive(true);

        playerCar.transform.position = transform.position;
        playerCar.tag = "Player";

        indexSpeed = playerCar.gameObject.GetComponent<CarController>().indexSpeed;
        indexSpeedCount = indexSpeed;
        
        distance = 0;
        speed = 0;
        _score = 0;

        StartCoroutine(Coins());
        
        if (!PlayerPrefs.HasKey("recordScore" + modeChoice)) recordScore.text = "0";
            else recordScore.text = "record: " + PlayerPrefs.GetFloat("recordScore"+ modeChoice).ToString("#0");
    }


    private void Update()
    {
        if (Interface.clickBrake)
        {
            speed += Interface.acceleration * Time.deltaTime;
        }
        else
            speed += indexSpeed / 2 * Interface.acceleration / 2 * Time.deltaTime;
        
        if (speed > 5f * indexSpeed / 5) speed = 5f * indexSpeed / 5;
         if (speed < 0) speed = 0;
            speedText.text = (speed * 10).ToString("#00");
            
        distance += speed * Time.deltaTime;
        distanceText.text = distance.ToString("#0");

        // Car movement
        _hor1 = Input.GetAxis("Horizontal");
        float hor = Interface.click;
             if (_hor1 != 0) hor = _hor1;
             
        var dir = new Vector3(hor, 0, 0);

        if ((-5.32f > transform.position.x && hor == -1f) || (5.32f < transform.position.x && hor == 1f)) hor += 0;
        else
        {
            transform.rotation = Quaternion.Euler(0, hor * 3, 0);
            transform.Translate(dir.normalized * (Time.deltaTime * SpeedAngular));
            
            foreach (var w in _wheels)
                w.transform.rotation = Quaternion.Euler(0, hor * 40, 0);
        }

        if (transform.position.z != -2f)
            transform.position = new Vector3(transform.position.x, transform.position.y, -2f);
    }
    
    public void DestroyCar(Collider collider1, Collider collider2)
    {
        if (collider1.CompareTag("TriggerME") && collider2.CompareTag("Player"))
        {
            Time.timeScale = 0;
            soundAcc.Play();

            GameObject.Find("SpeedText").SetActive(false);
            GameObject.Find("DistanceText").SetActive(false);
            GameObject.Find("ScoreText").SetActive(false);
            GameObject.Find("RecordScoreText").SetActive(false);
            
            GameObject.Find("Left").SetActive(false);
            GameObject.Find("Right").SetActive(false);
            GameObject.Find("PauseBtn").SetActive(false);

            Destroy(gameObject);

            GameObject.Find("Interface").GetComponent<Interface>().OnBrakeUp();

            Interface.click = 0;

            resultDistance.text = distance.ToString("#0");
            
            if (_score > PlayerPrefs.GetFloat("recordScore" + modeChoice)) 
            {
                PlayerPrefs.SetFloat("recordScore" + modeChoice, _score);                
                newRecord.gameObject.SetActive(true);
            }
            resultScore.text = "SCORE \n" + _score.ToString("#0");
            
            money = Menu.money;
            money += (int)distance / 12;
            moneyText.text = "$ " + money.ToString("#0");
            //resultScore.text += "     + $ " + (distance / 12).ToString("#0");
            Menu.money = money;
            PlayerPrefs.SetInt("money", money);
            
            panelFail.SetActive(true);
        }
        
        // Extra points
        if (collider1.CompareTag("Coin") && speed > 8f) {
            _score += 8 * speed;
            scoreText.color = new Color(255,255,0);
            Invoke(nameof(ColorChange), 0.3f);           
        }
    }
    
    private IEnumerator Coins()
    {
        while (true)
        {
            if (speed >= 4f) 
                _score += speed * 0.23f;
            
            scoreText.text = _score.ToString("#0");
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ColorChange()
    {
        scoreText.color = new Color(255, 255, 255);
    }
}