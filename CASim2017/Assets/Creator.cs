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

    

    public List<string> generateGameObjects()
    {

        List<string> models = new List<string>();
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Resources/models/");
        foreach (var directory in dir.GetDirectories())
        {
            foreach (var file in directory.GetFiles("*.3DS"))
            {
                string filesel = directory.Name + "/" + file.Name;
                string fpath = "models/" + filesel.Split('.')[0];
                models.Add(fpath);
                Debug.Log(fpath);
            }
        }
        return models;
    }

    public float[] calculateScaleAndMinY(GameObject go)
    {
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        float minX = float.MaxValue;
        float maxX = float.MinValue;

        float minZ = float.MaxValue;
        float maxZ = float.MinValue;

        foreach (MeshFilter child in go.GetComponentsInChildren<MeshFilter>())
        {

            Vector3[] vertices = child.mesh.vertices;
            foreach (var vertex in vertices)
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
        return new float[2] { maxDim, minY };
    }

    /**
     *         var fileName = "failed_to_load.txt";
        var sr = File.CreateText(fileName);
        
        sr.Close();
    */

    public bool fileIsGood(string fpath)
    {
        GameObject go = Instantiate(Resources.Load(fpath, typeof(GameObject))) as GameObject;

        go.transform.position = new Vector3(0f, 0f, 0f);

        return float.IsNegativeInfinity(calculateScaleAndMinY(go)[0]);
    }

    public void loadRandomProp()
    {
        var models = generateGameObjects();
        GameObject go = Instantiate(Resources.Load(models[Random.Range(0, models.Count)], typeof(GameObject))) as GameObject;
        go.transform.position = new Vector3(0f, 0f, 0f);
        var scaleInf = calculateScaleAndMinY(go);
        var scale = DEFAULT_METER_SCALE / scaleInf[0];
        var minY = scaleInf[1];
        go.transform.localScale = new Vector3(scale, scale, scale);
        go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y - (scale * minY), go.transform.position.z);
        props.Add(go);
        selectedObjectIndex = props.Count - 1;
    }

    GameObject selProp()
    {
        return props[selectedObjectIndex];
    }

}
