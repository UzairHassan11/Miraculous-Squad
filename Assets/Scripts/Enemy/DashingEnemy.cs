using UnityEngine;
using DG.Tweening;

public class DashingEnemy : Enemy
{
    bool dashing;

    [SerializeField] float indicatorDelay, dashTime, dashSpeed;

    float currentDelay;

    [SerializeField] GameObject indicator;

    Material indicatorMat;

    protected override void Start()
    {
        base.Start();
        indicatorDelay = GameManager.instance.levelSelection.CurrentEnvironment.spellsDelay;
        currentDelay = indicatorDelay;

        indicatorMat = indicator.GetComponent<MeshRenderer>().material;
        indicatorMat.DOOffset(new Vector2(0, -1), 1).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);

        playerDetectionRange = GameManager.instance.levelSelection.CurrentEnvironment.punchingEnemyDR;

        healthController.ResetHealth(GameManager.instance.levelSelection.CurrentEnvironment.dashingEnemyHealth);
    }

    protected override void Update()
    {
        base.Update();

        if (isDead || !Triggered)
            return;

        if(!dashing)
        {
            if (currentDelay <= 0)
            {
                if (PlayerManager.instance.healthController.OutOfHeath)
                {
                    Triggered = false;
                    return;
                }
                else
                {
                    currentDelay = dashTime;
                    dashing = true;
                    
                }
                indicator.SetActive(false);
            }
            else
            {
                mTransform.LookTowards(chasingTarget);
                currentDelay -= Time.deltaTime;
                indicator.SetActive(true);
            }
        }
        else
        {
            mTransform.position += mTransform.forward * Time.deltaTime * dashSpeed;

            if (currentDelay <= 0)
            {
                if (PlayerManager.instance.healthController.OutOfHeath)
                {
                    Triggered = false;
                    return;
                }
                else
                {
                    currentDelay = indicatorDelay;
                    dashing = false;
                }
            }
            else
               currentDelay -= Time.deltaTime;
        }
    }
}