using UnityEngine;
using System.Collections;

public class SpellCastingEnemy : ChasingEnemy
{
    float spellThrowDelay, spellsCount, fireDelay = .2f;
    float currentDelay;

    public Transform bulletSpawnPosition;

    [SerializeField] GameObject chargingEffect;

    protected override void Start()
    {
        base.Start();
        spellsCount = GameManager.instance.levelSelection.CurrentEnvironment.spellsCount;
        spellThrowDelay = GameManager.instance.levelSelection.CurrentEnvironment.spellsDelay;
        currentDelay = spellThrowDelay;
        fireDelay = .4f;

        playerDetectionRange = GameManager.instance.levelSelection.CurrentEnvironment.spellEnemyDR;
        navMeshAgent.stoppingDistance = GameManager.instance.levelSelection.CurrentEnvironment.spellEnemyAR;

        healthController.ResetHealth(GameManager.instance.levelSelection.CurrentEnvironment.spellEnemyHealth);
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

        HandleSpellCasting();
    }

    void HandleSpellCasting()
    {
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
                animationController.SetTrigger("Cast");
            }
        }else
            chargingEffect.SetActive(false);

        currentDelay -= Time.deltaTime;

    }
    void CastSpells()
    {
        chargingEffect.SetActive(false);
        StartCoroutine(_CastSpells());
    }

    IEnumerator _CastSpells()
    {
        EnemySpell enemySpell;
        for (int i = 0; i < spellsCount; i++)
        {
            yield return new WaitForSeconds(fireDelay);
            enemySpell = PoolManager.instance.GetObject(PoolableObjectType.EnemySpell).GetComponent<EnemySpell>();
            transform.LookTowards(PlayerManager.instance.playerController.mTransform);
            enemySpell.Shoot(bulletSpawnPosition);
        }
    }
}