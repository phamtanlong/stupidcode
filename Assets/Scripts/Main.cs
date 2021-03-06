﻿using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using MoonSharp.Interpreter;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour
{
    public InputField InpCode;

    private Script script;

    void Start () 
    {
        //Script.DefaultOptions.DebugPrint = Debug.Log;
        //script = new Script();

        script = Utils.GetNewInitedScript();
        //script.Globals["main"] = DynValue.FromObject (script, this);
        //Type t = typeof(Watcher);
        //script.Globals["Watcher_Type"] = DynValue.FromObject (script, t);

        //script.Globals["TestCode"] = (Func<string>)TestCode;
        script.Globals["Add"] = (Func<int,int,int>)Add;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            RunIt();
        }
    }

    public void RunIt()
    {
        Utils.RunFromStreamingAsset(script, @"main.lua");
    }

    public void OnRunInputCode () 
    {
        script.DoString(InpCode.text);
        InpCode.text = string.Empty;
        //focus
        InpCode.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    public static int Add (int a, int b)
    {
        return a + b;
    }

    [MenuItem("Jundat/Test Code")]
    public static string TestCode () 
    {
        var go = GameObject.Find("Cube");

//        iTweenEvent it = go.AddComponent<iTweenEvent>();
//        it.enabled = false;
//        it.type = iTweenEvent.TweenType.MoveTo;
//        it.Values = new Dictionary<string, object>();
//        it.Values.Add("position", new Vector3(0, 13, 0));
//        it.Values.Add("time", 1);
//        it.Values.Add("looptype", "pingPong");
//        it.enabled = true;
//
        iTween.MoveTo(go, iTween.Hash("position", new Vector3(0,13,0), "time", 1, "looptype", "pingPong"));

        return "ok";
    }
}
