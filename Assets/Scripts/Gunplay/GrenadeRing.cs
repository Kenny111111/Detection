using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

namespace Detection
{
    public class GrenadeRing : XRGrabInteractable
    {

        [HideInInspector] public bool isConnected = true;
        private Rigidbody ringBody;

        protected override void Awake()
        {
            base.Awake();
            selectEntered.AddListener(PullPin);
            selectExited.AddListener(DropPin);
            ringBody = GetComponent<Rigidbody>();
            ringBody.isKinematic = true;
        }

        private void PullPin(SelectEnterEventArgs args)
        {
            if (args.interactorObject is XRSocketInteractor) return;

            isConnected = false;
            AudioSystem.instance.Play("grenade_pin");
        }

        private void DropPin(SelectExitEventArgs args)
        {
            if (args.interactorObject is XRSocketInteractor) return;
            ringBody.isKinematic = false;

            Destroy(gameObject, 2f);
        }
    }
}