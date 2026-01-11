using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Player Hit Dead Zone");
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.Die();
                GameManager.Instance.RespawnPlayer();
            }
        }
    }
}
