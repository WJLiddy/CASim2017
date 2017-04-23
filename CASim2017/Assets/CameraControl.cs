using UnityEngine;
using System;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public float speedMov = 0.001f;
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    public float posX = 0.0f;
    public float posY = 2.0f;
    public float posZ = -18.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    public float speed = 10.0F;
    public float rotationSpeed = 100.0F;
    public float rot = 0.0F;

    void Update () {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        rot += rotation;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);

        if (Input.GetKey(KeyCode.A))
        {
            posX -= (float)Math.Cos(rot) * speedMov;
            posZ += (float)Math.Sin(rot) * speedMov;
        }

         
        if (Input.GetKey(KeyCode.W))
        {
           posX += (float)Math.Sin(rot) * speedMov;
           posZ += (float)Math.Cos(rot) * speedMov;
        }
 
        if (Input.GetKey(KeyCode.S))
        {
           posX -= (float)Math.Sin(rot) * speedMov;
            posZ -= (float)Math.Cos(rot) * speedMov;
        }
 
        if (Input.GetKey(KeyCode.D))
        {
            posX += (float)Math.Cos(rot) * speedMov;
            posZ -= (float)Math.Sin(rot) * speedMov;
        }

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        transform.position = new Vector3(posX, posY, posZ);
    }
}