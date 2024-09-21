using UnityEngine;

public class PrivacyLinksController : MonoBehaviour
{

    public string termsOfUseLink;
    public string privacyPolicyLink;


    public void OnClickPrivacyPolicyLink()
    {
        Application.OpenURL(privacyPolicyLink);
    }
    
    public void OnClickTermsOfUseLink()
    {
        Application.OpenURL(termsOfUseLink);
    }
}
