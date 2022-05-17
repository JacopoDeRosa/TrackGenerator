using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedRailSegment : RailSegment
{
    [SerializeField] private Transform _rotator;
    protected override void SpawnPlanks(int amount)
    {
        amount = amount * 2;
        _rotator.localRotation = Quaternion.identity;

        float angle = 45 / amount;
     // _rotator.Rotate(0, angle * 2, 0, Space.Self);

        for (int i = 0; i < amount; i++)
        {
            var plank = Instantiate(_plank, transform);
            plank.transform.localPosition = Vector3.zero;
            plank.transform.parent = _rotator;
            if (i == amount-1)
            {
                _rotator.Rotate(0, -angle / 2, 0, Space.Self);
            }
            else
            {
                _rotator.Rotate(0, -angle, 0, Space.Self);
            }
            _activePlanks.Add(plank);
        }
        
    }
}
