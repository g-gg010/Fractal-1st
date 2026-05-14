using UnityEngine;
using UnityEngine.InputSystem;

public class FreeCamera : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float shiftMultiplier = 2.5f;
    public float lookSensitivity = 2f;
    private float rotationX = 0f;
    private float rotationY = 0f;
    public Vector3 initialpos;
    public Quaternion initialrot; 

    void Start()
    {
        initialpos = this.transform.position;
        initialrot = this.transform.rotation;
        Vector3 rot = transform.localRotation.eulerAngles;
        rotationX = rot.y;
        rotationY = -rot.x;
    }

    void Update()
    {
        var mouse = Mouse.current;
        var keyboard = Keyboard.current;

        //右クリックで視点回転
        if (mouse.rightButton.isPressed)
        {
            Cursor.lockState = CursorLockMode.Locked;

            rotationX += mouse.delta.x.ReadValue() * lookSensitivity * 0.1f;
            rotationY += mouse.delta.y.ReadValue() * lookSensitivity * 0.1f;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f);

            transform.localRotation = Quaternion.Euler(-rotationY, rotationX, 0f);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        //移動
        float currentSpeed = moveSpeed;
        if (keyboard.leftShiftKey.isPressed)
            currentSpeed *= shiftMultiplier;

        float h = 0f;
        if (keyboard.aKey.isPressed) h -= 1f;
        if (keyboard.dKey.isPressed) h += 1f;

        float v = 0f;
        if (keyboard.wKey.isPressed) v += 1f;
        if (keyboard.sKey.isPressed) v -= 1f;

        float upDown = 0f;
        if (keyboard.eKey.isPressed) upDown += 1f;
        if (keyboard.qKey.isPressed) upDown -= 1f;

        Vector3 moveDir = transform.forward * v + transform.right * h + Vector3.up * upDown;

        transform.position += moveDir * currentSpeed * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        transform.position = initialpos;
        transform.rotation = initialrot;
        
        // もしリジッドボディの速度が残っていたらリセット（Kinematicなら不要ですが念のため）
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
