using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager cameraManager;
    public GameObject target;
    
    public float dis = 6f;
    
    public float xSpeed = 220f;
    public float ySpeed = 100f;
    
    public float sensitivity = 0.015f;
    public float Wsensitivity = 10f;

    public bool IsCharacter = false;
    
    
    float x = 0f;
    float y = 0f;
    
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    private void Awake()
    {
        if (cameraManager == null)
        {
            cameraManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
     void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; // 커서 화면안으로 고정
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

    }
    private void Update()
    {

        if (IsCharacter == true)
        {
            if (target == null)
                target = Character.Player.gameObject;
            

            if (Input.GetKey(KeyCode.Mouse1))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * sensitivity;
                y -= Input.GetAxis("Mouse Y") * ySpeed * sensitivity;

            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (dis <= 0)
                    return;

                dis -= Time.deltaTime * Wsensitivity;

            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (dis >= 15f)
                    return;

                dis += Time.deltaTime * Wsensitivity;
            }

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0, 1.5f, -dis) + target.transform.position;
            transform.rotation = rotation;
            transform.position = position;
        }


    }
    
}









