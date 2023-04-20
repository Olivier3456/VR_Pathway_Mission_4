using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Scanner : XRGrabInteractable
{
    [Header("Scanner Data")]
    public Animator animator;
    public LineRenderer laserRenderer;


    protected override void Awake()
    {
        base.Awake();
        laserRenderer.gameObject.SetActive(false);
    }



    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        animator.SetBool("Opened", true);
    }


    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        animator.SetBool("Opened", false);
    }


    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        laserRenderer.gameObject.SetActive(true);
    }


    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        base.OnDeactivated(args);
        laserRenderer.gameObject.SetActive(false);
    }

}
