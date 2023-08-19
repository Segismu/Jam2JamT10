using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public Transform leftLimit;
    public Transform rightLimit;
    public Transform topLimit;
    public Transform bottomLimit;

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

     
        float minX = leftLimit.position.x + Camera.main.orthographicSize * Camera.main.aspect;
        float maxX = rightLimit.position.x - Camera.main.orthographicSize * Camera.main.aspect;
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);

        float minY = bottomLimit.position.y + Camera.main.orthographicSize;
        float maxY = topLimit.position.y - Camera.main.orthographicSize;
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);

        transform.position = smoothedPosition;
    }
}