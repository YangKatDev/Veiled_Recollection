using UnityEngine;

public class DoorknobActivatorBehaviour : MonoBehaviour
{
    public SpinningWallBehaviour spinningWall;
    public GameObject doorknob;

    void Start()
    {
        if (doorknob != null)
        {
            doorknob.SetActive(false);
        }
    }

    void Update()
    {
        if (spinningWall != null && doorknob != null)
        {
            doorknob.SetActive(spinningWall.isSpinning);
        }
        else if (doorknob == null)
        {
            enabled = false;
        }
        else if (spinningWall == null)
        {
            enabled = false;
        }
    }
}
