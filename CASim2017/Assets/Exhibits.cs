using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Exhibits : MonoBehaviour {

    List<string> exhibitNames = new List<string>();
    List<Vector3> exhibitOffsets = new List<Vector3> { new Vector3(-0.3f, 0.2f, 4f), new Vector3(6f, 0.2f, 0.5f) };
	// Use this for initialization
	void Start () {
        Sock s = new Sock();
        var k = s.Recv();
        s.Close();
        setupExhibits(k);
        Debug.Log("done?");
	}

    public Dictionary<string,string> convertNameToPath()
    {
        Dictionary<string,string> models = new Dictionary<string, string>();
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
	void Update () {
		
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
