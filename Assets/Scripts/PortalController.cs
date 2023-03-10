using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField]
    GameObject mask;
    [SerializeField]
    GameObject leaveCollider;

    public bool IsOpen { get; private set; }

    Animator animator;
    bool initiated;
    bool isOpening;
    bool isClosing;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        if (initiated)
            return;

        animator = GetComponent<Animator>();
        mask.SetActive(false);
        leaveCollider.SetActive(false);
        initiated = true;
    }

    private void Update()
    {
        if (isOpening && animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            isOpening = false;
            IsOpen = true;
            leaveCollider.SetActive(true);
        }
        if (isClosing && animator.GetCurrentAnimatorStateInfo(0).IsName("Closed"))
        {
            isClosing = false;
            IsOpen = false;
            mask.SetActive(false);
        }
    }

    public void Open()
    {
        animator.SetBool("IsOpen", true);
        isOpening = true;
        isClosing = false;
        mask.SetActive(true);
    }

    public void OpenInstant()
    {
        Init();
        animator.SetBool("IsOpen", true);
        animator.SetTrigger("OpenInstant");
        IsOpen = true;
        mask.SetActive(true);
        leaveCollider.SetActive(true);
    }

    public void Close()
    {
        animator.SetBool("IsOpen", false);
        isOpening = false;
        isClosing = true;
        leaveCollider.SetActive(false);
    }
}
