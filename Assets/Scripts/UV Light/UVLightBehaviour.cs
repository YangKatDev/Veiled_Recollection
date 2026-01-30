using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVLightBehaviour : MonoBehaviour
{
    public float detectionRange = 10f;
    public float sphereRadius = 1f;
    public LayerMask detectionMask;

    private HiddenObjectBehaviour currentlyRevealed;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * detectionRange, Color.green);

        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, detectionRange, detectionMask))
        {
            HiddenObjectBehaviour hidden = hit.collider.GetComponent<HiddenObjectBehaviour>();

            if (hidden != null)
            {
                // If we're hitting a new object
                if (currentlyRevealed != hidden)
                {
                    // Hide the previously revealed object
                    if (currentlyRevealed != null)
                        currentlyRevealed.Hide();

                    // Reveal the new object
                    hidden.Reveal();
                    currentlyRevealed = hidden;
                }
            }
            else
            {
                // Hit something else, hide previously revealed
                if (currentlyRevealed != null)
                {
                    currentlyRevealed.Hide();
                    currentlyRevealed = null;
                }
            }
        }
        else
        {
            // Nothing hit – hide the currently revealed object
            if (currentlyRevealed != null)
            {
                currentlyRevealed.Hide();
                currentlyRevealed = null;
            }
        }
    }
}
