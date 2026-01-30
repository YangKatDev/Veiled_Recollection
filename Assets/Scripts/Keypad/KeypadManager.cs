using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class KeypadManager : MonoBehaviour
{
    [Header("Code Settings")]
    public string correctCode = "1234";
    public int maxDigits = 4;
    private string current = "";

    [Header("UI References")]
    public TMP_Text displayText;
    public GameObject keypadCanvas;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip wrongSound;
    public AudioClip correctSound;

    [Header("Player References")]
    public MonoBehaviour playerController;  // movement script
    public Camera playerCamera;

    [Header("Object To Remove When Correct")]
    public GameObject objectToRemove;

    [Header("Events")]
    public UnityEvent onCorrect;

    [HideInInspector]
    public bool keypadOpen = false;

    void Start()
    {
        if (keypadCanvas != null)
            keypadCanvas.SetActive(false);

        UpdateDisplay();
    }

    // ================== SHOW KEYPAD ==================
    public void ShowKeypad()
    {
        keypadOpen = true;
        keypadCanvas.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerController != null)
            playerController.enabled = false;
    }

    // ================== HIDE KEYPAD ==================
    public void HideKeypad()
    {
        keypadOpen = false;

        if (keypadCanvas != null)
            keypadCanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerController != null)
            playerController.enabled = true;

        current = "";
        UpdateDisplay();
    }

    // ================== INPUT ==================
    public void PressKey(string key)
    {
        if (current.Length >= maxDigits)
            return;

        current += key;
        UpdateDisplay();
    }

    public void Clear()
    {
        current = "";
        UpdateDisplay();
    }

    public void Backspace()
    {
        if (current.Length > 0)
        {
            current = current.Substring(0, current.Length - 1);
            UpdateDisplay();
        }
    }

    // ================== SUBMIT ==================
    public void Submit()
    {
        if (current == correctCode)
        {
            // Correct audio
            if (audioSource && correctSound)
                audioSource.PlayOneShot(correctSound);

            // REMOVE OBJECT
            if (objectToRemove != null)
                Destroy(objectToRemove);

            onCorrect?.Invoke();

            HideKeypad();
        }
        else
        {
            // Wrong audio
            if (audioSource && wrongSound)
                audioSource.PlayOneShot(wrongSound);

            current = "";
            UpdateDisplay();
        }
    }

    // ================== DISPLAY ==================
    private void UpdateDisplay()
    {
        if (displayText != null)
            displayText.text = current;
    }
}
