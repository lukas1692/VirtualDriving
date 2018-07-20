using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lap {

    public List<TimeStep> timestep = new List<TimeStep>();
    public List<CheckPoint> checkpoint = new List<CheckPoint>();
    public float laptime;
    public ScenarioType scene_type;
    public int mmr;
    public int round;
    public string myid;
    public string opponent_id;
    public int opponent_round;

    public Lap(ScenarioType scene, int curent_mmr)
    {
        scene_type = scene;
        mmr = curent_mmr;
    }

    public void AddCheckpoints(Dictionary<int, CheckPointActivation> checkpoints)
    {

        foreach (var i in checkpoints.Values)
        {

            CheckPoint point = new CheckPoint();
            point.nr = i.nr;
            point.time = i.time;
            checkpoint.Add(point);
        }

    }

    public string PositionToString()
    {
        string ret = "";

        for (int i = 0; i < timestep.Count; i++)
        {
            if(i%10 == 0)
            {
                ret += timestep[i].positionToString();
            }   
        }
        return ret;
    }

    public string TimeToString()
    {
        string ret = "";

        for (int i = 0; i < timestep.Count; i++)
        {
            if (i % 10 == 0)
            {
                ret += timestep[i].time.ToString("N1") + " ";
            }
        }
        return ret;
    }
    public string SpeedToString()
    {
        string ret = "";

        for (int i = 0; i < timestep.Count; i++)
        {
            if (i % 10 == 0)
            {
                ret += timestep[i].speed.ToString("N1") + " ";
            }
        }
        return ret;
    }
}
