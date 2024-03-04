using UnityEngine;
using UnityEngine.Events;

public class Pickup : Interactable
{
    public UnityEvent OnPickUp = new();
    public UnityEvent OnRelease = new();

    public bool IsBeingHeld => holder != null;
    
    public Renderer Renderer { get; private set; }
    InteractionController holder;

    public override void Awake()
    {
        base.Awake();
        Renderer = GetComponentInChildren<Renderer>();
    }

    public void PickUp(InteractionController holder)
    {
        this.holder = holder;

        col.enabled = false;
        rb.isKinematic = true;

        OnPickUp.Invoke();
    }

    public void Release()
    {
        holder = null;
        transform.parent = null;

        col.enabled = true;
        rb.isKinematic = false;

        OnRelease.Invoke();
    }

    public void ForceRelease()
    {
        holder.held = null;
        Release();
    }

    public void Place()
    {
        Release();
    }

    public void Throw(Vector3 force)
    {
        Release();
        Vector3 rot = transform.rotation.eulerAngles;
        rot.z = 0;
        rot.x = 0;
        transform.rotation = Quaternion.Euler(rot);
        rb.AddForce(force, ForceMode.Impulse);
    }
}
