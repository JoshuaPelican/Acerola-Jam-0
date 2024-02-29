using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] float lookSensitivityX = 3f;
    [SerializeField] float lookSensitivityY = 4f;
    [SerializeField] Vector2 lookRangeY = new Vector2(-75f, 60f);
    [SerializeField] Vector3 offset = new Vector3(0, 0.3f, 0);
    [SerializeField] new Camera camera;

    float cameraXRotation = 0;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        camera.transform.position = transform.position + offset;
    }

    public void InputLook(InputAction.CallbackContext callback)
    {
        Vector2 delta = callback.ReadValue<Vector2>();

        //Calculate the camera rotation based on mouse Y axis and clamp it
        cameraXRotation -= delta.y * lookSensitivityY;
        cameraXRotation = Mathf.Clamp(cameraXRotation, lookRangeY.x, lookRangeY.y);

        //Apply camera rotation
        camera.transform.localRotation = Quaternion.Euler(cameraXRotation, transform.rotation.eulerAngles.y, 0f);

        //Apply player rotation using mouse X axis
        transform.Rotate(delta.x * lookSensitivityX * Vector3.up);
    }

}
