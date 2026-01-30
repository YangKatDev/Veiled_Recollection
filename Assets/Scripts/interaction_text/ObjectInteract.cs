using UnityEngine;
using TMPro;

public class ObjectInteract : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text interactPrompt;
    public TMP_Text messageText;

    [Header("Messages")]
    public string promptMessage = "Left Click to Interact";
    public string pickupMessage = "Item Acquired!";

    [Header("Interaction Settings")]
    public float interactRange = 4f;
    public float messageDuration = 2.5f;

    [Header("Audio")]
    public AudioClip pickupSound;
    private AudioSource audioSource;

    private Transform playerCam;
    private bool interacted = false;
    private bool isNear = false;
    private float hideTimer = 0f;

    private float checkDelay = 0f;
    private const float CHECK_INTERVAL = 0.08f;

    private HiddenObjectBehaviour hiddenObj;

    void Start()
    {
        playerCam = Camera.main.transform;

        // Start UI invisible (no SetActive)
        SetAlpha(interactPrompt, 0f);
        SetAlpha(messageText, 0f);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        hiddenObj = GetComponent<HiddenObjectBehaviour>();
    }

    void Update()
    {
        if (!interacted)
        {
            CheckDistance();

            if (isNear && Input.GetMouseButtonDown(0))
                Pickup();
        }
        else
        {
            hideTimer -= Time.deltaTime;

            if (hideTimer <= 0f)
                SetAlpha(messageText, 0f); // fade out, no snapping
        }
    }

    void CheckDistance()
    {
        checkDelay -= Time.deltaTime;
        if (checkDelay > 0f) return;
        checkDelay = CHECK_INTERVAL;

        // Hidden? Don’t allow interaction
        if (hiddenObj != null)
        {
            Renderer r = GetComponent<Renderer>();
            Collider c = GetComponent<Collider>();

            if ((r && !r.enabled) || (c && !c.enabled))
            {
                SetAlpha(interactPrompt, 0f); // hide prompt safely
                isNear = false;
                return;
            }
        }

        float dist = Vector3.Distance(playerCam.position, transform.position);
        bool nowNear = dist <= interactRange;

        if (!isNear && nowNear)
            ShowPrompt();
        else if (isNear && !nowNear)
            HidePrompt();

        isNear = nowNear;
    }

    void ShowPrompt()
    {
        if (!interactPrompt) return;
        interactPrompt.text = promptMessage;
        SetAlpha(interactPrompt, 1f); // fade in — no SetActive
    }

    void HidePrompt()
    {
        if (!interactPrompt) return;
        SetAlpha(interactPrompt, 0f); // fade out — no SetActive
    }

    void Pickup()
    {
        interacted = true;
        HidePrompt();

        if (messageText != null)
        {
            messageText.text = pickupMessage;
            SetAlpha(messageText, 1f); // fade in — smooth
        }

        hideTimer = messageDuration;

        if (pickupSound != null)
            audioSource.PlayOneShot(pickupSound);

        var pick = GetComponent<PickUpFlashlightBehaviour>();
        if (pick != null)
            pick.ActiveFlashlight();

        if (TryGetComponent(out Renderer r)) r.enabled = false;
        if (TryGetComponent(out Collider c)) c.enabled = false;

        float destroyDelay = messageDuration +
                             (pickupSound != null ? pickupSound.length : 0.25f);
        Destroy(gameObject, destroyDelay);
    }

    // 🔥 The magic fix — changes alpha without rebuilding UI
    void SetAlpha(TMP_Text text, float a)
    {
        if (!text) return;
        Color c = text.color;
        c.a = a;
        text.color = c;
    }
}
