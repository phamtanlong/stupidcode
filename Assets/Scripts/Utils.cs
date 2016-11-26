using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

public class Utils : SingletonMono<Utils>
{
    
    #region Init, New

    public static Script GetNewInitedScript()
    {
        Script.DefaultOptions.DebugPrint = Debug.Log;
        Script script = new Script();
        RegisterAssemblies(script, true, true, true);
        return script;
    }

    public static Script GetNewEmptyScript()
    {
        Script.DefaultOptions.DebugPrint = Debug.Log;
        Script script = new Script();
        return script;
    }

    public static void RegisterAssembly (Script script, Assembly assem)
    {
        Type[] ts = assem.GetTypes();
        foreach (var type in ts)
        {
            Utils.RegisterType(script, type);
        }
    }

    public static void RegisterType(Script script, Type type)
    {
        UserData.RegisterType(type);
        script.Globals[type.Name] = UserData.CreateStatic(type);
    }

    public static void RegisterType(Script script, string strType)
    {
        Type type = Type.GetType(strType);
        if (type != null)
        {
            UserData.RegisterType(type);
            script.Globals[type.Name] = UserData.CreateStatic(type);
        }
        else
        {
            Debug.Log("Can not register type: " + strType);
        }
    }

    public static void RegisterAssemblies(Script script, bool core, bool unity, bool current)
    {
        //Extensions
        UserData.RegisterExtensionType(typeof(Extensions));

        //MoonSharp
        Assembly moonSharpAssem = Assembly.GetAssembly(typeof(Script));
        RegisterAssembly(script, moonSharpAssem);

        foreach (Assembly assem in AppDomain.CurrentDomain.GetAssemblies())
        {
            string assemName = assem.GetName().ToString();
            if (core && coreAssemblies.Contains(assemName))
            {
                RegisterAssembly(script, assem);
            }
            else if (unity && unityAssemblies.Contains(assemName))
            {
                RegisterAssembly(script, assem);
            }
            else if (current && currentAssemblies.Contains(assemName))
            {
                RegisterAssembly(script, assem);
            } 
        }
    }

    #endregion


    #region Run

    public static void RunFromResources(Script script, string path)
    {
        TextAsset t = Resources.Load<TextAsset>(path);
        script.DoString(t.text);
    }

    public static void RunFromStreamingAsset(Script script, string path)
    {
        Utils.Instance.StartCoroutine(Utils.Instance.WWWRunFromAssetStream(script, path));
    }

    private IEnumerator WWWRunFromAssetStream(Script script, string path)
    {
        string url;

        #if UNITY_IPHONE
        url = Application.dataPath + "/Raw/" + path;
        #endif

        #if UNITY_ANDROID
        url = "jar:file://" + Application.dataPath + "!/assets/" + path;
        #endif

        #if UNITY_STANDALONE || UNITY_EDITOR
        url = "file://" + Application.dataPath + "/StreamingAssets/" + path;
        #endif

        WWW w = new WWW(url);

        yield return w;

        if (string.IsNullOrEmpty(w.error))
        {
            script.DoString(w.text);
        }
        else
        {
            Debug.LogError(w.error);
        }
    }

    #endregion


    #region Data

    public static List<string> unityAssemblies = new List<string>()
    { 
        "UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
        "UnityEngine, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
    };

    public static List<string> coreAssemblies = new List<string>()
    { 
        //"System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
        //"System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
        //"System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
        "mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
    };

    public static List<string> currentAssemblies = new List<string>()
    { 
        "Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
        //"Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
    };

    #endregion

}

