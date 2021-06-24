using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundPiece : MonoBehaviour
{
    public bool isColored = false;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void ChangeColor(Color color)    // Dýþardan method ile renk alýr ve zemin rengi deðiþtirilir
    {
        GetComponent<MeshRenderer>().material.color = color;    // Zemin rengini deðiþtirir
        isColored = true;
        gameManager.instance.ChechComplete();   // Geçilen yüzeyi tamamlamýþ sayar
    }
}
