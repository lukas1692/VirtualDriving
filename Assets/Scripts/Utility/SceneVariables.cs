using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScenarioNr
{
    START,
    LOADTRACK,
    RACETRACK3,
    WHEELOFEMOTIONS,
    TRACK2,
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


