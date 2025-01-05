using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Manager : MonoBehaviour
{
    [SerializeField] P_Behaviour _playerScript;
    AbilityTimers _timersRef;
    [SerializeField] GameObject[] _markusUI;
    [SerializeField] GameObject[] _feranaUI;

    Image feranaBasic;
    Image feranaDamage;
    Image feranaCC;
    Image feranaUtility;

    Image markusBasic;
    Image markusDamage;
    Image markusCC;
    Image markusUtility;

    bool isMarkusRef;

    private void Awake()
    {
        isMarkusRef = _playerScript.isMarkus;
        _timersRef = _playerScript.gameObject.GetComponent<AbilityTimers>();
        feranaBasic = _feranaUI[1].GetComponent<Image>();
        feranaDamage = _feranaUI[2].GetComponent<Image>();
        feranaCC = _feranaUI[3].GetComponent<Image>();
        feranaUtility = _feranaUI[4].GetComponent<Image>();

        markusBasic = _markusUI[1].GetComponent<Image>();
        markusDamage = _markusUI[2].GetComponent<Image>();
        markusCC = _markusUI[3].GetComponent<Image>();
        markusUtility = _markusUI[4].GetComponent<Image>();
    }

    private void Update()
    {
        ChangeUI();
        AbilitiesCooldown();
    }

    public void ChangeUI()
    {
        if (_playerScript.isMarkus == true && isMarkusRef == false)
        {
            for (int i = 0; i < _markusUI.Length; i++)
            {
                _markusUI[i].gameObject.SetActive(true);
                _feranaUI[i].gameObject.SetActive(false);
                isMarkusRef = true;
            }
        }
        else if (_playerScript.isMarkus == false && isMarkusRef == true)
        {
            for (int i = 0; i < _markusUI.Length; i++)
            {
                _markusUI[i].gameObject.SetActive(false);
                _feranaUI[i].gameObject.SetActive(true);
                isMarkusRef = false;
            }
        }
    }

    public void AbilitiesCooldown()
    {
        if (_markusUI[1] != null)
        {
            markusBasic.fillAmount = _timersRef.GetCooldownProgress("MarkusBasicAttack");
            Debug.Log(feranaBasic);
        }
        if (_markusUI[2] != null)
        {
            markusDamage.fillAmount = _timersRef.GetCooldownProgress("MarkusDamage");
        }
        if (_markusUI[3] != null)
        {
            markusCC.fillAmount = _timersRef.GetCooldownProgress("MarkusCC");
        }
        if (_markusUI[4] != null)
        {
            markusUtility.fillAmount = _timersRef.GetCooldownProgress("MarkusUtility");
        }


        if (_feranaUI[1] != null)
        {
            feranaBasic.fillAmount = _timersRef.GetCooldownProgress("FeranaBasicAttack");
        }
        if (_feranaUI[2] != null)
        {
            feranaDamage.fillAmount = _timersRef.GetCooldownProgress("FeranaDamage");
        }
        if (_feranaUI[3] != null)
        {
            feranaCC.fillAmount = _timersRef.GetCooldownProgress("FeranaCC");
        }
        if (_feranaUI[4] != null)
        {
            feranaUtility.fillAmount = _timersRef.GetCooldownProgress("FeranaUtility");
        }
    }
}
