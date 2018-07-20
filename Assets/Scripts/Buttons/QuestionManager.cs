using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct Question
{
    public Question(uint number_, string text_)
    {
        number = number_;
        text = text_;
        rating = -1;
    }

    public uint number;
    public string text;
    public int rating;

    public override string ToString()
    {
        return number + "," + text + "," + rating;
    }
};

public class QuestionManager : MonoBehaviour
{

    private int activeQuestion = 0;

    private GameObject[] radioButtons;

    private Text questionText;

    private List<Question> allquestions = new List<Question>();

    private Question[] questions;

    private GameObject confirmButton;


    // Use this for initialization
    void Start()
    {

        radioButtons = GameObject.FindGameObjectsWithTag("RadioButton");

        questionText = GameObject.FindGameObjectWithTag("Question").GetComponent<Text>();

        confirmButton = GameObject.FindGameObjectWithTag("ConfirmButton");

        allquestions.Add(new Question(1, "The level was fun"));
        allquestions.Add(new Question(2, "I can do better"));

        Shuffle(allquestions);

        // Todo shuffle
        questions = (Question[])allquestions.ToArray();

        questionText.text = questions[activeQuestion].text;

        UncheckAll();

        DisableConfirmButton();
    }

    void WriteQuestionFile(string name)
    {
    }

    void Shuffle(List<Question> list)
    {
        int n = list.Count;

        System.Random rng = new System.Random();
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            Question value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    void DisableConfirmButton()
    {
        confirmButton.SetActive(false);
    }

    void EnableConfirmButton()
    {
        confirmButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

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
        UncheckAll();
        activeQuestion++;
        DisableConfirmButton();
        if (activeQuestion < questions.Length)
        {
            questionText.text = questions[activeQuestion].text;
        }
        //else
        //{
        //    WriteQuestionFile(GerateMTurkNumber.getFileName());
        //    play.onClick();
        //}

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

    public void ClickButton6()
    {
        UncheckAll();
        questions[activeQuestion].rating = 6;
        EnableConfirmButton();
    }

    public void ClickButton7()
    {
        UncheckAll();
        questions[activeQuestion].rating = 7;
        EnableConfirmButton();
    }
}
