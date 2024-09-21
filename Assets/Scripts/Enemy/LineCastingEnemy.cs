using UnityEngine;
using System.Collections;

public class LineCastingEnemy : ChasingEnemy
{
    float lineCastDelay;
    float currentDelay;

    public bool castingLine;

    [SerializeField] EnemySpellLine enemySpellLine;

    EnemySpellLine[] enemySpellLines;

    [SerializeField] int linesCount = 1;

    bool initiated;

    [SerializeField] GameObject chargingEffect;

    protected override void Start()
    {
        base.Start();
        linesCount = GameManager.instance.levelSelection.CurrentEnvironment.spellsLinesCount;
        lineCastDelay = GameManager.instance.levelSelection.CurrentEnvironment.spellLineDelay;
        currentDelay = lineCastDelay;

        playerDetectionRange = GameManager.instance.levelSelection.CurrentEnvironment.spellLineEnemyDR;
        navMeshAgent.stoppingDistance = GameManager.instance.levelSelection.CurrentEnvironment.spellLineEnemyAR;
        InitLines();
        healthController.ResetHealth(GameManager.instance.levelSelection.CurrentEnvironment.spellLineEnemyHealth);
    }

    protected override void Update()
    {
        if(!castingLine)
            base.Update();

        if (isDead)
            return;

        if (!Triggered)
            return;

        HandleSpellCasting();
    }

    private void InitLines()
    {
        enemySpellLines = new EnemySpellLine[linesCount];

        float angle = 360 / linesCount;
        for (int i = 0; i < linesCount; i++)
        {
            enemySpellLines[i] = Instantiate(enemySpellLine, mTransform);

            enemySpellLines[i].transform.localEulerAngles = Vector3.zero;

            enemySpellLines[i].transform.localPosition = Vector3.zero;

            //enemySpellLines[i].transform.LookTowards(PlayerManager.instance.playerController.mTransform);

            enemySpellLines[i].transform.AddTo_LocalEulerY(angle * (i + 1));

            enemySpellLines[i].SetVisibility(true);

            enemySpellLines[i].Init(this);
        }

        initiated = true;
    }

    void HandleSpellCasting()
    {
        if (PlayerManager.instance.healthController.OutOfHeath)
            return;

        if (ReachedStoppingDistance)
        {
            if(!castingLine)
                if (currentDelay <= 0)
                {
                    currentDelay = lineCastDelay;

                    // check if the player is in range of attack accoding to the current level of enemy-bullet
                    // chargingEffect?.SetActive(false);
                    // CastSpell();
                    StartCoroutine("CastSpellLine");
                }
        }
        else if(castingLine)
        {
            castingLine = false;
            chargingEffect.SetActive(false);
            
            StopCoroutine("CastSpellLine");

            for (int i = 0; i < enemySpellLines.Length; i++)
                enemySpellLines[i].KillLine();
        }

        currentDelay -= Time.deltaTime;
    }

    void CastSpell()
    {
        castingLine = true;

        for (int i = 0; i < enemySpellLines.Length; i++)
            enemySpellLines[i].Shoot();

        StartCoroutine(ResetCastingLine());
    }

    bool lookAtPlayer;
    IEnumerator CastSpellLine()
    {
        castingLine = true;
        chargingEffect.SetActive(true);
        mTransform.LookTowards(PlayerManager.instance.playerController.mTransform);
        
        for (int i = 0; i < enemySpellLines.Length; i++)
            enemySpellLines[i].Shoot();
        
        yield return new WaitForSeconds(1f);
        chargingEffect.SetActive(false);
        yield return new WaitForSeconds(.5f);
        castingLine = false;
    }

    IEnumerator ResetCastingLine()
    {
        yield return new WaitForSeconds(.5f);
        castingLine = false;
    }

    public override void Die(bool turnOff = true, float destroyTime = -1)
    {
        base.Die();

        for (int i = 0; i < enemySpellLines.Length; i++)
        {
            enemySpellLines[i].KillLine();
        }
    }
}