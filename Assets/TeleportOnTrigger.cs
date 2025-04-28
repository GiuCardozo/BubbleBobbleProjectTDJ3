using UnityEngine;

public class TeleportOnTrigger : MonoBehaviour
{
    public Vector2 teleportPosition; // Coordenada a la que quieres mover al enemigo

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Enemy2") || collision.CompareTag("Player"))
        {
            collision.transform.position = teleportPosition;
        }
    }
}

