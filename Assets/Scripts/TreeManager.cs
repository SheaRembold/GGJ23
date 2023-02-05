using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [SerializeField]
    PortalController[] portals;
    [SerializeField]
    Animator[] rootEndings;

    Animator animator;
    bool isGrowing;
    bool isExtending;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (GameManager.Instance.CurrentRoot > 0)
        {
            animator.SetInteger("Stage", GameManager.Instance.CurrentRoot * 2);
            animator.SetTrigger("Next");
            for (int i = 0; i < GameManager.Instance.CurrentRoot; i++)
            {
                portals[i].OpenInstant();
                if (i < GameManager.Instance.CurrentRoot - 1)
                    rootEndings[i].SetInteger("State", 2);
                else
                    rootEndings[i].SetInteger("State", 1);
            }
            isGrowing = true;
        }
    }

    private void Update()
    {
        if (isGrowing && animator.GetCurrentAnimatorStateInfo(0).IsName("stage" + (GameManager.Instance.CurrentRoot * 2 + 1)))
        {
            isGrowing = false;
        }
        if (isExtending && animator.GetCurrentAnimatorStateInfo(0).IsName("stage" + ((GameManager.Instance.CurrentRoot + 1) * 2)))
        {
            GameManager.Instance.StartNextRoot();
        }
        if (!isGrowing && !isExtending && GameManager.Instance.CurrentRoot < portals.Length && Input.GetButtonDown("Jump"))
        {
            portals[GameManager.Instance.CurrentRoot].Open();
            animator.SetTrigger("Next");
            isExtending = true;
        }
    }
}
