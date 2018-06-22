using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestRunController
{
    static public TestRun run = new TestRun();
    static public List<Lap> ghostlaps;

    static public Lap current_ghost = new Lap();
    static public Lap current_drive = new Lap();

    static private FileController  file_controller = new FileController();

    public static void init(List<Lap> laps)
    {
        ghostlaps = laps;
        
    }
        
    public static void addCurrentTimeStep(TimeStep step)
    {
        step.time = LapTimeController.getCurrentTime();
        current_drive.timestep.Add(step);
    }

    public static void startFinishLine()
    {
        current_drive = new Lap();
        current_ghost = ghostlaps[ghostlaps.Count - 1];
        LapTimeController.SetGhost(current_ghost);
        GhostCarScript.startGhost(current_ghost);
    }

    public static void passFinishLine(Dictionary<int, CheckPointActivation> checkpoints)
    {
        current_drive.addCheckpoints(checkpoints);
        current_drive.laptime = LapTimeController.getCurrentTime();
        run.lap.Add(current_drive);

        file_controller.saveFile(current_drive);

        LapTimeController.ResetController();

        SceneManager.LoadScene(1);
    }
}
