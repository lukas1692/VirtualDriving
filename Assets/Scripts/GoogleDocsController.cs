using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleDocsController : MonoBehaviour {

    private static List<Lap> laps = new List<Lap>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void UploadRound(Lap lap)
    {
        Debug.Log("uploaded lap to google docs");
        Debug.Log(lap.PositionToString().Length);
        
        StartCoroutine(PostRound(Time.time.ToString(), lap.PositionToString(), lap.TimeToString(), lap.SpeedToString()));
    }

    IEnumerator PostRound(string id, string data, string time, string speed)
    {
        // form action
        string Base_URL = "https://docs.google.com/forms/d/e/1FAIpQLSd3ZX-85Qe4Qf40P0zyLBGGHSYeSrM5JvtAkw7WJLevuJu88A/formResponse";
        WWWForm form = new WWWForm();
        form.AddField("entry.1614371190", id);
        form.AddField("entry.1370521960", data);
        form.AddField("entry.405827058", time);
        form.AddField("entry.2142598625", speed);
        WWW www = new WWW(Base_URL, form.data);
        yield return www;

        TestRunController.TriggerNextScene();
    }

    public void UploadLapQuestionary(GDocsQuestionaryEntry entry)
    {
        Debug.Log("uploaded questionary to google docs");

        StartCoroutine(PostLapQuestionary(entry.id, entry.round, entry.high, entry.medium, entry.low));
    }

    IEnumerator PostLapQuestionary(string id, int round, string high, string medium, string low)
    {
        string Base_URL = "https://docs.google.com/forms/d/e/1FAIpQLSemXH8zwVc8uisy4aLUW990i9BBcLqR0R89U_UDDMsBoTvWPw/formResponse";
        WWWForm form = new WWWForm();
        form.AddField("entry.1233701858", id);
        form.AddField("entry.1049965446", round);
        form.AddField("entry.1952122733", high);
        form.AddField("entry.600115171", medium);
        form.AddField("entry.462399844", low);
        WWW www = new WWW(Base_URL, form.data);
        yield return www;

        GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
        datamanager.SendMessage("EmotionsTriggerNextScene");
    }
}
