using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorknobBehaviour : MonoBehaviour
{

    void OnMouseDown()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
