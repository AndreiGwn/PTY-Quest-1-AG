using System.Collections;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] float jumpForce = 1000f;

    [Header("Light Pulse Settings")]
    [SerializeField] Light targetLight;
    [SerializeField] float minIntensity = 1f;
    [SerializeField] float maxIntensity = 10f;
    [SerializeField] float pulseDuration = 0.5f;
    [SerializeField] float autoShutdownDelay = 7f; // Auto shutoff after 7 seconds

    private Coroutine lightPulseRoutine;
    private Coroutine shutdownTimerRoutine;

    void OnTriggerEnter(Collider other)
    {
        JumpyJumpy(other);

        if (targetLight != null)
        {
            // Start pulsing light only if not already pulsing
            if (lightPulseRoutine == null)
            {
                lightPulseRoutine = StartCoroutine(PulseLightLoop());
            }

            // Restart auto-shutdown timer
            if (shutdownTimerRoutine != null)
            {
                StopCoroutine(shutdownTimerRoutine);
            }
            shutdownTimerRoutine = StartCoroutine(AutoShutdownLight());
        }
    }

    void JumpyJumpy(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce);
        }
    }

    IEnumerator PulseLightLoop()
    {
        while (true)
        {
            // Dim
            float elapsed = 0f;
            while (elapsed < pulseDuration)
            {
                targetLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, elapsed / pulseDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Brighten
            elapsed = 0f;
            while (elapsed < pulseDuration)
            {
                targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, elapsed / pulseDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

    IEnumerator AutoShutdownLight()
    {
        yield return new WaitForSeconds(autoShutdownDelay);
        StopLightPulse();
    }

    public void StopLightPulse()
    {
        if (lightPulseRoutine != null)
        {
            StopCoroutine(lightPulseRoutine);
            lightPulseRoutine = null;
        }

        StartCoroutine(FadeOutLight());
    }

    private IEnumerator FadeOutLight()
    {
        float startIntensity = targetLight.intensity;
        float elapsed = 0f;

        while (elapsed < pulseDuration)
        {
            targetLight.intensity = Mathf.Lerp(startIntensity, 0f, elapsed / pulseDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        targetLight.intensity = 0f;
    }
}
