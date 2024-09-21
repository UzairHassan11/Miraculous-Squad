using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MM_Footer : MonoBehaviour
{
    public MM_FooterButton[] footerButtons;

    [SerializeField] RectTransform[] menuScreens;

    int currentButton = 0;

    [SerializeField] Image container;

    float containerWidth, currentXpos, perBtnWdt, screenWidth;

    public Image currentButtonIndicator;

    [SerializeField] float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        containerWidth = container.rectTransform.rect.width;
        perBtnWdt = containerWidth / footerButtons.Length;
        screenWidth = Screen.width;
        SetButtonVisual(2);
    }

    void Update()
    {
        currentButtonIndicator.rectTransform.localPosition =
            Vector3.Lerp(currentButtonIndicator.rectTransform.localPosition,
            new Vector3(currentXpos, currentButtonIndicator.rectTransform.localPosition.y, 0), Time.deltaTime * moveSpeed);
    }

    public void SetButtonVisual(int n)
    {
        if (currentButton == n)
            return;

        SwitchMenu(n);

        for (int i = 0; i < footerButtons.Length; i++)
        {
            footerButtons[i].titleText.SetActive(i == n);
        }

        currentButton = n;
        currentXpos = perBtnWdt * (currentButton - (int)(footerButtons.Length / 2));
        SoundManager.Instance.PlaySound(ClipName.Button);
    }

    void SwitchMenu(int n)
    {
        for (int i = 0; i < menuScreens.Length; i++)
        {
            menuScreens[i].gameObject.SetActive(i == n);
            // print((-n + i));
            // menuScreens[i].DOAnchorPos(new Vector2(screenWidth * (-n + i), 0), .35f);
        }
    }
}