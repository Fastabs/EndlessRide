using System.Collections;
using UnityEngine;

public class Cars: MonoBehaviour
{    
    public GameObject[] cars;                                            // Массив с авто
    public GameObject[] carsOnRoad;                                      // Массив для авто
    private readonly float[] _positions = { -1.52f, -0.5f, 0.57f, 1.51f };         // Позиции для спавна авто
    
    // Создание автомобиля через каждую секунду
    void Start()
    {
        Player.modeChoice = 3;
        StartCoroutine(Spawn());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator Spawn()
    {
        while (true)
        {
            var spawnB = true;
            carsOnRoad = GameObject.FindGameObjectsWithTag("Car");   // Нахождение всех авто на сцене

            foreach (var t in carsOnRoad)
                if (t.transform.position.z > 42f) spawnB = false;           // Проверка, не мешают ли авто спавну

            if (Player.speed >= 3f && spawnB)
            {
                Instantiate(
                    cars[Random.Range(0, cars.Length)],
                    new Vector3(_positions[Random.Range(0, 2)], 0, 16), 
                    Quaternion.Euler(new Vector3(0, 180, 0)));

                Instantiate(
                    cars[Random.Range(1, cars.Length)],
                    new Vector3(_positions[Random.Range(2, 4)], 0, 16), 
                    Quaternion.Euler(new Vector3(0, 180, 0)));
            }
            yield return new WaitForSeconds(0.65f);
        }
    }
}
