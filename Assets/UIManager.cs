using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public int index;
    public TMP_Text _messageText;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
    }

    public void PlayDialogue(Dialogue dialogue)
    {
        Debug.Log("Test for dialogue function");
        index = 0;
        StartCoroutine(PlayDialogueRoutine(dialogue));
    }

    private IEnumerator PlayDialogueRoutine(Dialogue dialogue)
    {
        _messageText.transform.localPosition = dialogue.textPos;
        _messageText.transform.localEulerAngles = dialogue.textRotation;
        _messageText.color = dialogue.textColor;
        while(index != dialogue.subtitles.Count)
        {
            _messageText.text = dialogue.subtitles[index].message;
            yield return new WaitForSeconds(dialogue.subtitles[index].secondsDisplayed);
            index++;
        }
        _messageText.text = "";
    }
}
