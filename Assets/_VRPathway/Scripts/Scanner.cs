using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Scanner : XRGrabInteractable
{
    [Header("Scanner Data")]
    public Animator animator;
    public LineRenderer laserRenderer;


    private AudioSource audioSource;

    public AudioClip grabbedSound;
    public AudioClip throwedSound;
    public AudioClip activatedSound;


    protected override void Awake()
    {
        base.Awake();
        laserRenderer.gameObject.SetActive(false);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        animator.SetBool("Opened", true);
        audioSource.PlayOneShot(grabbedSound);

        args.interactorObject.transform.gameObject.GetComponent<XRBaseControllerInteractor>().SendHapticImpulse(1, 0.2f);
    }


    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        animator.SetBool("Opened", false);
        audioSource.PlayOneShot(throwedSound);
    }


    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        laserRenderer.gameObject.SetActive(true);
        audioSource.clip = activatedSound;
        audioSource.Play();
    }


    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        base.OnDeactivated(args);
        laserRenderer.gameObject.SetActive(false);
        audioSource.Stop();
    }

}
