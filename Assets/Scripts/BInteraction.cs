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
    private bool conductivy = false;
    private bool takenCable = false;
    string usableName;
    string interactableName;

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
            if (currentInteractableObject.CompareTag("Interactable"))
            {
                HandleInteractable();
            }
            else if (currentInteractableObject.CompareTag("Usable"))
            {
                HandleUsable();
            }

            currentInteractableObject = null; // Etkileşimi sonlandır
        }
    }

    private void HandleInteractable()
    {
        if (interactionImage != null)
        {
            InteractionIcon iconComponent = currentInteractableObject.GetComponent<InteractionIcon>();

            interactableName = currentInteractableObject.name;

            switch(interactableName){

                case "knife": 
                    conductivy = true;
                    break;

                case "poison":
                    break;

                case  "cable":
                    takenCable = true;
                    break;  

            }
            if (iconComponent != null && iconComponent.icon != null)
            {
                interactionImage.sprite = iconComponent.icon; // Yeni ikon
                interactionImage.enabled = true; // Resmi göster
                Destroy(currentInteractableObject);
            }
        }

        if (interactionMessage != null)
        {
            interactionMessage.text = currentInteractableObject.name + " Alındı!";
            interactionMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(2f));
        }
    }

    private void HandleUsable()
    {
        usableName = currentInteractableObject.name;

        switch(usableName){
            case "priz":
                if(conductivy){
                    UseUsable();
                    conductivy = false;
                    }
                break;
            case "yazici":
                if(takenCable){
                    UseUsable();
                    takenCable = false;
                    }
                break;
        }
    }

    private void UseUsable(){
        if (interactionImage != null)
        {
            interactionImage.enabled = false; // Resmi gizle
            interactionImage.sprite = defaultImage; // Varsayılan görüntüe dön
        }
        if (interactionMessage != null)
        {
            interactionMessage.text =  interactableName + " Kullanıldı!";
            interactionMessage.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(2f));
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

