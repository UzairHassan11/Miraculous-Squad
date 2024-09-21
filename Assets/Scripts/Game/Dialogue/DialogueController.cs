using UnityEngine;

public class DialogueController : MonoBehaviour
{
    #region singleton

    public static DialogueController instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public DialoguesData dialoguesData;
    [SerializeField] DialogueUI dialogueUI;

    bool showFromLeft;
    int currentSentenceIndex;
    Dialogue currentDialogue;

    [SerializeField] Dialogue exampleDialogue;

    // Start is called before the first frame update
    void Start()
    {
        dialogueUI.skipButton.onClick.AddListener(() => EndDialogue());
        //ShowDialogue(exampleDialogue);
    }

    public void ShowDialogue(Dialogue dialogue)
    {
        //Time.timeScale = 0;
        currentDialogue = dialogue;
        showFromLeft = dialogue.startFromLeft;
        currentSentenceIndex = 0;
        dialogueUI.ShowPanel(true);
        ShowSentence();
    }

    public void ShowSentence()
    {
        if(currentSentenceIndex == currentDialogue.dialogueSentences.Length)
        {
            EndDialogue();
            return;
        }
        dialogueUI.ShowDialogueSentence(currentDialogue.dialogueSentences[currentSentenceIndex], showFromLeft);
        showFromLeft = !showFromLeft;
        currentSentenceIndex++;
    }

    void EndDialogue()
    {
        if (currentDialogue.postDialogueActions != null)
        {
            currentDialogue.postDialogueActions.Invoke();
            currentDialogue.postDialogueActions = null;
        }
        dialogueUI.ShowPanel(false);
        //Time.timeScale = 1;
    }
}
