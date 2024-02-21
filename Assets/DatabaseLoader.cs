using UnityEngine;
using System.Xml;
using System.IO;

public class DatabaseLoader : MonoBehaviour
{
    public static Database[] db;
    void Start()
    {
        // pobranie tekstu xml ze œcie¿ki
        if (!File.Exists(Config.FilePathGlobal + "\\questions.xml"))
        {
            Debug.Log("Creating Default XML");
            File.WriteAllText(Config.FilePathGlobal + "\\questions.xml", Resources.Load<TextAsset>("XMLEXAMPLE").text);
        }
        TextAsset xmlAsset = new TextAsset(System.IO.File.ReadAllText(Config.FilePathGlobal + "\\questions.xml"));
        // pobranie dokuemtu z zachowaniem jego cech
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlAsset.text);

        XmlNodeList databases = xmlDoc.GetElementsByTagName("Database");
        ReadXML(databases);
    }
    public void ReadXML(XmlNodeList databases)
    {
        db = new Database[databases.Count];

        for (int i = 0; i < databases.Count; i++)
        {
            XmlNode database = databases[i];
            string name = database.Attributes["name"].Value;
            string question = database.SelectSingleNode("question").InnerText;

            XmlNodeList answers = database.SelectSingleNode("answerstext").SelectNodes("answer");
            string[] answerTexts = new string[answers.Count];
            if (answers.Count > 0)
            {
                for (int j = 0; j < answers.Count; j++)
                {
                    answerTexts[j] = answers[j].InnerText;
                }
            }
            string answerType = database.SelectSingleNode("answerType").InnerText;
            string correctanswer = database.SelectSingleNode("correctanswer").InnerText;

            db[i] = new Database()
            {
                name = name,
                question = question,
                answerstext = answerTexts,
                correctanswer = correctanswer,
                answerType = answerType
            };
        }
    }
}