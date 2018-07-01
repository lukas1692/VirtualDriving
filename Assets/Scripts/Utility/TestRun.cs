using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRun {


    public List<Lap> lap = new List<Lap>();
    public string modus;
    public int currentmmr;

    public int GetCurrentRound()
    {
        return lap.Count;
    }
}
