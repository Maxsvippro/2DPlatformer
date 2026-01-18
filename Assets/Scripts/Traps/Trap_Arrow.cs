using UnityEngine;

public class Trap_Arrow : Trap_Trampoline
{
    [Header("Additional Arrow Settings")]
    [SerializeField] private float cooldown;
    [SerializeField] private bool rotationRight;
    [SerializeField] private float rotationSpeed = 120;
    private int direction = -1;

    [Space]
    [SerializeField] private float scaleUpSpeed = 10;
    [SerializeField] private Vector3 targetScale;

    private void Start()
    {
        transform.localScale = new Vector3(.3f, .3f, .3f);
    }

    private void Update()
    {
        HandleScaleUp();
        HandleRotation();
    }

    private void HandleScaleUp()
    {
        if (transform.localScale.x < targetScale.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleUpSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        direction = rotationRight ? -1 : 1;
        transform.Rotate(0, 0, (direction * rotationSpeed) * Time.deltaTime);
    }

    private void DestroyMe()
    {
        GameObject arrowPrefab = GameManager.Instance.arrowPrefab;
        GameManager.Instance.CreateObject(arrowPrefab, transform, cooldown);
        Destroy(gameObject);
    }
  
}
