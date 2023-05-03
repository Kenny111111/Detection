using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Subtitles
{
    public string message;
    public float secondsDisplayed;

    public Subtitles(string newMessage, float newSecondsDisplayed)
    {
        message = newMessage;
        secondsDisplayed = newSecondsDisplayed;
    }
}

[CreateAssetMenu(fileName = "dialogue.asset", menuName = "Subtitles/Dialogue")]
public class Dialogue : ScriptableObject
{
    public List<Subtitles> subtitles;
    public Vector3 textPos;
    public Vector3 textRotation;
    public Color textColor = new Color(255, 255, 255, 188f);

    public Dialogue(List<Subtitles> newSubtitles, Vector3 newTextPos, Vector3 newTextRotation, Color newTextColor)
    {
        subtitles = newSubtitles;
        textPos = newTextPos;
        textRotation = newTextRotation;
        textColor = newTextColor;
    }
}