using UnityEngine;
using TMPro;

public class MementoItemPlacerBehaviour : MonoBehaviour
{
    [Header("Required Item")]
    [SerializeField] private string requiredItemName;

    [Header("References")]
    [SerializeField] private GameObject mementoItemPrefab;
    [SerializeField] private Dialogue playedDialogue;

    public InventoryBehaviour inventory;
    public PuzzleTwoManagerBehaviour puzzleManager;

    [Header("UI Prompt")]
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private string promptMessage = "Left Click to place item";

    private TextMeshProUGUI promptText;
    private bool isLookingAt = false;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip placeSound;


    void Start()
    {
        if (interactPrompt != null)
        {
            promptText = interactPrompt.GetComponentInChildren<TextMeshProUGUI>();
            if (promptText) promptText.text = promptMessage;

            interactPrompt.SetActive(false);
        }
    }

    void OnMouseEnter()
    {
        if (HasItem())
        {
            if (interactPrompt != null)
                interactPrompt.SetActive(true);

            isLookingAt = true;
        }
    }

    void OnMouseExit()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        isLookingAt = false;
    }

    void Update()
    {
        if (isLookingAt && Input.GetMouseButtonDown(0))
            TryPlace();
    }

    private void TryPlace()
    {
        var items = inventory.GetInventoryItems();
        InventoryItem mementoItem = items.Find(i => i.itemName == requiredItemName);

        if (mementoItem != null)
        {
            items.Remove(mementoItem);
            inventory.OnInventoryItemChange?.Invoke();

            if (mementoItemPrefab != null)
                mementoItemPrefab.SetActive(true);

            if (puzzleManager != null)
                puzzleManager.ItemPlaced();

            // ▶ PLAY SOUND
            if (audioSource && placeSound)
                audioSource.PlayOneShot(placeSound);

            if (playedDialogue != null)
                DialogueHolderBehaviour.OnSayDialogue?.Invoke(playedDialogue);

            if (interactPrompt != null)
                interactPrompt.SetActive(false);

            this.enabled = false;
        }
    }

    private bool HasItem()
    {
        var items = inventory.GetInventoryItems();
        return items.Exists(i => i.itemName == requiredItemName);
    }
}

