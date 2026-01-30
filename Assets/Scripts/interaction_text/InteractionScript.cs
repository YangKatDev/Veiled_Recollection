using UnityEngine;
using TMPro;

public class InteractionScript : MonoBehaviour
{
    [Header("UI Prompt")]
    public TMP_Text interactPrompt;
    public string promptMessage = "Left Click to Interact";

    [Header("Settings")]
    public float interactRange = 4f;

    [Header("Event Target")]
    public MonoBehaviour interactTarget;    // Script to call
    public string interactMethod = "Interact"; // Method name on that script

    private Transform playerCam;
    private bool isNear = false;

    void Start()
    {
        playerCam = Camera.main.transform;

        if (interactPrompt)
            interactPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckDistance();

        if (isNear && Input.GetMouseButtonDown(0))
        {
            TriggerInteraction();
        }
    }

    void CheckDistance()
    {
        float dist = Vector3.Distance(playerCam.position, transform.position);
        bool nowNear = dist < interactRange;

        if (!isNear && nowNear)
            ShowPrompt();
        else if (isNear && !nowNear)
            HidePrompt();

        isNear = nowNear;
    }

    void ShowPrompt()
    {
        if (interactPrompt == null) return;

        interactPrompt.text = promptMessage;
        interactPrompt.gameObject.SetActive(true);
    }

    void HidePrompt()
    {
        if (interactPrompt == null) return;

        interactPrompt.gameObject.SetActive(false);
    }

    void TriggerInteraction()
    {
        HidePrompt();

        if (interactTarget != null)
        {
            interactTarget.Invoke(interactMethod, 0f);
        }
        else
        {
            Debug.LogWarning($"{name}: No interaction target assigned!");
        }
    }
}
