﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ExceptionLogging : MonoBehaviour
{
    public string saveFile = @"Log.txt";
   private StringWriter logWriter;

    private void OnEnable()
    {
        Application.logMessageReceived += ExceptionWriter;
    }

    private void OnDisable()
    {
        Application.logMessageReceived += null;
    }

    void ExceptionWriter(string logString, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Exception:
            case LogType.Error:
                using(StreamWriter writer = new StreamWriter(new FileStream(saveFile, FileMode.Append)))
                {
                    writer.WriteLine(type);
                    writer.WriteLine(logString);
                    writer.WriteLine(stackTrace);
                }
                break;
            default:
                break;
        }
    }
}
