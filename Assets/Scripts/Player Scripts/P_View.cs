using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_View
{
    GameObject _markus;
    GameObject _ferana;

    SpriteRenderer _markusRenderer;
    SpriteRenderer _feranaRenderer;

    Animator _feranaAnimator;
    Animator _markusAnimator;


    public P_View(P_Behaviour playerRef ,GameObject markusRef, GameObject feranaRef)
    {
        _markus = markusRef;
        _ferana = feranaRef;

        _markusAnimator = _markus.GetComponent<Animator>();
        _feranaAnimator = _ferana.GetComponent<Animator>();

        _markusRenderer = _markus.GetComponent<SpriteRenderer>();
        _feranaRenderer = _ferana.GetComponent<SpriteRenderer>();
    }

    public void SwitchToMarkus()
    {
        _markus.SetActive(true);
        _ferana.SetActive(false);
    }
    public void SwitchToFerana()
    {
        _markus.SetActive(false);
        _ferana.SetActive(true);
    }

    public void SetAnimations(bool IsRunning, float dirX, float dirY)
    {
        _markusAnimator.SetFloat("X", dirX);
        _markusAnimator.SetFloat("Y", dirY);
        _feranaAnimator.SetFloat("X", dirX);
        _feranaAnimator.SetFloat("Y", dirY);
        _markusAnimator.SetBool("IsRunning", IsRunning);
        _feranaAnimator.SetBool("IsRunning", IsRunning);
    }
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
}
