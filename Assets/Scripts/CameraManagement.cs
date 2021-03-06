using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagement : MonoBehaviour
{
    public Transform ball;
    public Transform pota;
    public float yOffset = 1.5f, zOffset = 3.3f, divide;
    float yNewOffset, zNewOffset;
    float center;

    // Start is called before the first frame update
    void Start()
    {
        //Top ve pota aras?ndaki merkez nokta
        center = (pota.position.z + ball.position.z)/2;
        yNewOffset = yOffset;
        zNewOffset = zOffset;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Top merkezin gerisindeyse kamera topu sabit bir ?ekilde takip edecek
        if(ball.position.z > center)
        {
            transform.position = new Vector3(ball.position.x, yOffset, ball.position.z + zOffset);
            transform.LookAt(pota);
            //transform.LookAt(new Vector3(0f, pota.position.y, 0f));
        }
        //Top merkezi ge?tikten sonra topun durumuna g?re aktif olarak de?i?ecek
        else if(ball.position.z <= center)
        {
            zNewOffset = zOffset + ((center - ball.position.z) / divide);
            yNewOffset = yOffset + ((center - ball.position.z) / divide);
            transform.position = new Vector3(ball.position.x, yNewOffset, ball.position.z + zNewOffset);
            transform.LookAt(pota);
        }
        
    }
}
