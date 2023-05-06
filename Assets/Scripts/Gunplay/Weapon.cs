using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Detection
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] protected GunData gunData;
        protected XRGrabInteractable weapon;
        private float intensity = 0f;
        private float duration = 0f;
        private XRBaseControllerInteractor controller = null;
        protected AttackerType attackerType { get; private set; } = AttackerType.Enemy;

        private void Awake()
        {
            weapon = GetComponent<XRGrabInteractable>();
            SetupInteractions();
        }

        protected void SetupInteractions()
        {
            weapon.activated.AddListener(StartAttacking);
            weapon.deactivated.AddListener(StopAttacking);
            weapon.selectEntered.AddListener(Grab);
        }

        private void Grab(SelectEnterEventArgs args)
        {
            attackerType = AttackerType.Player;
        }

        protected virtual void StartAttacking(ActivateEventArgs args)
        {

        }

        protected virtual void StopAttacking(DeactivateEventArgs args)
        {

        }

        protected void SetHapticIntensityDuration(float intensity, float duration)
        {
            this.intensity = intensity;
            this.duration = duration;
        }

        protected virtual void ActivateHapticFeedback()
        {
            if (weapon.isSelected)
            {
                controller = weapon.interactorsSelecting[0] as XRBaseControllerInteractor;
                controller.SendHapticImpulse(intensity, duration);
            }
        }
    }
}