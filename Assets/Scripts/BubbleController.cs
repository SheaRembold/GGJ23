using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    [SerializeField]
    GameObject hitCollider;

    Animator animator;
    bool hasCollected;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Hit()
    {
        animator.SetTrigger("Hit");
        hitCollider.SetActive(false);
    }

    private void Update()
    {
        if (!hasCollected && animator.GetCurrentAnimatorStateInfo(0).IsName("Break"))
        {
            hasCollected = true;
            LevelManager.Instance.CollectWater();
        }
        if (hasCollected && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
