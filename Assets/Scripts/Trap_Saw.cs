using UnityEngine;

public class Trap_Saw : MonoBehaviour
{
    [SerializeField]  private float moveSpeed;
    [SerializeField] private Transform[] wayPoint;

    public int wayPointIndex = 1;

    private void Start()
    {
        transform.position = wayPoint[0].position;
    }
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, wayPoint[wayPointIndex].position, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, wayPoint[wayPointIndex].position) < 0.1f)
        {
            wayPointIndex++;
            if (wayPointIndex >= wayPoint.Length)
            {
                wayPointIndex = 0;
            }
        }
    }
}
