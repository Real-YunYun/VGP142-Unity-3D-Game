using System.Collections; using System.Collections.Generic; using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float mouseSensitivity = 1f;
    public GameObject PlayerBody;
    public Transform PlayerHead;

    [HideInInspector] public float mouseX;
    [HideInInspector] public float mouseY;

    float xRotation = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && !PlayerBody.GetComponent<PlayerController>().IsDying)
        {
            Cursor.lockState = CursorLockMode.None;
            mouseX = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            mouseX = Input.GetAxis("Mouse X") * (mouseSensitivity * 800) * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * (mouseSensitivity * 800) * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -45, 60);

            this.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            this.transform.position = PlayerHead.transform.position;
            PlayerBody.transform.Rotate(Vector3.up * mouseX);
        }
    }

}
