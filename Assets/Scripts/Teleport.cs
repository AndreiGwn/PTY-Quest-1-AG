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

    [Header("World Light Blink")]
    [SerializeField] Light worldLight;
    [SerializeField] float blinkDuration = 1f;
    [SerializeField] float worldDimIntensity = 0.2f;
    [SerializeField] float restoreDelay = 0.1f;

    void OnTriggerEnter(Collider other)
    {
        TeleportPlayer(other);
        IlluminateArea();
        StartCoroutine(BlinkWorldLight()); // Challenge 5
        // Challenge 6: TeleportPlayerRandom();
    }

    void TeleportPlayer(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = teleportTarget.position;
        }
    }

    [Header("Target Light Pulse")]
    [SerializeField] Light targetLight;
    [SerializeField] float pulseDimIntensity = 0.2f;
    [SerializeField] float fullIntensity = 10f;
    [SerializeField] float pulseDuration = 1f;

    void IlluminateArea()
    {
        if (targetLight != null)
        {
            StartCoroutine(DelayedPulseLight());
        }
    }

    IEnumerator DelayedPulseLight()
    {
        // Wait before helping the player
        yield return new WaitForSeconds(10f);
        yield return StartCoroutine(PulseLight());
    }

    IEnumerator PulseLight()
    {
        float halfDuration = pulseDuration / 2f;
        float timer = 0f;

        // Dimming light code
        while (timer < halfDuration)
        {
            targetLight.intensity = Mathf.Lerp(fullIntensity, pulseDimIntensity, timer / halfDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;

        // Code for making light brighter
        while (timer < halfDuration)
        {
            targetLight.intensity = Mathf.Lerp(pulseDimIntensity, fullIntensity, timer / halfDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensuring the light ends at full
        targetLight.intensity = fullIntensity;
    }

    IEnumerator BlinkWorldLight()
    {
        if (worldLight == null)
        {
            Debug.LogWarning("World light is not assigned.");
            yield break;
        }

        Debug.Log("BlinkWorldLight started.");

        // Save original rotation
        Quaternion originalRotation = worldLight.transform.rotation;

        // Create target rotation (X = -100)
        Vector3 euler = worldLight.transform.eulerAngles;
        Quaternion dimRotation = Quaternion.Euler(-100f, euler.y, euler.z);

        // Force update
        worldLight.enabled = false;
        worldLight.transform.rotation = dimRotation;
        worldLight.enabled = true;

        Debug.Log("Dimmed Rotation: " + worldLight.transform.eulerAngles);

        yield return new WaitForSeconds(restoreDelay);

        // Restore original
        worldLight.enabled = false;
        worldLight.transform.rotation = originalRotation;
        worldLight.enabled = true;

        Debug.Log("Restored Rotation: " + worldLight.transform.eulerAngles);
    }

    void TeleportPlayerRandom()
    {
        // code goes here... or you could modify one of your other methods to do the job.
    }
}
