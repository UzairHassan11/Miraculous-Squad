using UnityEngine;

public class SectionPlatform : MonoBehaviour
{
    public bool spawnEnemiesInStart = true;

    public Transform spawnPosition, exitPosition;

    public Transform enemyPositionsContainer;

    public Transform RandomSpawnPositionFromThisSection => enemyPositionsContainer.GetChild(Random.Range(0, enemyPositionsContainer.childCount));
}