using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct CheckPoint
{
    public int nr;
    public bool activated;
    public float time;
};

public class LapTimeController : MonoBehaviour {

    [SerializeField]
    Text LapTimeTextField;

    [SerializeField]
    int NrOfCheckpoints;

    static int nrcheckpoints;

    static float startTime;

    static bool firstlap = false;

    static float currentTime;

    static int nextCheckpoint;

    static Dictionary<int, CheckPoint> checkpoints = new Dictionary<int, CheckPoint>();

    private void Awake()
    {
        for (int i = 0; i < NrOfCheckpoints; i++)
        {
            CheckPoint newPoint = new CheckPoint();
            newPoint.nr = i;
            newPoint.activated = false;
            newPoint.time = 0.0f;
            checkpoints[i] = newPoint;
        }

        nrcheckpoints = NrOfCheckpoints;
    }

    // Use this for initialization
    void Start () {
        startTime = Time.time;
        LapTimeTextField.text = parseTimeString(0);
    }
	
	// Update is called once per frame
	void Update () {
        currentTime = Time.time - startTime;
        if (firstlap)
            LapTimeTextField.text = parseTimeString(currentTime);
        else
            LapTimeTextField.text = "0: 00.000";

    }

    public static void ResetLapTime()
    {
        if (!firstlap)
        {
            WheelDrive.clearGhostHistory();
            firstlap = true;
            return;
        }

        if(nextCheckpoint != nrcheckpoints)
            return;

        startTime = Time.time;

        nextCheckpoint = 0;
        for (int i = 0; i < nrcheckpoints; i++)
        {
            CheckPoint point = checkpoints[i];
            point.activated = false;
            point.time = 0.0f;
            checkpoints[i] = point;
        }

        WheelDrive.startNewGhost();
    }

    private string parseTimeString(float time)
    {
        string minutes = ((int)(time/60)).ToString();

        string seconds = "";
        if (time % 60 >= 10)
            seconds += ((int)time % 60).ToString();
        else
            seconds += "0" + ((int)time % 60).ToString();

        string milliseconds = ((int)((time % 1)*1000)).ToString();


        return minutes + ": " + seconds + "." + milliseconds; 
    }

    static public float getCurrentTime()
    {
        return currentTime;
    }

    static public void activateCheckPoint(int nr)
    {
        if(nextCheckpoint == nr)
        {
            CheckPoint point = checkpoints[nr];
            point.activated = true;
            point.time = currentTime;
            checkpoints[nr] = point;

            Debug.Log(currentTime);
            nextCheckpoint++;
        }
    }
}
