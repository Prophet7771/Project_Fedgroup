using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void ChangeSchene(string scene) => LevelManager.Instance.LoadByName(scene);
    public void LoadWaves() => LevelManager.Instance.LoadWaves();
    public void LoadBoss() => LevelManager.Instance.LoadBoss();
    public void CloseApplication() => LevelManager.Instance.CloseApplication();
}
