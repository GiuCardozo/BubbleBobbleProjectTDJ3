using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float shootSpeed = 5f;
    public float returnSpeed = 1.5f;
    public float floatSpeed = 1f;
    public float maxDistance = 5f;
    public Sprite defaultSprite;
    public Sprite enemy1Sprite;
    public Sprite enemy2Sprite;
    public Sprite enemy3Sprite;

    private Vector2 startPosition;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 shootDirection = Vector2.right;
    private bool isReturning = false;
    private bool isFloatingUp = false;
    private bool poppedByPlayer = false;
    private bool alreadyPopping = false;

    public enum State { Normal, Captured }
    public State currentState = State.Normal;

    private GameObject capturedEnemy;
    private Animator animator;

    private List<Bubble> connectedBubbles = new List<Bubble>();

    // Referencia al script Puntaje
    private Puntaje puntaje;
    [SerializeField] private float cantidadPuntos = 100f; // Puntos a sumar

    // Prefabs de frutas
    [SerializeField] private GameObject fruta1Prefab;
    [SerializeField] private GameObject fruta2Prefab;
    [SerializeField] private GameObject fruta3Prefab;
    [SerializeField] private GameObject fruta4Prefab;

    // 🔥 NUEVO: Sonido
    private AudioSource audioSource;
    [SerializeField] private AudioClip popSound;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        startPosition = transform.position;

        rb.velocity = shootDirection * shootSpeed;

        StartCoroutine(Lifetime());

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        GameObject puntosObject = GameObject.Find("Puntos");
        if (puntosObject != null)
        {
            puntaje = puntosObject.GetComponent<Puntaje>();
        }
        else
        {
            Debug.LogWarning("No se encontró el GameObject 'Puntos'.");
        }
    }

    public void SetDirection(Vector2 dir)
    {
        shootDirection = dir.normalized;
    }

    void Update()
    {
        if (currentState == State.Captured && isFloatingUp)
        {
            rb.velocity = Vector2.up * floatSpeed;
        }
        else if (!isReturning && currentState == State.Normal)
        {
            float distance = Vector2.Distance(transform.position, startPosition);
            if (distance >= maxDistance)
            {
                isReturning = true;
                rb.velocity = -shootDirection * returnSpeed;
            }
        }
    }

    IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(10f);
        DestroyBubble();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Bubble collided with {collision.gameObject.name} and tag {collision.gameObject.tag}");

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            DestroyBubble();
        }

        if (collision.gameObject.CompareTag("Player") && currentState == State.Captured)
        {
            poppedByPlayer = true;
            DestroyBubble();
        }

        if (currentState == State.Captured)
        {
            if (collision.gameObject.CompareTag("Bubble"))
            {
                Bubble otherBubble = collision.gameObject.GetComponent<Bubble>();
                if (otherBubble != null && otherBubble.currentState == State.Captured)
                {
                    TryConnect(otherBubble);
                }
            }
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            CaptureEnemy(collision.gameObject, enemy1Sprite);
        }
        else if (collision.gameObject.CompareTag("Enemy2"))
        {
            CaptureEnemy(collision.gameObject, enemy2Sprite);
        }
        else if (collision.gameObject.CompareTag("Enemy3"))
        {
            CaptureEnemy(collision.gameObject, enemy3Sprite);
        }
    }

    void CaptureEnemy(GameObject enemy, Sprite capturedSprite)
    {
        currentState = State.Captured;
        capturedEnemy = enemy;
        enemy.SetActive(false);
        isFloatingUp = true;
        rb.velocity = Vector2.up * floatSpeed;

        if (animator != null)
        {
            animator.enabled = false;
        }

        sr.sprite = capturedSprite;
    }

    void DestroyBubble()
    {
        if (alreadyPopping) return;
        alreadyPopping = true;

        // 🔥 Reproducimos el sonido ANTES de destruir
        if (popSound != null)
        {
            AudioSource.PlayClipAtPoint(popSound, transform.position);
        }

        if (puntaje != null)
        {
            puntaje.SumarPuntos(cantidadPuntos);
        }

        float probabilidad = Random.Range(0f, 1f);
        if (probabilidad <= 0.2f)
        {
            GameObject frutaElegida = EligeFrutaAleatoria();
            Instantiate(frutaElegida, transform.position, Quaternion.identity);
        }

        if (capturedEnemy != null && !poppedByPlayer)
        {
            capturedEnemy.transform.position = transform.position;
            capturedEnemy.SetActive(true);
        }

        if (animator != null)
        {
            animator.enabled = true;
            animator.SetTrigger("Pop");
            StartCoroutine(DestroyAfterAnimation());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    GameObject EligeFrutaAleatoria()
    {
        int frutaIndex = Random.Range(0, 4);
        switch (frutaIndex)
        {
            case 0: return fruta1Prefab;
            case 1: return fruta2Prefab;
            case 2: return fruta3Prefab;
            case 3: return fruta4Prefab;
            default: return null;
        }
    }

    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    void TryConnect(Bubble other)
    {
        if (connectedBubbles.Contains(other)) return;
        connectedBubbles.Add(other);
        other.connectedBubbles.Add(this);

        List<Bubble> allConnected = new List<Bubble>();
        GetAllConnectedBubbles(this, allConnected);

        if (allConnected.Count >= 4)
        {
            foreach (Bubble b in allConnected)
            {
                b.DestroyBubble();
            }
        }
    }

    void GetAllConnectedBubbles(Bubble bubble, List<Bubble> list)
    {
        if (!list.Contains(bubble))
        {
            list.Add(bubble);
            foreach (Bubble connected in bubble.connectedBubbles)
            {
                GetAllConnectedBubbles(connected, list);
            }
        }
    }
}
