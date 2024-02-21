using Unity.VisualScripting;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static string FilePathGlobal;
    private void Awake()
    {
        FilePathGlobal = Application.persistentDataPath;
    }
}