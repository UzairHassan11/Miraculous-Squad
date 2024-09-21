using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] EnemyType enemiesTypes;

    [SerializeField] Enemy[] enemiesPrefabs;

    //[HideInInspector]
    public int enemisCountInThisSection = 0;

    // [HideInInspector]
    public List<Enemy> enemies = new List<Enemy>();
    public bool enemiesExist=>enemies.Count > 0;

    public UnityAction afterKillingAllSectionEnemiesEvents;

    public void SpawnEnemiesForSection(LevelSection section)
    {
        enemisCountInThisSection = 0;
        enemies.Clear();
        Enemy spawnedEnemy;
        Transform enemyPosition;
        EnemyType enemyType;
        EnemySpawnPoint[] enemySpawnPoints = section.platform.enemyPositionsContainer.GetComponentsInChildren<EnemySpawnPoint>();
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            enemyType = enemySpawnPoints[i].enemyType;
            if (enemyType == EnemyType.None)
                continue;
            else
            {
                enemyPosition = section.platform.enemyPositionsContainer.GetChild(i);

                spawnedEnemy = Instantiate(enemiesPrefabs[(int)enemyType], enemyPosition.position, enemyPosition.rotation);

                spawnedEnemy.name = enemiesPrefabs[(int)enemyType].name + (enemisCountInThisSection + 1);

                enemisCountInThisSection++;

                enemies.Add(spawnedEnemy);

                spawnedEnemy.gameObject.SetActive(true);
            }
        }
    }

    public void SpawnEnemy(int i, Vector3 enemyPosition, bool triggerOnSpawn = false, bool spawnParticle = false)
    {
        if (i == 0)
            return;

        Enemy spawnedEnemy = Instantiate(enemiesPrefabs[i], enemyPosition, Quaternion.identity);

        enemisCountInThisSection++;

        enemies.Add(spawnedEnemy);

        spawnedEnemy.gameObject.SetActive(true);

        if(spawnParticle)
            ParticlesController.instance.SpawnParticle(PoolableObjectType.EnemySpawn, enemyPosition + new Vector3(0, 0.5f, 0));

        if (triggerOnSpawn)
        {
            spawnedEnemy.Trigger();
            //PlayerManager.instance.enemiesTrigger.AddEnemy(spawnedEnemy);
        }
    }

    public void EnemyDied(Enemy enemy)
    {
        enemisCountInThisSection--;
        enemies.Remove(enemy);

        if (enemisCountInThisSection == 1)
        {
            // put off mesh screen indicator on last enemy
            enemies[0].navigationHUD.SetActive(true);
        }
        else if (enemisCountInThisSection == 0)
        {
            GameManager.instance.levelManager.TurnExitIndicator(true);
            CameraManager.instance.Playconfetti();
            if (afterKillingAllSectionEnemiesEvents != null)
            {
                afterKillingAllSectionEnemiesEvents.Invoke();
                afterKillingAllSectionEnemiesEvents = null;
            }
        }
    }

    public void DiableAllEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].gameObject.SetActive(false);
        }
    }

    public void UntriggerAllEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Triggered = false;
            enemies[i].animationController.SetFloat("speed", 0);
        }
    }

    public Enemy ClosestEnemy()
    {
        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(PlayerManager.instance.playerController.transform.position, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        return closestEnemy;
    }

    public void EmptyEnemiesList()
    {
        while (enemies.Count > 0)
        {
            Enemy enemy = enemies[0];
            enemy.Die();
            // enemies.Remove(enemy);
            // Destroy(enemy);   
        }
        enemies.Clear();
    }
}
public enum EnemyType
{
    None,
    Punching,
    Bombing,
    SpellCasting,
    SpellLine,
    Dashing,
    BigEnemy,
    SpellCasting360,
    Boss
}