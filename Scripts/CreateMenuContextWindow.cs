using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Redcode.CreateMenuContext
{
    internal class CreateMenuContextWindow : EditorWindow
    {
        private static CreateMenuContextWindow _window;
        private static Settings _settings;
        private static Editor _settingsEditor;

        [MenuItem("Window/C# Generator/Settings")]
        private static void ShowWindow()
        {
            if (_window != null)
                return;

            _window = (CreateMenuContextWindow)GetWindow(typeof(CreateMenuContextWindow));
            _window.titleContent = new GUIContent("C# Generator Settings");
            _window.Show();
        }

        private void OnEnable()
        {
            _settings = Resources.Load<Settings>("Redcode/CSharpGenerator/Settings");
            _settingsEditor = Editor.CreateEditor(_settings);
        }

        private void OnDisable() => _window = null;

        private void OnGUI() => _settingsEditor.OnInspectorGUI();
    }
}