using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallControl : MonoBehaviour
{
    private Touch touch;
    [SerializeField] float speedX, speedZ, speed;

    Vector3 startPos, endPos, direction;
    float touchTimeStart, touchTimeFinish, timeInterval;

    [SerializeField] float jumpForce;
    [SerializeField] float basketForce, throwForce;
    bool isGround, throwCheck, moveCheck, obstacleCheck;
    bool underOfBasket, backOfBasket;
    [SerializeField] Transform under, back;

    Rigidbody ballRB;

    public ParticleSystem smokeEffect;


    bool zoneCheck, shotCheck;
    public Transform goal;


    float startTime;

    [SerializeField] float basketTime, distance, throwTime;
    Sequence seq;

    // Start is called before the first frame update
    void Start()
    {
        isGround = false;
        zoneCheck = false;
        ballRB = GetComponent<Rigidbody>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BallJumping();
        if(moveCheck && throwCheck)
        {
            moveCheck = false;
            ThrowBall();
        }
        else if(moveCheck && !throwCheck)
        {
            Movement();
        }
    }
    private void Update()
    {
        if (moveCheck)
        {
            Calculator();
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
        //Topu f�rlatmak i�in kontrol
        if (timeInterval <= 0.20f && endPos.y > startPos.y)
        {
            throwCheck = true;
            endPos = Vector3.zero;
            startPos = Vector3.zero;
            timeInterval = 0;
        }
    }

    #region BallMovement
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
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                ballRB.velocity = new Vector3(touch.deltaPosition.normalized.x * -speedX, ballRB.velocity.y, 
                    touch.deltaPosition.normalized.y * -speedZ); 
            }
        }
        else
        {
            ballRB.velocity = new Vector3(0f, ballRB.velocity.y, 0f);
        }
    }

    void ThrowBall()
    {
        //E�er basket alan� i�erisinde de�ilse topu ileriye f�rlat
        if (!zoneCheck && !GoalManager.goalCheck)
        {
            Vector3 target = new Vector3(transform.position.x, 0.2f, transform.position.z - distance);
            transform.DOJump(target, throwForce, 1, throwTime);
            throwCheck = false;

        }
        //Basket alan� i�indeyse top baskete girecek
        else if (zoneCheck && !GoalManager.goalCheck)
        {
            Basket();
            throwCheck = false;
        }

    }

    void Basket()
    {
        seq = DOTween.Sequence();
        Vector3 center = (transform.position + goal.position) * 0.5f;
        center = (center + goal.position) * 0.5f;

        if (underOfBasket)
        {
            transform.DOJump(under.position, basketForce/2, 1, basketTime);
        }
        else if (backOfBasket)
        {
            transform.DOJump(back.position, basketForce, 1, basketTime);
        }
        else
        {
            seq.Append(transform.DOJump(goal.position, basketForce, 1, basketTime));
            seq.Append(transform.DOMoveY(0.2f, 0.75f));
        }
    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            //Top yere de�di�i zaman �al��acak olan toz efekti
            smokeEffect.transform.position = new Vector3(transform.position.x, smokeEffect.transform.position.y, transform.position.z);
            smokeEffect.Play();
            
            //Topun yere de�mesi ve tekrardan hareket etmeye haz�r olup olmad��� kontrol�
            isGround = true;
            moveCheck = true;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        //Basket alan�na giri� kontrol�
        if (other.gameObject.CompareTag("Zone"))
        {
            zoneCheck = true;
        }
        if (other.gameObject.CompareTag("UnderofBasket"))
        {
            underOfBasket = true;
        }
        if (other.gameObject.CompareTag("Backofbasket"))
        {
            backOfBasket = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Basket alan�ndan ��k�� kontrol�
        if (other.gameObject.CompareTag("Zone"))
        {
            zoneCheck = false;
        }
        if (other.gameObject.CompareTag("UnderofBasket"))
        {
            underOfBasket = false;
        }
        if (other.gameObject.CompareTag("Backofbasket"))
        {
            backOfBasket = false;
        }
    }

}
