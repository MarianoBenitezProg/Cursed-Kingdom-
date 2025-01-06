using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUD_Manager : MonoBehaviour
{
    [SerializeField] private P_Behaviour _playerScript;
    private AbilityTimers _timersRef;

    [SerializeField] private GameObject[] _markusUI;
    [SerializeField] private GameObject[] _feranaUI;

    private Dictionary<string, Image> feranaImages = new Dictionary<string, Image>();
    private Dictionary<string, Image> markusImages = new Dictionary<string, Image>();

    private bool isMarkusRef;

    private void Awake()
    {
        isMarkusRef = _playerScript.isMarkus;
        _timersRef = _playerScript.GetComponent<AbilityTimers>();

        InitializeAbilityUI(_feranaUI, feranaImages, "Ferana");
        InitializeAbilityUI(_markusUI, markusImages, "Markus");

        _timersRef.OnCooldownUpdated.AddListener(UpdateCooldownUI);

        UpdateUIState();
    }

    private void InitializeAbilityUI(GameObject[] uiElements, Dictionary<string, Image> imageDict, string prefix)
    {
        string[] abilityNames = { "None", "BasicAttack", "Damage", "CC", "Utility" }; //El "None" es importante porque en la lista imagenes primero esta la UI de flecha o espada

        for (int i = 0; i < uiElements.Length; i++)
        {
            if (i < abilityNames.Length)
            {
                string key = prefix + abilityNames[i];
                imageDict[key] = uiElements[i].GetComponent<Image>();
            }
        }
    }

    private void Update()
    {
        if (_playerScript.isMarkus != isMarkusRef)
        {
            UpdateUIState();
        }
    }

    private void UpdateUIState()
    {
        bool markusActive = _playerScript.isMarkus;

        foreach (GameObject obj in _markusUI)
            obj.SetActive(markusActive);

        foreach (GameObject obj in _feranaUI)
            obj.SetActive(!markusActive);

        isMarkusRef = markusActive;
    }

    private void UpdateCooldownUI(string abilityName, float progress)
    {
        if (markusImages.ContainsKey(abilityName))
            markusImages[abilityName].fillAmount = progress;

        if (feranaImages.ContainsKey(abilityName))
            feranaImages[abilityName].fillAmount = progress;
    }
}
