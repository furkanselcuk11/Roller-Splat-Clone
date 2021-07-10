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
    private Vector2 swipePosCurrentFrame; // Kayd�rma hareketinin ilk pos.
    private Vector2 swipePosLastFrame;  // Kayd�rma hareketinin son pos.
    public Vector2 currentSwipe;    // Kayd�rma hareketinin mesafesi

    private Color solveColor;  

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        solveColor = Random.ColorHSV(0.5f, 1);  // Random renk olu�turur
        GetComponent<MeshRenderer>().material.color = solveColor;   // Random rengi topun Materyalime atar
    }
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (isMove) // E�er hareket ediyorsa
        {
            rb.velocity = speed * moveDirection;    // Hareket y�n�ne do�ru ilerler
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);  // Gizli k�renin boyutu Topun boyutunun y�zde be�i kadar olacak
        // Zemine temas edecek gizli k�reler olu�turulur
        int i = 0;
        while (i < hitColliders.Length)
        {
            groundPiece ground = hitColliders[i].transform.GetComponent<groundPiece>();
            if (ground && !ground.isColored)
            {   // E�er zeimne temas edilmi�se ve zemin renkli de�ilse 
                ground.ChangeColor(solveColor); //  Temas edilen zeminin rengini de�i�tirir
                // Zemin rengi de�i�tirir - "hitColliders" k�relerine temas eden zeminin rengini de�i�tirir
            }
            i++;
        }

        if (nextCollisionPos != Vector3.zero) // �arp�lacak olan duvar�n top ile aras�ndaki mesafe vekt�r� s�f�r de�ilse - Bir duvara �arp�yor mu?
        {
            if (Vector3.Distance(transform.position, nextCollisionPos)<1)
            {   // E�er (topun pozisyonu - �arp�lacak olan duvar�n uzakl���) mesafesi 1'den az ise
                isMove = false; // Hareket durumu false olur
                moveDirection = Vector3.zero;   // Hareket edilecek y�n� s�f�rlar
                nextCollisionPos = Vector3.zero;    // Topun bakt��� y�ndeki duvar�n mesafesi s�f�rlan�r
            }
        }

        if (isMove)
            return;
        if (Input.GetMouseButton(0))    // Dokunma devam ett�inde
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);   // Dokundu�umuz yerin pozisyonunu al�r 
            if (swipePosLastFrame != Vector2.zero)  // Kayd�rma hareketi sonland���nda vector de�eri s�f�r de�ilse
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;    // Dokununlan ilk pos- son pos (Toplam gidilecek mesafe)
                if (currentSwipe.magnitude < minSwipeRecognition)
                {   // magnitude= "currentSwipe" de�i�keninin karek�k�n� al�r ve uzunluk olarak belirtir
                    // "currentSwipe" uzunluk de�eri "minSwipeRecognition" (Minumum kayd�rma mesafesi) de�i�keninden k���kse
                    // E�er toplam gidilecek mesafe min gidilecek mesafeden az ise fonk. d�nd�r
                    return;
                }
                currentSwipe.Normalize();   // Toplam gidilecek mesafenin normalini al�r (Y�n�n� al�r - Hangi y�ne gitti�ini)
               
                if(currentSwipe.x>-0.5f && currentSwipe.x < 0.5f)
                {
                    // E�er "currentSwipe" vect�r�n�n "x" ekseni -0.5f ile 0.5f aras�nda ise Ileri veya Geri hareket et
                    // Yani "currentSwipe" vect�r�n�n "x" ekseninde hareket etmediyse
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);    // Hareket y�n�n�n�n "moveDirection" hangi y�nde oldu�unu belirtir
                    // E�er "currentSwipe" vect�r�n�n "y" ekseni 0'dan b�y�kse �leri hareket et
                    // E�er "currentSwipe" vect�r�n�n "y" ekseni 0'dan k���kse Geri hareket et
                }
                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    // E�er "currentSwipe" vect�r�n�n "y" ekseni -0.5f ile 0.5f aras�nda ise Sa�a veya Sola hareket et
                    // Yani "currentSwipe" vect�r�n�n "y" ekseninde hareket etmediyse
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);  // Hareket y�n�n�n�n "moveDirection" hangi y�nde oldu�unu belirtir
                    // E�er "currentSwipe" vect�r�n�n "x" ekseni 0'dan b�y�kse Sa�a hareket et
                    // E�er "currentSwipe" vect�r�n�n "x" ekseni 0'dan k���kse Sola hareket et
                }
            }
            swipePosLastFrame = swipePosCurrentFrame;   // Herket bittiksen sonra kayd�rma hareketinin son pos. kayd�rma haketinin ilk pozisyona e�itlenir
        }
        if (Input.GetMouseButtonUp(0))  // Dokunma b�rak�ld���nda
        {
            swipePosLastFrame = Vector2.zero;   // Elimizi Ekrandan kald�rd���m�zda pos de�erini s�f�rlar
            currentSwipe = Vector2.zero;    // Elimizi Ekrandan kald�rd���m�zda pos de�erini s�f�rlar
        }
    }
    private void SetDestination(Vector3 direction)
    {
        moveDirection = direction;  // Topun hareket edece�i y�n� belirtir

        RaycastHit hit;
        if(Physics.Raycast(transform.position,direction,out hit, 100f)) // Topun bulundu�u pozisyondan, Hareket y�n�ne do�ru ���n yollar
        {
            nextCollisionPos = hit.point;   // I��n mesafesi ile topun bakt��� y�ndeki duvara olan mesafeyi hesaplar
        }
        isMove = true; // Hareket etme true olur ve hareket eder
    }
}
