using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Renderer), typeof(Collider))]
public class HiddenObjectBehaviour : MonoBehaviour
{
    private Renderer rend;
    private Collider col;

    private bool isRevealed = false;
    private bool isLookingAt = false;

    public string requiredTag = "Hidden";
    public float revealDistance = 8f;

    [Header("Inventory")]
    [SerializeField] private InventoryItem itemToAdd;
    [SerializeField] private InventoryBehaviour inventory;

    public Light uvFlashlight;

    [Header("UI Prompt")]
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private string interactMessage = "Left Click to pick up";
    private TextMeshProUGUI promptText;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;

    [Header("Audio Settings")]
    [SerializeField, Range(0f, 1f)] private float pickupVolume = 1f;         // volume
    [SerializeField, Range(0.1f, 10f)] private float pickupDuration = 1f;   // duration of sound in seconds


    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();

        if (interactPrompt != null)
        {
            promptText = interactPrompt.GetComponentInChildren<TextMeshProUGUI>();
            if (promptText != null)
                promptText.text = interactMessage;

            interactPrompt.SetActive(false);
        }

        Hidden();
    }

    void Update()
    {
        HandleRevealLogic();

        // check pickup input while looking at the object
        if (isLookingAt && Input.GetMouseButtonDown(0))
            TryPickup();
    }


    //------------------------------------
    // Reveal logic
    //------------------------------------
    void HandleRevealLogic()
    {
        if (uvFlashlight != null && uvFlashlight.enabled && uvFlashlight.gameObject.activeInHierarchy)
        {
            Vector3 toObject = transform.position - uvFlashlight.transform.position;
            float distance = toObject.magnitude;
            float angle = Vector3.Angle(uvFlashlight.transform.forward, toObject);

            bool inCone = distance < revealDistance && angle < uvFlashlight.spotAngle * 0.5f;

            if (inCone) Reveal();
            else Hide();
        }
        else
        {
            Hide();
        }
    }

    void Hidden()
    {
        rend.enabled = false;
        col.enabled = false;
    }

    public void Reveal()
    {
        if (!isRevealed)
        {
            isRevealed = true;
            rend.enabled = true;
            col.enabled = true;
            gameObject.layer = LayerMask.NameToLayer("HiddenTest");
        }
    }

    public void Hide()
    {
        if (isRevealed)
        {
            isRevealed = false;
            rend.enabled = false;
            col.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("HiddenTest");
        }
    }


    //------------------------------------
    // Hover UI
    //------------------------------------
    private void OnMouseEnter()
    {
        if (isRevealed && interactPrompt != null)
        {
            interactPrompt.SetActive(true);
            isLookingAt = true;
        }
    }

    private void OnMouseExit()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        isLookingAt = false;
    }


    //------------------------------------
    // Pickup logic with volume & duration
    //------------------------------------
    private void TryPickup()
    {
        if (!isRevealed) return;

        if (pickupSound != null && Camera.main != null)
        {
            // Create temporary audio object at camera position
            GameObject tempAudio = new GameObject("TempPickupAudio");
            tempAudio.transform.position = Camera.main.transform.position;

            AudioSource aSource = tempAudio.AddComponent<AudioSource>();
            aSource.clip = pickupSound;
            aSource.volume = pickupVolume;
            aSource.spatialBlend = 0f; // 2D sound
            aSource.Play();

            // Destroy temporary object after specified duration
            Destroy(tempAudio, pickupDuration);
        }

        // Add item to inventory
        if (inventory != null && itemToAdd != null)
            inventory.AddInventoryItem(itemToAdd);

        // Hide prompt
        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        // Destroy object
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        if (isRevealed && CompareTag(requiredTag))
            TryPickup();
    }

    //------------------------------------
    // Interact method for other scripts
    //------------------------------------
    public void Interact()
    {
        TryPickup();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, revealDistance);
    }
}
