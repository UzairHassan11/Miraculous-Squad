using UnityEngine;
using Sirenix.OdinInspector;

public class ExitIndicator : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            DoTheThing();
        }
    }

    [Button]
    [ContextMenu("DoTheThing")]
    public void DoTheThing()
    {
        PlayerManager.instance.playerController.EnableControls(false);
        GameManager.instance.levelManager.SectionCompleted();
    }

    public void SetVisibility(bool state)
    {
        gameObject.SetActive(state);
    }
}