using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    public bool startFromLeft;
    public DialogueSentence[] dialogueSentences;
    public UnityAction postDialogueActions;
}