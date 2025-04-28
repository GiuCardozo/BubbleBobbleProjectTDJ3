using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player1Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    private bool isGrounded;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Collider2D playerCollider;
    private Animator animator;

    private AudioSource audioSource; // <- Nuevo: para reproducir sonidos
    [SerializeField] private AudioClip shootSound; // <- Nuevo: sonido del disparo

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>(); // <- Nuevo: obtenemos el AudioSource
    }

    void Update()
    {
        float moveX = 0f;

        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }

        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

        // Actualizar el parámetro "Horizontal" en el Animator
        animator.SetFloat("Horizontal", Mathf.Abs(moveX));

        // Voltear el sprite según la dirección
        if (moveX < 0)
        {
            transform.localScale = new Vector3(-0.9f, 0.9f, 0.9f);
        }
        else if (moveX > 0)
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }

        // Verificamos si el personaje está tocando el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Saltar con W
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // Dejarse caer presionando S
        if (Input.GetKeyDown(KeyCode.S) && isGrounded)
        {
            StartCoroutine(DisableCollision());
        }

        // Disparo con la barra espaciadora (disparo de burbuja)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Activamos el trigger de la animación "Shoot"
            animator.SetTrigger("Shoot");

            // Llamamos a la función que dispara la burbuja
            ShootBubble();

            // Reproducir sonido de disparo
            if (shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }

            // Reestablecemos el estado del personaje después de un corto tiempo
            StartCoroutine(ResetToIdle());
        }
    }

    IEnumerator DisableCollision()
    {
        playerCollider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        playerCollider.enabled = true;
    }

    void ShootBubble()
    {
        // Aquí puedes agregar la lógica para disparar la burbuja,
        // por ejemplo, instanciando la burbuja y configurando su dirección.
        Debug.Log("Disparo de burbuja");
    }

    IEnumerator ResetToIdle()
    {
        yield return new WaitForSeconds(0.01f);
        animator.SetTrigger("Idle");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Enemy2"))
        {
            SceneManager.LoadScene("GameOver"); // Carga la escena GameOver
        }
    }
}
