using UnityEngine;

public class UIManagerBehaviour : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject controlsMenu;
    public GameObject creditsMenu;

    void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void ShowCredits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
        controlsMenu.SetActive(false);
    }

    public void ShowControls()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void BackToMenu()
    {
        ShowMainMenu();
    }
}
