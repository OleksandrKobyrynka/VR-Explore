using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ThrowableInteractable : DistanceInteractable
{
    [Header("Throw Settings")]
    [SerializeField] private Transform _targetBasket;
    [SerializeField] private float _flightDuration = 1.0f;
    [SerializeField] private float _arcHeight = 1.5f;

    private bool _isThrown = false;
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    protected override void Awake()
    {
        base.Awake();
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();

        if (Interactable != null)
        {
            Interactable.selectEntered.AddListener(HandleSelectEntered);
        }
    }

    protected override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();

        if (Interactable != null)
        {
            Interactable.selectEntered.RemoveListener(HandleSelectEntered);
        }
    }

    public override void ResetInteractable()
    {
        StopAllCoroutines();
        _isThrown = false;
        transform.SetPositionAndRotation(_startPosition, _startRotation);

        if (Interactable != null)
        {
            Interactable.enabled = true;
        }

        gameObject.SetActive(true);
    }

    protected override void HandleHoverEntered(HoverEnterEventArgs args)
    {
        if (IsInRange() && !_isThrown)
        {
            base.HandleHoverEntered(args);
        }
    }

    private void HandleSelectEntered(SelectEnterEventArgs args)
    {
        if (IsInRange() && !_isThrown)
        {
            if (_targetBasket != null)
            {
                StartCoroutine(ThrowRoutine());
            }
            else
            {
                Debug.LogWarning($"{name}: Target Basket is not assigned!");
            }
        }
    }

    private IEnumerator ThrowRoutine()
    {
        _isThrown = true;

        if (Interactable != null)
        {
            Interactable.enabled = false;
        }

        Vector3 startPos = transform.position;
        Vector3 endPos = _targetBasket.position;

        Vector3 controlPoint = startPos + (endPos - startPos) / 2f;
        controlPoint.y += _arcHeight;

        float timePassed = 0f;

        while (timePassed < _flightDuration)
        {
            timePassed += Time.deltaTime;

            float t = timePassed / _flightDuration;

            transform.position = CalculateBezierPoint(t, startPos, controlPoint, endPos);
            yield return null;
        }

        CompleteInteraction();
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = uu * p0;
        point += 2f * u * t * p1;
        point += tt * p2;

        return point;
    }
}