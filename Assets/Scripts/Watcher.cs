using UnityEngine;
using System.Collections;
using System;

public class Watcher : MonoBehaviour
{
    public event EventHandler AwakeHappend;
    public event EventHandler OnEnableHappend;
    public event EventHandler StartHappend;

    public event EventHandler UpdateHappend;

    public event EventHandler OnMouseDownHappend;
    public event EventHandler OnMouseUpHappend;

    public event EventHandler OnCollisionEnterHappend;
    public event EventHandler OnCollisionExitHappend;

    public event EventHandler OnDisableHappend;
    public event EventHandler OnDestroyHappend;

    void Awake ()
    {
        if (AwakeHappend != null)
            AwakeHappend.Invoke(this, EventArgs.Empty);
    }

    void OnEnable()
    {
        if (OnEnableHappend != null)
            OnEnableHappend.Invoke(this, EventArgs.Empty);
    }

    void Start()
    {
        if (StartHappend != null)
            StartHappend.Invoke(this, EventArgs.Empty);
    }

    void Update()
    {
        if (UpdateHappend != null)
            UpdateHappend.Invoke(this, EventArgs.Empty);
    }

    void OnMouseDown()
    {
        if (OnMouseDownHappend != null)
            OnMouseDownHappend.Invoke(this, EventArgs.Empty);
    }

    void OnMouseUp()
    {
        if (OnMouseUpHappend != null)
            OnMouseUpHappend.Invoke(this, EventArgs.Empty);
    }

    void OnCollisionEnter(Collision collision) {
        if (OnCollisionEnterHappend != null)
        {
            OnCollisionEnterHappend.Invoke(this, new ObjectsEventArgs (collision));
        }
    }

    void OnCollisionExit(Collision collision) {
        if (OnCollisionExitHappend != null)
        {
            OnCollisionExitHappend.Invoke(this, new ObjectsEventArgs (collision));
        }
    }

    void OnDisable()
    {
        if (OnDisableHappend != null)
            OnDisableHappend.Invoke(this, EventArgs.Empty);
    }

    void OnDestroy()
    {
        if (OnDestroyHappend != null)
            OnDestroyHappend.Invoke(this, EventArgs.Empty);
    }
}

public class ObjectsEventArgs : EventArgs
{
    public object[] objs;
    public ObjectsEventArgs (params object[] _objs) 
    {
        objs = _objs;
    }
}
