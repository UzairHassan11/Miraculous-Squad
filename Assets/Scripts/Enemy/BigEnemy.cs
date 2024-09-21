using UnityEngine;

public class BigEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Inittt", .1f);
    }

    void Inittt()
    {
        ChasingEnemy chasingEnemy = GetComponent<ChasingEnemy>();
        chasingEnemy.playerDetectionRange = GameManager.instance.levelSelection.CurrentEnvironment.bigEnemyDR;
        chasingEnemy.navMeshAgent.stoppingDistance = GameManager.instance.levelSelection.CurrentEnvironment.bigEnemyAR;

        chasingEnemy.healthController.ResetHealth(GameManager.instance.levelSelection.CurrentEnvironment.bigEnemyHealth);
    }
}