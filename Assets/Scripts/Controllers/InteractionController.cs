using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InteractionController : MonoBehaviour
{
    public static UnityEvent<bool> OnCanInteract = new();
    public static UnityEvent<float> OnCharge = new();

    [Header("Interact Settings")]
    [SerializeField] float interactRange = 3f;

    [Header("Pickup Settings")]
    [Range(0, 1)]
    [SerializeField] float pickupSpeed = 0.75f;

    [Header("Throw Settings")]
    [SerializeField] float throwForce = 3f;
    [Range(0, 1)] 
    [SerializeField] float minimumCharge;

    [Header("Components")]
    [SerializeField] new Camera camera;
    [SerializeField] Transform handTransform;
    
    public Pickup held = null;

    float interactStartTime = 0;
    float chargeAmount = 0;
    bool isCharging;

    private void FixedUpdate()
    {
        OnCanInteract.Invoke(CanInteract());
    }

    private void Update()
    {
        if (isCharging)
            ChargeThrow();
    }

    public void InputInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            StartInteract();
        else if (context.canceled)
            EndInteract();
    }

    bool CanInteract()
    {
        Ray interactRay = camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        Physics.Raycast(interactRay, out RaycastHit hit, interactRange, LayerMask.GetMask("Interactable"));
        if (hit.transform == null)
            return false;

        hit.transform.TryGetComponent(out Interactable hitInteractable);

        if (!hitInteractable)
            return false;

        return hitInteractable.IsInteractable;
    }

    void StartInteract()
    {
        StopAllCoroutines();

        isCharging = true;
        interactStartTime = Time.time;
    }

    void ChargeThrow()
    {
        chargeAmount = Mathf.Clamp01(chargeAmount + Time.deltaTime);

        if(chargeAmount >= minimumCharge)
            OnCharge.Invoke(chargeAmount);
    }

    void ResetCharge()
    {
        chargeAmount = 0;
        OnCharge.Invoke(0);
    }

    void EndInteract()
    {
        isCharging = false;

        if (held && Time.time - interactStartTime >= minimumCharge)
        {
            Throw();
        }
        else if(Time.time - interactStartTime < minimumCharge)
        {
            TryInteract();
        }

        ResetCharge();
    }

    void TryInteract()
    {
        Ray interactRay = camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        Physics.Raycast(interactRay, out RaycastHit hit, interactRange);
        if (hit.transform == null)
            return;

        hit.transform.TryGetComponent(out Interactable hitInteractable);

        if (!hitInteractable) // No interactable
        {
            if (held)
                StartCoroutine(Place(hit.point));
        }
        else
        {
            Interact(hitInteractable);
        }
    }

    void Interact(Interactable interactable)
    {
        if (!interactable.IsInteractable)
            return;

        if (interactable is Pickup pickup)
        {
            if (pickup == held)
                return;

            if (held)
                StartCoroutine(Swap(pickup));
            else
                StartCoroutine(Pickup(pickup));
        }
    }

    IEnumerator Pickup(Pickup pickup)
    {
        held = pickup;
        held.transform.parent = handTransform;
        held.PickUp(this);

        while (Vector3.Distance(held.transform.localPosition, Vector3.zero) > 0.05f)
        {
            held.transform.localPosition = Vector3.Slerp(held.transform.localPosition, Vector3.zero, pickupSpeed);
            held.transform.localRotation = Quaternion.Slerp(held.transform.localRotation, Quaternion.identity, pickupSpeed);
            yield return null;
        }
    }

    IEnumerator Place(Vector3 targetPosition)
    {
        held.Place();

        while (Vector2.Distance(new Vector2(held.transform.position.x, held.transform.position.z), new Vector2(targetPosition.x, targetPosition.z)) > 0.25f)
        {
            //Interpolate the position
            held.transform.position = Vector3.Slerp(held.transform.position, targetPosition, pickupSpeed);

            //Interpolate the rotation, resetting any crooked rotations
            Vector3 localAngles = held.transform.localRotation.eulerAngles;
            localAngles.x = 0;
            localAngles.z = 0;
            held.transform.localRotation = Quaternion.Euler(localAngles);

            yield return null;
        }

        held = null;
    }

    IEnumerator Swap(Pickup pickup)
    {
        yield return Place(pickup.transform.position);
        yield return null;
        yield return Pickup(pickup);
    }

    void Throw()
    {
        Vector3 force = chargeAmount * throwForce * camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)).direction;
        held.Throw(force);
        held = null;
    }
}
