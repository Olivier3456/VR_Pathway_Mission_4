using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Inflator : XRGrabInteractable
{
    [Header("Ballon Data")]
    public Transform attachPoint;
    public Balloon balloonPrefab;

    public float delayBetweenTwoBalloons = 1.0f;

    private Balloon m_BalloonInstance;

    private XRBaseController m_controller;

    private float m_lastTriggerValue;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        m_BalloonInstance = Instantiate(balloonPrefab, attachPoint);


        // m_controller = args.interactorObject.transform.gameObject.GetComponent<XRBaseController>();


        var controllerInteractor = args.interactorObject as XRBaseControllerInteractor;
        m_controller = controllerInteractor.xrController;
    }




    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (isSelected && m_controller != null && m_BalloonInstance != null)
        {
            m_BalloonInstance.transform.localScale = Vector3.one *
                Mathf.Lerp(1.0f, 4.0f, m_controller.activateInteractionState.value);


            // m_controller.SendHapticImpulse(m_controller.activateInteractionState.value, 0.1f);

            float vibrationIntensity = Mathf.Abs(m_controller.activateInteractionState.value - m_lastTriggerValue);
            if (vibrationIntensity > 0)
            {
                m_controller.SendHapticImpulse(vibrationIntensity * 5, 0.1f);
            }

            m_lastTriggerValue = m_controller.activateInteractionState.value;
        }
    }



    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (m_BalloonInstance != null)
        {
            Destroy(m_BalloonInstance.gameObject);
        }


        //Debug.Log("[Inflator] : OnSelectExited");
    }



    public void DetachBalloon()
    {
        if (m_BalloonInstance != null)
        {
            m_BalloonInstance.Detach();
            //m_BalloonInstance = Instantiate(balloonPrefab, attachPoint);
            m_BalloonInstance = null;
            StartCoroutine(DelayBeforeNextBalloon());
        }

        //Debug.Log("[Inflator]: DetachBalloon()");
    }



    private IEnumerator DelayBeforeNextBalloon()
    {
        yield return new WaitForSeconds(delayBetweenTwoBalloons);
        m_BalloonInstance = Instantiate(balloonPrefab, attachPoint);
    }
}