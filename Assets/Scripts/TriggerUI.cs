using UnityEngine;

public class TriggerUI : MonoBehaviour
{
    // UI yazı nesnesi (örneğin Text veya TMP_Text)
    public GameObject uiText;

    private void Start()
    {
        // UI başlangıçta gizli olsun
        if (uiText != null)
        {
            uiText.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Sadece belirli taglere sahip objelerde çalışsın
        if (other.CompareTag("Usable") || other.CompareTag("Interactable") || other.CompareTag("NPC"))
        {
            if (uiText != null)
            {
                uiText.SetActive(true); // UI yazısını göster
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Etiket kontrolü tekrar
        if (other.CompareTag("Usable") || other.CompareTag("Interactable") || other.CompareTag("NPC"))
        {
            if (uiText != null)
            {
                uiText.SetActive(false); // UI yazısını gizle
            }
        }
    }
}