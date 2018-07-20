using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class TestRunController
{
    static private TestRun run = new TestRun();
    static public List<Lap> ghostlaps = new List<Lap>();

    static public Lap current_ghost;
    static public Lap current_drive = new Lap(ScenarioType.TRAINING, 1000);

    static private FileController  file_controller = new FileController();

    static ScenarioType scenario = ScenarioType.TRAINING;
    static RaceType race_type = RaceType.GHOST;
    static public int mmr = 1000;

    static public string id = "Invalid Id";

    static public SceneIndicies scene_indecies = null;

    public static int GetCurrentRound()
    {
        return run.GetCurrentRound();
    }

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

        if(current_ghost != null)
        {
            GhostCarScript.StartGhost(current_ghost);
            LapTimeController.SetGhost(current_ghost);
        }
    }

    public static void PassFinishLine(Dictionary<int, CheckPointActivation> checkpoints)
    {
        current_drive.AddCheckpoints(checkpoints);
        current_drive.laptime = LapTimeController.GetCurrentTime();
        run.lap.Add(current_drive);
        current_drive.round = GetCurrentRound();

        if(current_ghost != null)
        {
            current_drive.opponent_id = current_ghost.myid;
            current_drive.opponent_round = current_ghost.round;

            if (current_drive.laptime < current_ghost.laptime)
                mmr = CalculateCurrentMMR(current_ghost.mmr, current_drive.mmr, Game_Score.WON);
            else
                mmr = CalculateCurrentMMR(current_ghost.mmr, current_drive.mmr, Game_Score.LOSS);
        }
        else
        {
            current_drive.opponent_id = "No Opponent";
            current_drive.opponent_round = -1;
        }




        file_controller.SaveFile(current_drive);

        GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
        datamanager.SendMessage("UploadRound", current_drive);

        AudioListener.pause = true;
        Time.timeScale = 0f;
    }

    public static void TriggerNextScene()
    {
        // called when upload is finished
        SceneManager.LoadScene(ScenarioNr.WHEELOFEMOTIONS.ToString());
        AudioListener.pause = false;
        Time.timeScale = 1f;

    }

    public static int GetClosedGhost()
    {
        if (scene_indecies == null)
            return -1;
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
        //int k = 32;
        int k = 64;

        // Who is the 'Journal Grand Master'? A
        // new ranking based on the Elo rating
        // system

        float ea = 1f / (1f + (Mathf.Pow(10f, (rb - ra) / lammda)));

        ra = Mathf.FloorToInt(ra + k * (sa - ea));

        Debug.Log("new mmr=" + ra);

        return ra;
    }
}
