using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events; // For optional dialogue end callback

public class DialogueManager : MonoBehaviour
{
    public bool isEnded;


    // ... (UI References and Dialogue Settings remain the same) ...
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    [Header("Dialogue Settings")]
    public float typingSpeed = 0.05f;

    private DialogueData currentDialogue;
    public int currentLineIndex;
    private Coroutine typingCoroutine;
    private bool isTyping;
    private UnityAction onDialogueCompleteCallback;
    private UnityAction<int> onLineDisplayedCallback; // NEW: Callback for when a line is displayed

    public static DialogueManager Instance { get; private set; }

    void Awake()
    {
        // ... (Singleton and UI assignment checks remain the same) ...
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (dialoguePanel == null || nameText == null || dialogueText == null || continueButton == null)
        {
            Debug.LogError("Dialogue UI elements not assigned in DialogueManager!", this);
            enabled = false;
        }
        else
        {
            continueButton.onClick.AddListener(DisplayNextLine);
            dialoguePanel.SetActive(false);
        }
    }

    /// <summary>
    /// Starts a new dialogue sequence.
    /// </summary>
    /// <param name="dialogue">The DialogueData ScriptableObject containing the conversation.</param>
    /// <param name="onComplete">An optional UnityAction to call when the dialogue finishes.</param>
    /// <param name="onLineDisplayed">NEW: An optional UnityAction<int> to call with the line index when each line is displayed.</param>
    public void StartDialogue(DialogueData dialogue, UnityAction onComplete = null, UnityAction<int> onLineDisplayed = null)
    {
        if (dialogue == null)
        {
            Debug.LogError("Attempted to start dialogue with null DialogueData.", this);
            return;
        }

        currentDialogue = dialogue;
        currentLineIndex = 0;
        onDialogueCompleteCallback = onComplete;
        onLineDisplayedCallback = onLineDisplayed; // NEW: Store the line displayed callback

        dialoguePanel.SetActive(true);

        DisplayNextLine(); // Start displaying the first line
    }

    void DisplayNextLine()
    {
        // If currently typing, complete the line instantly and return
        if (isTyping)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            dialogueText.text = currentDialogue.conversation[currentLineIndex - 1].line;
            isTyping = false;
            return;
        }

        if (currentLineIndex < currentDialogue.conversation.Length)
        {
            DialogueLine line = currentDialogue.conversation[currentLineIndex];
            nameText.text = line.characterName;

            // NEW: Invoke the callback *before* typing the line, passing the currentLineIndex
            onLineDisplayedCallback?.Invoke(currentLineIndex);

            // Start typing the new line
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
        dialogueText.text = "";
        foreach (char letter in lineToType.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentDialogue = null;
        currentLineIndex = 0;
        isTyping = false;

        onDialogueCompleteCallback?.Invoke();
        onDialogueCompleteCallback = null;
        onLineDisplayedCallback = null; // NEW: Clear this callback too
        isEnded = true;
        Debug.Log("Dialogue ended!");
    }
}