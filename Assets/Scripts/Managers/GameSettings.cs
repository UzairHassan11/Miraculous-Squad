using UnityEngine;
using DG.Tweening;
// using GameAssets.GameSet.GameDevUtils.Managers;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[System.Serializable]
public class MyToggle
{
    public Button[] buttons = null;
    public Sprite OnSprite = null;
    public Sprite OffSprite = null;

    public Image[] SpriteImages = null;

    public void AddListener(UnityAction call)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(call);
        }

        //SpriteImage = button.GetComponent<Image>();
    }//AddListener() end

    public void SetState(bool Toggle)
    {
        for (int i = 0; i < SpriteImages.Length; i++)
        {
            SpriteImages[i].sprite = Toggle ? OnSprite : OffSprite;
        }
    }
}//class end

[HideMonoScript]
public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance {get; private set;}
    [Title("GAME SETTINGS", null, titleAlignment: TitleAlignments.Centered)]
    [SerializeField] RectTransform SettingPanel  = null;
    [SerializeField] Button        SettingButton = null;
    [Space]
    [SerializeField] MyToggle MusicToggle  = null;
    [SerializeField] MyToggle SFXToggle    = null;
    [SerializeField] MyToggle HapticToggle = null;

    private bool CanToggle = true;
    private bool Toggle    = false;
    private float PosY     = 0f;

    public bool toggleStatusBg
    {
        set
        {
            SaveData.Instance.Music = value;
            SaveSystem.SaveProgress();
        }
        get
        {
            return SaveData.Instance.Music;
        }

    }
    public bool toggleStatusSFX
    {
        set
        {
            SaveData.Instance.SFX = value;
            SaveSystem.SaveProgress();
        }
        get
        {
            return SaveData.Instance.SFX;
        }

    }
    public bool toggleStatusHaptic
    {
        set
        {
            SaveData.Instance.Haptic = value;
            SaveSystem.SaveProgress();
        }
        get
        {
            return SaveData.Instance.Haptic;
        }
    }
    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }//if end
        else
        {
            DestroyImmediate(gameObject);
            return;
        }//else end
    }//Awake() end

    private void Start()
    {
        //SettingPanel.anchoredPosition = new Vector2(SettingPanel.anchoredPosition.x, -SettingPanel.anchoredPosition.y);
        //PosY = -SettingPanel.anchoredPosition.y;
        //SettingButton.onClick.RemoveAllListeners();
        //SettingButton.onClick.AddListener(TogglePanel);
        MusicToggle. AddListener(ToggleMusic);
        SFXToggle.   AddListener(ToggleSFX);
        HapticToggle.AddListener(ToggleHaptic);
        LoadSettings();
        //SettingButton.gameObject.SetActive(false);
    }//Start() end
    
    public void InitializeSettings() => SettingButton.gameObject.SetActive(true);

    private void TogglePanel()
    {
        if(!CanToggle)
            return;
        CanToggle = false;
        Toggle    = !Toggle;
        if(Toggle)
        {
            SettingPanel.DOAnchorPos3DY(PosY, 0.25f, false).OnComplete(()=>CanToggle = true);
            if(SoundManager.Instance)
                SoundManager.Instance.PlaySound((ClipName.Button));
            Invoke(nameof(ClosePanel), 3.0f);
        }//if end
        else   
            ClosePanel();
    }//TogglePanel() end

    public void ClosePanel()
    {
        CancelInvoke();
        Toggle = false;
        SettingPanel.DOAnchorPos3DY(-PosY, 0.25f, false).OnComplete(()=>CanToggle = true);
    }//ClosePanel() end

    private void LoadSettings()
    {
        MusicToggle.SetState(toggleStatusBg);
        SFXToggle.SetState(toggleStatusSFX);
        HapticToggle.SetState(toggleStatusHaptic);
    }//LoadSettings() end

    private void ToggleMusic()
    {
        toggleStatusBg = !toggleStatusBg;
        MusicToggle.SetState(toggleStatusBg);
        SoundManager.Instance.SetBgSoundSetting(toggleStatusBg);

        // if(AudioManager.Instance)
        // {
        //     AudioManager.Instance.SetBGSetting(SaveData.Instance.Music);
        //     AudioManager.Instance.ButtonClick();
        // }//if end
        // SaveSystem.SaveProgress();
    }//ToggleMusic() end

    private void ToggleSFX()
    {
        toggleStatusSFX = !toggleStatusSFX;
        SFXToggle.SetState(toggleStatusSFX);
       

        // if(AudioManager.Instance)
        // {
        //     AudioManager.Instance.ButtonClick();
        //     AudioManager.Instance.SetSFXSetting(SaveData.Instance.SFX);
        // }//if end
        // SaveSystem.SaveProgress();
    }//ToggleSFX() end

    private void ToggleHaptic()
    {
        toggleStatusHaptic = !toggleStatusHaptic;
        HapticToggle.SetState(toggleStatusHaptic);
        
    }//ToggleHaptic() end

    public void PlaySound(ClipName clipName)
    {
        if (true)
        {
            
        }
    }

}