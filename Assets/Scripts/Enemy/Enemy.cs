using UnityEngine;
using Sirenix.OdinInspector;

public class Enemy : MonoBehaviour
{
    bool triggered;

    bool dead;

    protected bool inPlayerAttackRange;

    public bool IsInPlayerAttackRange
    {
        get { return inPlayerAttackRange; }
        set { inPlayerAttackRange = value; }
    }

    [Tooltip("if enemy has detected the player or not")]
    public bool Triggered
    {
        get { return triggered; }
        set { triggered = value; }
    }

    public bool isDead
    {
        get { return dead; }
        set { dead = value; }
    }

    [FoldoutGroup("Essentials")]
    public AnimationController animationController;

    [FoldoutGroup("Essentials")]
    public HealthController healthController;

    [FoldoutGroup("Essentials")]
    protected Transform chasingTarget;

    [FoldoutGroup("Essentials")]
    public float playerDetectionRange = 5;

    [FoldoutGroup("Essentials")]
    public GameObject navigationHUD;

    [FoldoutGroup("Essentials")]
    protected Transform mTransform;

    [FoldoutGroup("Essentials")]
    [SerializeField] GameObject[] characters;

    protected virtual void Start()
    {
        chasingTarget = PlayerManager.instance.playerController.mTransform;
        mTransform = transform;
        healthController.ResetHealth(GameManager.instance.levelSelection.CurrentEnvironment.enemyTotalHealth);

        Init();
    }

    public virtual void Init()
    {
        for (int i = 0; i < characters.Length; i++)
            characters[i].SetActive(false);
            
        GameObject character = characters[GameManager.instance.levelSelection.CurrentEnvironmentIndex];
        character.SetActive(true);
        AnimationEvents animationEvent = character.GetComponent<AnimationEvents>();
        if(animationEvent == null)
            character.AddComponent(typeof(AnimationEvents));
        animationController._animator = characters[GameManager.instance.levelSelection.CurrentEnvironmentIndex].GetComponent<Animator>();
    }

    public virtual void Trigger()
    {
        triggered = true;
        transform.LookTowards(chasingTarget);
        //print("called Enemy");
    }

    protected virtual void Update()
    {
        if(!triggered)
        {
            //print(Vector3.Distance(chasingTarget.position, mTransform.position).ToString("F0"));
            CheckDistacneAndTrigger();   
        }
    }

    protected void CheckDistacneAndTrigger()
    {
        if (Vector3.Distance(chasingTarget.position, mTransform.position) < playerDetectionRange)
        {
            Trigger();
        }
    }

    public virtual void Die(bool turnOff = true, float destroyTime = -1)
    {
        isDead = true;
        animationController.SetTrigger("Die");
        PlayerManager.instance.enemiesTrigger.RemoveEnemy(this);
        GameManager.instance.enemiesManager.EnemyDied(this);
        // print("called " + turnOff);
        EnemyIndicator enemyIndicator = mTransform.GetChild(mTransform.childCount - 1).GetComponent<EnemyIndicator>();
        if(enemyIndicator)
         {
            // print("came here");
            enemyIndicator.Unparent();
         }
        if(turnOff)
            gameObject.SetActive(false);
        ParticlesController.instance.SpawnParticle(PoolableObjectType.EnemyDead, transform.position + new Vector3(0, 1, 0));
        
        if(destroyTime > 0)
            Destroy(gameObject, destroyTime);
        else
            Destroy(gameObject, Random.Range(.5f, 1f));
    }

    public virtual void TakeDamage(float damage)
    {
        if (healthController)
        {
            healthController.TakeDamage(damage);
            if (!healthController.OutOfHeath)
            {
                //animationController.SetTrigger("GetHit");
            }
            else
            {
                Die();
                SpawnCoins();
            }
        }
    }

    protected virtual void SpawnCoins()
    {
        Rigidbody rb;
        GameObject [] coins = new GameObject[PlayerPrefsContainer.CurrentEnvironment + 1];
        float yOffset = 0f;
        for (int i = 0; i < coins.Length; i++)
        {
            coins[i] = PoolManager.instance.GetObject(PoolableObjectType.Coins);
            coins[i].transform.position = transform.position + new Vector3(0, 1 + yOffset, 0);
            yOffset += .2f;
            rb = coins[i].GetComponent<Rigidbody>();
            //rb.AddExplosionForce(10, coins[i].transform.position, 5, 10, ForceMode.Impulse);  
            coins[i].SetActive(true);
            rb.AddForce(new Vector3(Random.Range(-3, 3), 3, Random.Range(-3, 3)), ForceMode.Impulse);
            rb.AddTorque(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), ForceMode.Impulse);
        }
    }

    protected virtual void SpawnCoinsAndGems(int coinsCount, int gemsCount)
    {
        Rigidbody rb;
        GameObject [] coins = new GameObject[coinsCount];
        GameObject [] gems = new GameObject[gemsCount];
        float yOffset = 0f;
        for (int i = 0; i < coins.Length; i++)
        {
            coins[i] = PoolManager.instance.GetObject(PoolableObjectType.Coins);
            coins[i].transform.position = transform.position + new Vector3(0, 1 + yOffset, 0);
            yOffset += .2f;
            rb = coins[i].GetComponent<Rigidbody>();
            //rb.AddExplosionForce(10, coins[i].transform.position, 5, 10, ForceMode.Impulse);  
            coins[i].SetActive(true);
            rb.AddForce(new Vector3(Random.Range(-3, 3), 3, Random.Range(-3, 3)), ForceMode.Impulse);
            rb.AddTorque(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), ForceMode.Impulse);
        }

        yOffset = 0;
        for (int i = 0; i < gems.Length; i++)
        {
            gems[i] = PoolManager.instance.GetObject(PoolableObjectType.Diamonds);
            gems[i].transform.position = transform.position + new Vector3(0, 1 + yOffset, 0);
            yOffset += .3f;
            rb = gems[i].GetComponent<Rigidbody>();
            //rb.AddExplosionForce(10, coins[i].transform.position, 5, 10, ForceMode.Impulse);  
            gems[i].SetActive(true);
            rb.AddForce(new Vector3(Random.Range(-3, 3), 3, Random.Range(-3, 3)), ForceMode.Impulse);
            rb.AddTorque(new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)), ForceMode.Impulse);
        }
    }

    public void ResetAll()
    {
        dead = false;
        triggered = false;
        healthController.ResetHealth();
    }

    [ContextMenu("Add Animation Events Script")]
    void AddAnimationEventsScript()
    {
        AnimationEvents animationEvent;

        for (int i = 0; i < characters.Length; i++)
        {
            animationEvent = characters[i].GetComponent<AnimationEvents>();
            if(!animationEvent)
                animationEvent = characters[i].AddComponent<AnimationEvents>();
        }
    }
}