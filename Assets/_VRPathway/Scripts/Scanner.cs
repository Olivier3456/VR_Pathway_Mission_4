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
    public TextMeshProUGUI targetDistance;
    public TextMeshProUGUI targetSize;

    private AudioSource audioSource;
    [Space(10)]
    public AudioClip grabbedSound;
    public AudioClip throwedSound;
    public AudioClip activatedSound;
    [Space(10)]
    public Material hitObjectMaterial;
    private Material hitObjectOriginalMaterial;

    private Transform lastObjectHit;

    protected override void Awake()
    {
        base.Awake();
        ScannerActivated(false);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (laserRenderer.gameObject.activeSelf)
        {
            ScanForObjects();
        }
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
        audioSource.loop = false;
        audioSource.Stop();
        audioSource.PlayOneShot(throwedSound);
    }


    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);
        audioSource.clip = activatedSound;
        audioSource.Play();
        audioSource.loop = true;
        ScannerActivated(true);
    }


    protected override void OnDeactivated(DeactivateEventArgs args)
    {
        base.OnDeactivated(args);
        audioSource.Stop();
        audioSource.loop = false;
        ScannerActivated(false);
    }


    private void ScannerActivated(bool isActivated)
    {
        laserRenderer.gameObject.SetActive(isActivated);

        targetName.gameObject.SetActive(isActivated);
        targetPosition.gameObject.SetActive(isActivated);
        targetDistance.gameObject.SetActive(isActivated);
        targetSize.gameObject.SetActive(isActivated);

        if (!isActivated && lastObjectHit != null && lastObjectHit.transform.gameObject.GetComponent<Renderer>() != null) lastObjectHit.transform.gameObject.GetComponent<Renderer>().material = hitObjectOriginalMaterial;
        else if (lastObjectHit != null && lastObjectHit.transform.gameObject.GetComponent<Renderer>() != null) lastObjectHit.transform.gameObject.GetComponent<Renderer>().material = hitObjectMaterial;
    }


    private void ScanForObjects()
    {
        RaycastHit hit;
        Vector3 worldHit = laserRenderer.transform.position + laserRenderer.transform.forward * 1000.0f;

        if (Physics.Raycast(laserRenderer.transform.position, laserRenderer.transform.forward, out hit))
        {
            worldHit = hit.point;

            if (lastObjectHit != hit.collider.transform)
            {

                targetName.SetText(hit.collider.name);
                targetPosition.SetText(hit.collider.transform.position.ToString());

                if (hit.collider.GetComponent<MeshRenderer>() != null) targetSize.SetText("Size: " + hit.collider.GetComponent<MeshRenderer>().bounds.size);
                
                if (lastObjectHit != null && lastObjectHit.transform.gameObject.GetComponent<Renderer>() != null) lastObjectHit.transform.gameObject.GetComponent<Renderer>().material = hitObjectOriginalMaterial;

                if (hit.collider.transform.gameObject.GetComponent<Renderer>() != null)
                {
                    hitObjectOriginalMaterial = hit.collider.transform.gameObject.GetComponent<Renderer>().material;
                    hit.collider.transform.gameObject.GetComponent<Renderer>().material = hitObjectMaterial;
                }
            }

            targetDistance.SetText("Distance: " + hit.distance.ToString("0.00") + "m");

            lastObjectHit = hit.collider.transform;
        }
        else
        {
            targetName.SetText("Ready to scan");
            targetPosition.SetText("Ready to scan");
            targetDistance.SetText("Ready to scan");
            targetSize.SetText("Ready to scan");

            if (lastObjectHit != null
                && lastObjectHit.transform.gameObject.GetComponent<Renderer>() != null
                && hitObjectOriginalMaterial != null)
            {
                lastObjectHit.transform.gameObject.GetComponent<Renderer>().material = hitObjectOriginalMaterial;
            }

            lastObjectHit = null;
        }

        laserRenderer.SetPosition(1, laserRenderer.transform.InverseTransformPoint(worldHit));
    }
}
