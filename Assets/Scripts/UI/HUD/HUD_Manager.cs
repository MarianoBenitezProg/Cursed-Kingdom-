using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public struct InventoryUI
{
    public Image _uiImage;
    public PickUpType _item;
}

public class HUD_Manager : MonoBehaviour
{
    public static HUD_Manager instance;
    [SerializeField] private P_Behaviour _playerScript;
    private AbilityTimers _timersRef;

    [SerializeField] private GameObject[] _markusUI;
    [SerializeField] private GameObject[] _feranaUI;
    [SerializeField] private Image _lifeBar;

    private Dictionary<string, Image> feranaImages = new Dictionary<string, Image>();
    private Dictionary<string, Image> markusImages = new Dictionary<string, Image>();

    [SerializeField] List<InventoryUI> _inventory = new List<InventoryUI>();

    private bool isMarkusRef;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        isMarkusRef = _playerScript.isMarkus;
        _timersRef = _playerScript.GetComponent<AbilityTimers>();

        InitializeAbilityUI(_feranaUI, feranaImages, "Ferana");
        InitializeAbilityUI(_markusUI, markusImages, "Markus");

        _timersRef.OnCooldownUpdated.AddListener(UpdateCooldownUI);

        UpdateUIState();
        UpdateHealthBar(null);
        EventManager.Subscribe(TypeEvent.DamageTaken, UpdateHealthBar);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(TypeEvent.DamageTaken, UpdateHealthBar);
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

    public void SetInventoryUI(PickUpType itemPicked, bool isActive)
    {
        for (int i = 0; i < _inventory.Count; i++)
        {
            if(_inventory[i]._item == itemPicked)
            {
                _inventory[i]._uiImage.enabled = isActive;
                return;
            }
        }
    }

    public void UpdateHealthBar(object param)
    {
        float fillAmount = (float)_playerScript.life / _playerScript.maxLife;

        _lifeBar.fillAmount = Mathf.Clamp01(fillAmount);
    }
}
