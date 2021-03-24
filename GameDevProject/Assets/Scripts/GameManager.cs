using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static object safetyLock = new object();
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        lock (safetyLock)
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion

    private bool gameHasEnded = false;
    public bool GameHasEnded { get { return gameHasEnded; } }

    public void GameOver()
    {
        if (!gameHasEnded)
        {
            gameHasEnded = true;
            Time.timeScale = 0;
            Debug.Log("GAME OVER");
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Quit()
    {

    }
}
