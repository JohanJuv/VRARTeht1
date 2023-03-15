using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TelekinesisRight : MonoBehaviour
{
    public XRRayInteractor interactor;
    public InputActionReference telekinesisReference = null;
    public float turnThreshold;
    private bool levitating = false;
    private IXRSelectInteractable interactable;
    private float distance = 0f;
    private void Awake()
    {
        interactor = GetComponent<XRRayInteractor>();
    }
    private void Update()
    {
        if (levitating == false)
        {
            interactor.TryGetCurrent3DRaycastHit(out RaycastHit res);
            if (res.collider != null && res.collider.gameObject.GetComponent<IXRSelectInteractable>() != null)
            {
                interactable = res.collider.gameObject.GetComponent<IXRSelectInteractable>();
                if (interactable.IsSelectableBy(interactor))
                {
                    float value = telekinesisReference.action.ReadValue<float>();
                    if (telekinesisReference.action.ReadValue<float>() > turnThreshold)
                    {
                        distance = res.distance;
                        levitating = true;
                    }
                }
            }

        }
        else if (telekinesisReference.action.ReadValue<float>() < -1 * turnThreshold)
        {
            Debug.Log("No longer levitating");
            levitating = false;
        }
        else
        {
            interactor.TryGetCurrent3DRaycastHit(out RaycastHit res);
            if (res.collider.gameObject.GetComponent<IXRSelectInteractable>() != null && res.collider.gameObject.GetComponent<IXRSelectInteractable>().Equals(interactable))
            {
                Vector3 calculatedDistance = res.point.normalized;
                calculatedDistance *= distance;
                interactable.transform.position = calculatedDistance;
            }
            else
            {
                levitating = false;
            }
            
        }
    
    }
}

