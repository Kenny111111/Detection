using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public struct Subtitles
{
    public string message;
    public float secondsDisplayed;
    public Vector3 blackOverlaySize;
}

[CreateAssetMenu(fileName = "dialogue.asset", menuName = "Subtitles/Dialogue")]
public class Dialogue : ScriptableObject
{
    public Subtitles[] subtitles;
    public Vector3 textPos;
    public Vector3 textRotation;
    public Color textColor = new Color(255, 255, 255, 188f);
}