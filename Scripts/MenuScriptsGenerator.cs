using Redcode.CreateMenuContext;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Redcode.CreateMenuContext
{
    internal static class MenuScriptsGenerator
    {
        private static void CheckAndCreate(string templateName, string assetName)
        {
            var settings = Resources.Load<Settings>("Redcode/CSharpGenerator/Settings");

            #region Templates
            var before = $"{string.Join("", settings.DefaultUsings.Select(u => $"using {u};\n"))}\n";
            var tab = settings.GenerateNamespaces ? "\t" : string.Empty;

            if (settings.GenerateNamespaces)
                before = $"{before}namespace #NAMESPACE#\n{{\n";

            var scriptTemplate = before + $"{tab}public class #SCRIPTNAME# : MonoBehaviour\n{tab}{{\n{tab}}}";

            var scriptableObjectTemplate = before + $"{tab}[CreateAssetMenu(menuName = \"Configs/#SCRIPTNAME#\", fileName = \"#SCRIPTNAME#\")]\n";
            scriptableObjectTemplate += $"{tab}public class #SCRIPTNAME# : ScriptableObject\n{tab}{{\n{tab}}}";

            var classTemplate = before + $"{tab}public class #SCRIPTNAME#\n{tab}{{\n{tab}}}";

            var structTemplate = before + $"{tab}public struct #SCRIPTNAME#\n{tab}{{\n{tab}}}";

            var interfaceTemplate = before + $"{tab}public interface #SCRIPTNAME#\n{tab}{{\n{tab}}}";

            var enumTemplate = before + $"{tab}public enum #SCRIPTNAME#\n{tab}{{\n{tab}}}";

            Dictionary<string, string> tempaltes = new()
            {
                { "ScriptTemplate.txt", scriptTemplate },
                { "ScriptableObjectTemplate.txt", scriptableObjectTemplate },
                { "ClassTemplate.txt", classTemplate },
                { "StructTemplate.txt", structTemplate },
                { "InterfaceTemplate.txt", interfaceTemplate },
                { "EnumTemplate.txt", enumTemplate }
            };
            #endregion

            var templatesPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Temp", "Code Templates");

            if (!Directory.Exists(templatesPath))
                Directory.CreateDirectory(templatesPath);

            var templatePath = Path.Combine(templatesPath, templateName);

            var text = tempaltes[templateName];
            if (settings.GenerateNamespaces)
                text = $"{text}\n}}";

            File.WriteAllText(templatePath, text);

            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, assetName);
        }

        [MenuItem("Assets/Create/C# MonoBehaviour", priority = 60)]
        private static void CreateScript() => CheckAndCreate("ScriptTemplate.txt", "NewBehaviourScript.cs");

        [MenuItem("Assets/Create/C# ScriptableObject", priority = 61)]
        private static void CreateScriptableObject() => CheckAndCreate("ScriptableObjectTemplate.txt", "NewScriptableObject.cs");

        [MenuItem("Assets/Create/C# Class", priority = 62)]
        private static void CreateClass() => CheckAndCreate("ClassTemplate.txt", "NewClass.cs");

        [MenuItem("Assets/Create/C# Struct", priority = 63)]
        private static void CreateStruct() => CheckAndCreate("StructTemplate.txt", "NewStruct.cs");

        [MenuItem("Assets/Create/C# Interface", priority = 64)]
        private static void CreateInterface() => CheckAndCreate("InterfaceTemplate.txt", "NewInterface.cs");

        [MenuItem("Assets/Create/C# Enum", priority = 65)]
        private static void CreateEnum() => CheckAndCreate("EnumTemplate.txt", "NewEnum.cs");
    }
}