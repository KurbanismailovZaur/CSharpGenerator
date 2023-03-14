using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Redcode.CreateMenuContext
{
    internal class NamespaceGenerator
    {
        [MenuItem("Assets/Namespace/Change", priority = 18)]
        private static void ChangeNamespace() => Change(false);

        [MenuItem("Assets/Namespace/Change Force", priority = 18)]
        private static void ChangeNamespaceForce() => Change(true);

        private static void Change(bool forceChange)
        {
            var paths = GetSelectionAssetsPath();
            var pathToFiles = new List<string>();

            foreach (var path in paths)
            {
                if (path.IsDirectory())
                    pathToFiles.AddRange(path.GetFilePaths());
                else
                    pathToFiles.Add(path);
            }

            foreach (var path in pathToFiles)
            {
                if(path.IsCSharpFile())
                    SetNamespace(path, forceChange);
            }
        }

        [MenuItem("Assets/Namespace/Change", true)]
        private static bool ChangeNamespaceValid() => IsValidChangeNamespace();

        [MenuItem("Assets/Namespace/Change Force", true)]
        private static bool ChangeNamespaceForceValid() => IsValidChangeNamespace();
            
        private static bool IsValidChangeNamespace() => GetSelectionAssetsPath().Any(x => x.IsDirectory() || x.IsCSharpFile());

        private static void SetNamespace(string path, bool forceChange)
        {
            var @namespace = "namespace";
            var scriptTypes = new[] { "class", "struct", "interface", "enum" };
            var scriptLines = File.ReadAllLines(path);
            var lineIndexForAddNamespace = 0;
            var isContainsNamespace = false;

            for (int i = 0; i < scriptLines.Length; i++, lineIndexForAddNamespace++)
            {
                var line = scriptLines[i].Trim();

                if (line == "" || line.StartsWith("using") || line.StartsWith('/'))
                    continue;

                if (line.StartsWith(@namespace))
                {
                    isContainsNamespace = true;
                    goto Found;
                }

                if (line.StartsWith('['))
                    goto Found;

                foreach (var type in scriptTypes)
                {
                    if (line.Contains(type))
                        goto Found;
                }
            }

        Found:
            var namespaceLine = $"{@namespace} {path.GenerateNamespace()}";
            var text = GetScriptText(path, scriptLines, lineIndexForAddNamespace, isContainsNamespace, namespaceLine, forceChange);
            File.WriteAllText(path, text);
            AssetDatabase.Refresh();
        }

        private static string[] GetSelectionAssetsPath()
        {
            List<string> paths = new();
            foreach (var assetGUID in Selection.assetGUIDs)
                paths.Add(AssetDatabase.GUIDToAssetPath(assetGUID));

            return paths.ToArray();
        }

        private static string GetScriptText(string path, string[] scriptLines, int lineIndexForAddNamespace, bool isContainsNamespace, string @namespace, bool forceChangeNamespace)
        {
            var scriptText = new StringBuilder();

            if (isContainsNamespace)
            {
                for (int i = 0; i < scriptLines.Length; i++)
                {
                    if (forceChangeNamespace && i == lineIndexForAddNamespace)
                        scriptText.Append($"{@namespace}\n");
                    else
                        scriptText.Append(scriptLines[i] + "\n");
                }
            }
            else if (!isContainsNamespace)
            {
                for (int i = 0; i < scriptLines.Length; i++)
                {
                    if (i == lineIndexForAddNamespace)
                    {
                        if(i > 0 && !string.IsNullOrEmpty(scriptLines[i - 1]))
                            scriptText.Append("\n");    
                        scriptText.Append($"{@namespace}\n{"{"}\n");
                    }

                    scriptText.Append((i >= lineIndexForAddNamespace ? "\t" : string.Empty) + scriptLines[i] + "\n");
                }
                scriptText.Append("}");
            }

            return scriptText.ToString();
        }
    }
}