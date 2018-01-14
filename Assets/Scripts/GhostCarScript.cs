using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCarScript : MonoBehaviour {

    private static int ghostStreamIndex = 0;
    private static int ghostRound = 0;

	// Use this for initialization
	void Start () {
		
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
