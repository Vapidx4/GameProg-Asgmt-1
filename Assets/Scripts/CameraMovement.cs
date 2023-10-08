using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 5.0f;
    [SerializeField] private Transform target;
    [SerializeField] private Transform head;
    [SerializeField] private float distanceFromTarget = 3.0f;
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private Vector2 rotationXMinMax = new Vector2(-40, 40);

    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        _currentRotation.x += mouseX;
        _currentRotation.y += mouseY;

        _currentRotation.x = Mathf.Clamp(_currentRotation.x, rotationXMinMax.x, rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(_currentRotation.x, _currentRotation.y);

        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, smoothTime);
        transform.localEulerAngles = _currentRotation;

        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}