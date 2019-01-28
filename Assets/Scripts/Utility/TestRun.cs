using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRun {


    private List<Lap> laps = new List<Lap>();
    public string modus;
    public int currentmmr;
    private int round = 0;

    public void SetRound(Lap new_lap)
    {
        round++;
        laps.Add(new_lap);
    }

    public int GetCurrentRound()
    {
        Debug.Log("Round: " + round + laps.Count);
        return laps.Count;
    }

    public List<Lap> GetLaps()
    {
        return laps;
    }
}
