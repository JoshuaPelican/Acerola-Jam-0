using UnityEngine;

public class Pickup : Interactable
{
    public bool IsBeingHeld => Holder != null;

    InteractionController Holder;

    public void PickUp(InteractionController holder)
    {
        Holder = holder;

        col.enabled = false;
        rb.isKinematic = true;
    }

    public void Release()
    {
        Holder = null;
        transform.parent = null;

        col.enabled = true;
        rb.isKinematic = false;
    }

    public void ForceRelease()
    {
        Holder.held = null;
        Release();
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
}
