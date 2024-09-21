using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EnemySpellLine : MonoBehaviour
{
    [SerializeField] float damage, indicatorTime;

    [SerializeField] GameObject spellLineIndicator, spellLine;

    Enemy enemy;

    [SerializeField]
    Collider mCollider;

    bool playerTriggered;


    public void Init(Enemy _enemy)
    {
        enemy = _enemy;
        damage = GameManager.instance.levelSelection.CurrentEnvironment.enemyDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (playerTriggered)
                return;
            playerTriggered = true;
            PlayerManager.instance.TakeDamage(damage);
        }
    }

    [ContextMenu("Shoot")]
    public void Shoot()
    {
        StartCoroutine("LineCastingProcedure");
    }

    IEnumerator LineCastingProcedure()
    {
        transform.localScale = Vector3.one;
        spellLineIndicator.SetActive(true);

        yield return new WaitForSeconds(indicatorTime);

        spellLineIndicator.SetActive(false);
        spellLine.gameObject.SetActive(true);

        mCollider.enabled = true;
        playerTriggered = false;

        if (enemy)
            enemy.animationController.SetTrigger("Cast");

        transform.localScale = Vector3.one;

        transform.DOScaleX(0, .5f).SetEase(Ease.InBack);

        yield return new WaitForSeconds(.5f);

        spellLine.gameObject.SetActive(false);
        mCollider.enabled = false;

        if (enemy)
            if (!enemy.isDead)
                enemy.Triggered = true;
    }

    public void KillLine()
    {
        StopCoroutine("LineCastingProcedure");
        spellLineIndicator.SetActive(false);
        spellLine.SetActive(false);
        SetVisibility(false);
    }

    public void SetVisibility(bool state)
    {
        gameObject.SetActive(state);
    }
}