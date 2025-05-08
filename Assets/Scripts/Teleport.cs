using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameDev.tv Challenge Club. Got questions or want to share your nifty solution?
// Head over to - http://community.gamedev.tv

public class Teleport : MonoBehaviour
{
    [SerializeField] Transform teleportTarget;
    [SerializeField] GameObject player;
    [SerializeField] Light areaLight;
    [SerializeField] Light mainWorldLight;

    void Start() 
    {
        // CHALLENGE TIP: Make sure all relevant lights are turned off until you need them on
        // because, you know, that would look cool.
    }

    void OnTriggerEnter(Collider other) 
    {
        TeleportPlayer(other);
        DeactivateObject();
        IlluminateArea();
        // Challenge 5: StartCoroutine ("BlinkWorldLight");
        // Challenge 6: TeleportPlayerRandom();
    }

    void TeleportPlayer(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = teleportTarget.position;
        }
    }

    void DeactivateObject()
    {
        gameObject.SetActive(false);
        Invoke("ReactivateObject", 10f);
    }

    void ReactivateObject()
    {
        gameObject.SetActive(true);
    }

    [SerializeField] Light targetLight;
    [SerializeField] float dimIntensity = 0.2f;
    [SerializeField] float fullIntensity = 1f;
    [SerializeField] float pulseDuration = 1f;
    void IlluminateArea()
    {
       if (targetLight != null)
        {
            StartCoroutine(PulseLight());
        }
    }

    IEnumerator PulseLight()
    {
        float halfDuration = pulseDuration / 2f;
        float timer = 0f;

       // Dimming light code
       while (timer < halfDuration)
        {
            targetLight.intensity = Mathf.Lerp(fullIntensity, dimIntensity, timer / halfDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;

        // Code for making light brighter
        while (timer < halfDuration)
        {
            targetLight.intensity = Mathf.Lerp(dimIntensity, fullIntensity, timer / halfDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensuring the light ends at full
        targetLight.intensity = fullIntensity;
    }

    // IEnumerator BlinkWorldLight()
    // {
            // code goes here
    // }

    void TeleportPlayerRandom()
    {
        // code goes here... or you could modify one of your other methods to do the job.
    }

}
