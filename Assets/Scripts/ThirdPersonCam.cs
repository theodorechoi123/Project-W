using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    public float rotationSpeed;
    public CameraStyles currentStyle;
    public Transform combatLookAt;
    public GameObject basicCam;
    public GameObject combatCam;

    public enum CameraStyles
    {
        Basic,
        Combat,
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // switch styles
        if(Input.GetKeyDown(KeyCode.Alpha1)) SwitchCameraStyles(CameraStyles.Basic);
        if(Input.GetKeyDown(KeyCode.Alpha2)) SwitchCameraStyles(CameraStyles.Combat);

        //rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        if(currentStyle == CameraStyles.Basic)
        {
            //rotate player object
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if(inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else if(currentStyle == CameraStyles.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    private void SwitchCameraStyles(CameraStyles newStyle)
    {
        combatCam.SetActive(false);
        basicCam.SetActive(false);

        if(newStyle == CameraStyles.Basic) basicCam.SetActive(true);
        if(newStyle == CameraStyles.Combat) combatCam.SetActive(true);

        currentStyle = newStyle;
        
    }
}
