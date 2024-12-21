using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager'? kullanabilmek için

public class LevelManager : MonoBehaviour
{
    void Update()
    {
        // E?er "R" tu?una bas?lm??sa
        if (Input.GetKeyDown(KeyCode.R))
        {
            // ?u anki sahneden bir sonraki sahneye geçi? yap
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}

