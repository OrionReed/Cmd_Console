using System;

namespace CmdConsole
{
    public class Var<T> : IVar
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public Var(string name)
        {
            Name = name;
            Type = typeof(T);
        }
    }
}