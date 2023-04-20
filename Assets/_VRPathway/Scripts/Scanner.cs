using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Scanner : XRGrabInteractable
{
    [Header("Scanner Data")]
    public Animator animator;

  
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        animator.SetBool("Opened", true);
    }
}
