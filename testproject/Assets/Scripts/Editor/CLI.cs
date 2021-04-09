using System;
using UnityEditor;
using UnityEngine;

namespace TestProject.Editor
{
    public class CLI
    {
        public static void ThingsForFun()
        {
            var logLine = $"{nameof(CLI)}.{nameof(ThingsForFun)}()";
            Console.WriteLine(logLine);
            Debug.Log(logLine);

            EditorPrefs.SetString("kScriptsDefaultApp", "rider");
            Unity.CodeEditor.CodeEditor.CurrentEditor.SyncAll();
            EditorApplication.Exit(0);

            Console.WriteLine($"POST-QUIT {logLine}");
            Debug.Log($"POST-QUIT {logLine}");
        }
    }
}
