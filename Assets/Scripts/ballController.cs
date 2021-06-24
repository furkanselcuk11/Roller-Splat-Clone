using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballController : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 15f;
    public int minSwipeRecognition = 500;   // Hareket etttirmek i�in gerekli Min kayd�rma mesafesi

    private bool isMove;
    private Vector3 moveDirection;  // Hareketin y�n�n� belirtir
    private Vector3 nextCollisionPos;   // Hareket y�n�n�n kar��s�ndaki duvar�n pos.
    private Vector2 swipePosCurrentFrame; // Topun ilk pos.
    private Vector2 swipePosLastFrame;  // Topun son pos.
    public Vector2 currentSwipe;    // Topun gidece�i mesafe

    private Color solveColor;  

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        solveColor = Random.ColorHSV(0.5f, 1);  // Random renk olu�turur
        GetComponent<MeshRenderer>().material.color = solveColor;   // Random rengi topa belirler
    }
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (isMove)
        {
            rb.velocity = speed * moveDirection;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);
        // Zemine temas edecek gizli k�reler olu�turulur ve o k�reler temas etti�i zeminin rengini de�i�tirir
        int i = 0;
        while (i < hitColliders.Length)
        {
            groundPiece ground = hitColliders[i].transform.GetComponent<groundPiece>();
            if (ground && !ground.isColored)
            {   // E�er yere temas edilmi�se ve zemin renkli de�ilse
                ground.ChangeColor(solveColor); // Zemin rengi de�i�tirir
            }
            i++;
        }

        if (nextCollisionPos != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, nextCollisionPos)<1)
            {   // E�er (topun pos-�arp�lacak olan duvar) mesafesi 1'den az ise
                isMove = false; 
                moveDirection = Vector3.zero;   // Hareket edilecek y�n� s�f�rlar
                nextCollisionPos = Vector3.zero;
            }
        }

        if (isMove)
            return;
        if (Input.GetMouseButton(0))
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);   // Dokundu�umuz yerin pos al�r
            if (swipePosLastFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;    // Dokununlan ilk pos- son pos (Toplam gidilecek mesafe)
                if (currentSwipe.magnitude < minSwipeRecognition)   // Vekt�r�n uzunluk bilgisini verir
                {   // E�er toplam gidilecek mesafe min gidilecek mesafeden az ise fonk. d�nd�r
                    return;
                }
                currentSwipe.Normalize();   // Toplam gidilecek mesafenin normalini al�r
               
                if(currentSwipe.x>-0.5f && currentSwipe.x < 0.5f)
                {
                    // Go Up - Down
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                }
                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    // Go Left - Right
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                }
            }
            swipePosLastFrame = swipePosCurrentFrame;
        }
        if (Input.GetMouseButtonUp(0))
        {
            swipePosLastFrame = Vector2.zero;   // Elimizi Ekrandan kald�rd���m�zda pos de�erini s�f�rlar
            currentSwipe = Vector2.zero;    // Elimizi Ekrandan kald�rd���m�zda pos de�erini s�f�rlar
        }
    }
    private void SetDestination(Vector3 direction)
    {
        moveDirection = direction;  // Topun hareket edece�i y�n

        RaycastHit hit;
        if(Physics.Raycast(transform.position,direction,out hit, 100f)) // Topun pos'dan bakt��� y�ne ���n yoller 
        {
            nextCollisionPos = hit.point;   // I��n mesafesi ile bir sonrati �arpaca�� duvara olan mesafeyi hesaplar
        }
        isMove = true;
    }
}
