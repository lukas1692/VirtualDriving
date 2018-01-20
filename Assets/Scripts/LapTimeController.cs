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

public class LapTimeController : MonoBehaviour
{

    [SerializeField]
    Text LapTimeTextField;

    [SerializeField]
    Text LapTimeDifferenceTextField;

    [SerializeField]
    int NrOfCheckpoints;

    static float checkpoint_time_difference;

    static bool enabled_checkpoint;

    static int nrcheckpoints;

    static float startTime;

    static bool firstlap = false;

    static float currentTime;

    static int nextCheckpoint;


    static Dictionary<int, CheckPoint> current_checkpoints = new Dictionary<int, CheckPoint>();
    static Dictionary<int, CheckPoint> ghost_checkpoints = new Dictionary<int, CheckPoint>();

    private void Awake()
    {
        
        LapTimeDifferenceTextField.GetComponentInParent<Transform>().parent.gameObject.SetActive(false);

        enabled_checkpoint = false;

        nrcheckpoints = NrOfCheckpoints;

        resetCurrentCheckpoints();

        // reset ghost checkpoints
        for (int i = 0; i < nrcheckpoints; i++)
        {
            CheckPoint newPoint = new CheckPoint();
            newPoint.nr = i;
            newPoint.activated = false;
            newPoint.time = 0.0f;
            ghost_checkpoints[i] = newPoint;
        }

    }

    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
        LapTimeTextField.text = parseTimeString(0);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time - startTime;
        if (firstlap)
            LapTimeTextField.text = parseTimeString(currentTime);
        else
            LapTimeTextField.text = "0: 00.000";

        if (enabled_checkpoint)
        {
            enabled_checkpoint = false;

            StartCoroutine(Fade());
        }

    }

    public static void ResetLapTime()
    {
        if (!firstlap)
        {
            WheelDrive.clearGhostHistory();
            firstlap = true;
            return;
        }

        if (nextCheckpoint != nrcheckpoints)
            return;

        startTime = Time.time;

        nextCheckpoint = 0;

        ghost_checkpoints = new Dictionary<int, CheckPoint>(current_checkpoints);

        resetCurrentCheckpoints();
        //for (int i = 0; i < nrcheckpoints; i++)
        //{
        //    CheckPoint point = current_checkpoints[i];
        //    point.activated = false;
        //    point.time = 0.0f;
        //    current_checkpoints[i] = point;
        //}

        WheelDrive.startNewGhost();
    }

    private string parseTimeString(float time)
    {
        string minutes = ((int)(time / 60)).ToString();

        string seconds = "";
        if (time % 60 >= 10)
            seconds += ((int)time % 60).ToString();
        else
            seconds += "0" + ((int)time % 60).ToString();

        string milliseconds = ((int)((time % 1) * 1000)).ToString();


        return minutes + ": " + seconds + "." + milliseconds;
    }

    static public float getCurrentTime()
    {
        return currentTime;
    }

    static public void activateCheckPoint(int nr)
    {
        if (nextCheckpoint == nr)
        {
            CheckPoint point = current_checkpoints[nr];
            point.activated = true;
            point.time = currentTime;
            current_checkpoints[nr] = point;

            CheckPoint ghost_point = ghost_checkpoints[nr];
            if (ghost_point.activated)
            {
                float ghost_point_time = ghost_point.time;

                checkpoint_time_difference = ghost_point_time - point.time;

                Debug.Log("Ghost Time " + ghost_point_time);
                Debug.Log("Car Time " + point.time);
                Debug.Log("Difference: " + checkpoint_time_difference);

                enabled_checkpoint = true;

                
            }

            nextCheckpoint++;
        }
    }

    static private void resetCurrentCheckpoints()
    {
        for (int i = 0; i < nrcheckpoints; i++)
        {
            CheckPoint newPoint = new CheckPoint();
            newPoint.nr = i;
            newPoint.activated = false;
            newPoint.time = 0.0f;
            current_checkpoints[i] = newPoint;
        }
    }

    IEnumerator Fade()
    {
        LapTimeDifferenceTextField.GetComponent<Transform>().parent.transform.gameObject.SetActive(true);

        LapTimeDifferenceTextField.text = parseTimeString(Mathf.Abs(checkpoint_time_difference));

        Debug.Log("Ghost: " + checkpoint_time_difference);
        if (checkpoint_time_difference < 0)
            LapTimeDifferenceTextField.GetComponent<Transform>().parent.transform.gameObject.GetComponent<Image>().color = new Color(1, 0, 0);  //red
        else
            LapTimeDifferenceTextField.GetComponent<Transform>().parent.transform.gameObject.GetComponent<Image>().color = new Color(12.0f / 255f, 109.0f / 255, 10.0f / 255f); // green

        yield return new WaitForSeconds(5f);

        LapTimeDifferenceTextField.GetComponent<Transform>().parent.transform.gameObject.SetActive(false);

        LapTimeDifferenceTextField.text = "";
    }
}