
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

    public enum State { Normal, Captured }
    public State currentState = State.Normal;

    private GameObject capturedEnemy;
    private List<GameObject> nearbyBubbles = new List<GameObject>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        startPosition = transform.position;

        rb.velocity = shootDirection * shootSpeed;

        StartCoroutine(Lifetime());
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && currentState == State.Captured)
        {
            poppedByPlayer = true;
            DestroyBubble();
        }

        if (currentState == State.Captured) return;

        if (other.CompareTag("Enemy"))
        {
            sr.sprite = enemy1Sprite;
            CaptureEnemy(other.gameObject);
        }
        else if (other.CompareTag("Enemy2"))
        {
            sr.sprite = enemy2Sprite;
            CaptureEnemy(other.gameObject);
        }
        else if (other.CompareTag("Enemy3"))
        {
            sr.sprite = enemy3Sprite;
            CaptureEnemy(other.gameObject);
        }
    }

    void CaptureEnemy(GameObject enemy)
    {
        currentState = State.Captured;
        capturedEnemy = enemy;
        enemy.SetActive(false);
        isFloatingUp = true;
        rb.velocity = Vector2.up * floatSpeed;
    }

    void DestroyBubble()
    {
        if (capturedEnemy != null && !poppedByPlayer)
        {
            capturedEnemy.transform.position = transform.position;
            capturedEnemy.SetActive(true);
        }

        Destroy(gameObject);
    }

    public void ForcePop()
    {
        if (currentState == State.Captured) return;

        poppedByPlayer = true;
        DestroyBubble();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (currentState != State.Captured && collision.gameObject.CompareTag("Bubble"))
        {
            Bubble other = collision.gameObject.GetComponent<Bubble>();
            if (other != null && other.currentState != State.Captured && !nearbyBubbles.Contains(other.gameObject))
            {
                nearbyBubbles.Add(other.gameObject);
            }

            if (nearbyBubbles.Count >= 3)
            {
                List<GameObject> bubblesToPop = new List<GameObject>(nearbyBubbles);

                foreach (GameObject b in bubblesToPop)
                {
                    if (b != null)
                    {
                        Bubble bScript = b.GetComponent<Bubble>();
                        if (bScript != null)
                            bScript.ForcePop();
                    }
                }

                ForcePop(); // Esta burbuja también se revienta
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bubble"))
        {
            nearbyBubbles.Remove(collision.gameObject);
        }
    }
}
