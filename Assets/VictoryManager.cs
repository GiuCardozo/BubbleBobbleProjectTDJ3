using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    private bool victoryTriggered = false;
    private float victoryTimer = 0f;
    [SerializeField] private float waitTime = 10f; // Segundos para esperar antes de cargar la siguiente escena
    [SerializeField] private string nextSceneName; // Nombre de la siguiente escena

    void Update()
    {
        if (!victoryTriggered)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("Enemy2");

            if (enemies.Length == 0 && enemies2.Length == 0)
            {
                victoryTriggered = true;
                victoryTimer = waitTime;
            }
        }
        else
        {
            victoryTimer -= Time.deltaTime;
            if (victoryTimer <= 0f)
            {
                SceneManager.LoadScene("Win");
            }
        }
    }
}
