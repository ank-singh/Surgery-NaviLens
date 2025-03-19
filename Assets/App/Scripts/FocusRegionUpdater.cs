using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusRegionUpdater : MonoBehaviour
{
    public GameObject focusObject;
    public float boundingSize = 0.5f;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (focusObject)
        {
            Vector3 focusPos = focusObject.transform.position;
            _renderer.material.SetVector("_FocusPosition", focusPos);
            _renderer.material.SetFloat("_FocusRadius", boundingSize);
        }
    }
}
