using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider))]
public class TargetOrgan : MonoBehaviour
{

    [SerializeField] private GameObject _beaconPrefab;
    [SerializeField] private GameObject _label;
    private GameObject _currentBeaconPrefab;
    private Color _originalColour;
    private Renderer _renderer;
    public bool _confirmMovement;

    private Vector3 _originalPosition;
    private Vector3 _targetTransform;

    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _originalColour = _renderer.material.GetColor("_Color");
        _originalPosition = transform.position;
        _targetTransform = transform.position + Vector3.up * 0.5f;
        _label.SetActive(false);

    }
    public void Highlight()
    {
        _renderer.material.SetColor("_Color", Color.yellow);
        _renderer.material.SetColor("_EmissionColor", Color.yellow);
        _currentBeaconPrefab = Instantiate(_beaconPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, Vector3.up));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scalpel"))
        {
            ToolController controller = other.GetComponent<ToolController>();
            if (controller.FindingTarget.Equals(this))
            {
                _renderer.material.SetColor("_Color", _originalColour);
                _renderer.material.SetColor("_EmissionColor", _originalColour);
                GetComponent<AudioSource>().Play();
                _label.SetActive(true);

                Destroy(_currentBeaconPrefab);
                controller.NextOrgan();
                _confirmMovement = true;
            }
        }
    }

    private void Update()
    {
        float step = 1 * Time.deltaTime;
        if(_confirmMovement)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetTransform, step);
            if(Vector3.Distance(transform.position, _targetTransform) < 0.001f)
            {
                _confirmMovement=false;
            }
        }
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += duration;
            yield return null;
        }
        transform.position = targetPosition;
    }

    private void ResetPosition()
    {
        transform.position = _originalPosition;
        _renderer.material.SetColor("_Color", _originalColour);
        _renderer.material.SetColor("_EmissionColor", _originalColour);
        _label.SetActive(false);

    }
}
