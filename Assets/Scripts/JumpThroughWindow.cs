using UnityEngine;
using UnityEngine.SceneManagement; // Sahne y�netimi i�in gerekli k�t�phane

public class JumpThroughWindow : MonoBehaviour
{
    private bool isNearWindow = false; // Oyuncu pencereye yak?n m??
    public float jumpForce = 10f; // Atlama kuvveti
    public Vector3 jumpDirection = new Vector3(0, -1, 1); // At?lma y�n� (varsay?lan: a?a?? ve ileri)

    private Rigidbody playerRb;
    private bool hasJumped = false; // Oyuncunun z?play?p z?plamad???n? kontrol etmek i�in

    void Start()
    {
        // Oyuncunun Rigidbody'sini bul
        playerRb = GetComponent<Rigidbody>();
        if (playerRb == null)
        {
            Debug.LogError("Rigidbody bulunamad?! L�tfen Player'a Rigidbody ekleyin.");
        }
    }

    void Update()
    {
        // E?er oyuncu pencereye yak?nsa ve F tu?una bas?lm??sa
        if (isNearWindow && Input.GetKeyDown(KeyCode.F) && !hasJumped)
        {
            JumpOut();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // E?er tetikleme yap?lan obje "Window" etiketi ta??yorsa
        if (other.CompareTag("window"))
        {
            isNearWindow = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // E?er tetikleme yap?lan obje "Window" etiketi ta??yorsa
        if (other.CompareTag("window"))
        {
            isNearWindow = false;
        }
    }

    private void JumpOut()
    {
        if (playerRb != null)
        {
            // Oyuncuyu jumpDirection y�n�ne do?ru it
            playerRb.AddForce(jumpDirection.normalized * jumpForce, ForceMode.Impulse);
            hasJumped = true; // Oyuncu bir kez z?plad?
            Invoke("LoadNextLevel", 2f); // 2 saniye sonra yeni seviyeyi y�kle
        }
    }

    private void LoadNextLevel()
    {
        // Mevcut sahnenin index numaras?n? al ve bir sonraki sahneyi y�kle
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
