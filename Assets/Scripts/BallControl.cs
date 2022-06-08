using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    private Touch touch;
    [SerializeField] float speedModifier;

    Vector3 startPos, endPos, direction;
    float touchTimeStart, touchTimeFinish, timeInterval;

    [SerializeField] float jumpForce;
    bool isGround, throwCheck;

    //[Range(0.05f, 1f)]
    public float throwUpForse = 0.3f, throwForwardForse;

    Rigidbody ballRB;

    public ParticleSystem smokeEffect;


    bool zoneCheck, shotCheck;
    public Transform goal;

    //Ba�lang�� noktas�ndan biti� noktas�na gitme h�z�, saniye cinsinden
    [SerializeField] float journeyTime;

    float startTime;

    public Vector3 distance;

    // Start is called before the first frame update
    void Start()
    {
        isGround = false;
        zoneCheck = false;
        //journeyTime = 1f;

        ballRB = GetComponent<Rigidbody>();
        startTime = Time.time;
        //speedModifier = 0.01f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Calculator();
        BallJumping();

        if (throwCheck)
        {
            Movement();
        }
        if(timeInterval <= 0.20f && endPos.y > startPos.y)
        {
            ThrowBall();
            endPos = Vector3.zero;
            startPos = Vector3.zero;
            timeInterval = 0;
        }
        
        
    }

    //Topun s�rekli z�plamas�n� sa�layan fonksiyon
    void BallJumping()
    {
        if (isGround)
        {
            ballRB.AddForce(Vector3.up * jumpForce);
            isGround = false;
        }
    }

    //Topu oyun i�erisinde kontrol etme
    void Movement()
    {
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(transform.position.x + touch.deltaPosition.x * -speedModifier, transform.position.y, 
                    transform.position.z + touch.deltaPosition.y * -speedModifier);
            }
        }
    }

    void ThrowBall()
    {
        //E�er basket alan� i�erisinde de�ilse topu ileriye f�rlat
        if(throwCheck && !zoneCheck && !GoalManager.goalCheck)
        {
            ballRB.AddForce(Vector3.up * throwUpForse);
            ballRB.AddForce(Vector3.forward * -throwForwardForse);
            throwCheck = false;
            
        }
        //Basket alan� i�indeyse top baskete girecek
        else if (throwCheck && zoneCheck && !GoalManager.goalCheck)
        {
            Basket();
            throwCheck = false;
        }

    }

    void Calculator()
    {
        //Parma��m�z de�di�i an�n pozisyonu ve zaman�n� bulma
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchTimeStart = Time.time;
            startPos = Input.GetTouch(0).position;
        }

        //Parma��m�z� kald�rd�ktan sonraki pozisyonu ve ba�lang�� ile biti� aras�ndaki zaman� hesaplama
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            touchTimeFinish = Time.time;

            timeInterval = touchTimeFinish - touchTimeStart;

            endPos = Input.GetTouch(0).position;
        }
    }

    void Basket()
    {
        //Ba�lang�� ve biti� pozisyonlar�n�n merkez noktas�
        Vector3 center = (transform.position + goal.position) * 0.5f;

        //Yay �eklinde yapmak i�in merkez noktas�n� a�a��ya do�ru indirme
        center -= distance;

        Vector3 startCenter = transform.position - center;
        Vector3 endCenter = goal.position - center;

        //�ki nokta aras�ndaki yolculuk i�in ge�en s�renin istenen s�reye b�l�nmesi
        float fracComplete = (Time.time - startTime) / journeyTime;

        transform.position = Vector3.Slerp(startCenter, endCenter, fracComplete);
        transform.position += center;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            //Top yere de�di�i zaman �al��acak olan toz efekti
            smokeEffect.transform.position = new Vector3(transform.position.x, smokeEffect.transform.position.y, transform.position.z);
            smokeEffect.Play();
            
            //Topun yere de�mesi ve tekrardan f�rlat�lmaya haz�r olup olmad��� kontrol�
            isGround = true;
            throwCheck = true;
        }
    }

    

    private void OnTriggerStay(Collider other)
    {
        //Basket alan�na giri� kontrol�
        if (other.gameObject.CompareTag("Zone"))
        {
            zoneCheck = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Basket alan�ndan ��k�� kontrol�
        if (other.gameObject.CompareTag("Zone"))
        {
            zoneCheck = false;
        }
    }

}
