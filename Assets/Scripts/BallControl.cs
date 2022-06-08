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


    bool zoneCheck;
    public Transform goal;

    //Baþlangýç noktasýndan bitiþ noktasýna gitme hýzý, saniye cinsinden
    [SerializeField] float journeyTime;

    float startTime;

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
        if(timeInterval <= 0.15f && endPos.y > startPos.y)
        {
            ThrowBall();
        }
        
        
    }

    private void Update()
    {
        
    }

    void BallJumping()
    {
        if (isGround)
        {
            ballRB.AddForce(Vector3.up * jumpForce);
            isGround = false;
        }
    }

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
        if(throwCheck && !zoneCheck)
        {
            ballRB.AddForce(Vector3.up * throwUpForse);
            ballRB.AddForce(Vector3.forward * -throwForwardForse);
            throwCheck = false;
            endPos = Vector3.zero;
            startPos = Vector3.zero;
        }
        else if (throwCheck && zoneCheck)
        {
            Basket();
        }

    }

    void Calculator()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchTimeStart = Time.time;
            startPos = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            touchTimeFinish = Time.time;

            timeInterval = touchTimeFinish - touchTimeStart;

            endPos = Input.GetTouch(0).position;

            //direction = startPos - endPos;

            //ballRB.AddForce(new Vector3(0f, 1f, 1f) * -throwForse);
        }
    }

    void Basket()
    {
        //Baþlangýç ve bitiþ pozisyonlarýnýn merkez noktasý
        Vector3 center = (transform.position + goal.position) * 0.5f;

        //Yay þeklinde yapmak için merkez noktasýný aþaðýya doðru indirme
        center -= Vector3.up;

        Vector3 startCenter = transform.position - center;
        Vector3 endCenter = goal.position - center;

        float fracComplete = (Time.time - startTime) / journeyTime;

        transform.position = Vector3.Slerp(startCenter, endCenter, fracComplete);
        transform.position += center;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            smokeEffect.transform.position = new Vector3(transform.position.x, smokeEffect.transform.position.y, transform.position.z);
            smokeEffect.Play();
            
            isGround = true;
            throwCheck = true;
        }
    }

    

    private void OnTriggerStay(Collider other)
    {
        //Basket alanýna giriþ kontrolü
        if (other.gameObject.CompareTag("Zone"))
        {
            zoneCheck = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Basket alanýndan çýkýþ kontrolü
        if (other.gameObject.CompareTag("Zone"))
        {
            zoneCheck = false;
        }
    }

}
