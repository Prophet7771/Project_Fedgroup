using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
        playerHealth = player.GetComponent<HealthSystem>();
    }

    #endregion

    #region Variables

    [SerializeField] List<GameObject> spawnLocations = new List<GameObject>();
    [SerializeField] List<GameObject> activeEnemies = new List<GameObject>();
    [SerializeField] int waveCount = 1;
    [SerializeField] int enemyMaxCount = 5;
    [SerializeField] int enemyCurrentCount = 0;
    [SerializeField] int testCount = 0;
    [SerializeField] bool enemySpawned = false;
    [SerializeField] bool bossFight = false;

    [Header("Player Data")]
    public GameObject player;
    private HealthSystem playerHealth;

    [Header("UI")]
    [SerializeField] TMP_Text waveText;
    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject mainHud;

    #endregion

    #region Start Functions

    private void Start()
    {

    }

    #endregion

    #region Update Functions

    private void Update()
    {
        if (bossFight) return;

        enemyCountText.text = $"Enemies: {testCount}";

        if (playerHealth.GetHealth <= 0)
        {
            mainHud.SetActive(false);
            gameOverScreen.SetActive(true);

            return;
        }

        if (waveCount >= 5)
        {
            if (testCount == 0 || activeEnemies.Count == 0)
            {
                waveText.text = $"WAVES COMPLETED!";
                Invoke("CompleteLevel", 2f);
            }

            return;
        }

        if (activeEnemies.Count == 0)
            enemySpawned = false;

        if (enemySpawned) return;

        enemySpawned = true;
        Invoke("SpawnEnemies", 2.5f);
    }

    #endregion

    #region Basic Functions

    private void SpawnEnemies()
    {
        int spawnCount = Random.Range(1, 4);

        for (int i = 0; i < spawnCount; i++)
        {
            if (enemyCurrentCount >= enemyMaxCount)
            {
                if (activeEnemies.Count > 0)
                {
                    Debug.Log("ENEMIES NOT YET DEAD");
                    return;
                }

                Debug.Log("Enemy Count Increased");

                enemyMaxCount *= 2;
                enemyCurrentCount = 0;

                waveCount++;
                waveText.text = $"Wave: {waveCount}";

                enemySpawned = false;

                return;
            }

            GameObject gameObject = ObjectPooler.Instance.SpawnFromPool("Enemy", spawnLocations[Random.Range(0, 5)].transform.position, Quaternion.identity);
            activeEnemies.Add(gameObject);

            enemyCurrentCount++;
            testCount++;
        }

        enemySpawned = false;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        testCount--;
    }

    private void CompleteLevel()
    {
        player.GetComponent<PlayerController>().enabled = false;

        mainHud.SetActive(false);
        victoryScreen.SetActive(true);

        LevelManager.Instance.CompleteGame();
    }
    public void CompleteBossFight()
    {
        Invoke("CompleteLevel", 2f);
    }

    public void ChangeSchene(string scene) => LevelManager.Instance.LoadByName(scene);

    public void ReloadScene() => LevelManager.Instance.LoadCurrentScene();
    public void LoadBoss() => LevelManager.Instance.LoadBoss();
    public void LoadWaves() => LevelManager.Instance.LoadWaves();
    public void MainMenu() => LevelManager.Instance.LoadMainMenu();
    public void CloseApplication() => LevelManager.Instance.CloseApplication();

    #endregion
}
