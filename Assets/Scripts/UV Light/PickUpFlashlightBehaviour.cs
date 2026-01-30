using UnityEngine;

public class PickUpFlashlightBehaviour : MonoBehaviour
{
    public GameObject lightManager;

    void Awake()
    {
        lightManager.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create a ray from the camera through the mouse position
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if we hit the flashlight
                if (hit.collider.gameObject.CompareTag("flashlight"))
                {
                    Destroy(hit.collider.gameObject); // Destroy the flashlight (the object we hit)
                    ActiveFlashlight();
                }
            }
        }
    }

    public void ActiveFlashlight()
    {
        lightManager.gameObject.SetActive(true); // Activate the lightManager
    }
}
