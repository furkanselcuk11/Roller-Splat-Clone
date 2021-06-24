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
        allGroundPiece = FindObjectsOfType<groundPiece>();  // "grounPiece"  Scripti olan tüm objeler aranýr
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
        for (int i = 0; i < allGroundPiece.Length; i++) // allGroundPiece içerisindeki tüm objeleri döndür
        {
            if (allGroundPiece[i].isColored == false)   // Eðer objelerden [i] sýradaki zeminde isColored false ise(Zemin renkli deðilse)
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
        if (SceneManager.GetActiveScene().buildIndex == 1)  // Son seviye kaçsa son seviye geçilince ilk levele geri döner
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }        
    }
}
