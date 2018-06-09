using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace CmdConsole
{
    public class GlobalVar_Transform : MonoBehaviour, IVariable<Transform>
    {
        public GlobalVar_Transform(string name)
        {
            Name = name;
            Type = typeof(Transform);
        }
        public Transform GetDefault()
        {
            return FindObjectOfType<Transform>();
        }
        public List<Transform> GetAll()
        {
            return FindObjectsOfType<Transform>().ToList();
        }
        public string Name { get; private set; }
        public Type Type { get; private set; }


    }
}
