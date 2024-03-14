using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Singleton

    public static LevelManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Variables

    [SerializeField] bool wavesCompleted = false;

    #endregion

    #region Basic Functions

    public void LoadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void LoadByName(string value) => SceneManager.LoadScene(value);
    public void LoadWaves() => SceneManager.LoadScene("LevelWaves");
    public void LoadBoss()
    {
        if (wavesCompleted)
            SceneManager.LoadScene("LevelBoss");
    }

    public void LoadMainMenu() => SceneManager.LoadScene("MainMenu");
    public void CloseApplication() => Application.Quit();

    public void CompleteGame() => wavesCompleted = true;

    #endregion
}
