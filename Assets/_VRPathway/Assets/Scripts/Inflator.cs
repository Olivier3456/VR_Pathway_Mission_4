using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Inflator : XRGrabInteractable
{
    [Header("Ballon Data")]
    public Transform attachPoint;
    public Balloon balloonPrefab;

    private Balloon m_BalloonInstance;

    private XRBaseController m_controller;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        m_BalloonInstance = Instantiate(balloonPrefab, attachPoint);


        // m_controller = args.interactorObject.transform.gameObject.GetComponent<XRBaseController>();


        var controllerInteractor = args.interactorObject as XRBaseControllerInteractor;
        m_controller = controllerInteractor.xrController;



        //Debug.Log(m_controller);
        //m_controller.SendHapticImpulse(1, 0.5f);
        
    }




    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (isSelected && m_controller!= null)
        {
            m_BalloonInstance.transform.localScale = Vector3.one * Mathf.Lerp(1.0f, 4.0f, m_controller.activateInteractionState.value);
        }


    }



    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        Destroy(m_BalloonInstance.gameObject);
    }


}