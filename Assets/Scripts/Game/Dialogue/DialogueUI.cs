using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DialogueUI : MonoBehaviour
{
    public Button skipButton;
    [SerializeField] Image actor1Image, actor2Image, dialogue1Image, dialogue2Image;
    [SerializeField] TextMeshProUGUI dialogue1Text, dialogue2Text;
    [SerializeField] Transform actor_1_pos, actor_2_pos;
    bool animating;

    Image actorImage;
    Image dialogueImage;
    TextMeshProUGUI dialogueText;

    DialogueSentence currentSentence;

    public void ShowPanel(bool state)
    {
        gameObject.SetActive(state);
    }

    Tween textTween;
    public void ShowDialogueSentence(DialogueSentence sentence, bool showOnLeft)
    {
        animating = true;
        //print("showOnLeft " + showOnLeft);
        actorImage = showOnLeft ? actor1Image : actor2Image;
        dialogueImage = showOnLeft ? dialogue1Image : dialogue2Image;
        dialogueText = showOnLeft ? dialogue1Text : dialogue2Text;
        currentSentence = sentence;

        actor1Image.gameObject.SetActive(showOnLeft);
        actor2Image.gameObject.SetActive(!showOnLeft);

        actorImage.sprite = DialogueController.instance.dialoguesData.GetDialogueActorsSprite(sentence.dialogueActor);
        actorImage.transform.DOMoveX(1000 * (showOnLeft ? -1 : 1), .25f).From(true).SetId("dialogue").SetEase(Ease.OutBack).SetRelative(true)
        //.OnComplete(() =>
        //{
            
        //}
        //)
        ;

        dialogueImage.transform.localScale = Vector3.zero;
        dialogueImage.transform.DOScale(1, .25f).SetDelay(.25f)
            //.From(true)
            .SetEase(Ease.OutBack).SetId("dialogue")
        //    .OnComplete(() =>
        //{
            
        //})
            ;

        //dialogueText.text.DOText(sentence.sentence, .5f, true)
        //    .SetEase(Ease.Linear).SetId("dialogue").OnComplete(() => { animating = false; });

        if (textTween != null)
            if (textTween.IsPlaying())
                textTween.Kill(true);

        dialogueText.text = "";
        textTween = DOTween.To(() => dialogueText.text, x => dialogueText.text = x, currentSentence.sentence, .5f)
            .SetDelay(.4f).SetEase(Ease.Linear).SetId("dialogue").OnComplete(() => { animating = false; });
        //animating = false;
        //dialogueText.text = currentSentence.sentence;
        //print(currentSentence.sentence);
    }

    public void OnClickDialoguePanel()
    {
        if(animating)
        {
            animating = false;
            DOTween.Kill("dialogue", true);
            dialogueImage.transform.localScale = Vector3.one;
            dialogueText.text = currentSentence.sentence;
        }
        else
        {
            DialogueController.instance.ShowSentence();
        }
    }
}
