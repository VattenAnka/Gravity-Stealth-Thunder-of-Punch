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
        //Animation snapping
        /* float snappedHorizontal;
         float snappedVertical;
        */
        #region Snapped Horizontal
        /* if (horizontalMovement>0 && horizontalMovement < 0.55f)
         {
             snappedHorizontal = 0.5f;
         }
         else if (horizontalMovement > 0.55f)
         {
             snappedHorizontal = 1;
         }
         else if( horizontalMovement<0 && horizontalMovement > -0.55f)
         {
             snappedHorizontal = -0.5f;
         }
         else if(horizontalMovement < -0.55f)
         {
             snappedHorizontal = -1;
         }
         else
         {
             snappedHorizontal = 0;
         }*/
        #endregion
        #region Snapped Vertical
        /*if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }*/
        #endregion

        animator.SetFloat(horizontal, moveAmount, .15f, Time.deltaTime);
        animator.SetFloat(vertical, moveAmount, .15f, Time.deltaTime);
        //if (moveAmount == 1f) animator.speed = 1.2f;
        //else animator.speed = 1;
    }
}



