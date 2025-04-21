using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShooter : MonoBehaviour
{
    public GameObject bubblePrefab;     // Prefab de la burbuja
    public Transform firePointRight;    // Punto de disparo para la derecha
    public Transform firePointLeft;     // Punto de disparo para la izquierda
    private bool facingRight = true;    // Determina hacia dónde está mirando el jugador

    void Update()
    {
        // Cambiar la dirección cuando se presionan las teclas A o D
        if (Input.GetKey(KeyCode.A))
            facingRight = false;
        else if (Input.GetKey(KeyCode.D))
            facingRight = true;

        // Disparar la burbuja cuando se presiona la tecla espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBubble();
        }
    }

    void ShootBubble()
    {
        // Elegir el punto de disparo según la dirección del jugador
        Transform firePoint = facingRight ? firePointRight : firePointLeft;

        // Crear la burbuja en el punto de disparo adecuado
        GameObject bubble = Instantiate(bubblePrefab, firePoint.position, Quaternion.identity);

        // Determinar la dirección de la burbuja
        Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        bubble.GetComponent<Bubble>().SetDirection(direction);
    }
}



