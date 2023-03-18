using UnityEngine.XR.Interaction.Toolkit;


public class PrimaryHold : BaseHold
{
    protected override void StartAction(ActivateEventArgs args)
    {
        ObjectHeld.StartObjectAction();
    }

    protected override void StopAction(DeactivateEventArgs args)
    {
        ObjectHeld.StopObjectAction();
    }

    protected override void Grab(SelectEnterEventArgs args)
    {
        base.Grab(args);
        ObjectHeld.SetPrimaryHand(args);

        if (args.interactorObject is XRRayInteractor)
        {
            XRDirectInteractor interactor = args.interactorObject.transform.GetComponentInChildren<XRDirectInteractor>();
            interactor.gameObject.SetActive(false);
        }
        else
        {
            XRRayInteractor interactor = args.interactorObject.transform.GetComponentInChildren<XRRayInteractor>();
            interactor.gameObject.SetActive(false);
        }
    }

    protected override void Drop(SelectExitEventArgs args)
    {
        base.Drop(args);
        ObjectHeld.ClearPrimaryHand(args);

        if (args.interactorObject is XRRayInteractor)
        {
            XRDirectInteractor interactor = args.interactorObject.transform.GetComponentInChildren<XRDirectInteractor>();
            interactor.gameObject.SetActive(true);
        }
        else
        {
            XRRayInteractor interactor = args.interactorObject.transform.GetComponentInChildren<XRRayInteractor>();
            interactor.gameObject.SetActive(true);
        }
    }
}
