using System.Collections.Generic;
using UnityEngine;

public class TriggeredEnemies : MonoBehaviour
{
    List<Enemy> enemies = new List<Enemy>();

    [SerializeField] Transform enemyIndicator;

    [SerializeField] float checkClosesEnemyAfter = 1;
    float currentDelay;

    Transform closestEnemy;

    Transform player;

    public TriggeredEnemiesSubTrigger subTrigger;

    GameManager gameManager;

    public bool enemiesExist;

    public ShootingRangeVisualiser shootingRangeVisualiser;

    private void Start()
    {
        player = PlayerManager.instance.playerController.mTransform;
        gameManager = GameManager.instance;
    }

    private void Update()
    {
       if (gameManager.currentGameState != GameState.Running || !PlayerManager.instance.playerController.isGrounded)
           return;


       if (GameManager.instance.enemiesManager.enemiesExist) // game is playing
       {
        //    enemyIndicator.gameObject.SetActive(true);

           if (currentDelay <= 0)
           {
               currentDelay = checkClosesEnemyAfter;
               closestEnemy = GameManager.instance.enemiesManager.ClosestEnemy().transform;
               PlaceEnemyIndicator();
           }
           else
           {
               currentDelay -= Time.deltaTime;
           }
       }
       else
       {
           enemyIndicator.gameObject.SetActive(false);
       }
    }

    void PlaceEnemyIndicator()
    {
       enemyIndicator.position = closestEnemy.position + new Vector3(0, 0.01f, 0);
       enemyIndicator.parent = closestEnemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemy == null)
                return;
            if (enemy.isDead)
                return;
            AddEnemy(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            RemoveEnemy(enemy);
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            enemy.IsInPlayerAttackRange = false;

            if (enemies.Count == 0)
            {
                PlayerManager.instance.shootingController.EnableShoot(false);
                enemiesExist = false;
            }
        }
    }

    public void RemoveAllEnemies()
    {
        enemies.Clear();
        PlayerManager.instance.shootingController.EnableShoot(false);
        enemiesExist = false;
    }

    public void AddEnemy(Enemy enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            enemy.IsInPlayerAttackRange = true;
            enemiesExist = true;
            enemy.Trigger();
            PlayerManager.instance.shootingController.EnableShoot(true);
        }
    }

    //public bool HasEnemies => enemies.Count > 0;

    public void Enable(bool state)
    {
        gameObject.SetActive(state);
    }

    public Enemy ClosestEnemy(Vector3 playerPosition)
    {
        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(playerPosition, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        return closestEnemy;
    }
}