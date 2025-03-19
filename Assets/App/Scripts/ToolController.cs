using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToolController : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    [SerializeField] List<TargetOrgan> _targetOrgans;
    public TargetOrgan FindingTarget;


    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }
    private void OnEnable()
    {
        BeginOrganFinding();
    }

    public void BeginOrganFinding() 
    { 
        StartPathFinding(_targetOrgans.First());
    }
    private void StartPathFinding(TargetOrgan targetOrgan)
    {
        _lineRenderer = GetComponent<LineRenderer>();
        Debug.Log("null: organ / line renderer?" + (targetOrgan is null) + (_lineRenderer is null));
        Debug.Log("path finding with: " + targetOrgan.gameObject.name);

        targetOrgan.Highlight();
        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, targetOrgan.transform.position);
        _lineRenderer.SetPosition(1, this.transform.GetChild(0).position);
        FindingTarget = targetOrgan;

    }


    private void StopPathFinding()
    {
        FindingTarget = null;
        _lineRenderer.enabled = false;


    }

    public void NextOrgan()
    {
        int currentIndex = _targetOrgans.FindIndex(x => x == FindingTarget);
        StopPathFinding();
        if (currentIndex < _targetOrgans.Count - 1)
        {
            StartPathFinding(_targetOrgans[currentIndex + 1]);
        }
        else 
        {
            GetComponent<AudioSource>().Play();
        }

    }

    private void Update()
    {
        if (FindingTarget) 
        {
            _lineRenderer.SetPosition(1, this.transform.GetChild(0).position);
        }
    }
}
