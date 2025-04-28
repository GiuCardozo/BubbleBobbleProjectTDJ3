using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;
    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;
    public LayerMask wallLayer;

    private int moveDirection = -1; // Comienza hacia la izquierda

    void Update()
    {
        // Movimiento horizontal
        transform.Translate(Vector2.right * moveDirection * speed * Time.deltaTime);

        // Detección de muro
        RaycastHit2D wallInfo = Physics2D.Raycast(wallCheck.position, Vector2.right * moveDirection, wallCheckDistance, wallLayer);
        if (wallInfo.collider != null)
        {
            Flip();
        }
    }

    void Flip()
    {
        moveDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
