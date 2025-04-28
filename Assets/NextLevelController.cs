using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelController : MonoBehaviour
{
    public float delayBeforeNextLevel = 15f; // tiempo en segundos
    private float timer = 0f;
    private bool noEnemiesDetected = false;

    void Update()
    {
        // Buscar enemigos
        GameObject[] enemies1 = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] enemies2 = GameObject.FindGameObjectsWithTag("Enemy2");

        bool enemiesExist = (enemies1.Length > 0 || enemies2.Length > 0);

        if (!enemiesExist)
        {
            if (!noEnemiesDetected)
            {
                // Primer momento en que no hay enemigos
                noEnemiesDetected = true;
                timer = 0f; // reiniciar el temporizador
            }
            else
            {
                timer += Time.deltaTime;

                if (timer >= delayBeforeNextLevel)
                {
                    LoadNextLevel();
                }
            }
        }
        else
        {
            // Si vuelven a aparecer enemigos, cancelar el conteo
            noEnemiesDetected = false;
            timer = 0f;
        }
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}

