using UnityEngine;
using System.Collections;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using UnityEngine.UI;

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

    public static void RegisterType(Script script, Type type, bool registerType = false)
    {
        UserData.RegisterType(type);
        script.Globals[type.Name] = UserData.CreateStatic(type);
        if (registerType)
        {
            script.Globals[type.Name + "_Type"] = DynValue.FromObject(script, type);
        }
    }

    public static void RegisterType(Script script, string strType, bool registerType = false)
    {
        Type type = Type.GetType(strType);
        if (type != null)
        {
            UserData.RegisterType(type);
            script.Globals[type.Name] = UserData.CreateStatic(type);
            if (registerType)
            {
                script.Globals[type.Name + "_Type"] = DynValue.FromObject(script, type);
            }
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

        //current
        Assembly currentAssem = Assembly.GetAssembly(typeof(Utils));
        RegisterAssembly(script, currentAssem);

        //unity core
        Assembly unityCodeAssem = Assembly.GetAssembly(typeof(Vector3));
        RegisterAssembly(script, unityCodeAssem);

        //unity ui
        Assembly unityUIAssem = Assembly.GetAssembly(typeof(Image));
        RegisterAssembly(script, unityUIAssem);

        //system core
        RegisterType(script, typeof(Type), true);

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

    #endregion

}

