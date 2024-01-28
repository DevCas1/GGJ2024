using UnityEngine;

public class PointCollector : MonoBehaviour
{
    public GameManager GameManager;
    public int Points;

    public void OnTriggerEnter(Collider other)
    {
        Point point = other.GetComponent<Point>();
        
        if (point != null)
        {

        }
    }
}