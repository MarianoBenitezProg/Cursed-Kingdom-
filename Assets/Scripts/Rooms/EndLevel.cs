using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public string nextLevelName;
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            _animator.SetBool("IsEnding", true);
            StartCoroutine(StartAnimation());
        }
    }

    public IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(1f);
        ChangeScene();
    }

    void ChangeScene()
    {
        if (LevelsManager.instance != null)
        {
            for (int i = 0; i < SavedGameManager.instance.saveSlots.Count; i++)
            {
                if (SavedGameManager.instance.selectedSaveSlot == SavedGameManager.instance.saveSlots[i].slot)
                {
                    SavedGameData UpdateLifeData = SavedGameManager.instance.saveSlots[i]; //You can´t just change the Life from the save slot, you gotta change the whole SaveSlot
                    UpdateLifeData.level = nextLevelName;
                    SavedGameManager.instance.saveSlots[i] = UpdateLifeData;
                }
            }
            LevelsManager.instance.LoadScene(nextLevelName);
        }
    }
}
