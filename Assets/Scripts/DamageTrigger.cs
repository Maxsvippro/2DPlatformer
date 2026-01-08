using Unity.VisualScripting;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Player Hit Damage Trigger");
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.Knockback();
            }
        }
    }
}
