using UnityEngine;

public class ClosestEnemyFinder : MonoBehaviour
{
    Enemy prevClosestEnemy;
    Enemy closestEnemy;

    EnemiesManager enemiesManager;

    GameManager gameManager;

    [SerializeField]
    float checkClosesEnemyAfter;    
    float currentDelay;
    Transform player, mTransform;
    [SerializeField]
    EnemyIndicator enemyIndicator;

    public static ClosestEnemyFinder instance;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        mTransform = transform;
        enemiesManager = GameManager.instance.enemiesManager;
        gameManager = GameManager.instance;
        player = PlayerManager.instance.playerController.mTransform;
    }

    private void Update()
    {
        if (gameManager.currentGameState != GameState.Running || !PlayerManager.instance.playerController.isGrounded)
            return;

        if (enemiesManager.enemies.Count > 0) // game is playing
        {
            enemyIndicator.SetVisibility(true);

            if (currentDelay <= 0)
            {
                currentDelay = checkClosesEnemyAfter;
                closestEnemy = ClosestEnemy(player.position);
                enemyIndicator.PlaceMe(closestEnemy);
            }
            else
            {
                currentDelay -= Time.deltaTime;
            }
        }
        else
        {
            enemyIndicator.SetVisibility(false);
        }
    }

    public Enemy ClosestEnemy(Vector3 playerPosition)
    {
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemiesManager.enemies)
        {
            float distance = Vector3.Distance(playerPosition, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        prevClosestEnemy = closestEnemy;
        return closestEnemy;
    }

    public void SetEnemyIndicator(bool state)
    {
        enemyIndicator.SetVisibility(state);
    }
}