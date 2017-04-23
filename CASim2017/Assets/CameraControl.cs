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

    void Update () {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        if (Input.GetKey(KeyCode.A))
        {
            posX -= (float)Math.Cos((yaw * Math.PI) / 180.0f) * speedMov;
            posZ -= (float)Math.Sin((yaw * Math.PI) / 180.0f) * speedMov;
        }

         
        if (Input.GetKey(KeyCode.W))
        {
           posX += (float)Math.Sin((yaw * Math.PI) / 180.0f) * speedMov;
           posZ += (float)Math.Cos((yaw * Math.PI) / 180.0f) * speedMov;
        }
 
        if (Input.GetKey(KeyCode.S))
        {
           posX -= (float)Math.Sin((yaw * Math.PI) / 180.0f) * speedMov;
           posZ -= (float)Math.Cos((yaw * Math.PI) / 180.0f) * speedMov;
        }
 
        if (Input.GetKey(KeyCode.D))
        {
            posX += (float)Math.Cos((yaw * Math.PI) / 180.0f) * speedMov;
            posZ += (float)Math.Sin((yaw * Math.PI) / 180.0f) * speedMov;
        }

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        transform.position = new Vector3(posX, posY, posZ);
    }
}