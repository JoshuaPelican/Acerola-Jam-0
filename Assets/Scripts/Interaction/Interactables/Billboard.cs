using UnityEngine;

public class Billboard : MonoBehaviour
{
    new Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void FixedUpdate()
    {
        
        Vector3 look = Quaternion.LookRotation((this.transform.position - camera.transform.position).normalized, camera.transform.up).eulerAngles;
        look.z = 0;
        look.x = 0;
        transform.rotation = Quaternion.Euler(look);
    }
}
