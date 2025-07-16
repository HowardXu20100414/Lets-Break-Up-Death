using UnityEngine;

[System.Serializable] // Allows ConversationData to be serialized and appear in the Inspector
public class DialogueLine
{
    public string characterName;
    [TextArea(3, 5)] // Makes the string field a multi-line text area in Inspector
    public string line;
    // Add more fields if needed, e.g., sprite for character portrait, audio clip
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue Conversation")]
public class DialogueData : ScriptableObject
{
    public DialogueLine[] conversation;
}