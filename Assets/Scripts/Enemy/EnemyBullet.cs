using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 2;

    protected virtual void Start()
    {
        damage = GameManager.instance.levelSelection.CurrentEnvironment.enemyDamage;
    }

    public virtual void Throw()
    {
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //print(other.tag);
        if (other.CompareTag("Player"))
        {
            PlayerManager.instance.TakeDamage(damage);
        }
    }
}