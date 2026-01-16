using System.Collections;
using UnityEngine;

public class Trap_Saw : MonoBehaviour
{   
    private Animator anim;
    private SpriteRenderer sr;
    [SerializeField] private Transform wayPointsRoot;
    [SerializeField] private float cooldownTime = 1;
    [SerializeField] private float moveSpeed;

    private Transform[] points;
    private int index;
    private int moveDirection = 1;

    private bool canMove = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        if (wayPointsRoot == null || wayPointsRoot.childCount < 2)
        {
            Debug.LogError("Trap_Saw: Waypoints Root cần ít nhất 2 point");
            enabled = false;
            return;
        }

        int count = wayPointsRoot.childCount;
        points = new Transform[count];

        for (int i = 0; i < count; i++)
        {
            points[i] = wayPointsRoot.GetChild(i);
        }
    }

    private void Start()
    {
        transform.position = points[0].position;
        index = 1;
    }

    private void Update()
    {
        anim.SetBool("active", canMove);

        if (canMove == false)
            return;

        transform.position = Vector2.MoveTowards(transform.position,points[index].position,moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, points[index].position) < 0.05f)
        {
            if (index == points.Length - 1)
                moveDirection = -1;
            else if (index == 0)
                moveDirection = 1;

            index += moveDirection;
            StartCoroutine(StopMovement(cooldownTime));
        }
    }

    private IEnumerator StopMovement(float delay)
    {   
        canMove = false;
        yield return new WaitForSeconds(delay);
        canMove = true;
        //sr.flipX = !sr.flipX;  //muốn lật hình ảnh cưa khi đổi hướng di chuyển thì bỏ comment dòng này
    }
}
