using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Redcode.CreateMenuContext
{
    //[CreateAssetMenu(menuName = "Configs/Settings", fileName = "Settings")]
    public class Settings : ScriptableObject
    {
        [SerializeField]
        [Tooltip("Should namespaces be generated when scripts are created?")]
        private bool _generateNamespaces = true;

        public bool GenerateNamespaces => _generateNamespaces;

        [SerializeField]
        [Tooltip("Use these namespaces in any script you create.")]
        private string[] _defaultUsings = new string[]
        {
            "System",
            "System.Collections",
            "System.Collections.Generic",
            "System.Linq",
            "UnityEngine",
            "UnityRandom = UnityEngine.Random"
        };

        public string[] DefaultUsings => _defaultUsings;
    }
}