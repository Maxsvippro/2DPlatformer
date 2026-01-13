using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    private bool canBeActivited;
    private bool active;
    [SerializeField] private bool canBeReactivited;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        canBeActivited = GameManager.Instance.canReactivate;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && canBeReactivited == false)
            return;

        Player player= collision.GetComponent<Player>();

        if (player != null)
            ActivateCheckpoint();
    }
    private void ActivateCheckpoint()
    {
        active = true;
        anim.SetTrigger("activate");
        GameManager.Instance.UpdateRespawnPosition(transform);
    }
}
