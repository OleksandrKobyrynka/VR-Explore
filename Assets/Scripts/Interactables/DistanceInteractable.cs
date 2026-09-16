using System;
using UnityEngine;

public abstract class DistanceInteractable : BaseInteractableFeedback
{
    [Header("Distance Settings")]
    [SerializeField] protected float ActivationDistance = 2.0f;

    [Header("Reward Settings")]
    [SerializeField] protected int ScoreValue = 1;

    public event Action PerformedEvent;

    private Transform _cachedPlayerTransform;

    protected Transform PlayerTransform
    {
        get
        {
            if (_cachedPlayerTransform == null && Camera.main != null)
            {
                _cachedPlayerTransform = Camera.main.transform;
            }
            return _cachedPlayerTransform;
        }
    }

    public abstract void ResetInteractable();

    protected bool IsInRange()
    {
        if (PlayerTransform == null)
        {
            return false;
        }

        float sqrDistance = (transform.position - PlayerTransform.position).sqrMagnitude;
        float sqrActivation = ActivationDistance * ActivationDistance;

        return sqrDistance <= sqrActivation;
    }

    protected virtual void CompleteInteraction()
    {
        ScoreManager.Instance.AddScore(ScoreValue);
        PerformedEvent?.Invoke();
        gameObject.SetActive(false);
    }
}