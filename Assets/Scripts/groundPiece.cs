using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundPiece : MonoBehaviour
{
    public bool isColored = false;
    public void ChangeColor(Color color)    // Dýþardan method ile renk alýr ve zemin rengi deðiþtirilir
    {
        GetComponent<MeshRenderer>().material.color = color;    //  Temas edilen zeminin rengini deðiþtirir
        isColored = true;   // Renk Deðiþimi true olur
        gameManager.instance.ChechComplete();   // Tüm zeminin temas edildiði kontrol eder
    }
}
