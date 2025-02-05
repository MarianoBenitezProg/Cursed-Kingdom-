using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsTriggers : MonoBehaviour
{
    [SerializeField]Animator ui_animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            ui_animator.SetBool("IsTrigger", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            ui_animator.SetBool("IsTrigger", false);
        }
    }
}
