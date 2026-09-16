using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class BaseMissionManager : MonoBehaviour, IResettable
{
    [Header("Mission Elements")]
    [SerializeField] protected DistanceInteractable[] Interactables;

    [Header("Base Audio")]
    [SerializeField] protected AudioClip ProgressSound;
    [SerializeField] protected AudioClip CompleteSound;

    protected AudioSource AudioSource;
    protected int CurrentCount;

    protected virtual void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        AudioSource.playOnAwake = false;
    }

    protected virtual void OnEnable()
    {
        SubscribeEvents();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeEvents();
    }

    protected virtual void Start()
    {
        GameResetManager.Instance?.Register(this);
        ResetState();
    }

    protected virtual void OnDestroy()
    {
        GameResetManager.Instance?.Unregister(this);
    }

    public virtual void ResetState()
    {
        CurrentCount = 0;
        if (AudioSource != null)
        {
            AudioSource.Stop();
        }

        foreach (var item in Interactables)
        {
            if (item != null)
            {
                item.ResetInteractable();
            }
        }
    }

    public virtual void AddItem()
    {
        if (CurrentCount >= Interactables.Length)
        {
            return;
        }

        if (ProgressSound != null && AudioSource != null)
        {
            AudioSource.PlayOneShot(ProgressSound);
        }

        CurrentCount++;

        if (CurrentCount == Interactables.Length)
        {
            CompleteMission();
        }
    }

    protected virtual void SubscribeEvents()
    {
        foreach (var item in Interactables)
        {
            if (item != null)
            {
                item.PerformedEvent += AddItem;
            }
        }
    }

    protected virtual void UnsubscribeEvents()
    {
        foreach (var item in Interactables)
        {
            if (item != null)
            {
                item.PerformedEvent -= AddItem;
            }
        }
    }

    protected virtual void CompleteMission()
    {
        if (CompleteSound != null && AudioSource != null)
        {
            AudioSource.PlayOneShot(CompleteSound);
        }
    }
}