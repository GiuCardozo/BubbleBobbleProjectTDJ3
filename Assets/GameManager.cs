using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int puntos = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No destruir este objeto al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Si ya existe uno, destruir el nuevo
        }
    }
}

