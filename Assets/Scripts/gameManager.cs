using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    private groundPiece[] allGroundPiece;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }      
        else if (instance != null)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        SetupNewLevel();
    }
    private void SetupNewLevel()
    {
        allGroundPiece = FindObjectsOfType<groundPiece>();  // "grounPiece"  Scripti olan t�m objeler aran�r
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetupNewLevel();
    }
    public void ChechComplete()
    {
        bool isFinished = true;
        for (int i = 0; i < allGroundPiece.Length; i++) // allGroundPiece i�erisindeki t�m objeleri d�nd�r
        {
            if (allGroundPiece[i].isColored == false)   // E�er objelerden [i] s�radaki zeminde isColored false ise(Zemin renkli de�ilse)
            {
                isFinished = false;
                break;
            }
        }
        if (isFinished)
        {
            NextLevel();
        }
    }
    private void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)  // Son seviye ka�sa son seviye ge�ilince ilk levele geri d�ner
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }        
    }
}
