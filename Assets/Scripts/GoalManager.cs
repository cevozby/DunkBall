using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{
    public static int goalNumber;
    public static bool goalCheck;
    public ParticleSystem confetti;
    public ParticleSystem confetti2;
    public ParticleSystem confetti3;


    // Start is called before the first frame update
    void Start()
    {
        goalCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Basket girdiði an efektleri çalýþtýr ve oyunu 2.5 saniye sonra yeniden baþlat
        if (other.gameObject.CompareTag("Basket"))
        {
            goalCheck = true;
            confetti.Play();
            confetti2.Play();
            confetti3.Play();
            goalNumber++;
            StartCoroutine(RestartWorld());
        }
    }

    IEnumerator RestartWorld()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(0);

        
    }

}
