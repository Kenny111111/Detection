using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        else instance = this;
    }
    //setup for subtitle
    public int index;
        public GameObject blackOverlay;
        public TMP_Text _messageText;

        //play subtitle
        public void PlayDialogue(Dialogue dialogue)
        {
            Debug.Log("Test for dialogue function");
            index = 0;
            StartCoroutine(PlayDialogueRoutine(dialogue));
        }

        //displaying subtitle
        private IEnumerator PlayDialogueRoutine(Dialogue dialogue)
        {
            _messageText.transform.localPosition = dialogue.textPos;
            _messageText.transform.localEulerAngles = dialogue.textRotation;
            _messageText.color = dialogue.textColor;
            while(index != dialogue.subtitles.Length)
            {
                _messageText.text = dialogue.subtitles[index].message;
                blackOverlay.transform.localScale = dialogue.subtitles[index].blackOverlaySize;
                yield return new WaitForSeconds(dialogue.subtitles[index].secondsDisplayed);
                index++;
            }
            blackOverlay.transform.localScale = Vector3.zero;
            _messageText.text = "";
        }
}
