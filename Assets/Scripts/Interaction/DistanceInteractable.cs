using UnityEngine;

public abstract class DistanceInteractable : BaseInteractableFeedback
{
    [Header("Distance Settings")]
    [SerializeField] protected float _activationDistance = 2.0f;

    private Transform _cachedPlayerTransform;

    protected bool IsInRange()
    {
        if (_cachedPlayerTransform == null)
        {
            if (Camera.main == null)
            {
                return false;
            }
            _cachedPlayerTransform = Camera.main.transform;
        }

        float distance = Vector3.Distance(transform.position, _cachedPlayerTransform.position);
        return distance <= _activationDistance;
    }
}