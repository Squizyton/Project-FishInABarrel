using Input;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    //private PlayerControls _controls;
 
     [SerializeField] private Transform playerHead;
    
     [SerializeField] private Rigidbody playerBody;
     
     [SerializeField] private CinemachineCamera cam;
     [SerializeField] private float rotationSpeed = 1f;
     [SerializeField] private UnityEngine.Camera mainCamera;
 
     
     
     private Vector2 mousePos;
     private float xRotation = 0f;
 
 
     public void Start()
     {
         if (cam == null)
         {
             if (GameObject.FindGameObjectWithTag("MainCamera").TryGetComponent(out Camera newcam))
                 mainCamera = newcam;
         }
         
         Cursor.lockState = CursorLockMode.Locked;
     }
 
     private void GetMousePos()
     {
         mousePos = InputWrapper.Instance.performMouseMovement.ReadValue<Vector2>() * (rotationSpeed * Time.deltaTime);
        // Debug.Log(InputWrapper.Instance.performMouseMovement.ReadValue<Vector2>());
     }
     
 
     private void Update()
     {
         GetMousePos();
         RotateCamera();
        
         
         transform.position = playerHead.transform.position;
     }
     private void RotateCamera()
     {
         
         //Find current look rotation
         Vector3 rot = transform.localRotation.eulerAngles;
         var desiredX = rot.y + mousePos.x;
         
         //Rotate, and also make sure we don't over or under rotate
         xRotation -= mousePos.y;
         xRotation = Mathf.Clamp(xRotation, -90f, 90);
         
         //Perform the rotation of camera
         transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
         playerBody.MoveRotation(Quaternion.Euler(0,desiredX,0));
     }
 
     public void FixedUpdate()
     {
         playerBody.MoveRotation(playerBody.rotation * Quaternion.AngleAxis(mousePos.x,Vector3.up));
       
     }
 
     public CinemachineCamera GetCamera()
     {
         return cam;
     }
     
     public UnityEngine.Camera GetMainCamera()
     {
         return mainCamera;
     }
     
}
