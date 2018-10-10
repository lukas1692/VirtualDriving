using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class TestRunController
{
    static private int NR_OF_EVALUATIONRUNS = 2;

    static private TestRun run = new TestRun();
    static public List<Lap> ghostlaps = new List<Lap>();

    static public Lap current_ghost;
    static public Lap current_drive = new Lap(ScenarioType.TRAINING, 1000);

    static private FileController  file_controller = new FileController();

    static ScenarioType scenario_type = ScenarioType.TRAINING;
    static RaceType race_type = RaceType.GHOST;
    static public int mmr = 1000;

    static public string id = "Invalid Id";

    static public SceneIndicies scene_indecies = null;

    static public ScenarioNr visible_scene = ScenarioNr.START;

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
        current_drive = new Lap(scenario_type, mmr);

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
        current_drive.myid = id;

        if (current_ghost != null)
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

    //public static void TriggerNextScene()
    //{
    //    // called when upload is finished
    //    SceneManager.LoadScene(ScenarioNr.WHEELOFEMOTIONS.ToString());
    //    AudioListener.pause = false;
    //    Time.timeScale = 1f;

    //}

    public static int GetClosedGhost()
    {
        if (scenario_type == ScenarioType.TRAINING)
            return -1;
        if (scene_indecies == null)
            return -1;
        int file_index = -1;

        int min_mmr = int.MaxValue;
        foreach(var s in scene_indecies.indicies)
        {
            if (scenario_type != s.scene_type)
                continue;
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
        int k = 128;

        // Who is the 'Journal Grand Master'? A
        // new ranking based on the Elo rating
        // system

        float ea = 1f / (1f + (Mathf.Pow(10f, (rb - ra) / lammda)));

        ra = Mathf.FloorToInt(ra + k * (sa - ea));

        Debug.Log("new mmr=" + ra);

        return ra;
    }

    public static void TriggerNextScene()
    {
        switch (visible_scene)
        {
            case ScenarioNr.START:
                visible_scene = ScenarioNr.BIGFIVE;
                SceneManager.LoadScene(ScenarioNr.BIGFIVE.ToString());
                break;
            case ScenarioNr.BIGFIVE:
                visible_scene = ScenarioNr.INITIALQUESTIONNAIRE;
                SceneManager.LoadScene(ScenarioNr.INITIALQUESTIONNAIRE.ToString());
                break;
            case ScenarioNr.INITIALQUESTIONNAIRE:
                visible_scene = ScenarioNr.INSTRUCTIONS;
                SceneManager.LoadScene(ScenarioNr.INSTRUCTIONS.ToString());
                break;
            case ScenarioNr.INSTRUCTIONS:
                visible_scene = ScenarioNr.LOADTRACK;
                SceneManager.LoadScene(ScenarioNr.LOADTRACK.ToString());
                break;
            case ScenarioNr.EVALUATIONTRACK:
                visible_scene = ScenarioNr.WHEELOFEMOTIONS;
                if (run.GetCurrentRound() == NR_OF_EVALUATIONRUNS)
                {
                    scenario_type = ScenarioType.TRACK1;
                }
                    
                SceneManager.LoadScene(ScenarioNr.WHEELOFEMOTIONS.ToString());
                break;
            case ScenarioNr.LOADTRACK:
                Debug.Log("Round: " + run.GetCurrentRound());

                if (run.GetCurrentRound() < NR_OF_EVALUATIONRUNS)
                {
                    scenario_type = ScenarioType.TRAINING;
                    visible_scene = ScenarioNr.EVALUATIONTRACK;
                    SceneManager.LoadScene(ScenarioNr.EVALUATIONTRACK.ToString());
                }
                else if (run.GetCurrentRound() < 8)
                //if (run.GetCurrentRound() < 8)
                {
                    scenario_type = ScenarioType.TRACK1;
                    visible_scene = ScenarioNr.RACETRACK1;
                    SceneManager.LoadScene(ScenarioNr.RACETRACK1.ToString());
                }
                else if (run.GetCurrentRound() < 11)
                {
                    SceneManager.LoadScene(ScenarioNr.END.ToString());
                }
                break;
            case ScenarioNr.WHEELOFEMOTIONS:
                visible_scene = ScenarioNr.Questionnaire;
                SceneManager.LoadScene(ScenarioNr.Questionnaire.ToString());
                AudioListener.pause = false;
                Time.timeScale = 1f;
                break;
            case ScenarioNr.Questionnaire:
                visible_scene = ScenarioNr.LOADTRACK;
                SceneManager.LoadScene(ScenarioNr.LOADTRACK.ToString());
                break;
            case ScenarioNr.RACETRACK1:
                visible_scene = ScenarioNr.WHEELOFEMOTIONS;
                SceneManager.LoadScene(ScenarioNr.WHEELOFEMOTIONS.ToString());
                break;
            case ScenarioNr.RACETRACK2:
                visible_scene = ScenarioNr.WHEELOFEMOTIONS;
                SceneManager.LoadScene(ScenarioNr.WHEELOFEMOTIONS.ToString());
                break;
            default:
                Debug.LogError("Invalid Scene");
                break;
        }
    }
}
