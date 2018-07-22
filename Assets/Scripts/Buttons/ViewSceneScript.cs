using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewSceneScript : MonoBehaviour {

    FileController file = new FileController();

    List<Lap> laps = new List<Lap>();

    const int MIN_MMR = 500;
    const int MAX_MMR = 1500;

    [SerializeField]
    Text text_filed;
    // Use this for initialization

    float min_laptime_track1 = float.MaxValue;
    float max_laptime_track1 = float.MinValue;

    float min_laptime_track2 = float.MaxValue;
    float max_laptime_track2 = float.MinValue;

    void Start () {
        laps = file.LoadAllFiles();

        SceneIndicies index = file.LoadIndexFile();

        foreach(var i in index.indicies)
        {
            Debug.Log("file"+i.file_index + " " + i.laptime + " " + i.mmr);
        }

        ViewList();
	}
	
    public void ViewList()
    {
        string text = "";
        foreach (var lap in laps)
        {
            text += lap.laptime + ", " + lap.mmr + ", " + lap.scene_type.ToString() + "\n";

            if(lap.scene_type == ScenarioType.TRACK1)
            {
                if (lap.laptime < min_laptime_track1)
                    min_laptime_track1 = lap.laptime;
                if (lap.laptime > max_laptime_track1)
                    max_laptime_track1 = lap.laptime;
            }
            if (lap.scene_type == ScenarioType.TRACK2)
            {
                if (lap.laptime < min_laptime_track2)
                    min_laptime_track2 = lap.laptime;
                if (lap.laptime > max_laptime_track2)
                    max_laptime_track2 = lap.laptime;
            }


        }
        text_filed.text = text;
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void Sort()
    {
        file.DeleteAllFiles();

        SceneIndicies index_file = new SceneIndicies();

        int i = 0;
        foreach (var lap in laps)
        {
            float t = 0;
            switch(lap.scene_type)
            {
                case ScenarioType.TRACK1:
                        t = 1f - ((lap.laptime - min_laptime_track1) / (max_laptime_track1 - min_laptime_track1));
                    break;
                case ScenarioType.TRACK2:
                    t = 1f - ((lap.laptime - min_laptime_track2) / (max_laptime_track2 - min_laptime_track2));
                    break;
                default:
                    break;
            }

            lap.mmr = Mathf.RoundToInt(t * MAX_MMR) + MIN_MMR;

            file.SaveFile(lap);

            index_file.indicies.Add(new SceneIndex(i, lap.mmr, lap.laptime, lap.scene_type));
            i++;

        }
        
        ViewList();
        file.SaveIndexFile(index_file);
    }
}
