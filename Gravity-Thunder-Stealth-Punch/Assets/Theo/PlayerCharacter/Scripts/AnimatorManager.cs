using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    int horizontal, vertical;
   
    private void Awake()
    {
       
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting) 
    {
        animator.SetBool("IsInteracting", isInteracting);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void UpdateAnimatorValues(float moveAmount)
    {
        animator.SetFloat(horizontal, moveAmount, .15f, Time.deltaTime);
        animator.SetFloat(vertical, moveAmount, .15f, Time.deltaTime);
    }
       
}

       



