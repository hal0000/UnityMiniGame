using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[ExecuteInEditMode]
public class CanvasController : MonoBehaviour
{
    public UnityEvent OnResolutionOrOrientationChanged;
    private bool _screenChangeVarsInitialized = false;
    private Vector2 _lastResolution = Vector2.zero;
    private CanvasScaler _canvasScaler;

    void Awake()
    {
        _canvasScaler = GetComponent<CanvasScaler>();
        if (_screenChangeVarsInitialized) return;
        _lastResolution.x = Screen.width;
        _lastResolution.y = Screen.height;
        _screenChangeVarsInitialized = true;
        SetScaleFactor();
    }

    void SetScaleFactor()
    {
        Vector2 designSize = _canvasScaler.referenceResolution;
        float ww = designSize.x;
        float hh = designSize.y;
        float ratio = hh / ww;
        float w = Screen.width;
        float h = (int)(w * ratio);
        if (h - 2 > Screen.height)
        {
            h = Screen.height;
            w = (int)(h / ratio);
            _canvasScaler.matchWidthOrHeight = 1;
        }
        else
            _canvasScaler.matchWidthOrHeight = 0;
    }

    void Update()
    {
        if (!(Math.Abs(Screen.width - _lastResolution.x) > 2) && !(Math.Abs(Screen.height - _lastResolution.y) > 2)) return;
        ResolutionChanged();
    }

    void ResolutionChanged()
    {
        //Logger.Log("Resolution changed from " + lastResolution + " to (" + Screen.width + ", " + Screen.height + ") at " + Time.time);
        _lastResolution.x = Screen.width;
        _lastResolution.y = Screen.height;
        OnResolutionOrOrientationChanged?.Invoke();
    }
}