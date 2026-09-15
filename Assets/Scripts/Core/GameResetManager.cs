using System.Collections.Generic;
using UnityEngine;

public class GameResetManager : MonoBehaviour
{
    public static GameResetManager Instance { get; private set; }

    private readonly List<IResettable> _resettables = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Register(IResettable resettable)
    {
        if (resettable == null || _resettables.Contains(resettable))
        {
            return;
        }

        _resettables.Add(resettable);
    }

    public void Unregister(IResettable resettable)
    {
        if (resettable == null)
        {
            return;
        }

        _resettables.Remove(resettable);
    }

    public void ResetAll()
    {
        foreach (var resettable in _resettables)
        {
            if (resettable != null)
            {
                resettable.ResetState();
            }
        }

        ScoreManager.Instance?.ResetScore();
    }
}