using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lap {

    public List<TimeStep> timestep = new List<TimeStep>();
    public List<CheckPoint> checkpoint = new List<CheckPoint>();
    public float laptime;
    public string scenarioname;
    public int mmr;

    public void addCheckpoints(Dictionary<int, CheckPointActivation> checkpoints)
    {
        Debug.Log("addCheckpoints");
        foreach (var i in checkpoints.Values)
        {
            Debug.Log("point nr=" + i.nr);
            CheckPoint point = new CheckPoint();
            point.nr = i.nr;
            point.time = i.time;
            checkpoint.Add(point);
        }
    }
}
