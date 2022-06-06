using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagement : MonoBehaviour
{
    public Transform ball;
    public Transform pota;
    public float yOffset = 1.5f, zOffset = 3.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(ball.position.x, yOffset, ball.position.z + zOffset);
        transform.LookAt(pota);
    }
}
