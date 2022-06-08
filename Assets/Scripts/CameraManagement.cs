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
        center = (pota.position.z - ball.position.z)/2;
        yNewOffset = yOffset;
        zNewOffset = zOffset;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(ball.position.z > center)
        {
            transform.position = new Vector3(ball.position.x, yOffset, ball.position.z + zOffset);
            transform.LookAt(pota);
        }
        else if(ball.position.z <= center)
        {
            zNewOffset = zOffset + ((center - ball.position.z) / divide);
            yNewOffset = yOffset + ((center - ball.position.z) / divide);
            transform.position = new Vector3(ball.position.x, yNewOffset, ball.position.z + zNewOffset);
            transform.LookAt(pota);
        }
        
    }
}
