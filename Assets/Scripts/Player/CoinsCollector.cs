using UnityEngine;
using DG.Tweening;

public class CoinsCollector : MonoBehaviour
{
    UiManager uiManager;
    Transform player;

    public int coinsCollected, gemsCollected;

    private void Start()
    {
        uiManager = GameManager.instance.uiManager;
        player = PlayerManager.instance.playerController.mTransform;
    }

    public void ResetAll()
    {
        coinsCollected = 0;
        gemsCollected = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            other.transform.SetParent(player);
            float originalScale = other.transform.localScale.x;
            other.transform.DOLocalMove(Vector3.zero, .25f).SetEase(Ease.Linear).OnComplete(() => GiveReward(true, other.gameObject, originalScale));
            // other.transform.DOScale(0.1f, .25f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
            coinsCollected++;
            SoundManager.Instance.PlaySound(ClipName.CoinPick);
            uiManager.ShowCoin(true, true);
        }
        else if (other.CompareTag("Diamond"))
        {
            other.transform.SetParent(player);
            float originalScale = other.transform.localScale.x;
            other.transform.DOLocalMove(Vector3.zero, .25f).SetEase(Ease.Linear).OnComplete(() => GiveReward(false, other.gameObject, originalScale));
            // other.transform.DOScale(0.1f, .25f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);  
            gemsCollected++;
            SoundManager.Instance.PlaySound(ClipName.GemPick);
        }
    }

    void GiveReward(bool coin, GameObject obj, float originalScale)
    {
        obj.SetActive(false);
        obj.GetComponent<PoolableObject>().ReturnToThePool();
        // obj.transform.SetParent(PoolManager.instance.transform);
        // obj.transform.SetLocalScale(originalScale);
        PlayerPrefsContainer.PlayerCoins++;
        if(coin)
            GameManager.instance.uiManager.UpdateCoinsText();
        else
            GameManager.instance.uiManager.UpdateGemsText();
    }
}