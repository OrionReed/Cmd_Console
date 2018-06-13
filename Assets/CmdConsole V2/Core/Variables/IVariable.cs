using System.Collections.Generic;
using System;

namespace CmdConsole
{
    public interface IVariable
    {
        string Name { get; set; }
        Type type { get; set; }
        object Value { get; set; }
        int Parts { get; set; }
    }
    public interface IVariable<T> : IVariable
    {
        ValidInputList<T> ParseOptions(string tryParse);
    }
    public abstract class BaseVariable : IVariable<T>
    {
        public Type Type { get; private set; }
        public string Name { get; set; }

        public BaseVariable()
        {
            Type =
        }
        public int Parts()
        {
            return;
        }
        public ValidInputList<T> ParseOptions(string tryParse)
        {

        }
    }

    public class Variable<T> : IVariable<T>
    {
        public Variable(string name)
        {
            Name = name;
            Type = typeof(T);
        }
        public int Parts()
        {
            return Regex.Split(default(T).ToString(), @"\s").Length;
        }
        public ValidInputList<T> ParseOptions(string toParse)
        {
            if (toParse.CanParse(typeof(T)))
            {
                return new ValidInputList<T>(toParse, toParse.Parse<T>());
            }
            else
            {
                return new ValidInputList<T>(String.Empty, toParse.Parse<T>());
            }
        }

        public string Name { get; set; }
        public Type Type { get; set; }
    }
}
