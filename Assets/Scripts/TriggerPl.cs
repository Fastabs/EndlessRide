using UnityEngine;

public class TriggerPl : MonoBehaviour
{
    public Collider collider1;

    public void OnTriggerEnter(Collider collider)
    {
        GameObject.Find("Player").GetComponent<Player>().DestroyCar(collider, collider1);
    }
}
