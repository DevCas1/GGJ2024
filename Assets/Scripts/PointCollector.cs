using UnityEngine;

public class PointCollector : MonoBehaviour
{
    public GameManager GameManager;
    public int Points;

    private void Start() => GameManager = FindObjectOfType<GameManager>();

    public void OnTriggerEnter(Collider other)
    {
        Point point = other.GetComponent<Point>();
        
        if (point != null)
        {
            GameManager.RegisterPoint(transform, point);
        }
    }
}