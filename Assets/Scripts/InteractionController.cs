using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
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
    
    Interactable heldInteractable = null;

    float interactStartTime = 0;
    float chargeAmount = 0;
    bool isCharging;

    public void InputInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            StartInteract();
        else if (context.canceled)
            EndInteract();
    }

    void StartInteract()
    {
        StopAllCoroutines();

        Debug.Log("Start Interact");
        isCharging = true;
        interactStartTime = Time.time;
    }

    private void Update()
    {
        if (isCharging)
            ChargeThrow();
    }

    void ChargeThrow()
    {
        chargeAmount = Mathf.Clamp01(chargeAmount + Time.deltaTime);
    }

    void EndInteract()
    {
        Debug.Log("End Interact");

        if (heldInteractable && Time.time - interactStartTime >= minimumCharge)
        {
            Throw();
        }
        else if(Time.time - interactStartTime < minimumCharge)
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Ray interactRay = camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        Physics.Raycast(interactRay, out RaycastHit hit, interactRange);
        if (hit.transform == null)
            return;

        Debug.Log("Hit");
        hit.transform.TryGetComponent(out Interactable hitInteractable);

        if (hitInteractable == heldInteractable)
            return;

        if (!hitInteractable)
        {
            if (heldInteractable)
                StartCoroutine(Place(hit.point));
        }
        else
        {
            Interact(hitInteractable);
        }
    }

    void Interact(Interactable interactable)
    {
        switch (interactable.type)
        {
            case Interactable.InteractType.Pickup:
                if (heldInteractable)
                    StartCoroutine(Swap(interactable));
                else
                    StartCoroutine(Pickup(interactable));
                break;
        }
    }

    IEnumerator Pickup(Interactable interactable)
    {
        Debug.Log("Pickup");

        heldInteractable = interactable;
        heldInteractable.transform.parent = handTransform;
        heldInteractable.Pickup();

        while (Vector3.Distance(heldInteractable.transform.localPosition, Vector3.zero) > 0.05f)
        {
            heldInteractable.transform.localPosition = Vector3.Slerp(heldInteractable.transform.localPosition, Vector3.zero, pickupSpeed);
            heldInteractable.transform.localRotation = Quaternion.Slerp(heldInteractable.transform.localRotation, Quaternion.identity, pickupSpeed);
            //Quaternion.LookRotation((camera.transform.position - handTransform.position).normalized, handTransform.up)
            yield return null;
        }
    }

    IEnumerator Place(Vector3 targetPosition)
    {
        Debug.Log("Place");

        heldInteractable.Place();

        while (Vector2.Distance(new Vector2(heldInteractable.transform.position.x, heldInteractable.transform.position.z), new Vector2(targetPosition.x, targetPosition.z)) > 0.25f)
        {
            //Interpolate the position
            heldInteractable.transform.position = Vector3.Slerp(heldInteractable.transform.position, targetPosition, pickupSpeed);

            //Interpolate the rotation, resetting any crooked rotations
            Vector3 localAngles = heldInteractable.transform.localRotation.eulerAngles;
            localAngles.x = 0;
            localAngles.z = 0;
            heldInteractable.transform.localRotation = Quaternion.Euler(localAngles);

            yield return null;
        }

        heldInteractable = null;
    }

    IEnumerator Swap(Interactable interactable)
    {
        Debug.Log("Swap");

        yield return Place(interactable.transform.position);
        yield return null;
        yield return Pickup(interactable);
    }

    void Throw()
    {
        Debug.Log("Throw");

        Vector3 force = chargeAmount * throwForce * camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)).direction;
        heldInteractable.Throw(force);
        heldInteractable = null;
    }
}
