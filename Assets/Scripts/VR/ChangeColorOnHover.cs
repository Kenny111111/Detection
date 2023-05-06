using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Detection
{
    public class ChangeColorOnHover : MonoBehaviour
    {
        public Color desiredColorOnHover;
        private Color startingHandColor;

        private XRDirectInteractor directInteractor;
        private Outline handOutline;


        void Start()
        {
            directInteractor = gameObject.GetComponent<XRDirectInteractor>();
            handOutline = gameObject.GetComponent<Outline>();

            startingHandColor = handOutline.OutlineColor;

            directInteractor.hoverEntered.AddListener(ChangeColorOnHoverEnter);
            directInteractor.hoverExited.AddListener(ChangeColorOnHoverExit);
        }

        private void ChangeColorOnHoverEnter(HoverEnterEventArgs arg0)
        {
            handOutline.OutlineColor = desiredColorOnHover;

            //Outline weaponOutline = arg0.interactableObject.transform.GetComponentInChildren<Outline>();

            //startingWeaponColor = weaponOutline.OutlineColor;
            //weaponOutline.OutlineColor = desiredColorOnHover;
        }

        private void ChangeColorOnHoverExit(HoverExitEventArgs arg0)
        {
            handOutline.OutlineColor = startingHandColor;
            //arg0.interactableObject.transform.GetComponentInChildren<Outline>().OutlineColor = startingWeaponColor;
        }
    }
}
