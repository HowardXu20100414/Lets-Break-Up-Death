using UnityEngine;
using UnityEngine.Events; // For events like triggering animations or portal

public class NPCDialogueTrigger : MonoBehaviour
{
    public bool triggerOnStart = false;
    [Header("Dialogue Data")]
    public DialogueData dialogueToPlay; // Assign your DialogueData ScriptableObject here

    [Header("Character Animation (Optional)")]
    public GameObject characterGameObject; // The GameObject that has an Animator (e.g., "Death")
    private Animator characterAnimator;
    public string talkingAnimationState = "Talking";
    public string idleAnimationState = "Idle";
    public string angryAnimationState = "Angry"; // For specific lines

    [Header("Dialogue Events (Optional)")]
    public UnityEvent onDialogueStart; // Events to trigger when dialogue begins
    public UnityEvent onDialogueEnd;   // Events to trigger when dialogue finishes

    void Awake()
    {
        if (characterGameObject != null)
        {
            characterAnimator = characterGameObject.GetComponent<Animator>();
            if (characterAnimator == null)
            {
                Debug.LogWarning($"No Animator found on {characterGameObject.name}. Character animations will not play.", this);
            }
        }
        if (triggerOnStart)
        {
            TriggerDialogue();
        }
    }

    // Call this method to initiate the dialogue from this NPC
    public void TriggerDialogue()
    {
        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager instance not found! Make sure it's in the scene.", this);
            return;
        }
        if (dialogueToPlay == null)
        {
            Debug.LogWarning("No DialogueData assigned to this NPCDialogueTrigger.", this);
            return;
        }

        onDialogueStart?.Invoke(); // Trigger events set for dialogue start

        // Start the dialogue and provide a callback for when it completes
        DialogueManager.Instance.StartDialogue(dialogueToPlay, OnDialogueComplete);

        // Example of handling animations based on lines (more complex logic might need to be in DialogueManager with a delegate or event system)
        // For simple cases, you might handle the initial animation here
        if (characterAnimator != null)
        {
            characterAnimator.Play(talkingAnimationState);
        }
    }

    private void OnDialogueComplete()
    {
        onDialogueEnd?.Invoke(); // Trigger events set for dialogue end

        // Reset character animation to idle after dialogue
        if (characterAnimator != null)
        {
            characterAnimator.Play(idleAnimationState);
        }
        Debug.Log("NPC specific dialogue complete actions executed.");
    }

    // You can add more methods here to control specific animations or events during dialogue
    // For example, if you want specific animations for specific dialogue lines, 
    // you would need to extend DialogueManager to include events for each line
    // or use a more robust custom event system.
    // For the "Angry" animation on line 2, you'd need to adapt the DialogueManager
    // or implement a system that notifies this script when a specific line is displayed.
    // A simpler approach for the "Angry" animation is to set it up in the Animator with transitions
    // that are triggered by parameters, and then set those parameters from here based on dialogue events.
}