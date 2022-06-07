using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSlerp : MonoBehaviour
{
    bool zoneCheck;
    public Transform goal;

    //Baþlangýç noktasýndan bitiþ noktasýna gitme hýzý, saniye cinsinden
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
        //Basket alanýnýn içerisindeyse basket olmasý için gerekli fonksiyonun çaðýrýlmasý
        if (zoneCheck)
        {
            Basket();
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
