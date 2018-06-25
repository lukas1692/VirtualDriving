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

    float min_laptime = float.MaxValue;
    float max_laptime = float.MinValue;

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
            text += lap.laptime + ", " + lap.mmr + "\n";

            if (lap.laptime < min_laptime)
                min_laptime = lap.laptime;
            if (lap.laptime > max_laptime)
                max_laptime = lap.laptime;
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
            float t = 1f - ((lap.laptime-min_laptime) / (max_laptime-min_laptime));
            lap.mmr = Mathf.RoundToInt(t * MAX_MMR) + MIN_MMR;

            file.SaveFile(lap);

            index_file.indicies.Add(new SceneIndex(i, lap.mmr, lap.laptime, lap.scene_type));
            i++;

        }
        
        ViewList();
        file.SaveIndexFile(index_file);
    }
}
