using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform doorTransform; // the hinge or object to rotate
    public Vector3 openEuler = new Vector3(0f, 90f, 0f);
    public Vector3 closedEuler = Vector3.zero;
    public float openTime = 1f;
    private bool isOpen = false;

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(closedEuler, openEuler, openTime));
    }

    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(openEuler, closedEuler, openTime));
    }

    private System.Collections.IEnumerator RotateDoor(Vector3 from, Vector3 to, float t)
    {
        float elapsed = 0f;
        while (elapsed < t)
        {
            elapsed += Time.deltaTime;
            float f = Mathf.SmoothStep(0f, 1f, elapsed / t);
            doorTransform.localEulerAngles = Vector3.Lerp(from, to, f);
            yield return null;
        }
        doorTransform.localEulerAngles = to;
    }
}
