using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool IsInteractable = true;

    protected Collider col;
    protected Rigidbody rb;
    
    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }
}
