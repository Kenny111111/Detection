using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class Weapon : MonoBehaviour
{
    [SerializeField] protected GunData gunData;
    protected XRGrabInteractable weapon;
    private float intensity = 0f;
    private float duration = 0f;
    private XRBaseControllerInteractor controller = null;

    private void Awake()
    {
        weapon = GetComponent<XRGrabInteractable>();
        SetupInteractions();
    }

    protected void SetupInteractions()
    {
        weapon.activated.AddListener(StartAttacking);
        weapon.deactivated.AddListener(StopAttacking);
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
        if(weapon.isSelected)
        {
            controller = weapon.interactorsSelecting[0] as XRBaseControllerInteractor;
            controller.SendHapticImpulse(intensity, duration);
        }
    }
}
