using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private const float MAX_X = 90f;
    private const float MIN_X = -90f;

    private const string MOUSEX_AXIS = "Mouse X";
    private const string MOUSEY_AXIS = "Mouse Y";

    private float _xRotation;

    [Header("Mouse Sensivity")]
    [SerializeField] private float _mouseSensivity;

    [Header("Player")]
    [SerializeField] private Transform _character;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CameraMove();
    }

    private void CameraMove()
    {
        float mouseX = Input.GetAxis(MOUSEX_AXIS) * _mouseSensivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis(MOUSEY_AXIS) * _mouseSensivity * Time.fixedDeltaTime;

        if (mouseX != 0 || mouseY != 0)
        {
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, MIN_X, MAX_X);

            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            _character.Rotate(Vector3.up * mouseX);
        }
    }
}
