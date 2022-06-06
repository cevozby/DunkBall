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
    public float throwForse = 0.3f;

    Rigidbody ballRB;

    // Start is called before the first frame update
    void Start()
    {
        isGround = false;
        ballRB = GetComponent<Rigidbody>();
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
        if(timeInterval <= 0.15f && endPos.y > startPos.y && endPos.y - startPos.y <=365f)
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
        if(throwCheck)
        {
            ballRB.AddForce(Vector3.up * throwForse);
            ballRB.AddForce(Vector3.forward * -throwForse/100f);
            throwCheck = false;
            endPos = Vector3.zero;
            startPos = Vector3.zero;
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

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            
            isGround = true;
            throwCheck = true;
        }
    }

}
