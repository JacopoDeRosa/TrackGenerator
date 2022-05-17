using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    [SerializeField] private Transform _connector;
    [SerializeField] private LineRenderer _cable;
    [SerializeField] private float _range;


    public Transform Connector { get => _connector; }

    public void Init()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _range, transform.forward);
        List<Pole> poles = new List<Pole>(FilterPoles(hits));
        Pole closestPole = null;
        foreach (var pole in poles)
        {
            if(closestPole == null)
            {
                closestPole = pole;
            }
            else if(Vector3.Distance(pole.transform.position, transform.position) < Vector3.Distance(closestPole.transform.position, transform.position))
            {
                closestPole = pole;
            }
        }
        if(closestPole != null)
        {
            Vector3 pos = transform.InverseTransformPoint(closestPole.Connector.position);
            _cable.SetPosition(1, pos);
        }
        else
        {
            _cable.gameObject.SetActive(false);
        }
    }

    private IEnumerable<Pole> FilterPoles(RaycastHit[] hits)
    {
        foreach (var hit in hits)
        {
           var pole = hit.collider.gameObject.GetComponent<Pole>();
            
           if(pole != null)
           {
                if (pole == this) continue;

                yield return pole;
           }
        }
    }
}
