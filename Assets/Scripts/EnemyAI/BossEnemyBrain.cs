using UnityEngine;

public class BossEnemyBrain : MonoBehaviour
{
    #region Variables
    [Header("Enemy Data")]
    HealthSystem enemyHealth;
    bool isDead = false;

    [Header("Projectiles")]
    [SerializeField] float projectileDamage = 25;
    [SerializeField] float projectileRotation = 0;
    [SerializeField] private int attackOneCount = 0;
    [SerializeField] private int attackTwoCount = 0;
    [SerializeField] Transform bombSpawnLoc;

    [Header("Player Data")]
    [SerializeField] private GameObject player;
    private HealthSystem playerHealth;

    #endregion

    #region Start Functions

    private void Awake()
    {
        playerHealth = player.GetComponent<HealthSystem>();
        enemyHealth = GetComponent<HealthSystem>();

        enemyHealth.onDeath += KillBoss;
    }

    private void Start()
    {
        Invoke("ShootProjectile", 2f);
    }

    #endregion

    #region Basic Functions

    private void ShootProjectile()
    {
        if (isDead) return;

        for (int i = 0; i < 60; i++)
        {
            Debug.Log($"Projectile {i}");

            GameObject temp = ObjectPooler.Instance.SpawnFromPool("EnemyProjectile", transform.position + new Vector3(0, 2, 0), Quaternion.EulerAngles(0, projectileRotation, 0));
            projectileRotation += 23;

            EnemyProjectile proj = temp.GetComponent<EnemyProjectile>();

            proj.SetPlayer = player;
            proj.SetHealth = playerHealth;
            proj.SetData();
        }

        if (attackOneCount <= 5)
        {
            Invoke("ShootProjectile", 6f);
            attackOneCount++;
        }
        else
        {
            attackOneCount = 0;

            Invoke("ShootBomb", 2f);
        }
    }

    private void ShootBomb()
    {
        if (isDead) return;

        GameObject temp = ObjectPooler.Instance.SpawnFromPool("BossBomb", bombSpawnLoc.position, Quaternion.EulerAngles(Random.Range(5, 15), Random.Range(-360, 360), 0));

        if (attackTwoCount <= 25)
        {
            Invoke("ShootBomb", 0.5f);
            attackTwoCount++;
        }
        else
        {
            attackTwoCount = 0;
            Invoke("ShootProjectile", 2f);
        }
    }

    private void KillBoss()
    {
        isDead = true;
        GameManager.Instance.CompleteBossFight();
    }

    #endregion
}
