using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interaction : MonoBehaviour
{
    public Transform rayOrigin; // Ray'in başlayacağı obje (örneğin, kamera)
    public float interactionDistance = 3f; // Etkileşim mesafesi
    public KeyCode interactionKey = KeyCode.F; // Etkileşim tuşu

    public Image interactionImage; // Sağ üstteki UI Image
    public TMP_Text interactionMessage; // Alt ortadaki UI Text
    private Sprite defaultImage; // Varsayılan boş görüntü

    void Start()
    {
        // UI Image başlangıçta gizli olmalı
        if (interactionImage != null)
        {
            defaultImage = interactionImage.sprite;
            interactionImage.enabled = false;
        }

        // UI Text başlangıçta gizli olmalı
        if (interactionMessage != null)
        {
            interactionMessage.text = "";
            interactionMessage.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        CheckInteraction();
    }

    void CheckInteraction()
    {
        if (rayOrigin == null)
        {
            Debug.LogWarning("RayOrigin atanmadı! Lütfen bir Transform ekleyin.");
            return;
        }

        // Ray'i rayOrigin pozisyonundan ileri doğru oluştur
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;

        // Ray bir nesneye çarparsa ve etkileşim mesafesi içindeyse
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            // Çarpılan nesnenin etiketi "Interactable" ise
            if (hit.collider.CompareTag("Interactable"))
            {
                Debug.Log("Etkileşim yapılabilir! F tuşuna basabilirsiniz.");

                // Kullanıcı F tuşuna bastığında
                if (Input.GetKeyDown(interactionKey))
                {
                    Debug.Log("Etkileşim gerçekleşti: " + hit.collider.gameObject.name);

                    // UI Image'i güncelle ve göster
                    if (interactionImage != null)
                    {
                        InteractionIcon iconComponent = hit.collider.gameObject.GetComponent<InteractionIcon>();
                        if (iconComponent != null && iconComponent.icon != null)
                        {
                            interactionImage.sprite = iconComponent.icon; // Yeni ikon
                            interactionImage.enabled = true; // Image'i göster
                        }
                        else
                        {
                            Debug.LogWarning("Etkileşim yapılan nesnede ikon atanmadı!");
                        }
                    }

                    // Etkileşim yapılan nesneyi yok et
                    Destroy(hit.collider.gameObject);
                }
            }

            // Çarpılan nesnenin etiketi "Usable" ise (ikinci tür obje)
            if (hit.collider.CompareTag("Usable"))
            {
                Debug.Log("İkinci obje ile etkileşim yapılabilir!");

                // Kullanıcı F tuşuna bastığında
                if (Input.GetKeyDown(interactionKey))
                {
                    Debug.Log("İkinci obje kullanıldı: " + hit.collider.gameObject.name);

                    // Sağ üstteki Image'i gizle
                    if (interactionImage != null)
                    {
                        interactionImage.sprite = defaultImage; // Varsayılan görüntüye sıfırla
                        interactionImage.enabled = false; // Image'i gizle
                    }

                    // UI Text'i güncelle ve göster
                    if (interactionMessage != null)
                    {
                        interactionMessage.text = hit.collider.gameObject.name + " kullanıldı!";
                        interactionMessage.gameObject.SetActive(true);

                        // Mesajı bir süre sonra gizlemek için coroutine başlat
                        StartCoroutine(HideMessageAfterDelay(2f));
                    }
                }
            }
        }
    }

    // Mesajı belli bir süre sonra gizlemek için coroutine
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
