using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballController : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 15f;
    public int minSwipeRecognition = 500;   // Hareket etttirmek için gerekli Min kaydýrma mesafesi

    private bool isMove;
    private Vector3 moveDirection;  // Hareketin yönünü belirtir
    private Vector3 nextCollisionPos;   // Hareket yönünün karþýsýndaki duvarýn pos.
    private Vector2 swipePosCurrentFrame; // Kaydýrma hareketinin ilk pos.
    private Vector2 swipePosLastFrame;  // Kaydýrma hareketinin son pos.
    public Vector2 currentSwipe;    // Kaydýrma hareketinin mesafesi

    private Color solveColor;  

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        solveColor = Random.ColorHSV(0.5f, 1);  // Random renk oluþturur
        GetComponent<MeshRenderer>().material.color = solveColor;   // Random rengi topun Materyalime atar
    }
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (isMove) // Eðer hareket ediyorsa
        {
            rb.velocity = speed * moveDirection;    // Hareket yönüne doðru ilerler
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);  // Gizli kürenin boyutu Topun boyutunun yüzde beþi kadar olacak
        // Zemine temas edecek gizli küreler oluþturulur
        int i = 0;
        while (i < hitColliders.Length)
        {
            groundPiece ground = hitColliders[i].transform.GetComponent<groundPiece>();
            if (ground && !ground.isColored)
            {   // Eðer zeimne temas edilmiþse ve zemin renkli deðilse 
                ground.ChangeColor(solveColor); //  Temas edilen zeminin rengini deðiþtirir
                // Zemin rengi deðiþtirir - "hitColliders" kürelerine temas eden zeminin rengini deðiþtirir
            }
            i++;
        }

        if (nextCollisionPos != Vector3.zero) // Çarpýlacak olan duvarýn top ile arasýndaki mesafe vektörü sýfýr deðilse - Bir duvara çarpýyor mu?
        {
            if (Vector3.Distance(transform.position, nextCollisionPos)<1)
            {   // Eðer (topun pozisyonu - çarpýlacak olan duvarýn uzaklýðý) mesafesi 1'den az ise
                isMove = false; // Hareket durumu false olur
                moveDirection = Vector3.zero;   // Hareket edilecek yönü sýfýrlar
                nextCollisionPos = Vector3.zero;    // Topun baktýðý yöndeki duvarýn mesafesi sýfýrlanýr
            }
        }

        if (isMove)
            return;
        if (Input.GetMouseButton(0))    // Dokunma devam ettðinde
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);   // Dokunduðumuz yerin pozisyonunu alýr 
            if (swipePosLastFrame != Vector2.zero)  // Kaydýrma hareketi sonlandýðýnda vector deðeri sýfýr deðilse
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;    // Dokununlan ilk pos- son pos (Toplam gidilecek mesafe)
                if (currentSwipe.magnitude < minSwipeRecognition)
                {   // magnitude= "currentSwipe" deðiþkeninin karekökünü alýr ve uzunluk olarak belirtir
                    // "currentSwipe" uzunluk deðeri "minSwipeRecognition" (Minumum kaydýrma mesafesi) deðiþkeninden küçükse
                    // Eðer toplam gidilecek mesafe min gidilecek mesafeden az ise fonk. döndür
                    return;
                }
                currentSwipe.Normalize();   // Toplam gidilecek mesafenin normalini alýr (Yönünü alýr - Hangi yöne gittiðini)
               
                if(currentSwipe.x>-0.5f && currentSwipe.x < 0.5f)
                {
                    // Eðer "currentSwipe" vectörünün "x" ekseni -0.5f ile 0.5f arasýnda ise Ileri veya Geri hareket et
                    // Yani "currentSwipe" vectörünün "x" ekseninde hareket etmediyse
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);    // Hareket yönününün "moveDirection" hangi yönde olduðunu belirtir
                    // Eðer "currentSwipe" vectörünün "y" ekseni 0'dan büyükse Ýleri hareket et
                    // Eðer "currentSwipe" vectörünün "y" ekseni 0'dan küçükse Geri hareket et
                }
                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    // Eðer "currentSwipe" vectörünün "y" ekseni -0.5f ile 0.5f arasýnda ise Saða veya Sola hareket et
                    // Yani "currentSwipe" vectörünün "y" ekseninde hareket etmediyse
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);  // Hareket yönününün "moveDirection" hangi yönde olduðunu belirtir
                    // Eðer "currentSwipe" vectörünün "x" ekseni 0'dan büyükse Saða hareket et
                    // Eðer "currentSwipe" vectörünün "x" ekseni 0'dan küçükse Sola hareket et
                }
            }
            swipePosLastFrame = swipePosCurrentFrame;   // Herket bittiksen sonra kaydýrma hareketinin son pos. kaydýrma haketinin ilk pozisyona eþitlenir
        }
        if (Input.GetMouseButtonUp(0))  // Dokunma býrakýldýðýnda
        {
            swipePosLastFrame = Vector2.zero;   // Elimizi Ekrandan kaldýrdýðýmýzda pos deðerini sýfýrlar
            currentSwipe = Vector2.zero;    // Elimizi Ekrandan kaldýrdýðýmýzda pos deðerini sýfýrlar
        }
    }
    private void SetDestination(Vector3 direction)
    {
        moveDirection = direction;  // Topun hareket edeceði yönü belirtir

        RaycastHit hit;
        if(Physics.Raycast(transform.position,direction,out hit, 100f)) // Topun bulunduðu pozisyondan, Hareket yönüne doðru ýþýn yollar
        {
            nextCollisionPos = hit.point;   // Iþýn mesafesi ile topun baktýðý yöndeki duvara olan mesafeyi hesaplar
        }
        isMove = true; // Hareket etme true olur ve hareket eder
    }
}
