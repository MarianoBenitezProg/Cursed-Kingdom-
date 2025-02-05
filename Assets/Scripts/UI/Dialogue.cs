using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

[System.Serializable] public struct Dialogues
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    [SerializeField, TextArea(4, 6)] public string[] dialogueLines;
}

public class Dialogue : MonoBehaviour
{
    [SerializeField] private Dialogues[] dialogueSequences;

    private int currentSequenceIndex = 0;
    private int currentLineIndex = 0;
    private bool canProceed = true;
    private float typingTime = 0.05f;
    private bool sawDialogue;

    [SerializeField] CinemachineVirtualCameraBase[] cinematicCamera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(sawDialogue == false)
        {
            if (collision.gameObject.layer == 3)
            {
                StartDialogue();
                sawDialogue = true;
                EventManager.Trigger(TypeEvent.CinematicOn);
                SingleCameraTransition();
            }
        }
        
    }
    private void Update()
    {
        if(canProceed == true && Input.GetKeyDown(KeyCode.Mouse0) && sawDialogue == true)
        {
            currentLineIndex++;
            if(currentLineIndex < dialogueSequences[currentSequenceIndex].dialogueLines.Length)
            {
                Debug.Log(dialogueSequences[currentSequenceIndex]);
                StartDialogue();
            }
            else
            {
                dialogueSequences[currentSequenceIndex].dialoguePanel.SetActive(false);
                currentSequenceIndex++;
                currentLineIndex = 0;
                if(currentSequenceIndex < dialogueSequences.Length)
                {
                    StartDialogue();
                }
                else
                {
                    ResetCameras();
                    EventManager.Trigger(TypeEvent.CinematicOff);
                    Destroy(gameObject);
                }
            }
        }
    }

    public void StartDialogue()
    {
        canProceed = false;
        dialogueSequences[currentSequenceIndex].dialoguePanel.SetActive(true);
        StartCoroutine(ShowLine(dialogueSequences[currentSequenceIndex].dialogueText));
    }

    private IEnumerator ShowLine(TMP_Text text)
    {
        text.text = string.Empty;

        foreach (char ch in dialogueSequences[currentSequenceIndex].dialogueLines[currentLineIndex])
        {
            text.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
        canProceed = true;
    }


    #region Camera Control
    public void SingleCameraTransition()
    {
        if (cinematicCamera.Length > 0)
        {
            cinematicCamera[0].Priority = 20;
        }
    }
    public void ResetCameras()
    {
        if(cinematicCamera.Length > 0)
        {
            for (int i = 0; i < cinematicCamera.Length; i++)
            {
                cinematicCamera[i].Priority = 0;
            }
        }
        
    }

    #endregion
}