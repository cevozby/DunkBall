using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalManager : MonoBehaviour
{
    public static int goalNumber;
    public ParticleSystem confetti;
    public ParticleSystem confetti2;
    public ParticleSystem confetti3;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Basket"))
        {
            confetti.Play();
            confetti2.Play();
            confetti3.Play();
            goalNumber++;
            StartCoroutine(RestartWorld());
        }
    }

    IEnumerator RestartWorld()
    {
        SceneManager.LoadScene(0);

        yield return new WaitForSeconds(1.5f);
    }

}
