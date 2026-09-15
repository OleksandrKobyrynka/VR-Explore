using System;
using UnityEngine;

public abstract class DistanceInteractable : BaseInteractableFeedback
{
    [Header("Distance Settings")]
    [SerializeField] protected float _activationDistance = 2.0f;

    [Header("Reward Settings")]
    [SerializeField] protected int _scoreValue = 1;

    public event Action OnPerformed;

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

    protected bool IsInRange()
    {
        if (PlayerTransform == null)
        {
            return false;
        }

        float sqrDistance = (transform.position - PlayerTransform.position).sqrMagnitude;
        float sqrActivation = _activationDistance * _activationDistance;

        return sqrDistance <= sqrActivation;
    }

    protected virtual void CompleteInteraction()
    {
        ScoreManager.Instance.AddScore(_scoreValue);
        OnPerformed?.Invoke();
        gameObject.SetActive(false);
    }

    public abstract void ResetInteractable();
}