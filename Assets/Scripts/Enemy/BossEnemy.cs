using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class BossEnemy : ChasingEnemy
{
    bool waiting, walking, spawningEnemies;

    [SerializeField] EnemyType[] typesOfEnemiesToSpawn;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    Transform enemySpawnPointsContainer;

    Transform walkTarget, player;

    [SerializeField] EnemyType secondAttackType;

    public GameObject parabolaRoot;

    public Transform bombThrowPosition, grenadeTargetInidcator;

    int firstAttackIntensity, secondAttackIntensity;

    [SerializeField] float multiple = 3;

    [SerializeField] GameObject chargingEffect;

    protected override void Start()
    {
        base.Start();
        GameManager.instance.uiManager.InitForBoss(this, GameManager.instance.levelSelection.CurrentEnvironment.bossName);
        player = PlayerManager.instance.playerController.mTransform;
        navMeshAgent.baseOffset = -0.1f;
        navMeshAgent.speed = minMaxRunAnimSpeed.y * multiple;

        firstAttackIntensity = GameManager.instance.levelSelection.CurrentEnvironment.bossAttack1;
        secondAttackIntensity = GameManager.instance.levelSelection.CurrentEnvironment.bossAttack2;

        playerDetectionRange = GameManager.instance.levelSelection.CurrentEnvironment.bossEnemyDR;
        navMeshAgent.stoppingDistance = GameManager.instance.levelSelection.CurrentEnvironment.bossEnemyAR;

        healthController.ResetHealth(GameManager.instance.levelSelection.CurrentEnvironment.bossEnemyHealth);

        secondAttackType = GameManager.instance.levelSelection.CurrentEnvironment.bossAttack2Type;
        typesOfEnemiesToSpawn[1] = GameManager.instance.levelSelection.CurrentEnvironment.ally2Type;

        if (secondAttackType == EnemyType.SpellLine)
            InitSpellLines();
    }

    protected override void Update()
    {
        if (isDead)
            return;

        if (!Triggered)
        {
            CheckDistacneAndTrigger();
            return;
        }

        if (walking)
        {
            //print("walking");
            if (ReachedStoppingDistance)
            {
                //print("ending walking");
                //print(chasingTarget.name);
                walking = false;
                StartCoroutine(WaitAndDoWhatNeedsToBeDone());
                animationController.SetFloat("speed", 0);
            }
            else
            {
                navMeshAgent.SetDestination(chasingTarget.position);
                //print(name + " update");
                SetRunAnimSpeed();
            }
        }
        //SetRunAnimSpeed();
    }

    public override void Trigger()
    {
        base.Trigger();
        StartCoroutine(WaitAndDoWhatNeedsToBeDone());
        healthController.healthBar.ShowHealthBar(true);
    }

    //NavMeshHit hit;
    //RaycastHit hitt;

    IEnumerator GetRandomChaseTarget()
    {
        //Physics.Raycast(mTransform.position, Vector3.down, out hitt);
        //while (!NavMesh.SamplePosition(mTransform.position, out hit, 5, layerMask))
        walkTarget = GameManager.instance.levelManager.GetRandomSpawnPositionFromThisSection;
        while (Vector3.Distance(mTransform.position, walkTarget.position) < 1)
        {
            walkTarget = GameManager.instance.levelManager.GetRandomSpawnPositionFromThisSection;
            yield return new WaitForEndOfFrame();
            print("3");
        }

        print("Got Random Chase Target");

        //print("4");
        //animationController.SetFloat("speed", minMaxRunAnimSpeed.y);

        chasingTarget = walkTarget;
        navMeshAgent.SetDestination(chasingTarget.position);
        walking = true;
        //taregtIdentifier.position = walkTarget.position;
        //navMeshAgent.SetDestination(walkTarget.position);
    }

    IEnumerator SpawnEnemies()
    {
        int index = 0;
        spawningEnemies = true;

        animationController.SetBool("Spawn", true);
        List<Transform> enemySpawnPoints = new List<Transform>();
        Transform enemySpawnPoint;
        NavMeshPath path = new NavMeshPath();
        for (int i = 0; i < typesOfEnemiesToSpawn.Length; i++)
        {
            //Physics.Raycast(mTransform.position, Vector3.down, out hitt);
            enemySpawnPoint = GetRandomEnemySpawnPoint;
            while (enemySpawnPoints.Contains(enemySpawnPoint) || !NavMesh.CalculatePath(enemySpawnPoint.position, player.position, layerMask, path))
            {
                //print("5");
                yield return new WaitForEndOfFrame();
                enemySpawnPoint = GetRandomEnemySpawnPoint;
            }

            //print("6");
            
            enemySpawnPoints.Add(enemySpawnPoint);
        }

        for (int i = 0; i < typesOfEnemiesToSpawn.Length; i++)
        {
            ParticlesController.instance.SpawnParticle(PoolableObjectType.EnemySpawn, enemySpawnPoints[i].position + new Vector3(0, 0.5f, 0));
        }
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < typesOfEnemiesToSpawn.Length; i++)
        {
            index = Random.Range(0, typesOfEnemiesToSpawn.Length);

            GameManager.instance.enemiesManager.SpawnEnemy((int)typesOfEnemiesToSpawn[index], enemySpawnPoints[i].position, true);
        }

        animationController.SetBool("Spawn", false);
        yield return new WaitForSeconds(1);

        spawningEnemies = false;

        StartCoroutine(GetRandomChaseTarget());
    }

    IEnumerator WaitAndDoWhatNeedsToBeDone()
    {
        if (waiting)
        {
            //print("already waiting");
            yield break;
        }
        //if(waiting)
            //print("x x x already waiting");

        waiting = true;
        //print("Wait And Do What Needs To Be Done");
        yield return new WaitForSeconds(1);
        //print("0");
        //print(GameManager.instance.enemiesManager.enemisCountInThisSection);
        if (GameManager.instance.enemiesManager.enemisCountInThisSection == 1)
        {
        //print("1");
            StartCoroutine(SpawnEnemies());
            print("Spawning Enemies");
            yield return new WaitForSeconds(2);
        }
        else
        {
        //print("2");
            yield return new WaitForSeconds(1);
            if (Random.value < 0.5f)
            {
                CastSpells360();
                print("CastSpells360");
                yield return new WaitForSeconds(1);
            }
            else
            {
                switch(secondAttackType)
                {
                    case EnemyType.Bombing:
                        yield return new WaitForSeconds(.5f);
                        ThrowGrenade();
                        //yield return new WaitWhile(() => throwingBomb);
                        //yield return new WaitUntil(()=> !currentGrenade.thrown);
                        yield return new WaitForSeconds(1);
                        break;
                    case EnemyType.SpellLine:
                        yield return new WaitForSeconds(.75f);
                        CastSpellLines();
                        yield return new WaitForSeconds(2f);
                        break;
                    case EnemyType.SpellCasting:
                        CastSpells360();
                        print("CastSpells360");
                        yield return new WaitForSeconds(1);
                        break;
                }
            }
            StartCoroutine(GetRandomChaseTarget());
            //print("Casting Spells");
        }
        waiting = false;
    }

    public Transform bulletSpawnPosition;
    void CastSpells360()
    {
        EnemySpell enemySpell;
        animationController.SetTrigger("Cast");
        transform.LookTowards(PlayerManager.instance.playerController.mTransform);
        float angle = 360 / firstAttackIntensity;
        for (int i = 0; i < firstAttackIntensity; i++)
        {
            enemySpell = PoolManager.instance.GetObject(PoolableObjectType.EnemySpell).GetComponent<EnemySpell>();

            enemySpell.Shoot(bulletSpawnPosition, false);

            enemySpell.transform.LookTowards(PlayerManager.instance.playerController.mTransform);

            enemySpell.transform.AddTo_LocalEulerY(angle * (i+1));
        }
        //Debug.Break();
    }

    Transform GetRandomEnemySpawnPoint => enemySpawnPointsContainer.GetChild(Random.Range(0, enemySpawnPointsContainer.childCount));

    #region bombing
    EnemyGrenade currentGrenade;

    void ThrowGrenade()
    {
        animationController.SetTrigger("Throw");
        currentGrenade = PoolManager.instance.GetObject(PoolableObjectType.EnemyGrenade).GetComponent<EnemyGrenade>();
        currentGrenade.Throw(this, 7, 10);
        PlaceGrenadeTargetInidcator();
        TurnGrenadeTargetInidcator(true);
        mTransform.LookTowards(player);
    }

    void PlaceGrenadeTargetInidcator()
    {
        grenadeTargetInidcator.SetParent(null);
        grenadeTargetInidcator.position = player.position;
    }

    void TurnGrenadeTargetInidcator(bool state)
    {
        grenadeTargetInidcator.gameObject.SetActive(state);
    }

    public void BombBlasted()
    {
        TurnGrenadeTargetInidcator(false);
    }
    #endregion

    #region spell-casting
    [SerializeField] EnemySpellLine enemySpellLine;

    EnemySpellLine [] enemySpellLines;

    [SerializeField] int linesCount = 1;

    private void InitSpellLines()
    {
        linesCount = GameManager.instance.levelSelection.CurrentEnvironment.bossAttack2;
        enemySpellLines = new EnemySpellLine[linesCount];

        float angle = 360 / linesCount;
        for (int i = 0; i < linesCount; i++)
        {
            enemySpellLines[i] = Instantiate(enemySpellLine, mTransform);

            enemySpellLines[i].transform.localEulerAngles = Vector3.zero;

            // enemySpellLines[i].transform.localPosition = Vector3.zero;

            //enemySpellLines[i].transform.LookTowards(PlayerManager.instance.playerController.mTransform);

            enemySpellLines[i].transform.AddTo_LocalEulerY(angle * (i + 1));

            enemySpellLines[i].SetVisibility(true);

            enemySpellLines[i].Init(this);
        }
    }

    void CastSpellLines()
    {
        mTransform.LookTowards(PlayerManager.instance.playerController.mTransform);

        for (int i = 0; i < enemySpellLines.Length; i++)
            enemySpellLines[i].Shoot();

        Triggered = false;
    }

    public override void Die(bool turnOff = true, float destroyTime = -1)
    {
        base.Die(false, 4);
        
        print("called");

        if(secondAttackType == EnemyType.SpellLine)
        for (int i = 0; i < enemySpellLines.Length; i++)
        {
            enemySpellLines[i].KillLine();
        }

        SpawnCoinsAndGems(GameManager.instance.levelSelection.CurrentEnvironment.bossCoins,
        GameManager.instance.levelSelection.CurrentEnvironment.bossGems);
        animationController.SetTrigger("Die");
        gameObject.SetActive(true);
        mTransform.DOScale(0, .5f).SetDelay(3);
    }
    #endregion
}