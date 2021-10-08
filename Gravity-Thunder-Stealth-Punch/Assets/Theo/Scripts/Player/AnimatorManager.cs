using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Rigidbody rb;
    public Animator animator;
   
   
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
      
    }

    public void PlayTargetAnimation(string targetAnimation) 
    {
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void UpdateJumpBlendValue(float yVel)
    {
        animator.SetFloat("yVelocity", yVel);
    }

       
    public void UpdateWalkBlendValue(float moveAmount)
    {
       
        animator.SetFloat("SpeedPercentage", moveAmount, 0.2f,Time.deltaTime);
    }
}

        
       

       



