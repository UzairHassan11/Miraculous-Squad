using UnityEngine;
using UnityEngine.AI;

public class ChasingEnemy : Enemy
{
    public
    NavMeshAgent navMeshAgent;
    public Vector2 minMaxRunAnimSpeed;
    [HideInInspector]
    public int damage;

    [SerializeField] GameObject hitEffect;

    protected override void Start()
    {
        base.Start();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.baseOffset = -0.04f;
        navMeshAgent.speed = minMaxRunAnimSpeed.y * 2;
        damage = 1;    // assign damage here according to the level of the enemy
    }

    protected override void Update()
    {
        base.Update();

        if (isDead || !Triggered)
            return;

        if (PlayerManager.instance.healthController.OutOfHeath)
            return;

        if (chasingTarget)
        {
            navMeshAgent.SetDestination(chasingTarget.position);
            SetRunAnimSpeed();
        }
    }

    protected void SetRunAnimSpeed()
    {
        animationController.SetFloat("speed", ReachedStoppingDistance ? minMaxRunAnimSpeed.x : minMaxRunAnimSpeed.y);
    }

    public override void Trigger()
    {
        //print("called ChasingEnemy");

        base.Trigger();
    }

    public override void Die(bool turnOff = true, float destroyTime = -1)
    {
        base.Die(turnOff, destroyTime);
        navMeshAgent.speed = 0;
    }

    public void GiveDamage()
    {
        if(Vector3.Distance(chasingTarget.position, mTransform.position) <= navMeshAgent.stoppingDistance + 1)
         {
            if(hitEffect)
                hitEffect.SetActive(true);
            PlayerManager.instance.TakeDamage(damage);
    }
    }

    public bool ReachedStoppingDistance => navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;

    //void HandleAnimationSpeed()
    //{
    //    print(Mathf.InverseLerp(0, navMeshAgent.stoppingDistance, navMeshAgent.remainingDistance));
    //    animationController.SetFloat("speed", Mathf.Lerp(minMaxRunAnimSpeed.x, minMaxRunAnimSpeed.y,
    //        Mathf.InverseLerp(0, navMeshAgent.stoppingDistance, navMeshAgent.remainingDistance)));
    //}
}