using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    float rotateX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        if (invertY)
            rotateX += mouseY;
        else
            rotateX -= mouseY;

        rotateX = Mathf.Clamp(rotateX, lockVertMin, lockVertMax);

        transform.localRotation = Quaternion.Euler(rotateX, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
