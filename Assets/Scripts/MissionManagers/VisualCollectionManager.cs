using UnityEngine;

public abstract class VisualCollectionManager : BaseMissionManager
{
    [Header("Visual Elements")]
    [SerializeField] protected GameObject[] VisualObjects;

    public override void ResetState()
    {
        base.ResetState();

        foreach (var obj in VisualObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    public override void AddItem()
    {
        if (CurrentCount < VisualObjects.Length && VisualObjects[CurrentCount] != null)
        {
            VisualObjects[CurrentCount].SetActive(true);
        }

        base.AddItem();
    }
}