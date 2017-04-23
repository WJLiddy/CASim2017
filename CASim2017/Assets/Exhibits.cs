using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Exhibits : MonoBehaviour
{

    List<string> exhibitNames = new List<string>();
    //kill me
    List<Vector3> exhibitOffsets = new List<Vector3>
	{ new Vector3(-0.3f, 0.1f, 4f), new Vector3(8.5f, 0f, -4.75f), new Vector3(8.5f, 0f, 4f), new Vector3(8.5f, 0f, 12.75f), new Vector3(-0.3f, 0f, 12.75f), 
		new Vector3(-8.85f, 0f, 12.75f), new Vector3(-8.85f, 0f, 4f), new Vector3(-8.85f, 0f, -4.75f), new Vector3(-19.15f, 0f, 12.75f), new Vector3(-19.15f, 0f, 4f), 
		new Vector3(-19.15f, 0f, -4.75f), new Vector3(-27.85f, 0f, 12.75f), new Vector3(-27.85f, 0f, 4f), new Vector3(-27.85f, 0f, -4.75f), new Vector3(-36.75f, 0f, 12.75f), 
		new Vector3(-36.75f, 0f, 4f), new Vector3(-36.75f, 0f, -4.75f), new Vector3(18.85f, 0f, 12.75f), new Vector3(18.85f, 0f, 4f), new Vector3(18.85f, 0f, -4.75f), 
		new Vector3(27.6f, 0f, 12.75f), new Vector3(27.6f, 0f, 4f), new Vector3(27.6f, 0f, -4.75f), new Vector3(36.25f, 0f, 12.75f), new Vector3(36.25f, 0f, 4f), 
		new Vector3(36.25f, 0f, -4.75f), new Vector3(8.5f, 0f, 23.15f), new Vector3(8.5f, 0f, 31.85f), new Vector3(8.5f, 0f, 40.65f), new Vector3(-0.3f, 0f, 23.15f), 
		new Vector3(-0.3f, 0f, 31.85f), new Vector3(-0.3f, 0f, 40.65f), new Vector3(-8.85f, 0f, 23.15f), new Vector3(-8.85f, 0f, 31.85f), new Vector3(-8.85f, 0f, 40.65f)

    };
    // Use this for initialization
    void Start()
    {
        Sock s = new Sock();
        var k = s.Recv();
        s.Close();
        setupExhibits(k);
        setupWojaks();
        Debug.Log("done?");
    }

    public void setupWojaks()
    {
        foreach (var ex in exhibitOffsets)
        {
            GameObject go = Instantiate(Resources.Load("kiosk", typeof(GameObject))) as GameObject;
            go.transform.localPosition = new Vector3(ex.x, ex.y - 0.2f, ex.z - 2);
            go.transform.eulerAngles = new Vector3(0, 180, 0);
            go.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);

            for (int i = 0; i != 4; i++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.name = "WOJAK";
                cube.transform.localScale = new Vector3(3*0.07F, 3*0.07F, 0.001F);
                cube.transform.eulerAngles = new Vector3(0, 180, 0);
                cube.transform.localPosition = new Vector3(ex.x + 0.12f + (i * 0.22f), ex.y + 0.9f, ex.z - 2.09f);
                cube.GetComponent<Renderer>().material.mainTexture = Resources.Load("feels/" + (i + 2), typeof(Texture)) as Texture;
            }



        }
    }
    public Dictionary<string, string> convertNameToPath()
    {
        Dictionary<string, string> models = new Dictionary<string, string>();
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Resources/models/");
        foreach (var directory in dir.GetDirectories())
        {
            foreach (var file in directory.GetFiles("*.3DS"))
            {
                string filesel = directory.Name + "/" + file.Name;
                string modelname = "models/" + filesel.Split('.')[0];
                var split = modelname.Split('/');
                var name = split[split.Length - 1];
                models[name] = modelname;
            }
        }
        return models;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void setupExhibits(List<SimpleJSON.JSONNode> ex)
    {
        var models = convertNameToPath();
        for (int i = 0; i != 2; i++)
        {
            var json = ex[i];
            Debug.Log(json);

            exhibitNames.Add(json["name"]);
            foreach (SimpleJSON.JSONNode part in json["prop"].AsArray)
            {

                Debug.Log("PROP IS" + part["name"]);
                GameObject go = Instantiate(Resources.Load(models[part["name"]], typeof(GameObject))) as GameObject;
                go.transform.position = exhibitOffsets[i] + new Vector3(part["x"], part["y"], part["z"]);
                go.transform.eulerAngles = new Vector3(part["rotx"], part["roty"], part["rotz"]);
                go.transform.localScale = new Vector3(part["scale"], part["scale"], part["scale"]);
            }
        }
    }
}
