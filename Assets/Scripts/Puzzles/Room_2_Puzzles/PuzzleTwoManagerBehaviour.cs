using UnityEngine;

public class PuzzleTwoManagerBehaviour : MonoBehaviour
{
    public int requiredItems = 4;
    private int placedItems = 0;

    public SpinningWallBehaviour spinningWall;
    public GameObject doorknob;

    public AudioSource audio;

    void Awake()
    {
        if (doorknob != null)
        {
            doorknob.SetActive(false);
        }
    }

    public void ItemPlaced()
    {
        placedItems++;
        Debug.Log("Item placed. Total: " + placedItems);

        if (placedItems >= requiredItems)
        {
            audio.Play();
            doorknob.SetActive(true);
        }
    }
}
