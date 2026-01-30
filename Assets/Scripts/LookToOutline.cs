using UnityEngine;

public class LookToOutline : MonoBehaviour
{
    public float interactDistance = 3f;
    private AlwaysOutline current;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            var outline = hit.collider.GetComponentInChildren<AlwaysOutline>();

            if (outline != current)
            {
                if (current != null)
                    current.DisableOutline();

                current = outline;

                if (current != null)
                    current.EnableOutline();
            }
        }
        else if (current != null)
        {
            current.DisableOutline();
            current = null;
        }

        Debug.DrawRay(ray.origin, ray.direction * interactDistance, Color.cyan);
    }
}
