using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace CmdConsole
{
    public class GlobalVar_Transform : IVariable<Transform>
    {
        public GlobalVar_Transform(string name)
        {
            Name = name;
            Type = typeof(Transform);
        }

        public Transform GetDefault()
        {
            return GameObject.FindObjectOfType<Transform>();
        }
        public List<Transform> GetAll()
        {
            return GameObject.FindObjectsOfType<Transform>().ToList();
        }
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}