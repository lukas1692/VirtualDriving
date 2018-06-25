using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScenarioNr
{
    START,
    LOADTRACK,
    TRACK1,
    TRACK2,
    WHEELOFEMOTIONS,
    QUESTIONARY,
}

public struct CheckPointActivation
{
    public int nr;
    public bool activated;
    public float time;
};

public enum RaceType
{
    GHOST,
    TIME
}

public enum ScenarioType
{
    TRAINING,
    TRACK1,
    TRACK2,
}

public enum Game_Score
{
    WON,
    DRAW,
    LOSS,
}


