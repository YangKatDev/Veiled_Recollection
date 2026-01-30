using UnityEngine;

public class LightToggleBehaviour : MonoBehaviour
{
    [SerializeField]
    GameObject uvLight, flashLight;

    private bool isOn = false;

    //public AudioSource audio;

    void Start()
    {
        uvLight.SetActive(false);
        flashLight.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //audio.Play();
            ToggleLight();
        }
    }

    void ToggleLight()
    {
        isOn = !isOn;
        uvLight.SetActive(isOn);
        flashLight.SetActive(!isOn);
    }
}
