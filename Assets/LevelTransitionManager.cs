using System.Collections;
using UnityEngine;

public class LevelTransitionManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Player1Movement playerMovement;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Transform cameraTransform;

    [Header("Transición")]
    [SerializeField] private Vector3 playerTargetPosition;
    [SerializeField] private Vector3 cameraTargetPosition;
    [SerializeField] private float playerMoveSpeed = 5f;
    [SerializeField] private float cameraMoveSpeed = 5f;
    [SerializeField] private float timeAfterEnemiesDefeated = 15f;

    private bool transitionStarted = false;
    private bool enemiesCleared = false;
    private float timer = 0f;
    private float originalGravityScale;

    private void Start()
    {
        if (playerRb != null)
            originalGravityScale = playerRb.gravityScale;
    }

    private void Update()
    {
        if (!enemiesCleared)
        {
            if (AllEnemiesDefeated())
            {
                Debug.Log("Todos los enemigos derrotados, empieza el conteo de 15s...");
                enemiesCleared = true;
                timer = 0f; // Reinicia el timer
            }
        }
        else if (!transitionStarted)
        {
            timer += Time.deltaTime;
            Debug.Log($"Contando... {timer:F2}s");

            if (timer >= timeAfterEnemiesDefeated)
            {
                Debug.Log("¡Iniciando la transición!");
                StartCoroutine(HandleTransition());
            }
        }
    }


    private bool AllEnemiesDefeated()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("Enemy2");

        return enemies.Length == 0 && enemies2.Length == 0;
    }

    private IEnumerator HandleTransition()
    {
        transitionStarted = true;

        if (playerMovement == null) Debug.LogError("Falta asignar 'playerMovement'");
        if (playerRb == null) Debug.LogError("Falta asignar 'playerRb'");
        if (playerCollider == null) Debug.LogError("Falta asignar 'playerCollider'");
        if (cameraTransform == null) Debug.LogError("Falta asignar 'cameraTransform'");
        if (playerAnimator == null) Debug.LogWarning("No hay 'playerAnimator' (seguimos sin animaciones)");

        // Si falta algo crítico, salir
        if (playerMovement == null || playerRb == null || playerCollider == null || cameraTransform == null)
        {
            yield break;
        }

        //  Desactivar controles del jugador
        playerMovement.enabled = false;
        playerCollider.enabled = false;
        playerRb.velocity = Vector2.zero;
        playerRb.gravityScale = 0f;

        //  Activar animación de transición
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("TransitionMove");
        }

        bool playerArrived = false;
        bool cameraArrived = false;

        while (!playerArrived || !cameraArrived)
        {
            // Mover jugador
            if (!playerArrived)
            {
                playerRb.MovePosition(Vector3.MoveTowards(playerRb.position, playerTargetPosition, playerMoveSpeed * Time.deltaTime));

                if (Vector3.Distance(playerRb.position, playerTargetPosition) <= 0.05f)
                {
                    playerRb.position = playerTargetPosition;
                    playerArrived = true;
                }
            }

            // Mover cámara
            if (!cameraArrived)
            {
                cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, cameraTargetPosition, cameraMoveSpeed * Time.deltaTime);

                if (Vector3.Distance(cameraTransform.position, cameraTargetPosition) <= 0.05f)
                {
                    cameraTransform.position = cameraTargetPosition;
                    cameraArrived = true;
                }
            }

            yield return null;
        }

        // 🔓Reactivar controles
        playerCollider.enabled = true;
        playerRb.gravityScale = originalGravityScale;
        playerMovement.enabled = true;

        Debug.Log("¡Transición completada!");
    }

}

