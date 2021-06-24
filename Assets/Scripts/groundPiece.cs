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
    public void ChangeColor(Color color)    // D��ardan method ile renk al�r ve zemin rengi de�i�tirilir
    {
        GetComponent<MeshRenderer>().material.color = color;    // Zemin rengini de�i�tirir
        isColored = true;
        gameManager.instance.ChechComplete();   // Ge�ilen y�zeyi tamamlam�� sayar
    }
}
