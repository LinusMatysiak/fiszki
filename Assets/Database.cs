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
    [XmlElement("answerType")]
    public string answerType;
    [XmlElement("correctanswer")]
    public string correctanswer;
}