using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    private groundPiece[] allGroundPiece;   // Leveldeki t�m zeminlerin tutuldu�u dizi
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
        SetupNewLevel();    // Oyun ba�lad���nda leveldeki t�m zeminlerin say�s�n� (toplam zemin) "allGroundPiece" dizisiene aktar�r
    }
    private void SetupNewLevel()
    {
        allGroundPiece = FindObjectsOfType<groundPiece>();  
        // "grounPiece"  Scripti olan t�m objeler aran�r ve "allGroundPiece" dizisine eklenir - Temas edilecek zemin say�s� belirlenir
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
        bool isFinished = true; // T�m zeminin temas� ger�ekle�mi�tir
        for (int i = 0; i < allGroundPiece.Length; i++) // "allGroundPiece" i�erisindeki t�m nesneleri d�nd�r
        {
            if (allGroundPiece[i].isColored == false)   // E�er nesnelerde [i] s�radaki zeminde "isColored" false ise(Zemin renkli de�ilse) Oyun hen�z bitmemi�tir
            {
                isFinished = false; // Oyun tamamlanmam��t�r
                break;
            }
        }
        if (isFinished) // E�er t�m zeminlere temas edilmi�se
        {
            NextLevel();    // Bir sonraki levele ge�
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   // Bir sonrali levele ge�er
        }        
    }
}
