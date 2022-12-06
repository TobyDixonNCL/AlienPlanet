using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    // Start is called before the first frame update

    private float speed;
    private float maxspeed;

    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
 
    private float rotY = 0.0f;
    private float rotX = 0.0f; 

    // Cubemap
    public Cubemap cubemap;

    void Start() {
        speed = 50.0f;
        maxspeed = 100.0f;

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;

        GameObject tempCamera = new GameObject("tempCamera");

        tempCamera.AddComponent<Camera>();
        tempCamera.transform.position = new Vector3(500, 300, 500);
        tempCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
        tempCamera.GetComponent<Camera>().RenderToCubemap(cubemap);

        Destroy(tempCamera);
        Shader.SetGlobalTexture("_RealtimeCubemap", cubemap);
    }

    // Update is called once per frame
    void Update() {


        // Movement
        Vector3 moveVec = Vector3.zero; 

        moveVec += Vector3.forward * Input.GetAxisRaw("Vertical");
        moveVec += Vector3.right * Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift)) moveVec *= maxspeed;
        else moveVec *= speed;

        moveVec *= Time.deltaTime;

        transform.Translate(moveVec);
        // Vector3 mouse_pos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        // // Camera Rotation
        // transform.rotation *= Quaternion.Euler(
        //     (Mathf.Abs(mouse_pos.y - 0.5f) > 0.2 ? -(mouse_pos.y - 0.5f) : 0) * 5.0f,
        //     (Mathf.Abs(mouse_pos.x - 0.5f) > 0.2 ? mouse_pos.x - 0.5f : 0) * 5.0f,
        //     0
        // );

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
 
        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;
 
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
 
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

    }
}
