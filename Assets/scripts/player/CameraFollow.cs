using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float smoothSpeed = 5f;
    public string obstacleTag = "wall"; 

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        RaycastHit hit;
        if (Physics.Raycast(target.position, desiredPosition - target.position, out hit))
        {
            if (hit.collider.CompareTag(obstacleTag))
            {
                desiredPosition = hit.point;
            }
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}
