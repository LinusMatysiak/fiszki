using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    //[SerializeField] public Database[] db;
    public Database[] db;

    public Image image;
    public TMP_Text question;

    public GameObject[] panels;
    public GameObject[] answers;
    public GameObject[] statistics;
    public GameObject answerString;


    int page;
    int goodAnswers; int wrongAnswers;

    private void Awake()
    {
        db = DatabaseLoader.db;
    }
    // ustawia obrazki, napisy
    public void SetData()
    {
        SetImage();
        //image.sprite = Resources.Load<Sprite>(db[page].name.ToLower());

        //video.GetComponent<VideoPlayer>().clip = Resources.Load<VideoClip>(db[page].name.ToLower());
        question.text = db[page].question;
        if (db[page].answerType == "text")
        {
            for (int i = 0; i < db[page].answerstext.Length; i++)
            {
                answers[i].SetActive(false);
                panels[3].SetActive(false);
                panels[4].SetActive(true);
            }
        }
        else if (db[page].answerType == "buttons")
        {
            for (int i = 0; i < db[page].answerstext.Length; i++)
            {
                answers[i].SetActive(true);
                panels[3].SetActive(true);
                panels[4].SetActive(false);
                answers[i].GetComponentInChildren<TMP_Text>().text = db[page].answerstext[i];
            }
        }
    }
    public void SetImage()
    {
        try
        {
            byte[] bytesImage = File.ReadAllBytes(Config.FilePathGlobal + "\\" + db[page].name + ".png");
            Texture2D FileImage = new Texture2D(8, 8, TextureFormat.RGBA32, false);
            FileImage.LoadImage(bytesImage);
            image.GetComponent<Transform>().gameObject.SetActive(true);
            image.sprite = Sprite.Create(FileImage, new Rect(0, 0, FileImage.width, FileImage.height), new Vector2());
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            // wy³¹cza komponent obrazka gdy go nie znajdzie
            image.GetComponent<Transform>().gameObject.SetActive(false);
            image.sprite = Resources.Load<Sprite>("error");
        }
    }
    // ustawia guziki z powrotem na interactable
    public void ResetButtons()
    {
        for (int i = 0; i < answers.Length; i++)
        {
            answers[i].SetActive(false);
            answers[i].gameObject.GetComponent<Button>().interactable = true;
        }
    }
    // wywo³ywane inputem albo buttonem
    public void Answer(string inputanswer)
    {
        if (db[page].correctanswer == inputanswer || db[page].correctanswer == answerString.GetComponent<TMP_InputField>().text)
        {
            GoodAnswer();
        }
        else
        {
            WrongAnswer(inputanswer);
        }
    }
    void GoodAnswer()
    {
        goodAnswers++;
        page++;
        Debug.Log("Good Answer" + goodAnswers);
        Debug.Log("Page " + page);
        if (db.Length == page)
        {
            GameEnd();
        }
        else
        {
            ResetButtons();
            SetData();
        }
    }
    void WrongAnswer(string inputanswer)
    {
        wrongAnswers++;
        Debug.Log("Wrong Answer " + wrongAnswers);
        if (db[page].answerType == "buttons")
        {
            // zmienia odpowiedŸ na int ¿eby wy³¹czyæ guzik który nacisn¹³ u¿ytkownik
            int btt = Convert.ToInt32(inputanswer);
            answers[btt].gameObject.GetComponent<Button>().interactable = false;
        }
    }
    void GameEnd()
    {
        panels[0].SetActive(false);
        panels[1].SetActive(false);
        panels[2].SetActive(true);
        statistics[0].gameObject.GetComponent<TMP_Text>().text = "Good Answers: " + goodAnswers.ToString();
        statistics[1].gameObject.GetComponent<TMP_Text>().text = "Wrong Answers: " + wrongAnswers.ToString();
        statistics[2].gameObject.GetComponent<TMP_Text>().text = "Questions Completed: " + page.ToString();
    }
}