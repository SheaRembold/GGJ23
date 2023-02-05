using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 1f;

    public bool IsLanding { get; private set; } = true;
    public bool IsLeaving { get; private set; }
    public bool IsHitting { get; private set; }

    Animator animator;
    Rigidbody2D _rigidbody;
    List<BubbleController> touchedBubbles = new List<BubbleController>();
    Vector2 moveDir;
    bool wasHitting;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (IsLanding && animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            IsLanding = false;
        }
        if (IsLeaving && animator.GetCurrentAnimatorStateInfo(0).IsName("leave") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            IsLeaving = false;
        }
        if (IsHitting)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("hit"))
            {
                wasHitting = true;
            }
            else if (wasHitting)
            {
                IsHitting = false;
            }
        }

        if (LevelManager.Instance.IsCutscene || IsHitting)
        {
            moveDir = Vector2.zero;
            animator.SetFloat("Speed", 0f);
            return;
        }

        if (touchedBubbles.Count > 0 && Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("Hit");
            IsHitting = true;
            wasHitting = false;
            moveDir = Vector2.zero;
            animator.SetFloat("Speed", 0f);
            return;
        }

        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        animator.SetFloat("Speed", moveDir.magnitude);

        if (moveDir.x < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (moveDir.x > 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void FixedUpdate()
    {
        touchedBubbles.Clear();

        if (moveDir != Vector2.zero)
            _rigidbody.MovePosition(_rigidbody.position + moveDir * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PortalController portal = collision.GetComponentInParent<PortalController>();
        if (portal != null)
        {
            if (!LevelManager.Instance.IsCutscene)
            {
                LevelManager.Instance.EnterPortal();
            }
            return;
        }

        BubbleController bubble = collision.GetComponentInParent<BubbleController>();
        if (bubble != null)
        {
            if (!touchedBubbles.Contains(bubble))
                touchedBubbles.Add(bubble);
            return;
        }
    }

    public void Hit()
    {
        for (int i = 0; i < touchedBubbles.Count; i++)
        {
            touchedBubbles[i].Hit();
        }
    }

    public void Leave()
    {
        animator.SetTrigger("Leave");
        IsLeaving = true;
    }
}
