using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSlerp : MonoBehaviour
{
    bool zoneCheck;
    public Transform goal;

    //Ba�lang�� noktas�ndan biti� noktas�na gitme h�z�, saniye cinsinden
    [SerializeField] float journeyTime;

    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Basket alan�n�n i�erisindeyse basket olmas� i�in gerekli fonksiyonun �a��r�lmas�
        if (zoneCheck)
        {
            Basket();
        }
    }

    void Basket()
    {
        //Ba�lang�� ve biti� pozisyonlar�n�n merkez noktas�
        Vector3 center = (transform.position + goal.position) * 0.5f;

        //Yay �eklinde yapmak i�in merkez noktas�n� a�a��ya do�ru indirme
        center -= Vector3.up;

        Vector3 startCenter = transform.position - center;
        Vector3 endCenter = goal.position - center;

        float fracComplete = (Time.time - startTime) / journeyTime;

        transform.position = Vector3.Slerp(startCenter, endCenter, fracComplete);
        transform.position += center;
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
