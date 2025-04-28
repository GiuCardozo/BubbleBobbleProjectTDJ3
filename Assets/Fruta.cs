using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruta : MonoBehaviour
{
    [SerializeField] private int cantidadPuntos; // Cambi� a int (por simplicidad)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Sumar puntos directamente al GameManager
            GameManager.Instance.puntos += cantidadPuntos;

            // Instanciar efecto visual aqu� si quieres
            // Instantiate(efecto, transform.position, Quaternion.identity);

            Destroy(gameObject); // Destruir la fruta despu�s de ser recogida
        }
    }
}
