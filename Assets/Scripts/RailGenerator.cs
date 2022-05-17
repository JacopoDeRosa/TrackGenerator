using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class RailGenerator : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private RailSegment _straight;
    [SerializeField] private RailSegment _curveL, _curveR;
    [SerializeField] private Pole _polePrefab;
    [SerializeField] private int _polesInterval;


    #region Private Variables
    [SerializeField][ReadOnly]
    private List<RailSegment> _activeRails;

    [SerializeField][ReadOnly]
    private RailSegment _lastSegment;

    [SerializeField][ReadOnly]
    private bool _editMode = false;

    [SerializeField][ReadOnly]
    private Transform _controller;

    [SerializeField][ReadOnly]
    private int _planksPerSegment;
    #endregion

    public int PlanksPerSegment { get => _planksPerSegment; }

    private void Update()
    {
        if(_editMode && _controller.hasChanged)
        {
            RedrawRails();
        }
    }

    private void RedrawRails()
    {
        DeleteCurrentRails();
        int amountOfRails = (int) (_controller.localPosition.z / 2);
        for (int i = 0; i < amountOfRails; i++)
        {          
            RailSegment rail = null;



            // Draw the first curve here
            if(i == amountOfRails - 2 && amountOfRails > 2)
            {
                if(_controller.localPosition.x < -3)
                {
                    rail = Instantiate(_curveL, _lastSegment.Dock.position, _lastSegment.Dock.rotation);
                }
                else if(_controller.localPosition.x > 3)
                {
                    rail = Instantiate(_curveR, _lastSegment.Dock.position, _lastSegment.Dock.rotation);
                }
            }
            else if(i == amountOfRails - 1 && amountOfRails > 1)
            {
                if (_controller.localPosition.x < -6)
                {
                    rail = Instantiate(_curveL, _lastSegment.Dock.position, _lastSegment.Dock.rotation);
                }
                else if (_controller.localPosition.x > 6)
                {
                    rail = Instantiate(_curveR, _lastSegment.Dock.position, _lastSegment.Dock.rotation);
                }
            }

            // If a curve isn't drawn a straight will be drawn instead
            if (rail == null)
            {
                if (_lastSegment == null)
                {
                    rail = Instantiate(_straight, transform.position, transform.rotation);
                }
                else
                {
                    rail = Instantiate(_straight, _lastSegment.Dock.position, _lastSegment.Dock.rotation);
                }
            }

            rail.Init(this);
            _lastSegment = rail;
            _activeRails.Add(rail);
        }
        DrawPoles();
    }

    private void DrawPoles()
    {
        for (int i = 0; i < _activeRails.Count; i += _polesInterval)
        {
            var segment = _activeRails[i];
            var pole = Instantiate(_polePrefab, segment.PoleDock);
            pole.Init();
        }
    }

    private void DeleteCurrentRails()
    {
        foreach (var item in _activeRails)
        {
            if (item == null) continue;
            DestroyImmediate(item.gameObject);
        }
        _activeRails.Clear();
    }

    [Button]
    public void ToggleEditMode()
    {
        if(_editMode)
        {
            if(_controller != null)
            {
                DestroyImmediate(_controller.gameObject);
                _controller = null;
            }
            _editMode = false;
        }
        else
        {
            var cont = new GameObject();
            cont.transform.parent = transform;
            cont.transform.localPosition = Vector3.zero;
            cont.transform.localRotation = Quaternion.identity;
            cont.name = "CONTROLLER [DO NOT TOUCH]";
            _controller = cont.transform;
            _editMode = true; 
            UnityEditor.ActiveEditorTracker.sharedTracker.isLocked = true;
            UnityEditor.Selection.activeObject = cont;
      

        }
    }

    [Button]
    public void Bake()
    {
        if (_editMode == false) return;
        ToggleEditMode();

        transform.position = _lastSegment.Dock.position;
        transform.rotation = _lastSegment.Dock.rotation;

        var container = new GameObject();
        container.name = "Baked Rails";
        foreach (var item in _activeRails)
        {
            item.transform.parent = container.transform;
        }
        _activeRails.Clear();
        _lastSegment = null;

        ToggleEditMode();
    }

    [Button]
    public void SetPlankNumber(int number)
    {
        _planksPerSegment = number;
        foreach(var rail in FindObjectsOfType<RailSegment>())
        {
            rail.Init(this);
        }
    }
#endif

}
