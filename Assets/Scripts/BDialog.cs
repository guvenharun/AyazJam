using UnityEngine;
using TMPro;
using UnityEngine.UI; // UI Image için gerekli

public class DialogueTrigger : MonoBehaviour
{
    public GameObject dialogueUI; // UI Panel referansı
    public TMP_Text dialogueText; // Metin alanı referansı
    
    public Image dialogueImage; // Fotoğraf alanı referansı
    public Sprite[] dialogueSprites; // Diyalog fotoğrafları
    public string[] dialogueLines; // Diyalog metinleri
    private int currentLineIndex = 0;
    private bool playerInRange = false; // Oyuncunun menzilde olup olmadığını kontrol etmek için

    private bool isDisplaying = false; // Mesajları sırayla göstermek için

    void Start()
    {
        // UI panelini başlangıçta gizle
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Oyuncu etiketiyle kontrol
        {
            playerInRange = true; // Oyuncu menzile girdi
            currentLineIndex = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false; // Oyuncu menzilden çıktı
            if (dialogueUI != null)
            {
                dialogueUI.SetActive(false); // Paneli gizle
            }
            StopAllCoroutines(); // Mesaj gösterme işlemini durdur
            isDisplaying = false;
        }
    }

    private void ShowDialogue()
    {
        if (dialogueLines.Length > 0)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
            if (dialogueSprites.Length > currentLineIndex && dialogueImage != null)
            {
                dialogueImage.sprite = dialogueSprites[currentLineIndex]; // Fotoğrafı göster
            }
        }
    }

    private System.Collections.IEnumerator DisplayDialogueLines()
    {
        isDisplaying = true;
        while (currentLineIndex < dialogueLines.Length)
        {
            ShowDialogue();
            yield return new WaitForSeconds(3f);
            currentLineIndex++;
        }

        // Diyalog bittiğinde paneli gizle
        yield return new WaitForSeconds(3f); // Diyalog bitiminde 3 saniye bekle
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false);
        }
        isDisplaying = false;
    }

    public void NextLine()
    {
        if (!isDisplaying && currentLineIndex < dialogueLines.Length - 1)
        {
            currentLineIndex++;
            ShowDialogue();
        }
        else
        {
            if (dialogueUI != null)
            {
                dialogueUI.SetActive(false); // Diyalog bittiğinde paneli gizle
            }
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (!dialogueUI.activeSelf)
            {
                dialogueUI.SetActive(true); // Diyaloğu başlat
                currentLineIndex = 0; // Diyaloğu sıfırdan başlat
                StartCoroutine(DisplayDialogueLines());
            }
        }
    }
}
