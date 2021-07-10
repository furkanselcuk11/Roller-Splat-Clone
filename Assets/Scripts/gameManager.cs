using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    private groundPiece[] allGroundPiece;   // Leveldeki tüm zeminlerin tutulduðu dizi
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
        SetupNewLevel();    // Oyun baþladýðýnda leveldeki tüm zeminlerin sayýsýný (toplam zemin) "allGroundPiece" dizisiene aktarýr
    }
    private void SetupNewLevel()
    {
        allGroundPiece = FindObjectsOfType<groundPiece>();  
        // "grounPiece"  Scripti olan tüm objeler aranýr ve "allGroundPiece" dizisine eklenir - Temas edilecek zemin sayýsý belirlenir
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
        bool isFinished = true; // Tüm zeminin temasý gerçekleþmiþtir
        for (int i = 0; i < allGroundPiece.Length; i++) // "allGroundPiece" içerisindeki tüm nesneleri döndür
        {
            if (allGroundPiece[i].isColored == false)   // Eðer nesnelerde [i] sýradaki zeminde "isColored" false ise(Zemin renkli deðilse) Oyun henüz bitmemiþtir
            {
                isFinished = false; // Oyun tamamlanmamýþtýr
                break;
            }
        }
        if (isFinished) // Eðer tüm zeminlere temas edilmiþse
        {
            NextLevel();    // Bir sonraki levele geç
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   // Bir sonrali levele geçer
        }        
    }
}
