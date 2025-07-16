using System.Collections;
using TMPro; // Required for TextMeshPro
using UnityEngine;
using UnityEngine.UI; // Required for Button

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    public float typingSpeed = 0.05f; // Speed at which text appears letter by letter

    private DialogueData currentDialogue;
    private int currentLineIndex;
    private Coroutine typingCoroutine;
    private bool isTyping; // To prevent input while text is typing

    public static DialogueManager Instance { get; private set; } // Singleton pattern

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Ensure UI elements are assigned
        if (dialoguePanel == null || nameText == null || dialogueText == null || continueButton == null)
        {
            Debug.LogError("Dialogue UI elements not assigned in DialogueManager!", this);
            enabled = false; // Disable script if critical elements are missing
        }
        else
        {
            continueButton.onClick.AddListener(DisplayNextLine); // Add listener for the button
            dialoguePanel.SetActive(false); // Hide panel initially
        }
    }

    public void StartDialogue(DialogueData dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true); // Show dialogue panel

        DisplayNextLine(); // Start displaying the first line
    }

    void DisplayNextLine()
    {
        if (isTyping)
        {
            // If currently typing, complete the line instantly
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            dialogueText.text = currentDialogue.conversation[currentLineIndex - 1].line; // Complete the previous line
            isTyping = false;
            return;
        }

        if (currentLineIndex < currentDialogue.conversation.Length)
        {
            DialogueLine line = currentDialogue.conversation[currentLineIndex];
            nameText.text = line.characterName;

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeLine(line.line));
            currentLineIndex++;
        }
        else
        {
            EndDialogue(); // No more lines, end conversation
        }
    }

    IEnumerator TypeLine(string lineToType)
    {
        isTyping = true;
        dialogueText.text = ""; // Clear previous text
        foreach (char letter in lineToType.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false); // Hide dialogue panel
        currentDialogue = null;
        currentLineIndex = 0;
        isTyping = false;
        // Optionally, re-enable player movement here
        Debug.Log("Dialogue ended!");
    }
}