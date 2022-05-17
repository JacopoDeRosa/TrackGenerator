using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailSegment : MonoBehaviour
{
    [SerializeField] private Transform _dock;
    [SerializeField] private Transform _poleDock;
    [SerializeField] private Transform _stationDock;
    [SerializeField] protected GameObject _plank;
    [SerializeField] protected List<GameObject> _activePlanks;


    public Transform Dock { get => _dock; }
    public Transform PoleDock { get => _poleDock; }
    public Transform StationDock { get => _stationDock; }

    public virtual void Init(RailGenerator parent)
    {
        ClearActivePlanks();
        SpawnPlanks(parent.PlanksPerSegment);
    }

    protected void ClearActivePlanks()
    {
        foreach (var plank in _activePlanks)
        {
            if (plank == null) continue;
            DestroyImmediate(plank);
        }
        _activePlanks.Clear();
    }

    protected virtual void SpawnPlanks(int amount)
    {
        List<Vector3> positions = new List<Vector3>(GetPositions(amount, 2));
        foreach (var pos in positions)
        {
           var plank = Instantiate(_plank, transform);
           plank.transform.localPosition = pos;
           _activePlanks.Add(plank);
        }
    }

    // Used by this class to get the positions
    protected virtual IEnumerable<Vector3> GetPositions(int amount, float lenght)
    {
        float step = lenght / amount;
        for (int i = 1; i <= amount; i++)
        {

                yield return new Vector3(0, 0, step * i) - new Vector3(0, 0, step / 2);
        }
    }
}
