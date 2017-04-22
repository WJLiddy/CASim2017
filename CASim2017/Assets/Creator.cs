using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Creator : MonoBehaviour {
    
    public Transform target;
    public int selectedObjectIndex = 0;
    public List<GameObject> props = new List<GameObject>();
    public float angle = 0.0f;
    public static float radius = 0.9f;
    public Vector3 anchorVector = new Vector3(0f, 0f, 0f);
    public float camSpeed = 0.03f;
    public float objMoveSpeed = 0.01f;
    public enum FACING {N,W,S,E}
    public FACING facing;
    // Use this for initialization
    void Start () {
	    target = GameObject.FindGameObjectsWithTag("Pedestal")[0].transform;
        Debug.Log("TARGET IS" + target);
        anchorVector = new Vector3(0, 0, -1);
    }
	
    void moveCam()
    {
        Debug.Log(angle);
        float newY = anchorVector.y;

        if (Input.GetKey(KeyCode.W))
        {
            newY += camSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            newY -= camSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            angle += camSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            angle -= camSpeed;
        }

        float newX = radius * Mathf.Cos(angle);
        float newZ = radius * Mathf.Sin(angle);

        Vector3 newPos = new Vector3(newX, newY, newZ);
        anchorVector = newPos;
        //boost y by 0.5
        newPos = new Vector3(anchorVector.x, anchorVector.y, anchorVector.z);
        transform.position = newPos;
        transform.LookAt(target);
        newPos = new Vector3(anchorVector.x, anchorVector.y - 0.1f, anchorVector.z);
        transform.position = newPos;
        
        if(angle < 0)
        {
            angle += (2f * Mathf.PI);
        }

        float locAngle = (angle + (.25f * Mathf.PI)) % (2f * Mathf.PI);
        if (locAngle < Mathf.PI/2f)
        {
            facing = FACING.N;
        }
        else if (locAngle < Mathf.PI)
        {
            facing = FACING.W;
        }
        else if (locAngle < Mathf.PI * 1.5f)
        {
            facing = FACING.S;
        }
        else 
        {
            facing = FACING.E;
        }

        Debug.Log(facing);
    }


    void moveObj()
    {
        float newX = selProp().transform.position.x;
        float newY = selProp().transform.position.y;
        float newZ = selProp().transform.position.z;

        switch(facing)
        {
            case FACING.N:
                if (Input.GetKey(KeyCode.DownArrow))
                    newX += objMoveSpeed;
                if (Input.GetKey(KeyCode.UpArrow))
                    newX -= objMoveSpeed;
                if (Input.GetKey(KeyCode.LeftArrow))
                    newZ -= objMoveSpeed;
                if (Input.GetKey(KeyCode.RightArrow))
                    newZ += objMoveSpeed;
                break;
            case FACING.S:
                if (Input.GetKey(KeyCode.DownArrow))
                    newX -= objMoveSpeed;
                if (Input.GetKey(KeyCode.UpArrow))
                    newX += objMoveSpeed;
                if (Input.GetKey(KeyCode.LeftArrow))
                    newZ += objMoveSpeed;
                if (Input.GetKey(KeyCode.RightArrow))
                    newZ -= objMoveSpeed;
                break;

            case FACING.W:
                if (Input.GetKey(KeyCode.DownArrow))
                    newZ += objMoveSpeed;
                if (Input.GetKey(KeyCode.UpArrow))
                    newZ -= objMoveSpeed;
                if (Input.GetKey(KeyCode.LeftArrow))
                    newX += objMoveSpeed;
                if (Input.GetKey(KeyCode.RightArrow))
                    newX -= objMoveSpeed;
                break;
            case FACING.E:
                if (Input.GetKey(KeyCode.DownArrow))
                    newZ -= objMoveSpeed;
                if (Input.GetKey(KeyCode.UpArrow))
                    newZ += objMoveSpeed;
                if (Input.GetKey(KeyCode.LeftArrow))
                    newX -= objMoveSpeed;
                if (Input.GetKey(KeyCode.RightArrow))
                    newX += objMoveSpeed;
                break;

        }
      
        selProp().transform.position = new Vector3(newX, newY, newZ);
    }
	// Update is called once per frame
	void Update () {
        moveCam();
        moveObj();
    }


    public void addGameObject()
    {

        float scale = 0.0001f;
        GameObject go = Instantiate(Resources.Load("soft", typeof(GameObject))) as GameObject;
        go.transform.localScale = new Vector3(scale, scale, scale);
        go.transform.position = new Vector3(0f, 0f, 0f);
        props.Add(go);
        float minY = float.MaxValue;
        foreach (Transform child in go.transform)
        {
            Vector3[] vertices = child.gameObject.GetComponent<MeshFilter>().mesh.vertices;
            foreach(var vertex in vertices)
            {
                if (vertex.y < minY)
                    minY = vertex.y;
                //Debug.Log(vertex);
            }
        }
        Debug.Log(minY);
        go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y - (scale * minY), go.transform.position.z);
        //GameObject.FindGameObjectsWithTag("Pedestal")[0];
        props.Add(go);
        selectedObjectIndex = props.Count - 1;
    }

    GameObject selProp()
    {
        return props[selectedObjectIndex];
    }


}
