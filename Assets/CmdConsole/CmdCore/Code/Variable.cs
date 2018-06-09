using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CmdConsole
{
    public class Variable<T> : IVariable
    {
        public Variable(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; private set; }
        public Type Type { get; private set; }
    }
}
