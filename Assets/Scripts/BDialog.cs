using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    public KeyCode interactionKey = KeyCode.F; // Etkileşim tuşu

    public GameObject dialogueUI; // Diyalog penceresi UI
    public TMP_Text dialogueText; // Diyalog içeriği için TMP_Text
    //asdasd

    private bool isPlayerNear = false; // Oyuncunun NPC'ye yakın olup olmadığını kontrol etmek için bayrak
    private bool dialogueActive = false; // Diyalog aktif mi

    [TextArea]
    public string[] dialogueLines; // Diyalog metinleri
    private int currentLineIndex = 0; // Mevcut diyalog satırı

    void Start()
    {
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false); // Başlangıçta diyalog UI'yi gizle
        }
    }

    void Update()
    {
        // Oyuncu yakınsa ve F tuşuna basıldığında
        if (isPlayerNear && Input.GetKeyDown(interactionKey))
        {
            if (!dialogueActive)
            {
                StartDialogue();
            }
            else
            {
                ShowNextDialogueLine();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            EndDialogue();
        }
    }

    private void StartDialogue()
    {
        if (dialogueUI != null && dialogueLines.Length > 0)
        {
            dialogueUI.SetActive(true); // Diyalog penceresini göster
            dialogueActive = true;
            currentLineIndex = 0; // Diyalog başından başla
            dialogueText.text = dialogueLines[currentLineIndex]; // İlk satırı göster
        }
    }

    private void ShowNextDialogueLine()
    {
        currentLineIndex++;

        if (currentLineIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLineIndex]; // Sonraki satırı göster
        }
        else
        {
            EndDialogue(); // Diyalog bittiğinde sonlandır
        }
    }

    private void EndDialogue()
    {
        if (dialogueUI != null)
        {
            dialogueUI.SetActive(false); // Diyalog penceresini gizle
        }
        dialogueActive = false;
    }
}
