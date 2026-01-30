using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RippedPaperInteract : MonoBehaviour
{
    [Header("Interaction")]
    public float interactDistance = 3f;
    Camera cam;

    [Header("UI - Note Panel")]
    public CanvasGroup paperCanvasGroup;
    public Image paperImage;
    public TextMeshProUGUI bodyText;

    [Header("UI - Prompt")]
    public GameObject interactionPrompt;   // "Left Click to Read"

    [Header("Note Text")]
    [TextArea(3, 10)]
    public string textToShow = "This is a torn note...";

    bool isOpen = false;

    void Start()
    {
        cam = Camera.main;

        // make sure UI starts hidden
        paperCanvasGroup.alpha = 0;
        bodyText.text = "";
        if (interactionPrompt) interactionPrompt.SetActive(false);
    }

    void Update()
    {
        if (isOpen)
        {
            if (Input.GetMouseButtonDown(0))
                ClosePaper();

            // hide prompt while open
            if (interactionPrompt) interactionPrompt.SetActive(false);
            return;
        }

        HandlePromptAndInteraction();
    }

    void HandlePromptAndInteraction()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (interactionPrompt && !interactionPrompt.activeSelf)
                    interactionPrompt.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                    OpenPaper();

                return;
            }
        }

        // not looking at object → hide prompt
        if (interactionPrompt && interactionPrompt.activeSelf)
            interactionPrompt.SetActive(false);
    }

    void OpenPaper()
    {
        isOpen = true;
        paperCanvasGroup.alpha = 1;
        bodyText.text = textToShow;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f; // optional pause
    }

    void ClosePaper()
    {
        isOpen = false;
        paperCanvasGroup.alpha = 0;
        bodyText.text = "";

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
    }
}
