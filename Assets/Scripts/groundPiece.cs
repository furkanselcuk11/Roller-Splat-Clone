using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundPiece : MonoBehaviour
{
    public bool isColored = false;
    public void ChangeColor(Color color)    // D��ardan method ile renk al�r ve zemin rengi de�i�tirilir
    {
        GetComponent<MeshRenderer>().material.color = color;    //  Temas edilen zeminin rengini de�i�tirir
        isColored = true;   // Renk De�i�imi true olur
        gameManager.instance.ChechComplete();   // T�m zeminin temas edildi�i kontrol eder
    }
}
