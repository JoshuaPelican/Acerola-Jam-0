using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] float lookSensitivityX = 3f;
    [SerializeField] float lookSensitivityY = 4f;
    [SerializeField] Vector2 lookRangeY = new Vector2(-75f, 60f);
    [SerializeField] new Camera camera;

    float cameraXRotation = 0;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void InputLook(InputAction.CallbackContext callback)
    {
        Vector2 delta = callback.ReadValue<Vector2>();

        //Calculate the camera rotation based on mouse Y axis and clamp it
        cameraXRotation -= delta.y * lookSensitivityY;
        cameraXRotation = Mathf.Clamp(cameraXRotation, lookRangeY.x, lookRangeY.y);

        //Apply camera rotation
        camera.transform.localRotation = Quaternion.Euler(cameraXRotation, 0f, 0f);

        //Apply player rotation using mouse X axis
        transform.Rotate(delta.x * lookSensitivityX * Vector3.up);
    }

}
