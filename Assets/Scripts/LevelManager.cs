using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager'? kullanabilmek i�in

public class LevelManager : MonoBehaviour
{
    void Update()
    {
        // E?er "R" tu?una bas?lm??sa
        if (Input.GetKeyDown(KeyCode.R))
        {
            // ?u anki sahneden bir sonraki sahneye ge�i? yap
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}

