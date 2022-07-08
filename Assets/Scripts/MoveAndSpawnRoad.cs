using UnityEngine;

public class MoveAndSpawnRoad : MonoBehaviour
{
    public GameObject[] roads = { };
    public GameObject[] barriers = { };

    private void Start()
    {
        for (var i = 0; i < barriers.Length; i++) 
            barriers[i].transform.parent = roads[i].transform;
    }

    private void Update()
    {
        var speed = Player.speed;
        
        foreach (var road in roads)
        {
            road.transform.Translate(Vector3.forward * (speed * Time.deltaTime));
            road.gameObject.SetActive(!(road.transform.position.z > 45f));
            
            if (road.transform.position.z < -12f)                
                road.transform.SetPositionAndRotation(
                    new Vector3(0, 0, road.transform.position.z + 60f),
                    new Quaternion(0, 180f, 0, 0));
        }
    }
}