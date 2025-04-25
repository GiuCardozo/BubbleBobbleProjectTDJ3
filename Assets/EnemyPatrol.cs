using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;  // Velocidad de movimiento
    public Transform wallCheck;  // Referencia para la comprobaci�n de pared
    public LayerMask wallLayer;  // Capa de la pared (Layer "wall")
    public float wallCheckDistance = 0.2f;  // Distancia para comprobar la pared
    public LayerMask groundLayer;  // Capa para verificar el borde (Layer "ground")
    public float groundCheckDistance = 0.5f;  // Distancia para comprobar el borde

    private Rigidbody2D rb;  // Componente Rigidbody2D
    private int moveDirection = 1;  // Direcci�n de movimiento (1 es hacia la derecha, -1 hacia la izquierda)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Patrol();  // Llamamos a la funci�n de patrullaje en cada actualizaci�n del FixedUpdate
    }

    void Patrol()
    {
        // Comprobamos si el enemigo est� bloqueado por una pared o si est� en el borde
        bool isBlocked = IsBlocked();
        bool isAtEdge = IsAtEdge();

        // Si est� bloqueado o en el borde, hacemos que gire
        if (isBlocked || isAtEdge)
        {
            Flip();  // Volteamos la direcci�n
        }

        // Aplicamos el movimiento del enemigo
        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);  // Movimiento horizontal
    }

    bool IsBlocked()
    {
        // Comprobamos si el enemigo est� bloqueado por una pared
        Vector2 checkPosition = wallCheck.position;
        Vector2 rayDirection = Vector2.right * moveDirection;  // Direcci�n en la que el enemigo est� mirando

        // Realizamos el raycast
        RaycastHit2D hitWall = Physics2D.Raycast(checkPosition, rayDirection, wallCheckDistance, wallLayer);

        // Dibujamos la l�nea del raycast para depuraci�n
        Debug.DrawRay(checkPosition, rayDirection * wallCheckDistance, Color.red);

        return hitWall.collider != null;  // Si colisiona con algo, devuelve verdadero
    }

    bool IsAtEdge()
    {
        // Comprobamos si el enemigo est� en el borde de una plataforma (sin suelo debajo)
        Vector2 checkPosition = wallCheck.position;
        Vector2 rayDirection = Vector2.down;

        RaycastHit2D hitGround = Physics2D.Raycast(checkPosition, rayDirection, groundCheckDistance, groundLayer);

        // Dibujamos la l�nea del raycast para depuraci�n
        Debug.DrawRay(checkPosition, rayDirection * groundCheckDistance, Color.green);

        return hitGround.collider == null;  // Si no hay suelo debajo, devuelve verdadero (indicando el borde)
    }

    void Flip()
    {
        // Invertimos la direcci�n de movimiento
        moveDirection *= -1;

        // Rotamos el sprite del enemigo para que apunte en la nueva direcci�n
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * moveDirection;  // Solo modificamos el valor de X
        transform.localScale = scale;

        // Aseguramos que el wallCheck se mueva con el enemigo
        Vector3 wallCheckPos = wallCheck.localPosition;
        wallCheckPos.x = Mathf.Abs(wallCheckPos.x) * moveDirection;  // Actualizamos la posici�n en X
        wallCheck.localPosition = wallCheckPos;
    }

    // Funci�n para depurar los raycasts (se puede ver en el editor)
    void OnDrawGizmos()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * moveDirection * wallCheckDistance));
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.down * groundCheckDistance));
        }
    }
}









