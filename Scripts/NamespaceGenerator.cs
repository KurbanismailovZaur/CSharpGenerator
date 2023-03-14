using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Redcode.CreateMenuContext
{
    internal class NamespaceGenerator
    {
        [MenuItem("Assets/Generate/Namespaces For Selected C# Scripts", priority = 18)]
        private static void GenerateNamespace() => Generate(false);

        [MenuItem("Assets/Generate/Namespaces For Selected C# Scripts (Forced)", priority = 18)]
        private static void GenerateNamespaceForce() => Generate(true);

        private static void Generate(bool forceChange)
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

            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/Namespace/Generate", true)]
        private static bool GenerateNamespaceValid() => IsValidChangeNamespace();

        [MenuItem("Assets/Namespace/Generate Force", true)]
        private static bool GenerateNamespaceForceValid() => IsValidChangeNamespace();
            
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
                    break;
                }

                if (line.StartsWith('['))
                    break;

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