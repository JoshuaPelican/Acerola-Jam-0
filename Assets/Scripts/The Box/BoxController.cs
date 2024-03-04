using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public static List<Pickup> InBox = new();
    public static bool IsInBox(Pickup pickup) => InBox.Contains(pickup);

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Pickup pickup))
            return;

        Debug.Log($"{pickup.name} in Box");

        InBox.Add(pickup);
        pickup.OnPickUp.AddListener(() => InBox.Remove(pickup));
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out Pickup pickup))
            return;

        Debug.Log($"{pickup.name} out Box");

        InBox.Remove(pickup);
        pickup.OnPickUp.RemoveListener(() => InBox.Remove(pickup));
    }

    private void Reset()
    {
        InBox.Clear();
    }
}
