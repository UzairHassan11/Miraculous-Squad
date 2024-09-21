using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/DialogueData", order = 2)]
public class DialoguesData : ScriptableObject
{
    [SerializeField] DialogueActor dialogueActors;
    [SerializeField] Sprite[] dialogueActorsSprites;

    public Sprite GetDialogueActorsSprite(DialogueActor dialogueActor) => dialogueActorsSprites[(int)dialogueActor];
}
public enum DialogueActor
{
    Player,
    Enemy,
    Boss
}