using UnityEditor;
using UnityEngine;

public class DeveloperMenu
{
    [MenuItem("DeveloperMenu / Clear Saves")]
    public static void ClearSaves()
    {
        Debug.Log("Saves cleared (No implementation yet)");
    }

    [MenuItem("DeveloperMenu / Test Method")]
    public static void TestMethod()
    {
        Debug.Log("A testing method triggered (No implementation yet)");
    }
}
