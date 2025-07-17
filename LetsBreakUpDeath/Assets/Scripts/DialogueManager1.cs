using System.Collections;
using TMPro; // Required for TextMeshPro
using UnityEngine;
using UnityEngine.UI; // Required for Button

public class DialogueManager1 : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    public float typingSpeed = 0.05f; // Speed at which text appears letter by letter

    // --- Variables for Smooth Scaling (only if you re-introduce an object to scale) ---
    // public float targetScaleMultiplier = 1.5f;
    // public float scaleDuration = 1.0f;
    // private Vector3 initialScaledObjectScale; // If scaling another object

    private DialogueData currentDialogue;
    private int currentLineIndex;
    private Coroutine typingCoroutine;
    // private Coroutine currentAnimationCoroutine; // Removed: only for Death
    // private Coroutine currentScaleCoroutine; // Removed: only for Death or specific scalable object
    private bool isTyping; // To prevent input while text is typing

    public DialogueData playerDialogue; // Drag your DialogueData ScriptableObject here in the Inspector
    public static DialogueManager1 Instance { get; private set; } // Singleton pattern

    void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            // Optional: If this manager should persist across scene loads, uncomment next line
            // DontDestroyOnLoad(gameObject);
        }
        //else // This crucial 'else' block was missing!
        //{
        //    Destroy(gameObject); // Destroy this duplicate instance
        //    return; // Exit Awake to prevent further execution on this destroyed object
        //}

        // Ensure UI elements are assigned
        if (dialoguePanel == null || nameText == null || dialogueText == null || continueButton == null)
        {
            Debug.LogError("Dialogue UI elements not assigned in DialogueManager! Please assign them in the Inspector.", this);
            enabled = false; // Disable script if critical elements are missing
        }
        else
        {
            continueButton.onClick.AddListener(DisplayNextLine); // Add listener for the button
            dialoguePanel.SetActive(false); // Hide panel initially
        }
    }

    void Start()
    {
        // Start dialogue here to ensure all Awakes have completed
        if (playerDialogue != null && playerDialogue.conversation != null && playerDialogue.conversation.Length > 0)
        {
            StartDialogue(playerDialogue);
        }
        else
        {
            Debug.LogWarning("Player Dialogue Data is not assigned or is empty. Dialogue will not start.", this);
            EndDialogue(); // Immediately end if there's no dialogue to show
        }
    }

    public void StartDialogue(DialogueData dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true); // Show dialogue panel

        // Removed Death-related scale stopping from here
        // if (currentScaleCoroutine != null)
        // {
        //     StopCoroutine(currentScaleCoroutine);
        //     currentScaleCoroutine = null;
        // }

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
            // Use currentLineIndex - 1 because currentLineIndex has already been incremented
            // for the NEXT line in the previous call of DisplayNextLine.
            // If this is the very first line being skipped, currentLineIndex - 1 would be -1.
            // Handle this edge case or ensure it's not called on the first line skip.
            if (currentLineIndex > 0 && currentDialogue != null && currentDialogue.conversation != null && currentLineIndex - 1 < currentDialogue.conversation.Length)
            {
                dialogueText.text = currentDialogue.conversation[currentLineIndex - 1].line;
            }
            isTyping = false;
            return;
        }

        // Removed Death-related animation and scale stopping from here
        // if (currentAnimationCoroutine != null) { StopCoroutine(currentAnimationCoroutine); currentAnimationCoroutine = null; }
        // if (currentScaleCoroutine != null) { StopCoroutine(currentScaleCoroutine); currentScaleCoroutine = null; }


        if (currentDialogue != null && currentLineIndex < currentDialogue.conversation.Length)
        {
            DialogueLine line = currentDialogue.conversation[currentLineIndex];
            nameText.text = line.characterName;

            // Removed Death-related animation/scaling logic from here

            // Start typing the new line
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeLine(line.line));
            currentLineIndex++;
        }
        else // No more lines, end conversation
        {
            Debug.Log("Dialogue ended!");
            EndDialogue();
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

    public void EndDialogue()
    {
        print("did end dialogue");
        dialoguePanel.SetActive(false); // Hide dialogue panel
        currentDialogue = null;
        currentLineIndex = 0;
        isTyping = false;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        // Removed Death-related coroutine stopping from here
        // if (currentAnimationCoroutine != null) { StopCoroutine(currentAnimationCoroutine); currentAnimationCoroutine = null; }
        // if (currentScaleCoroutine != null) { StopCoroutine(currentScaleCoroutine); currentScaleCoroutine = null; }

        // Example: If you want to load a new scene after dialogue ends
        // GameManager.instance.LoadScene("Level");
    }
}