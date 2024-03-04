using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] Image cursor;
    [SerializeField] Image chargeBar;


    private void OnEnable()
    {
        InteractionController.OnCanInteract.AddListener(UpdateInteractCursor);
        InteractionController.OnCharge.AddListener(UpdateChargeBar);
    }

    void UpdateInteractCursor(bool canInteract)
    {
        cursor.CrossFadeAlpha(canInteract ? 1f : 0.5f, 0.25f, false);
    }

    void UpdateChargeBar(float charge)
    {
        chargeBar.fillAmount = charge / 1;
    }
}
