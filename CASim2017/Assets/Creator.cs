using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using SimpleJSON;

public class Creator : MonoBehaviour {
    
    public Transform target;
    public int propItr = 0;
    public List<GameObject> props = new List<GameObject>();
    public List<string> propnames = new List<string>();
    public List<float> scales = new List<float>();
    public List<float> floor = new List<float>();
    public List<float[]> cachedBox = new List<float[]>();
    public float angle = 0.0f;
    public static float radius = 2.9f;
    public Vector3 anchorVector = new Vector3(0f, 0f, 0f);
    public float camSpeed = 0.03f;
    public float objMoveSpeed = 0.04f;
    public enum FACING {N,W,S,E}
    public FACING facing;
    public float DEFAULT_METER_SCALE = 0.7f;
    public List<string> models;
    // Use this for initialization
    void Start () {
	    target = GameObject.FindGameObjectsWithTag("Pedestal")[0].transform;
        anchorVector = new Vector3(0, 0, -1);
        models = generateGameObjects();
    }
	
    void moveCam()
    {
        if (focus)
            return;

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
        if (focus)
            return;
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
    bool focus = false;
	// Update is called once per frame
	void Update () {


        focus = GameObject.FindGameObjectsWithTag("focus")[0].GetComponent<InputField>().isFocused;
        moveCam();

        if (props.Count != 0)
        {
            float y = heightAt();
            //Debug.Log(y);
            selProp().transform.position = new Vector3(selProp().transform.position.x, floor[floor.Count - 1] + y, selProp().transform.position.z);
            moveObj();
            cachedBox[cachedBox.Count - 1] = getBoxNoCache(props[props.Count - 1]);
        }
    }

    public bool collide(int a, int b)
    {

        var ab = getBox(a);
        var bb = getBox(b);
        // { minX, maxX, minY, maxY, minZ, maxZ };
        if (ab[0] < bb[1] &&
           ab[1] > bb[0] &&
           ab[4] < bb[5] &&
           ab[5] > bb[4])
        {
            // collision detected!
            return true;
        }

        return false;
    }

    float heightAt()
    {
        float y = 0;
        int i = 0;
        foreach(var obj in props)
        {
            if (obj == selProp())
                break;
            // { minX, maxX, minY, maxY, minZ, maxZ };
            var e = getBox(i);
            if(collide(i,props.Count - 1))
            {
                //Debug.Log("Collide!");
                y = Mathf.Max(y,e[3]);
            }
            i++;
        }
        return y;
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
                //Debug.Log(fpath);
            }
        }
        return models;
    }

    public float[] getBox(int i)
    {
        return cachedBox[i];
    }
    //minX, maxX, minY, maxY, minZ,maxZ
    public float[] getBoxNoCache(GameObject go)
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
            foreach (var r_vertex in vertices)
            {
                var vertex = go.transform.TransformPoint(r_vertex);
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
        return new float[] { minX, maxX, minY, maxY, minZ, maxZ };
    }


    public float[] calculateScaleAndMinY(float[] vals)
    {
        float maxDim = Mathf.Max(Mathf.Max(vals[1] - vals[0], vals[3]- vals[2]), vals[5] - vals[4]);
        return new float[2] { maxDim, vals[2] };
    }

    /**
     *         var fileName = "failed_to_load.txt";
        var sr = File.CreateText(fileName);
        
        sr.Close();
    */

    public void loadRandomProp()
    {
        loadProp(propItr);
    }

    public void loadProp(int pid)
    {
        var modelname = models[pid];
        GameObject go = Instantiate(Resources.Load(modelname, typeof(GameObject))) as GameObject;
        go.transform.position = new Vector3(0f, 0f, 0f);
        var scaleInf = calculateScaleAndMinY(getBoxNoCache(go));
        var scale = DEFAULT_METER_SCALE / scaleInf[0];
        var minY = scaleInf[1];
        go.transform.localScale = new Vector3(scale, scale, scale);
        go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y - (scale * minY), go.transform.position.z);
        props.Add(go);
        var selectedObjectIndex = props.Count - 1;
        var plist = GameObject.FindGameObjectsWithTag("PropList")[0];
        var child = new GameObject();
        var childTextBox = child.AddComponent(typeof(Text)) as Text;
        childTextBox.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        var split = modelname.Split('/');
        var name = split[split.Length - 1];
        childTextBox.text = name;
        propnames.Add(name);
        scales.Add(scale);
        cachedBox.Add(getBoxNoCache(go));
        child.transform.SetParent(plist.transform);
        floor.Add(go.transform.position.y);
    }

    GameObject selProp()
    {
        return props[props.Count - 1];
    }

    public void publish()
    {
        Debug.Log("joining server...\n");
        Sock s = new Sock();
        Debug.Log("trying submitting...\n");
        s.Submit(toJSON());
        Debug.Log("closing...\n");
        s.Close();
    }

    public void remove()
    {
        props[props.Count - 1].SetActive(false);
        props.RemoveAt(props.Count - 1);
        propnames.RemoveAt(propnames.Count - 1);
        scales.RemoveAt(scales.Count - 1) ;
        cachedBox.RemoveAt(cachedBox.Count - 1);
        floor.RemoveAt(floor.Count - 1);
    }

    public void next()
    {
        remove();
        propItr = (propItr + 1) % models.Count;
        loadProp(propItr);
    }

    public void prev()
    {
        remove();
        propItr = (propItr - 1) % models.Count;
        if (propItr < 0)
            propItr = 0;
        loadProp(propItr);
    }

    public void rotate()
    {
        selProp().transform.Rotate(0f, 90f, 0f);
    }


    public void sizeUP()
    {
        scales[props.Count - 1] = scales[props.Count - 1] * 1.5f;
        var scale = scales[props.Count - 1]; 
        props[props.Count - 1].transform.localScale = new Vector3(scale, scale, scale);
    }

    public void sizeDOWN()
    {
        scales[props.Count - 1] = scales[props.Count - 1] * 0.7f;
        var scale = scales[props.Count - 1];
        props[props.Count - 1].transform.localScale = new Vector3(scale, scale, scale);
    }


    public string toJSON()
    {
        JSONObject json = new JSONObject();
        json["title"] = "Untitled Work";
        
        for(int i = 0; i != props.Count; i++)
        {
            JSONObject prop = new JSONObject();
            prop["name"] = propnames[i];
            prop["x"] = props[i].transform.position.x;
            prop["y"] = props[i].transform.position.y;
            prop["z"] = props[i].transform.position.z;
            prop["rotx"] = props[i].transform.eulerAngles.x;
            prop["roty"] = props[i].transform.eulerAngles.y;
            prop["rotz"] = props[i].transform.eulerAngles.z;
            prop["scale"] = scales[i];
            json["prop"][-1] = prop;
        }
        return json.ToString();
    }

}
