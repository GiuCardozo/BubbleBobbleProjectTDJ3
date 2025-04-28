using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruta : MonoBehaviour
{
    [SerializeField] private int cantidadPuntos; // Cambié a int (por simplicidad)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Sumar puntos directamente al GameManager
            GameManager.Instance.puntos += cantidadPuntos;

            // Instanciar efecto visual aquí si quieres
            // Instantiate(efecto, transform.position, Quaternion.identity);

            Destroy(gameObject); // Destruir la fruta después de ser recogida
        }
    }
}
