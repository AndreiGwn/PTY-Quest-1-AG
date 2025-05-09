using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPad : MonoBehaviour
{
    [SerializeField] Bounce bouncePad; // Drag the Bounce Pad object here (with the script)

    void OnTriggerEnter(Collider other)
    {
        // When the player touches this pad, stop the light pulse on the Bounce pad
        if (other.CompareTag("Player") && bouncePad != null)
        {
            bouncePad.StopLightPulse();
        }

        
    }
}
