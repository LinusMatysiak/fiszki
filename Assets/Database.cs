using System.Xml;
using System.Xml.Serialization;
[System.Serializable]
public class Database
{
    [XmlAttribute("name")]
    public string name;
    [XmlElement("question")]
    public string question;
    [XmlArray("answertext")]
    [XmlArrayItem("answer")]
    public string[] answerstext;
    [XmlElement("answertype")]
    public string answertype;
    [XmlElement("correctanswer")]
    public string correctanswer;
}