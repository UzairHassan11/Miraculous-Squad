using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "CityOfLevels", menuName = "ScriptableObjects/City Of Levels")]
public class CityOfLevels : ScriptableObject
{
    public string onBoarding;
    public Sprite icon;
    public EnemyType bossAttack2Type, ally2Type;

    public string bossName;

    public Level[] levels;

    [FoldoutGroup("Enemies Data")]
    public float grenadeDelay = 2f, enemyDamage = 1, dashingDelay, dashingSpeed,
        dashTime, spellLineDelay = 2, spellsDelay = 2, punchDelay = 1;
    [FoldoutGroup("Enemies Data")]
    public int enemyTotalHealth = 3, spellsCount = 1,
        spellsLinesCount = 1, bossAttack1 = 5,
        bossAttack2 = 5, bossCoins, bossGems;
    [FoldoutGroup("Enemies Detection Ranges")]
    public float punchingEnemyDR, bombingEnemyDR, spellEnemyDR, spell360EnemyDR, spellLineEnemyDR, dashingEnemyDR, bigEnemyDR, bossEnemyDR;
    [FoldoutGroup("Player Attack Ranges")]
    public float punchingEnemyAR, bombingEnemyAR, spellEnemyAR, spell360EnemyAR, spellLineEnemyAR, dashingEnemyAR, bigEnemyAR, bossEnemyAR;
    [FoldoutGroup("Enemies' Health")]
    public float punchingEnemyHealth, bombingEnemyHealth, spellEnemyHealth, spell360EnemyHealth, spellLineEnemyHealth, dashingEnemyHealth, bigEnemyHealth, bossEnemyHealth;

    [ContextMenu("RemoveAllDialogues")]
    void RemoveAllDialogues()
    {
        for(int i=0; i< levels.Length; i++)
        {
            for(int j=0; j< levels[i].levelSections.Length; j++)        
            {
                levels[i].levelSections[0].endDialogue=null;
                levels[i].levelSections[0].startDialogue=null;
            }
        }
    }
}
[System.Serializable]
public class Level
{
    public LevelSection[] levelSections;
    public LevelReward[] levelReward;
    public int levelUpAmount;

    public LevelSection GetLastSection => levelSections[levelSections.Length - 1];
}
[System.Serializable]
public class LevelSection
{
    [Tooltip("Dialogues to show at the start of the section")]
    public Dialogue startDialogue;
    [Tooltip("Dialogues to show at the end of the section")]
    public Dialogue endDialogue;

    public SectionPlatform platform;

    //public EnemyType[] enemyTypes;
}
[System.Serializable]
public class LevelReward
{
    public LevelRewardType levelRewardType;
    public PowerUpType powerUpType;
    public int rewardAmount;
}
public enum LevelRewardType
{
    Coins,
    Gems,
    PowerUp
}