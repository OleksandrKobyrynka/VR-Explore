using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisappearingInteractable : DistanceInteractable
{
    [Header("Disappearing Settings")]
    [SerializeField] private int _scoreValue = 1;

    public event Action OnCollected;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_interactable != null)
        {
            _interactable.selectEntered.AddListener(OnSelectEntered);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (_interactable != null)
        {
            _interactable.selectEntered.RemoveListener(OnSelectEntered);
        }
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (IsInRange())
        {
            base.OnHoverEntered(args);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (IsInRange())
        {
            ScoreManager.Instance.AddScore(_scoreValue);
            OnCollected?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
    }
}