using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float MouseX { get; private set; }


    private void Update()
    {
        RefreshInput();
    }

    private void RefreshInput()
    {
        MouseX = Input.GetAxis("Mouse X");
    }
}
