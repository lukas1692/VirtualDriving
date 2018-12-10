using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lap {

    public List<TimeStep> timestep = new List<TimeStep>();
    public List<CheckPoint> checkpoint = new List<CheckPoint>();
    public List<KeyValuePair<Vector3Serializable, Vector3Serializable>> resetpoints = new List<KeyValuePair<Vector3Serializable, Vector3Serializable>>();

    public float laptime;
    public ScenarioType scene_type;
    public RaceType race_type;
    public int mmr;
    public int round;
    public string myid;
    public string opponent_id;
    public int opponent_round;

    public int max_fps;
    public float avg_fps;

    public Lap(ScenarioType scene, RaceType race, int curent_mmr)
    {
        scene_type = scene;
        race_type = race;
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
                ret += timestep[i].PositionToString();
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

    public void AddResetpoints(Vector3 position, Vector3 checkpoint)
    {
        resetpoints.Add(new KeyValuePair<Vector3Serializable, Vector3Serializable>(new Vector3Serializable(position), new Vector3Serializable(checkpoint)));
    }
}
