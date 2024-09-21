using UnityEngine;

[System.Serializable]
public class DialogueSentence
{
    public DialogueActor dialogueActor;

    [TextArea]
    public string sentence;
}