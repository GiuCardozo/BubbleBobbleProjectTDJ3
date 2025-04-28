using UnityEngine;
using TMPro;

public class ActualizarPuntos : MonoBehaviour
{
    public TextMeshProUGUI puntosText;

    void Update()
    {
        if (puntosText != null)
        {
            puntosText.text = GameManager.Instance.puntos.ToString();
        }
    }
}


