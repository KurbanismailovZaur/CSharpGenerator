using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Redcode.ScriptsGenerator
{
    internal static class MenuScriptsGenerator
    {
        private static void CheckAndCreate(string templateName, string assetName)
        {
            #region Templates
            string namespaces =
                "using System;\n" +
                "using System.Collections;\n" +
                "using System.Collections.Generic;\n" +
                "using System.Linq;\n" +
                "using UnityEngine;\n";



            //var redcodePath = Path.Combine(Application.dataPath, "Redcode");
            var redcodeDirectiores = Directory.GetDirectories(Application.dataPath, "Redcode", SearchOption.AllDirectories);
            var redcodePath = redcodeDirectiores.Length >= 1 ? redcodeDirectiores.OrderBy(p => p.Length).FirstOrDefault() : "Redcode";

            if (Directory.Exists(Path.Combine(redcodePath, "Extensions")))
                namespaces += "using Redcode.Extensions;\n";

            if (Directory.Exists(Path.Combine(redcodePath, "Moroutines")))
                namespaces += "using Redcode.Moroutines;\n" +
                "using Redcode.Moroutines.Extensions;\n";

            var before = namespaces + "\nnamespace #NAMESPACE#\n{\n";

            var scriptTemplate = before + "\tpublic class #SCRIPTNAME# : MonoBehaviour\n\t{\n\t}\n}";

            var scriptableObjectTemplate = before + "\t[CreateAssetMenu(menuName = \"Configs/#SCRIPTNAME#\", fileName = \"#SCRIPTNAME#\")]\n";
            scriptableObjectTemplate += "\tpublic class #SCRIPTNAME# : ScriptableObject\n\t{\n\t}\n}";

            var classTemplate = before + "\tpublic class #SCRIPTNAME#\n\t{\n\t}\n}";

            var structTemplate = before + "\tpublic struct #SCRIPTNAME#\n\t{\n\t}\n}";

            var interfaceTemplate = before + "\tpublic interface #SCRIPTNAME#\n\t{\n\t}\n}";

            var enumTemplate = before + "\tpublic enum #SCRIPTNAME#\n\t{\n\t}\n}";

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
            File.WriteAllText(templatePath, tempaltes[templateName]);

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
#endif