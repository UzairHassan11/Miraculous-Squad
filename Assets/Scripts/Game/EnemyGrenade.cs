using UnityEngine;

public class EnemyGrenade : EnemyBullet
{
    public float height = 5, bombSpeed = 5;

    [SerializeField] ParabolaController parabolaController;

    [SerializeField] PoolableObject poolableObject;

    ChasingBombingEnemy bombingEnemy;

    BossEnemy bossEnemy;

    [HideInInspector]
    public bool thrown;

    public void Throw(ChasingBombingEnemy _enemy)
    {
        base.Throw();

        thrown = true;
        bombingEnemy = _enemy;
        gameObject.SetActive(true);
        parabolaController.ThrowGrenade(
            PlayerManager.instance.playerController.mTransform
            ,bombingEnemy.bombThrowPosition
            ,bombSpeed
            ,height
            ,_enemy.parabolaRoot);
        //Debug.Break();
    }

    public void Throw(BossEnemy _enemy, float _height, float _speed)
    {
        base.Throw();

        thrown = true;
        bossEnemy = _enemy;
        gameObject.SetActive(true);
        parabolaController.ThrowGrenade(
            PlayerManager.instance.playerController.mTransform
            , bossEnemy.bombThrowPosition
            , _speed
            , _height
            , _enemy.parabolaRoot);
        //Debug.Break();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        //print(other.tag);

        poolableObject.ReturnToThePool();
        gameObject.SetActive(false);
        ParticlesController.instance.SpawnParticle(PoolableObjectType.EnemyGrenadeHit, transform.position);
        thrown = false;

        if(bombingEnemy)
            bombingEnemy.TurnGrenadeTargetInidcator(false);
        else if(bossEnemy)
            bossEnemy.BombBlasted();
    }

    public void ChangeBombSpeed(float speed)
    {
        bombSpeed = speed;
    }
}