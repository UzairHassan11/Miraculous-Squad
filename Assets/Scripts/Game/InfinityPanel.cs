using UnityEngine;
using UnityEngine.Events;
public class InfinityPanel : MonoBehaviour
{
    [SerializeField] CanvasGroup gameplayPanel;
    public UnityAction onClickEvents;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onClickEvents?.Invoke();
            onClickEvents=null;
            gameObject.SetActive(false);
            gameplayPanel.alpha = 1;
            PlayerManager.instance.enemiesTrigger.shootingRangeVisualiser.SetToMaxScale();
            PlayerManager.instance.healthController.SetHealthToMax();
        }
    }

    private void OnEnable()
    {
        gameplayPanel.alpha = 0;
    }
}