using UnityEngine;

public abstract class VisualCollectionManager : BaseMissionManager
{
    [Header("Visual Elements")]
    [SerializeField] protected GameObject[] _visualObjects;

    public override void ResetState()
    {
        base.ResetState();

        foreach (var obj in _visualObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    public override void AddItem()
    {
        if (_currentCount < _visualObjects.Length && _visualObjects[_currentCount] != null)
        {
            _visualObjects[_currentCount].SetActive(true);
        }

        base.AddItem();
    }
}