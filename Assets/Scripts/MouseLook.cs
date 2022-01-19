using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class MouseLook : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity;
    private PlayerInput _playerInput;
    
    private float _rotationY = 0f;
 
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        
        Cursor.lockState = CursorLockMode.Locked;

        _rotationY = transform.localRotation.eulerAngles.x;
    }
 
    private void Update()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        _rotationY += _playerInput.MouseX * _mouseSensitivity * Time.deltaTime;
        
        transform.rotation = Quaternion.Euler(0f, _rotationY, 0f);
    }
}
