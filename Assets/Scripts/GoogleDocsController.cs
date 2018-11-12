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
        
        StartCoroutine(PostRound(TestRunController.id, lap.round.ToString(), lap.PositionToString(), lap.TimeToString(), lap.SpeedToString(), lap.opponent_id, lap.opponent_round.ToString(), lap.mmr.ToString(), lap.scene_type.ToString()));
    }

    IEnumerator PostRound(string id, string round, string data, string time, string speed, string oppid, string oppround, string mmr, string type)
    {
        // form action
        string Base_URL = "https://docs.google.com/forms/d/e/1FAIpQLSd3ZX-85Qe4Qf40P0zyLBGGHSYeSrM5JvtAkw7WJLevuJu88A/formResponse";
        WWWForm form = new WWWForm();
        form.AddField("entry.1614371190", id);
        form.AddField("entry.306343255", round);
        form.AddField("entry.1370521960", data);
        form.AddField("entry.405827058", time);
        form.AddField("entry.2142598625", speed);
        form.AddField("entry.141393872", oppid);
        form.AddField("entry.1334400903", oppround);
        form.AddField("entry.747666729", mmr);
        form.AddField("entry.231219547", type);
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

    public void UploadBetweenQuestions(GDocsQuestionManagerEntry entry)
    {
        Debug.Log("upload between questions to google docs");
        StartCoroutine(PostBetweenQuestions(entry.id, entry.round, entry.fun_rating, entry.skill_rating));
    }

    IEnumerator PostBetweenQuestions(string id, int round, int fun, int skill)
    {
        string Base_URL = "https://docs.google.com/forms/d/e/1FAIpQLSf0pGRwatKkkSYk-1bvZNF_pX_25E4LKqEihcnekiQdQdsMAA/formResponse";
        WWWForm form = new WWWForm();
        form.AddField("entry.1319038938", id);
        form.AddField("entry.917292570", round);
        form.AddField("entry.929870134", fun);
        form.AddField("entry.1484980464", skill);
        WWW www = new WWW(Base_URL, form.data);
        yield return www;

        GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
        datamanager.SendMessage("QuestionManagerTriggerNextScene");
    }

    public void UploadBig5Questions(GDocsBigFiveQuestionManagerEntry entry)
    {
        Debug.Log("upload between big 5 to google docs");
        StartCoroutine(PostBig5Questions(entry.id, entry.agreeableness, entry.conscientiousness, entry.extraversion, entry.neuroticism, entry.openness,
            entry.reserved_rating_1, entry.trust_rating_2, entry.lazy_rating_3, entry.stress_rating_4, entry.artistic_rating_5, 
            entry.sozial_rating_6, entry.fault_rating_7, entry.job_rating_8, entry.nervous_rating_9, entry.imagination_rating_10));
    }

    IEnumerator PostBig5Questions(string id, double agreeableness, double conscientiousness, double extraversion, double neuroticism, double openness,
        int reserved_1, int trust_2, int lazy_3, int stress_4, int artistic_5, 
        int sozial_6, int fault_7, int job_8, int nervous_9, int imagination_10)
    {
        string Base_URL = "https://docs.google.com/forms/d/e/1FAIpQLSeC8D-taISJo1Lr--pIXr4vweYqlLuaB9ZPbu2388InmwKs_g/formResponse";
        WWWForm form = new WWWForm();
        form.AddField("entry.1426296430", id);
        form.AddField("entry.345350860", agreeableness.ToString());
        form.AddField("entry.1350764584", conscientiousness.ToString());
        form.AddField("entry.188715868", extraversion.ToString());
        form.AddField("entry.977959719", neuroticism.ToString());
        form.AddField("entry.2102337265", openness.ToString());

        form.AddField("entry.1097491537", reserved_1.ToString());
        form.AddField("entry.715410856", trust_2.ToString());
        form.AddField("entry.1369914652", lazy_3.ToString());
        form.AddField("entry.407465352", stress_4.ToString());
        form.AddField("entry.175625305", artistic_5.ToString());
        form.AddField("entry.106750218", sozial_6.ToString());
        form.AddField("entry.1085228240", fault_7.ToString());
        form.AddField("entry.2107677931", job_8.ToString());
        form.AddField("entry.322928645", nervous_9.ToString());
        form.AddField("entry.728921133", imagination_10.ToString());

        WWW www = new WWW(Base_URL, form.data);
        yield return www;

        GameObject datamanager = GameObject.FindGameObjectWithTag("DataManagment");
        datamanager.SendMessage("QuestionManagerTriggerNextScene");
    }
}
