using System.Collections;
using UnityEngine;

public class Obstacles: MonoBehaviour
{    
    public GameObject[] cars;
    public GameObject[] carsOnRoad;
    private readonly float[] _positions = { -4.02f, -1.34f, 1.37f, 4.05f };
    
    private void Start()
    {
        StartCoroutine(SpawnObstacles());
    }
    
    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            var spawnB = true;
            carsOnRoad = GameObject.FindGameObjectsWithTag("Car");

            foreach (var t in carsOnRoad)
                if (t.transform.position.z > 34f) spawnB = false;

            if (Player.speed >= 5f && spawnB)
            {
                switch(Player.modeChoice)
                {
                    case 1:
                        CreateObstacles(180f, 0);
                        break;
                    case 2:
                        CreateObstacles(0, 0);
                        break;
                    case 3:
                        CreateObstacles(180f, 180f);
                        break;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void CreateObstacles(float rotation1, float rotation2)
    {
        var numCar = Random.Range(0, cars.Length);
        
        Instantiate(
            cars[numCar],
            new Vector3(_positions[Random.Range(0, 2)], cars[numCar].transform.position.y, 45), 
            Quaternion.Euler(new Vector3(0, rotation1, 0)));
                        
        numCar = Random.Range(0, cars.Length);

        Instantiate(
            cars[numCar],
            new Vector3(_positions[Random.Range(2, 4)], cars[numCar].transform.position.y, 45), 
            Quaternion.Euler(new Vector3(0, rotation2, 0)));
    }
}