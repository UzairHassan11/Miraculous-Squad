using UnityEngine;

public class Spell360CastingEnemy : ChasingEnemy
{
    public float spellThrowDelay, spellsCount, fireDelay;
    float currentDelay;

    public Transform bulletSpawnPosition;

    [SerializeField] GameObject chargingEffect;

    protected override void Start()
    {
        base.Start();
        spellsCount = GameManager.instance.levelSelection.CurrentEnvironment.spellsCount;
        spellThrowDelay = GameManager.instance.levelSelection.CurrentEnvironment.spellsDelay;
        currentDelay = spellThrowDelay;

        playerDetectionRange = GameManager.instance.levelSelection.CurrentEnvironment.spell360EnemyDR;
        navMeshAgent.stoppingDistance = GameManager.instance.levelSelection.CurrentEnvironment.spell360EnemyAR;

        healthController.ResetHealth(GameManager.instance.levelSelection.CurrentEnvironment.spell360EnemyHealth);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (isDead)
            return;

        if (!Triggered)
            return;

        CheckRange_CastSpell();
    }

    void CheckRange_CastSpell()
    {
        if (PlayerManager.instance.healthController.OutOfHeath)
            return;

        if (ReachedStoppingDistance)
        {
            //if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
            //{
            //print("reached");
            //}
            chargingEffect.SetActive(true);
            if (currentDelay <= 0)
            {
                currentDelay = spellThrowDelay;
                // check if the player is in range of attack accoding to the current level of enemy-bullet
                CastSpells();
                chargingEffect.SetActive(false);
                animationController.SetTrigger("Cast");
            }
        }
        else
            chargingEffect.SetActive(false);

        currentDelay -= Time.deltaTime;
    }

    [SerializeField] float angleDiff = 2;
    void CastSpells()
    {
        EnemySpell enemySpell;
        animationController.SetTrigger("Cast");
        transform.LookTowards(PlayerManager.instance.playerController.mTransform);
        float angle = 360 / spellsCount;
                chargingEffect.SetActive(false);
        for (int i = 0; i < spellsCount; i++)
        {
            enemySpell = PoolManager.instance.GetObject(PoolableObjectType.EnemySpell).GetComponent<EnemySpell>();

            enemySpell.Shoot(bulletSpawnPosition, false);

            enemySpell.transform.LookTowards(PlayerManager.instance.playerController.mTransform);

            enemySpell.transform.AddTo_LocalEulerY(angle * (i + 1));
        }
    }
}