using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTimeController : MonoBehaviour {

    [SerializeField]
    Text LapTimeTextField;

    static float startTime;

    static bool firstlap = false;

    // Use this for initialization
    void Start () {
        LapTimeTextField.text = parseTimeString(0);
        
    }
	
	// Update is called once per frame
	void Update () {

        if(firstlap)
            LapTimeTextField.text = parseTimeString(Time.time - startTime);
        else
            LapTimeTextField.text = "0: 00.000";

    }

    public static void ResetLapTime()
    {
        firstlap = true;   
        startTime = Time.time;
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
}
