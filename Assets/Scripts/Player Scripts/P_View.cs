using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_View
{
    P_Behaviour _playerScript;

    GameObject _markusGO;
    GameObject _feranaGO;

    SpriteRenderer _markusRenderer;
    SpriteRenderer _feranaRenderer;

    Animator _feranaAnimator;
    Animator _markusAnimator;

    MaterialTintColor _tintMarkus;
    MaterialTintColor _tintFerana;


    public P_View(P_Behaviour playerRef ,GameObject markusRef, GameObject feranaRef)
    {
        _playerScript = playerRef;
        _markusGO = markusRef;
        _feranaGO = feranaRef;

        _markusAnimator = _markusGO.GetComponent<Animator>();
        _feranaAnimator = _feranaGO.GetComponent<Animator>();

        _markusRenderer = _markusGO.GetComponent<SpriteRenderer>();
        _feranaRenderer = _feranaGO.GetComponent<SpriteRenderer>();
    }

    public void SwitchToMarkus()
    {
        _markusGO.SetActive(true);
        _feranaGO.SetActive(false);
    }
    public void SwitchToFerana()
    {
        _markusGO.SetActive(false);
        _feranaGO.SetActive(true);
    }

    public void SetAnimations(bool IsRunning, float dirX, float dirY)
    {
        if(_markusAnimator != null)
        {
            _markusAnimator.SetFloat("X", dirX);
            _markusAnimator.SetFloat("Y", dirY);
            _markusAnimator.SetBool("IsRunning", IsRunning);
        }
        if(_feranaAnimator != null)
        {
            _feranaAnimator.SetFloat("X", dirX);
            _feranaAnimator.SetFloat("Y", dirY);
            _feranaAnimator.SetBool("IsRunning", IsRunning);
        }

    }

    #region Animation Triggers
    public void TriggerFeranaAttack()
    {
        _feranaAnimator.SetBool("IsAttacking",true);
        _playerScript.AnimationCoroutineManage();
    }
    public void TriggerFeranaSlam()
    {
        TriggerFeranaAttack();
        _feranaAnimator.SetTrigger("Slam");
    }
    public void TriggerFeranaSlash()
    {
        TriggerFeranaAttack();
        _feranaAnimator.SetTrigger("Slash");
    }
    public void TriggerFeranaBasic()
    {
        TriggerFeranaAttack();
        _feranaAnimator.SetTrigger("Basic");
    }
    public void TriggerFeranaDagger()
    {
        TriggerFeranaAttack();
        _feranaAnimator.SetTrigger("Dagger");
    }
    #endregion
    public void FlipRenderer(Direction lookingDir)
    {
        if (lookingDir == Direction.Left)
        {
            _markusRenderer.flipX = false;
            _feranaRenderer.flipX = false;
        }
        if (lookingDir == Direction.Right)
        {
            _markusRenderer.flipX = true;
            _feranaRenderer.flipX = true;
        }
    }

    public void TintCharacter(Color colorChange)
    {
        if(_playerScript.isMarkus == false)
        {
            if (_tintFerana == null)
            {
                _tintFerana = _feranaGO.GetComponent<MaterialTintColor>();
                _tintFerana.SetTintColor(colorChange);
            }
            else
            {
                _tintFerana.SetTintColor(colorChange);
            }
        }

        else
        {
            if (_tintMarkus == null)
            {
                _tintMarkus = _markusGO.GetComponent<MaterialTintColor>();
                _tintMarkus.SetTintColor(colorChange);
                Debug.Log("Markus change Color");
            }
            else
            {
                _tintMarkus.SetTintColor(colorChange);
                Debug.Log("Markus change Color");
            }
        }
        
    }

    public IEnumerator ResetAttackBool()
    {
        yield return new WaitForSeconds(.5f);
        _feranaAnimator.SetBool("IsAttacking",false);

    }
}
