using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //Top zeminin dýþýna çýkarsa oyunu yeniden baþlat
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
