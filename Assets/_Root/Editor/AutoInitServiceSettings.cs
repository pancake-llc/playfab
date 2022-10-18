using System;
using UnityEditor;
using UnityEngine;

namespace Pancake.Editor
{
    [InitializeOnLoad]
    public class AutoInitServiceSettings
    {
        static AutoInitServiceSettings()
        {
            if (!EditorPrefs.GetBool($"__servicesettings__{PlayerSettings.productGUID}", false))
            {
                Run();
            }
        }
        
        [MenuItem("Tools/Pancake/Create ServiceSettings", priority = 32000)]
        private static void Run()
        {
            Setup();
        }
        
        private static void Setup()
        {
            UnityEditor.EditorUtility.DisplayProgressBar("Creating the necessary settings", $"Creating GameServiceSettings.asset and PlayFabSharedSettings ...", 1f);
            var resourcePath = InEditor.DefaultResourcesPath();
            if (!$"{resourcePath}/GameServiceSettings.asset".FileExists() && !$"{resourcePath}/PlayFabSharedSettings.asset".FileExists())
            {
                CreateInstance(Complete);
            }
            else
            {
                Complete();
            }
            
            void Complete()
            {
                Debug.Log("Finish creating the game service settings");
                EditorPrefs.SetBool($"__servicesettings__{PlayerSettings.productGUID}", true);
                UnityEditor.EditorUtility.ClearProgressBar();
            }
        }
        
        private static void CreateInstance(Action onComplete)
        {
            MenuManager.InvokeCreate();
            onComplete?.Invoke();
        }
    }
}
