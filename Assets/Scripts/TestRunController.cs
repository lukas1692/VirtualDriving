using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class TestRunController
{
    static public TestRun run = new TestRun();
    static public List<Lap> ghostlaps = new List<Lap>();

    static public Lap current_ghost;
    static public Lap current_drive = new Lap(ScenarioType.TRAINING, 1000);

    static private FileController  file_controller = new FileController();

    static ScenarioType scenario = ScenarioType.TRAINING;
    static RaceType race_type = RaceType.GHOST;
    static public int mmr = 1000;

    static public SceneIndicies scene_indecies = null;

    public static void Init(List<Lap> laps)
    {
        ghostlaps = laps;
    }

    public static void AddNewGhostLap(Lap lap)
    {
        ghostlaps.Add(lap);
        current_ghost = lap;
    }

    public static void AddCurrentTimeStep(TimeStep step)
    {
        step.time = LapTimeController.GetCurrentTime();
        current_drive.timestep.Add(step);
    }

    public static void StartFinishLine()
    {
        current_drive = new Lap(scenario, mmr);

        GhostCarScript.StartGhost(current_ghost);

        LapTimeController.SetGhost(current_ghost);
    }

    public static void PassFinishLine(Dictionary<int, CheckPointActivation> checkpoints)
    {
        current_drive.AddCheckpoints(checkpoints);
        current_drive.laptime = LapTimeController.GetCurrentTime();
        run.lap.Add(current_drive);

        if (current_drive.laptime < current_ghost.laptime)
            mmr = CalculateCurrentMMR(current_ghost.mmr, current_drive.mmr, Game_Score.WON);
        else
            mmr = CalculateCurrentMMR(current_ghost.mmr, current_drive.mmr, Game_Score.LOSS);

        file_controller.SaveFile(current_drive);

        GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
        datamanager.SendMessage("UploadRound", current_drive);

        

        Time.timeScale = 0f;
    }

    public static void TriggerNextScene()
    {
        // called when upload is finished
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    public static int GetClosedGhost()
    {
        int file_index = 0;

        int min_mmr = int.MaxValue;
        foreach(var s in scene_indecies.indicies)
        {
            int mmr_dif = Mathf.Abs(s.mmr - mmr);
            if (min_mmr > mmr_dif)
            {
                min_mmr = mmr_dif;
                file_index = s.file_index;
            }
        }

        return file_index;
    }

    public static int CalculateCurrentMMR(int ghost, int current, Game_Score outcome)
    {
        float sa = 0.5f;
        if (outcome == Game_Score.WON)
            sa = 1f;
        if (outcome == Game_Score.LOSS)
            sa = 0f;

        int lammda = 400;
        int ra = current;
        int rb = ghost;
        int k = 32;

        Debug.Log("ra="+ra);
        Debug.Log("rb="+rb);
        Debug.Log("sa="+sa);

        // Who is the 'Journal Grand Master'? A
        // new ranking based on the Elo rating
        // system

        float ea = 1f / (1f + (Mathf.Pow(10f, (rb - ra) / lammda)));

        Debug.Log("ea=" + ea);

        ra = Mathf.FloorToInt(ra + k * (sa - ea));

        Debug.Log("new mmr=" + ra);

        return ra;
    }
}
