using Unity.VisualScripting;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static string filePathGlobal;
    private void Awake()
    {
        filePathGlobal = Application.persistentDataPath;
    }
}