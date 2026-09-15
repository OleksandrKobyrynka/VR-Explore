using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FlyingInteractable : DistanceInteractable
{
    [Header("Flying Settings")]
    [SerializeField] private float _flySpeed = 5.0f;
    [SerializeField] private float _arrivalDistance = 0.2f;

    [Header("Target Offset")]
    [SerializeField] private Vector3 _targetOffset = new Vector3(0f, -0.3f, 0f);

    private bool _isFlying = false;
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    protected override void Awake()
    {
        base.Awake();
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

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
        if (IsInRange() && !_isFlying)
        {
            base.OnHoverEntered(args);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (IsInRange() && !_isFlying)
        {
            StartCoroutine(FlyToPlayerRoutine());
        }
    }

    private IEnumerator FlyToPlayerRoutine()
    {
        _isFlying = true;

        if (_interactable != null)
        {
            _interactable.enabled = false;
        }

        Transform targetCamera = PlayerTransform;

        if (targetCamera == null)
        {
            yield break;
        }

        float sqrArrival = _arrivalDistance * _arrivalDistance;

        while (true)
        {
            Vector3 targetPosition = targetCamera.TransformPoint(_targetOffset);

            if ((transform.position - targetPosition).sqrMagnitude <= sqrArrival)
            {
                break;
            }

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                _flySpeed * Time.deltaTime
            );

            yield return null;
        }

        CompleteInteraction();
    }

    public override void ResetInteractable()
    {
        StopAllCoroutines();
        _isFlying = false;

        transform.SetPositionAndRotation(_startPosition, _startRotation);

        if (_interactable != null)
        {
            _interactable.enabled = true;
        }

        gameObject.SetActive(true);
    }
}