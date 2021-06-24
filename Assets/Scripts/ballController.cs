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
    private Vector2 swipePosCurrentFrame; // Topun ilk pos.
    private Vector2 swipePosLastFrame;  // Topun son pos.
    public Vector2 currentSwipe;    // Topun gideceði mesafe

    private Color solveColor;  

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        solveColor = Random.ColorHSV(0.5f, 1);  // Random renk oluþturur
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
        // Zemine temas edecek gizli küreler oluþturulur ve o küreler temas ettiði zeminin rengini deðiþtirir
        int i = 0;
        while (i < hitColliders.Length)
        {
            groundPiece ground = hitColliders[i].transform.GetComponent<groundPiece>();
            if (ground && !ground.isColored)
            {   // Eðer yere temas edilmiþse ve zemin renkli deðilse
                ground.ChangeColor(solveColor); // Zemin rengi deðiþtirir
            }
            i++;
        }

        if (nextCollisionPos != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, nextCollisionPos)<1)
            {   // Eðer (topun pos-çarpýlacak olan duvar) mesafesi 1'den az ise
                isMove = false; 
                moveDirection = Vector3.zero;   // Hareket edilecek yönü sýfýrlar
                nextCollisionPos = Vector3.zero;
            }
        }

        if (isMove)
            return;
        if (Input.GetMouseButton(0))
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);   // Dokunduðumuz yerin pos alýr
            if (swipePosLastFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;    // Dokununlan ilk pos- son pos (Toplam gidilecek mesafe)
                if (currentSwipe.magnitude < minSwipeRecognition)   // Vektörün uzunluk bilgisini verir
                {   // Eðer toplam gidilecek mesafe min gidilecek mesafeden az ise fonk. döndür
                    return;
                }
                currentSwipe.Normalize();   // Toplam gidilecek mesafenin normalini alýr
               
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
            swipePosLastFrame = Vector2.zero;   // Elimizi Ekrandan kaldýrdýðýmýzda pos deðerini sýfýrlar
            currentSwipe = Vector2.zero;    // Elimizi Ekrandan kaldýrdýðýmýzda pos deðerini sýfýrlar
        }
    }
    private void SetDestination(Vector3 direction)
    {
        moveDirection = direction;  // Topun hareket edeceði yön

        RaycastHit hit;
        if(Physics.Raycast(transform.position,direction,out hit, 100f)) // Topun pos'dan baktýðý yöne ýþýn yoller 
        {
            nextCollisionPos = hit.point;   // Iþýn mesafesi ile bir sonrati çarpacaðý duvara olan mesafeyi hesaplar
        }
        isMove = true;
    }
}
