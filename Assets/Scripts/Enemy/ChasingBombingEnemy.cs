using UnityEngine;

public class ChasingBombingEnemy : ChasingEnemy
{
    public float grenadeThrowDelay;
    float currentDelay;

    [SerializeField] Transform grenadeTargetInidcator;

    public Transform bombThrowPosition;
    public GameObject parabolaRoot;

    EnemyGrenade currentGrenade;

    protected override void Start()
    {
        base.Start();
        currentDelay = 1;
        grenadeThrowDelay = GameManager.instance.levelSelection.CurrentEnvironment.grenadeDelay;

        playerDetectionRange = GameManager.instance.levelSelection.CurrentEnvironment.bombingEnemyDR;
        navMeshAgent.stoppingDistance = GameManager.instance.levelSelection.CurrentEnvironment.bombingEnemyAR;

        healthController.ResetHealth(GameManager.instance.levelSelection.CurrentEnvironment.bombingEnemyHealth);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (isDead)
            return;

        if (!Triggered)
        {
            return;
        }

        if (currentGrenade)
            if (currentGrenade.thrown)
                return;

        CheckRange_ThrowGrenade();
    }

    void CheckRange_ThrowGrenade()
    {
        if (PlayerManager.instance.healthController.OutOfHeath)
            return;

        if (ReachedStoppingDistance)
        {
            //if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
            //{
            //print("reached");
            //}
            if (currentDelay <= 0)
            {
                currentDelay = grenadeThrowDelay;
                // check if the player is in range of attack accoding to the current level of enemy-bullet
                //print("here");
                ThrowGrenade();
                mTransform.LookTowards(chasingTarget);
                animationController.SetTrigger("Throw");
                PlaceGrenadeTargetInidcator();
                TurnGrenadeTargetInidcator(true);
            }
            currentDelay -= Time.deltaTime;
        }
    }


    void ThrowGrenade()
    {
        currentGrenade = PoolManager.instance.GetObject(PoolableObjectType.EnemyGrenade).GetComponent<EnemyGrenade>();
        currentGrenade.Throw(this);
    }

    void PlaceGrenadeTargetInidcator()
    {
        grenadeTargetInidcator.SetParent(null);
        grenadeTargetInidcator.position = chasingTarget.position;
    }

    public void TurnGrenadeTargetInidcator(bool state)
    {
        grenadeTargetInidcator.gameObject.SetActive(state);
    }

    public override void Die(bool turnOff = true, float destroyTime = -1)
    {
        base.Die();
        TurnGrenadeTargetInidcator(false);
    }
}