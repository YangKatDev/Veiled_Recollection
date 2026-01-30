using UnityEngine;
using TMPro;

public class KeypadTrigger : MonoBehaviour
{
    [Header("References")]
    public KeypadManager keypad;

    [Header("UI")]
    public TMP_Text interactText;       // "Press E to Interact"
    public string promptMessage = "Press E to Interact";

    [Header("Settings")]
    public float interactDistance = 3f;

    private Transform player;

    void Start()
    {
        // FPS camera or Player
        if (Camera.main != null)
            player = Camera.main.transform;
        else
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (interactText)
            interactText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!player || !keypad) return;

        float dist = Vector3.Distance(player.position, transform.position);

        // --------------------------------------------
        // 🔥 1. Out of range → hide prompt + close keypad
        // --------------------------------------------
        if (dist > interactDistance)
        {
            if (interactText) interactText.gameObject.SetActive(false);

            if (keypad.keypadOpen)
                keypad.HideKeypad();

            return;
        }

        // --------------------------------------------
        // 🔥 2. If keypad is open → NEVER show interact text
        // --------------------------------------------
        if (keypad.keypadOpen)
        {
            if (interactText) interactText.gameObject.SetActive(false);
            return;
        }

        // --------------------------------------------
        // 🔥 3. In range + keypad closed → show prompt
        // --------------------------------------------
        if (interactText)
        {
            interactText.text = promptMessage;
            interactText.gameObject.SetActive(true);
        }

        // --------------------------------------------
        // 🔥 4. Press E to open keypad
        // --------------------------------------------
        if (Input.GetKeyDown(KeyCode.E))
        {
            keypad.ShowKeypad();

            // Force hide text so both UI layers never overlap
            if (interactText)
                interactText.gameObject.SetActive(false);
        }
    }
}
