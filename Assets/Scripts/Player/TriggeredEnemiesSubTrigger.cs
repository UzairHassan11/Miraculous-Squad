using UnityEngine;
using DG.Tweening;

public class TriggeredEnemiesSubTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy.isDead)
                return;
            PlayerManager.instance.powerUpsController.SlashEnemy(enemy);
            gameObject.SetActive(false);
        }
    }

    public void GetClosestEnemyWithoutTriggering()
    {
        gameObject.SetActive(true);
        transform.DOScale(5, .5f).SetLoops(2, LoopType.Yoyo).OnComplete(()=> gameObject.SetActive(false));
    }
}