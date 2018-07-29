using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Personality
{
    Extraversion,
    Agreeableness,
    Conscientiousness,
    Neuroticism,
    Openness
}

struct BigFiveQuestion
{
    public BigFiveQuestion(uint number_, string text_, bool inverse_, Personality personality_)
    {
        number = number_;
        text = text_;
        rating = -1;
        inverse = inverse_;
        personality = personality_;
    }
    
    public uint number;
    public string text;
    public int rating;
    public bool inverse;
    public Personality personality;

    public override string ToString()
    {
        return number + "," + text;
    }

    public double GetRating()
    {
        if(inverse)
        {
            return (6-rating) / 10.0f;
        }
        else
        {
            return rating / 10.0f;
        }
    }
};

public class BigFiveQuestionManager : MonoBehaviour {

    private string q = "I see myself as someone who ";

    private int activeQuestion = 0;

    private GameObject[] radioButtons;

    private Text questionText;

    private List<BigFiveQuestion> allquestions = new List<BigFiveQuestion>();

    private BigFiveQuestion[] questions;

    private GameObject confirmButton;

    // Use this for initialization
    void Start () {
        radioButtons = GameObject.FindGameObjectsWithTag("RadioButton");

        questionText = GameObject.FindGameObjectWithTag("Question").GetComponent<Text>();

        confirmButton = GameObject.FindGameObjectWithTag("ConfirmButton");

        allquestions.Add(new BigFiveQuestion(1, "is reserved", false, Personality.Extraversion));
        allquestions.Add(new BigFiveQuestion(2, "is generally trusting", false, Personality.Agreeableness));
        allquestions.Add(new BigFiveQuestion(3, "tends to be lazy", false,Personality.Conscientiousness));
        allquestions.Add(new BigFiveQuestion(4, "is relaxed, handles stress well", false, Personality.Neuroticism));
        allquestions.Add(new BigFiveQuestion(5, "has few artistic interests", false, Personality.Openness));
        allquestions.Add(new BigFiveQuestion(6, "is outgoing, sociable", true, Personality.Extraversion));
        allquestions.Add(new BigFiveQuestion(7, "tends to find fault with others", true, Personality.Agreeableness));
        allquestions.Add(new BigFiveQuestion(8, "does a thorough job", true, Personality.Conscientiousness));
        allquestions.Add(new BigFiveQuestion(9, "gets nervous easily", true, Personality.Neuroticism));
        allquestions.Add(new BigFiveQuestion(10, "has an active imagination", true, Personality.Openness));

        Shuffle(allquestions);

        // Todo shuffle
        questions = (BigFiveQuestion[])allquestions.ToArray();

        SetQuestionText();

        UncheckAll();

        DisableConfirmButton();
    }

    void SetQuestionText()
    {
        questionText.text = "I see myself as someone who " + questions[activeQuestion].text;
    }

    void Shuffle(List<BigFiveQuestion> list)
    {
        int n = list.Count;

        System.Random rng = new System.Random();
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            BigFiveQuestion value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // Update is called once per frame
    void DisableConfirmButton()
    {
        confirmButton.SetActive(false);
    }

    void EnableConfirmButton()
    {
        confirmButton.SetActive(true);
    }

    void UncheckAll()
    {
        foreach (GameObject r in radioButtons)
        {
            r.GetComponentInChildren<Image>().gameObject.SetActive(false);
        }
    }

    public void Confirm()
    {
        Dictionary<Personality, double> personality = new Dictionary<Personality, double>();

        UncheckAll();
        activeQuestion++;
        DisableConfirmButton();
        if (activeQuestion < questions.Length)
        {
            SetQuestionText();
        }
        else
        {
            // TODO: Save Questions
            foreach (var q in questions)
            {
                if (!personality.ContainsKey(q.personality))
                    personality[q.personality] = 0.0f;
                personality[q.personality] += q.GetRating();
            }

            foreach (var item in personality)
            {
                Debug.Log(item.Key.ToString() + ": "+ item.Value.ToString());
            }

            TestRunController.TriggerNextScene();
        }
    }

    public void ClickButton1()
    {
        UncheckAll();
        questions[activeQuestion].rating = 1;
        EnableConfirmButton();
    }

    public void ClickButton2()
    {
        UncheckAll();
        questions[activeQuestion].rating = 2;
        EnableConfirmButton();
    }

    public void ClickButton3()
    {
        UncheckAll();
        questions[activeQuestion].rating = 3;
        EnableConfirmButton();
    }

    public void ClickButton4()
    {
        UncheckAll();
        questions[activeQuestion].rating = 4;
        EnableConfirmButton();
    }

    public void ClickButton5()
    {
        UncheckAll();
        questions[activeQuestion].rating = 5;
        EnableConfirmButton();
    }

}
