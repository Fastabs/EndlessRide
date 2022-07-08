using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Garage : MonoBehaviour
{
    public static bool btnPlayOfGarage;
    public static int numCar = -1;
    public GameObject car;
    public GameObject[] cars = new GameObject[3];
    public TextMeshProUGUI maxSpeedText;
    
    public Text moneyText;
    public int money;
    private readonly int[] _price = {0, 500, 1200 /*, 3000, 0, 10000*/};
    
    public GameObject lockCar;
    public Text priceText;
    public Button playButton;
    public AudioSource soundButton;
    public GameObject left, right;

    public void Start()
    {
        money = Menu.money;
        moneyText.text = money.ToString("#0") + " $";

        Player.modeChoice = 0;

        numCar = !PlayerPrefs.HasKey("NumCar") ? 0 : PlayerPrefs.GetInt("NumCar");

        maxSpeedText.text = "MAX SPEED: " + cars[numCar].gameObject.GetComponent<CarController>().indexSpeed * 10;
        car = Instantiate(cars[numCar], new Vector3(0.4f, 0f, -0.89f),
            Quaternion.Euler(new Vector3(0, 156, 0)));
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Back();
        left.SetActive(numCar != 0);
        right.SetActive(numCar != cars.Length - 1);
    }
    
    public void Back()
    {
        soundButton.Play();
        numCar = -1;
        SceneManager.LoadScene(0);
    }

    public void MainMenu()
    {
        PlayerPrefs.SetInt("NumCar", numCar);
        btnPlayOfGarage = true;        
        SceneManager.LoadScene(0);
    }

    public void Buy()
    {
        soundButton.Play();
        if (Menu.money < _price[numCar]) return;
        lockCar.SetActive(false);
        playButton.interactable = true;
        PlayerPrefs.SetInt(numCar + "Car", 1);
        Menu.money -= _price[numCar];
        moneyText.text = "$ " + Menu.money;
        PlayerPrefs.SetFloat("money", Menu.money);
    }

    public void Change(bool ch)
    {
        switch (ch)
            {
                case true:
                if (numCar != cars.Length - 1)
                {
                    numCar += 1;
                    CarsPosition();
                }
                break;

                case false:
                if (numCar != 0)
                {
                    numCar -= 1;
                    CarsPosition();
                }
                break;
            }

        if (PlayerPrefs.GetString("money1") != "done" /*|| numCar != 3*/) return;
        lockCar.SetActive(false);
        playButton.interactable = true;
    }

    private void CarsPosition()
    {
        Destroy(car);
        car = numCar switch
        {
            0 => Instantiate(cars[numCar], new Vector3(0.4f, 0f, -0.89f),
                Quaternion.Euler(new Vector3(0, 156, 0))),
            1 => Instantiate(cars[numCar], new Vector3(0.18f, 0f, -0.92f),
                Quaternion.Euler(new Vector3(0, 156, 0))),
            2 => Instantiate(cars[numCar], new Vector3(0.32f, 0f, -0.47f),
                Quaternion.Euler(new Vector3(0, 153, 0))),
            _ => car
        };

        maxSpeedText.text = "MAX SPEED: " + cars[numCar].gameObject.GetComponent<CarController>().indexSpeed * 10;
        if (PlayerPrefs.GetInt(numCar + "Car") == 0)
        {
            priceText.text = "$ " + _price[numCar];
            /*if (numCar == 3)
            {
                priceText.text = " 10000 km ";
            }*/
            if (numCar == 0)
            {
                lockCar.SetActive(false);
                playButton.interactable = true;
            }
            else
            {
                lockCar.SetActive(true);
                playButton.interactable = false;
            }
        }
        else
        {                        
            lockCar.SetActive(false);
            playButton.interactable = true;
        }
    }
}
