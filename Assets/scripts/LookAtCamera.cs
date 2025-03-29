using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCamera;
    public bool lockX = false;
    public bool lockY = false;
    public bool lockZ = false;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera != null)
        {
            Vector3 targetDirection = mainCamera.transform.forward;

            if (lockX) targetDirection.x = transform.forward.x;
            if (lockY) targetDirection.y = transform.forward.y;
            if (lockZ) targetDirection.z = transform.forward.z;

            transform.forward = targetDirection;
        }
    }
}