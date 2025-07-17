using System.Collections;
using TMPro; // Required for TextMeshPro
using UnityEngine;
using UnityEngine.UI; // Required for Button

public class DialogueManager : MonoBehaviour
{
    public GameObject death; // Drag your Death GameObject here in the Inspector
    Animator deathAnim; // Animator component of the Death GameObject
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    public float typingSpeed = 0.05f; // Speed at which text appears letter by letter
    private float talkingAnimationSwitchSpeed = .2f; // How fast Death switches between Idle/Talking

    private DialogueData currentDialogue;
    private int currentLineIndex;
    private Coroutine typingCoroutine;
    private Coroutine currentAnimationCoroutine; // NEW: To control the animation switching coroutine
    private bool isTyping; // To prevent input while text is typing

    public static DialogueManager Instance { get; private set; } // Singleton pattern

    void Awake()
    {
        // Get Death's Animator component
        if (death != null)
        {
            deathAnim = death.GetComponent<Animator>();
            if (deathAnim == null)
            {
                Debug.LogError("Death GameObject assigned but has no Animator component!", this);
            }
        }
        else
        {
            Debug.LogError("Death GameObject is not assigned in DialogueManager! Please assign it in the Inspector.", this);
        }

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
        // If currently typing, complete the line instantly and return
        if (isTyping)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            // Ensure the text is fully displayed for the previous line
            dialogueText.text = currentDialogue.conversation[currentLineIndex - 1].line;
            isTyping = false;
            return;
        }

        // Stop any ongoing animation coroutine before starting a new one
        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
        }

        if (currentLineIndex < currentDialogue.conversation.Length)
        {
            DialogueLine line = currentDialogue.conversation[currentLineIndex];
            nameText.text = line.characterName;

            // --- NEW: Animation Logic based on line index ---
            if (deathAnim != null) // Only attempt to control animation if Animator exists
            {
                if (currentLineIndex == 0 || currentLineIndex == 1) // First two lines
                {
                    currentAnimationCoroutine = StartCoroutine(AnimateTalkingRapidly());
                }
                else if (currentLineIndex == 2) // Third line
                {
                    deathAnim.Play("Angry"); // Play the "Angry" animation directly
                }
                else // For any subsequent lines, default to Idle (or specific anim if needed)
                {
                    deathAnim.Play("Idle");
                }
            }

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
        dialogueText.text = ""; // Clear previous text
        foreach (char letter in lineToType.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    // NEW: Coroutine for rapidly switching between Idle and Talking
    IEnumerator AnimateTalkingRapidly()
    {
        while (true) // Loop indefinitely until stopped
        {
            deathAnim.Play("Talking");
            yield return new WaitForSeconds(talkingAnimationSwitchSpeed + Random.Range(-.2f, .2f));
            deathAnim.Play("Idle");
            yield return new WaitForSeconds(talkingAnimationSwitchSpeed + Random.Range(-.2f, .2f));
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false); // Hide dialogue panel
        currentDialogue = null;
        currentLineIndex = 0;
        isTyping = false;

        // NEW: Stop any ongoing animation coroutine and reset Death's animation to Idle
        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
            currentAnimationCoroutine = null;
        }
        if (deathAnim != null)
        {
            deathAnim.Play("Idle"); // Reset to idle when dialogue ends
        }

        // Optionally, re-enable player movement here
        Debug.Log("Dialogue ended!");
    }
}