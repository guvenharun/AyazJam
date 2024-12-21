using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interaction : MonoBehaviour
{
    public float interactionDistance = 3f; // Etkileşim mesafesi
    private bool interacted = false;
    public KeyCode interactionKey = KeyCode.F; // Etkileşim tuşu

    public Image interactionImage; // Sağ üstteki UI Image
    public TMP_Text interactionMessage; // Alt ortadaki UI Text
    private Sprite defaultImage; // Varsayılan boş görüntü

    private GameObject currentInteractableObject; // Geçerli etkileşimde olunan obje

    void Start()
    {
        // UI Image başlangıçta gizli olmalı
        if (interactionImage != null)
        {
            defaultImage = interactionImage.sprite;
            interactionImage.enabled = false; // Başlangıçta gizle
        }

        // UI Text başlangıçta gizli olmalı
        if (interactionMessage != null)
        {
            interactionMessage.text = "";
            interactionMessage.gameObject.SetActive(false); // Başlangıçta gizle
        }
    }

    void Update()
    {
        // Eğer geçerli bir etkileşimli obje varsa ve F tuşuna basıldığında
        if (currentInteractableObject != null && Input.GetKeyDown(interactionKey))
        {
            // Etkileşim işlemi yapılır
            HandleInteraction();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable") || other.CompareTag("Usable"))
        {
            Debug.Log("Etkileşim yapılabilir! F tuşuna basabilirsiniz.");

            // Geçerli etkileşimli nesneyi belirle
            currentInteractableObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable") || other.CompareTag("Usable"))
        {
            // Etkileşimli obje ile teması sonlandır
            currentInteractableObject = null;

            // UI Image'i gizle
            if (interactionImage != null && other.CompareTag("Usable"))
            {
                interactionImage.enabled = false;
                interactionImage.sprite = defaultImage;
            }

            // UI Text'i gizle
            if (interactionMessage != null)
            {
                interactionMessage.gameObject.SetActive(false);
            }
        }
    }

    private void HandleInteraction()
    {
        if (currentInteractableObject != null)
        {
            // Eğer Interactable nesnesiyse
            if (currentInteractableObject.CompareTag("Interactable"))
            {
                if (interactionImage != null)
                {
                    InteractionIcon iconComponent = currentInteractableObject.GetComponent<InteractionIcon>();
                    if (iconComponent != null && iconComponent.icon != null)
                    {
                        interactionImage.sprite = iconComponent.icon; // Yeni ikon
                        interactionImage.enabled = true; // Resmi göster
                        Destroy(currentInteractableObject);
                    }
                }

                if (interactionMessage != null)
                {
                    interactionMessage.text = (currentInteractableObject.name)+" Alındı!";
                    interactionMessage.gameObject.SetActive(true);
                    StartCoroutine(HideMessageAfterDelay(2f));
                }
            }

            // Eğer Usable nesnesiyse
            if (currentInteractableObject.CompareTag("Usable"))
            {
                if (interactionImage != null)
                {
                    interactionImage.enabled = false; // Resmi gizle
                    interactionImage.sprite = defaultImage; // Varsayılan görüntüye dön
                }

                if (interactionMessage != null)
                {
                    interactionMessage.text = (currentInteractableObject.name) +" Kullanıldı!";
                    interactionMessage.gameObject.SetActive(true);
                    StartCoroutine(HideMessageAfterDelay(2f));
                }
            }

            currentInteractableObject = null; // Etkileşimi sonlandır
        }
    }

    private System.Collections.IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (interactionMessage != null)
        {
            interactionMessage.text = "";
            interactionMessage.gameObject.SetActive(false);
        }
    }
}
