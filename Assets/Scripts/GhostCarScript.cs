using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostCarScript : MonoBehaviour {

    private static int ghostStreamIndex = 0;
    private static int ghostRound = 0;

    [SerializeField]
    Text path;

    private string result = "";

    IEnumerator Example()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "MyFile.txt");

        path.text = "Out1: " + filePath + "\n" + result;

        if (filePath.Contains("://"))
        {
            WWW www = new WWW(filePath);
            yield return www;
            result = www.text;
        }
        else
            result = System.IO.File.ReadAllText(filePath);

        path.text = "Out2: "+ filePath + "\n"+ result;
    }

    // Use this for initialization
    void Start () {

        StartCoroutine(Example());
        
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (WheelDrive.replayGhostEventStream.Count <= ghostStreamIndex)
            return;

        if (ghostRound > 0)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
            return;
        }
            

        GhostEvent current = WheelDrive.replayGhostEventStream[ghostStreamIndex];

        transform.position = current.position;
        transform.rotation = current.rotation;

        ghostStreamIndex++;

    }

    public static void startGhost()
    {
        ghostRound++;
        ghostStreamIndex = 0;
    }
}
