using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Scanner : XRGrabInteractable
{
    [Header("Scanner Data")]
    public Animator animator;
    public LineRenderer laserRenderer;
    public TextMeshProUGUI targetName;
    public TextMeshProUGUI targetPosition;
    
    private AudioSource audioSource;
    [Space(10)]
    public AudioClip grabbedSound;
    public AudioClip throwedSound;
    public AudioClip activatedSound;


    protected override void Awake()
    {
        base.Awake();        
        ScannerActivated(false);
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
        Debug.Log(args.interactorObject.transform.name + " a vibré.");
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
        audioSource.clip = activatedSound;
        audioSource.Play();
        ScannerActivated(true);
        ScanForObjects();
    }


    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        base.OnDeactivated(args);       
        audioSource.Stop();
        ScannerActivated(false);
    }


    private void ScannerActivated(bool isActivated)
    {
        laserRenderer.gameObject.SetActive(isActivated);
        targetName.gameObject.SetActive(isActivated);
        targetPosition.gameObject.SetActive(isActivated);
    }


    private void ScanForObjects()
    {
        RaycastHit hit;
        if (Physics.Raycast(laserRenderer.transform.position, laserRenderer.transform.forward, out hit))
        {
            targetName.SetText(hit.collider.name);
            targetPosition.SetText(hit.collider.transform.position.ToString());
        }
    }

}
