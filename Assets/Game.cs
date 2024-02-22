using System;
using System.IO;
using TMPro;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Unity.Properties;

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

    int goodAnswers; int wrongAnswers;

    int page;
    List<int> pagesVisited;

    private void Awake()
    {
        pagesVisited = new List<int>();
        db = DatabaseLoader.db;
        RandomPage();
    }
    // ustawia obrazki, napisy
    public void SetData()
    {
        SetImage();
        question.text = db[page].question;
        switch (db[page].answertype)
        {
            case "text":
                for (int i = 0; i < db[page].answerstext.Length; i++)
                {
                    answers[i].SetActive(false);
                    panels[3].SetActive(false);
                    panels[4].SetActive(true);
                }
                break;
            case "buttons":
                for (int i = 0; i < db[page].answerstext.Length; i++)
                {
                    answers[i].SetActive(true);
                    panels[3].SetActive(true);
                    panels[4].SetActive(false);
                    answers[i].GetComponentInChildren<TMP_Text>().text = db[page].answerstext[i];
                }
                break;
        }
    }
    public void SetImage()
    {
        try
        {
            byte[] bytesImage = File.ReadAllBytes(Config.filePathGlobal + "\\" + db[page].name + ".png");
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
        }
    }
    // ustawia guziki z powrotem na interactable
    public void ResetButtons()
    {
        answerString.GetComponent<TMP_InputField>().text = null;
        for (int i = 0; i < answers.Length; i++)
        {
            answers[i].SetActive(false);
            answers[i].gameObject.GetComponent<Button>().interactable = true;
        }
    }
    // wywo³ywane inputem albo buttonem
    public void Answer(string inputanswer)
    {
        if (db[page].correctanswer == inputanswer || db[page].correctanswer == answerString.GetComponent<TMP_InputField>().text.ToLower())
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
        Debug.Log("Good Answer " + goodAnswers);
        if (pagesVisited.Count == db.Length)
        {
            GameEnd();
        }
        else
        {
            RandomPage();
            ResetButtons();
            SetData();
        }
    }
    void WrongAnswer(string inputanswer)
    {
        wrongAnswers++;
        Debug.Log("Wrong Answer " + wrongAnswers);
        if (db[page].answertype == "buttons")
        {
            // zmienia odpowiedŸ na int ¿eby wy³¹czyæ guzik który nacisn¹³ u¿ytkownik
            int btt = Convert.ToInt32(inputanswer);
            answers[btt].gameObject.GetComponent<Button>().interactable = false;
        }
    }
    //losuje strony, przypisuje odwiedzone strony tak aby sie nie powtarza³y
    void RandomPage()
    {
        do {
            page = UnityEngine.Random.Range(0, db.Length);
        } while (pagesVisited.Contains(page));
        pagesVisited.Add(page);
    }
    void GameEnd()
    {
        panels[0].SetActive(false);
        panels[1].SetActive(false);
        panels[2].SetActive(true);
        statistics[0].gameObject.GetComponent<TMP_Text>().text = "Good Answers: " + goodAnswers.ToString();
        statistics[1].gameObject.GetComponent<TMP_Text>().text = "Wrong Answers: " + wrongAnswers.ToString();
        statistics[2].gameObject.GetComponent<TMP_Text>().text = "Questions Completed: " + pagesVisited.Count.ToString();
    }
}