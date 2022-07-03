using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager cameraManager = null;

    public GameObject target;

    public float dis = 6f;    
    public float xSpeed = 220f;
    public float ySpeed = 100f;    
    public float sensitivity = 0.015f;
    public float Wsensitivity = 10f;

    
    
    
    float eulerAngleX = 0f;
    float eulerAngleY = 0f;
    
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    [SerializeField]
    Camera main;
    private void Awake()
    {
        if (cameraManager == null)
        {
            cameraManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        main = Camera.main;
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
        eulerAngleX = angles.y;
        eulerAngleY = angles.x;
    }
    public void CameraTargetOnCharacter()
    {
        target = GameManager.gameManager.character.gameObject;
        isTargetCharacter = true;       
    }
    public void CamearaTargetOff()
    {
        isTargetCharacter = false;
    }
   
    bool isTargetCharacter = false;

    private void Update()
    {
        if (isTargetCharacter)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                eulerAngleX += Input.GetAxis("Mouse X") * xSpeed * sensitivity;
                eulerAngleY -= Input.GetAxis("Mouse Y") * ySpeed * sensitivity;

            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (dis <= 0)
                {
                    return;
                }


                dis -= Time.deltaTime * Wsensitivity;

            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (dis >= 15f)
                {
                    return;
                }

                dis += Time.deltaTime * Wsensitivity;
            }

            eulerAngleY = ClampAngle(eulerAngleY, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(eulerAngleY, eulerAngleX, 0);
            Vector3 position = rotation * new Vector3(0, 1.5f, -dis) + target.transform.position;
            transform.rotation = rotation;
            transform.position = position;


            return;
        }

    }
   
}









