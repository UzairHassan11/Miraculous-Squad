using UnityEngine;

public class ChasingPunchingEnemy : ChasingEnemy
{
    [SerializeField] float punchDelay = 1f, currentDelay;

    protected override void Start()
    {
        base.Start();
        punchDelay = GameManager.instance.levelSelection.CurrentEnvironment.punchDelay;
        currentDelay = punchDelay;
        playerDetectionRange = GameManager.instance.levelSelection.CurrentEnvironment.punchingEnemyDR;
        navMeshAgent.stoppingDistance = GameManager.instance.levelSelection.CurrentEnvironment.punchingEnemyAR;

        healthController.ResetHealth(GameManager.instance.levelSelection.CurrentEnvironment.punchingEnemyHealth);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (isDead)
            return;

        if (!Triggered)
            return;

        HandlePunching();
    }

    void HandlePunching()
    {
        if (PlayerManager.instance.healthController.OutOfHeath)
            return;

        if(ReachedStoppingDistance)
            mTransform.LookTowards(chasingTarget);

        if (currentDelay <= 0)
        {
            currentDelay = punchDelay;
            // check if the player is in range of attack accoding to the current level of enemy-bullet
            if(ReachedStoppingDistance)
            { 
                Punch();
            }
        }

        currentDelay -= Time.deltaTime;
    }

    void Punch()
    {
        if(PlayerManager.instance.healthController.OutOfHeath)
        {
            Triggered = false;
            return;
        }

        animationController.SetTrigger("Punch");
    }

}