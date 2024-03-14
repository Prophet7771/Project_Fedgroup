using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour
{
    #region Variables

    [SerializeField] bool isRanged = false;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject player;
    private HealthSystem playerHealth;
    [SerializeField] bool projectileFired = false;
    [SerializeField] HealthSystem enemyHealth;
    bool isReady = false;

    [Header("FVX")]
    [SerializeField] private GameObject explotion;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject shootLocation;

    [Header("Materials")]
    [SerializeField] MeshRenderer enemyMesh;
    [SerializeField] Material rangedEnemy;

    #endregion

    #region Getters and Setters

    public GameObject SetPlayer { set { player = value; } }

    #endregion

    #region Startup Functions

    private void OnEnable()
    {
        enemyHealth.onDeath += ResetEnemy;

        isReady = true;
    }

    private void OnDisable()
    {
        enemyHealth.onDeath -= ResetEnemy;

        isReady = false;
    }

    private void Awake()
    {
        player = GameManager.Instance.player;

        playerHealth = player.GetComponent<HealthSystem>();

        if (Random.Range(1, 3) == 1)
        {
            isRanged = true;
            enemyMesh.material = rangedEnemy;
        }

        if (isRanged)
            agent.stoppingDistance = 10;

        isReady = true;
    }

    #endregion

    #region Update Functions

    private void Update()
    {
        if (!isReady) return;

        agent.SetDestination(player.transform.position);
        transform.LookAt(player.transform);
    }

    private void FixedUpdate()
    {
        if (!isReady) return;

        if (rangedEnemy)
        {
            if (agent.destination != Vector3.zero && agent.remainingDistance != 0)
                if (agent.remainingDistance <= agent.stoppingDistance)
                    Attack();
        }
        else
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= 1)
                Attack();
        }
    }

    #endregion

    #region Basic Functions

    private void Attack()
    {
        if (playerHealth.GetHealth <= 0) return;

        if (projectileFired) return;

        projectileFired = true;

        Invoke("ResetProjectile", 1.5f);

        if (isRanged)
            ShootProjectile();
        else
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= 1)
            {
                playerHealth.TakeDamage(25);
                Instantiate(explotion, transform.position, Quaternion.identity);

                // Destroy(gameObject);
                ResetEnemy();
            }
        }
    }

    private void ResetProjectile() => projectileFired = false;

    private void ShootProjectile()
    {
        GameObject temp = Instantiate(projectile, shootLocation.transform.position, Quaternion.identity);

        EnemyProjectile proj = temp.GetComponent<EnemyProjectile>();

        proj.SetPlayer = player;
        proj.SetHealth = playerHealth;
        proj.SetData();
    }

    private void ResetEnemy()
    {
        Debug.Log("ENEMY RESET");
        GameManager.Instance.RemoveEnemy(gameObject);

        enemyHealth.RestoreHealth(100);
        gameObject.SetActive(false);
    }

    #endregion
}
