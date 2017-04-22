using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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
    public float DEFAULT_METER_SCALE = 0.2f;
    // Use this for initialization
    void Start () {
	    target = GameObject.FindGameObjectsWithTag("Pedestal")[0].transform;
        anchorVector = new Vector3(0, 0, -1);
    }
	
    void moveCam()
    {
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

        if(props.Count != 0)
            moveObj();
    }


    public void addGameObject()
    {

        List<string> models = new List<string>();
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Resources/models/");
        foreach(var directory in dir.GetDirectories())
        {
            foreach (var file in directory.GetFiles("*.3DS"))
            {
                models.Add(directory.Name + "/" + file.Name);
            }
        }
        string filesel = models[Random.Range(0, models.Count)];

        string fpath = "models/" + filesel.Split('.')[0];
        Debug.Log(fpath);

        GameObject go = Instantiate(Resources.Load(fpath, typeof(GameObject))) as GameObject;

        go.transform.position = new Vector3(0f, 0f, 0f);
        props.Add(go);
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        float minX = float.MaxValue;
        float maxX = float.MinValue;

        float minZ = float.MaxValue;
        float maxZ = float.MinValue;

        foreach (Transform child in go.transform)
        {
            if(child.gameObject.GetComponent<MeshFilter>() == null)
            {
                continue;
            }
            Vector3[] vertices = child.gameObject.GetComponent<MeshFilter>().mesh.vertices;
            foreach(var vertex in vertices)
            {
                if (vertex.y < minY)
                    minY = vertex.y;
                if (vertex.y > maxY)
                    maxY = vertex.y;

                if (vertex.x < minX)
                    minX = vertex.x;
                if (vertex.x > maxX)
                    maxX = vertex.x;

                if (vertex.z < minZ)
                    minZ = vertex.z;
                if (vertex.z > maxZ)
                    maxZ = vertex.z;
            }
        }

        float maxDim = Mathf.Max(Mathf.Max(maxZ - minZ, maxY - minY), maxX - minX);
        float scale = DEFAULT_METER_SCALE / maxDim;
        go.transform.localScale = new Vector3(scale, scale, scale);

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
