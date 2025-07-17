using System.Collections;
using TMPro; // Required for TextMeshPro
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; // Required for Button

public class DialogueManager : MonoBehaviour
{
    int sceneNumber = 1;
    public GameObject death; // Drag your Death GameObject here in the Inspector
    Animator deathAnim; // Animator component of the Death GameObject
    SpriteRenderer deathRend;
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    public float typingSpeed = 0.05f; // Speed at which text appears letter by letter
    private float talkingAnimationSwitchSpeed = .2f; // How fast Death switches between Idle/Talking

    // --- NEW: Variables for Smooth Scaling ---
    public float targetScaleMultiplier = 1.5f; // The desired final scale multiplier (e.g., 1.5 for 1.5x original size)
    public float scaleDuration = 1.0f; // How long the scaling animation should take (in seconds)
    private Vector3 initialDeathScale; // To store Death's original scale

    private DialogueData currentDialogue;
    private int currentLineIndex;
    private Coroutine typingCoroutine;
    private Coroutine currentAnimationCoroutine; // NEW: To control the animation switching coroutine
    private Coroutine currentScaleCoroutine; // NEW: To control the scaling coroutine
    private bool isTyping; // To prevent input while text is typing

    public DialogueData playerDialogue;
    public static DialogueManager Instance { get; private set; } // Singleton pattern

    void Awake()
    {
        if (sceneNumber == 1)
        {
            if (death != null)
            {
                deathAnim = death.GetComponent<Animator>();
                if (deathAnim == null)
                {
                    Debug.LogError("Death GameObject assigned but has no Animator component!", this);
                }
                // Store the initial scale of Death
                initialDeathScale = death.transform.localScale;
            }
            else
            {
                Debug.LogError("Death GameObject is not assigned in DialogueManager! Please assign it in the Inspector.", this);
            }

            deathRend = death.GetComponent<SpriteRenderer>();
        }

        // Get Death's Animator component
        

        if (Instance == null)
        {
            Instance = this;
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
        if (sceneNumber == 1)
        {
            death.transform.localScale = initialDeathScale;
        }
        // Ensure Death is at its initial scale when starting dialogue
        // Also stop any ongoing scale coroutine
        if (currentScaleCoroutine != null)
        {
            StopCoroutine(currentScaleCoroutine);
            currentScaleCoroutine = null;
        }

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
            currentAnimationCoroutine = null; // Set to null after stopping
        }

        // Stop any ongoing scale coroutine before potentially starting a new one
        if (currentScaleCoroutine != null)
        {
            StopCoroutine(currentScaleCoroutine);
            currentScaleCoroutine = null; // Set to null after stopping
        }

        if (currentLineIndex < currentDialogue.conversation.Length)
        {
            DialogueLine line = currentDialogue.conversation[currentLineIndex];
            nameText.text = line.characterName;
            if (sceneNumber == 1) 
            {
                if (deathAnim != null) // Only attempt to control animation if Animator exists
                {
                    if (currentLineIndex == 0 || currentLineIndex == 1) // First two lines
                    {
                        deathRend.sortingOrder = 0; // Ensure Death is in foreground initially
                        death.transform.localScale = initialDeathScale; // Reset scale for these lines if it was changed
                        currentAnimationCoroutine = StartCoroutine(AnimateTalkingRapidly());
                    }
                    else if (currentLineIndex == 2) // Third line
                    {
                        deathRend.sortingOrder = 0; // Ensure Death is in foreground
                        death.transform.localScale = initialDeathScale; // Reset scale for this line
                        deathAnim.Play("Angry"); // Play the "Angry" animation directly
                    }
                    else // For any subsequent lines (Line 3 and beyond, or currentLineIndex >= 3)
                    {
                        deathRend.sortingOrder = 100; // Move Death to background/higher sorting layer
                                                      // ADD SMOOTH SIZE INCREASE HERE
                        currentScaleCoroutine = StartCoroutine(SmoothlyScaleDeath(targetScaleMultiplier, scaleDuration));
                        deathAnim.Play("Angry"); // Play "Angry" animation
                    }
                }
            }
            // --- Animation and Scaling Logic based on line index 

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

    // Coroutine for rapidly switching between Idle and Talking
    IEnumerator AnimateTalkingRapidly()
    {
        while (true) // Loop indefinitely until stopped
        {
            deathAnim.Play("Talking");
            yield return new WaitForSeconds(talkingAnimationSwitchSpeed + Random.Range(-.075f, .075f));
            deathAnim.Play("Idle");
            yield return new WaitForSeconds(talkingAnimationSwitchSpeed + Random.Range(-.075f, .075f));
        }
    }

    // --- NEW: Coroutine for Smoothly Scaling Death GameObject ---
    IEnumerator SmoothlyScaleDeath(float scaleFactor, float time)
    {
        Vector3 startScale = death.transform.localScale;
        Vector3 endScale = initialDeathScale * scaleFactor;
        float timer = 0f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            float t = timer / time;
            t = Mathf.SmoothStep(0f, 1f, t); // Apply smoothstep for smoother animation
            death.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null; // Wait for the next frame
        }
        death.transform.localScale = endScale; // Ensure it reaches the exact target scale
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false); // Hide dialogue panel
        currentDialogue = null;
        currentLineIndex = 0;
        isTyping = false;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        if (currentAnimationCoroutine != null)
        {
            StopCoroutine(currentAnimationCoroutine);
            currentAnimationCoroutine = null;
        }
        if (currentScaleCoroutine != null)
        {
            StopCoroutine(currentScaleCoroutine);
            currentScaleCoroutine = null;
        }
        // Optionally, re-enable player movement here
        GameManager.instance.LoadScene("Level");
        if (sceneNumber == 1)
        {
            sceneNumber = 2;
        }
    }
}