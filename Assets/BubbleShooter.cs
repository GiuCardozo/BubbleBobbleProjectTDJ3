using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShooter : MonoBehaviour
{
    public GameObject bubblePrefab;
    public Transform firePoint; // 🔥 Solo uno ahora

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBubble();
        }
    }

    void ShootBubble()
    {
        bool facingRight = transform.localScale.x > 0;

        // Siempre usamos el mismo firePoint
        GameObject bubble = Instantiate(bubblePrefab, firePoint.position, Quaternion.identity);

        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        bubble.GetComponent<Bubble>().SetDirection(direction);

        // 🚀 Impulso realista
        Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * 300f);
    }
}






