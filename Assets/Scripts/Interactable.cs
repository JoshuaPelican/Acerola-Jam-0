using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool IsInteractable = true;
    public InteractType type;

    Collider col;
    Rigidbody rb;
    
    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public void Pickup()
    {
        col.enabled = false;
        rb.isKinematic = true;
    }

    void Release()
    {
        transform.parent = null;

        col.enabled = true;
        rb.isKinematic = false;
    }

    public void Place()
    {
        Release();
    }

    public void Throw(Vector3 force)
    {
        Release();
        rb.AddForce(force, ForceMode.Impulse);
    }

    public enum InteractType
    {
        Pickup
    }
}
