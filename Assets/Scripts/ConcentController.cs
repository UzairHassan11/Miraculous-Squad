using UnityEngine;

public class ConcentController : MonoBehaviour
{
    public GameObject consentPanel;
    public ImplementPrivacySettings implementPrivacySettings;
    public static bool ConsentFormShown
    {
        set => PlayerPrefs.SetInt("ConsentFormShown",value ? 1:0);
        get => PlayerPrefs.GetInt("ConsentFormShown",0) == 1;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        consentPanel.SetActive(false);
        if (ConsentFormShown == false)
            ShowConcentPanel();
    }
    
    public void ShowConcentPanel()
    {
        consentPanel.SetActive(true);
    }

    public void OnClickAcceptButton()
    {
        ConsentFormShown = true;
        consentPanel.SetActive(false);
        implementPrivacySettings.Initialize();
    }
}
