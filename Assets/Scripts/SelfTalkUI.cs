using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class InteractionUI : MonoBehaviour
{
    public string targetObjectName = "TargetObject"; // Etkileşimde bulunmak istediğiniz objenin ismi
    public GameObject interactionPanel; // UI'deki panel objesi
    public TMP_Text interactionText; // Paneldeki yazıyı gösterecek TMP_Text objesi
    public Image interactionImage; // Paneldeki resmi gösterecek Image objesi
    private bool talked = true;

    private GameObject currentInteractable;

    private void Start()
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false); // Başlangıçta paneli gizle
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == targetObjectName) // Eğer obje istenilen isme sahipse
        {
            currentInteractable = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == targetObjectName)
        {
            currentInteractable = null;
            HideInteractionPanel();
        }
    }

    private void Update()
    {

        if (currentInteractable != null && Input.GetKeyDown(KeyCode.F) && talked) // F tuşuna basıldığında etkileşim başlasın
        {
            ShowInteractionPanel("Her şeyin sona ermesi için bir yol buldum... Belki bu, tüm acılarımı sonlandırır.", null);
            StartCoroutine(HideAfterDelay(3f)); // 3 saniye sonra paneli gizle
            talked = false;
        }
    }

    private void ShowInteractionPanel(string message, Sprite image)
    {
        if (interactionPanel != null && interactionText != null && interactionImage != null)
        {
            interactionText.text = message;
            interactionImage.sprite = image;
            interactionPanel.SetActive(true);
        }
    }

    private void HideInteractionPanel()
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(false);
        }
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideInteractionPanel();
    }
}
