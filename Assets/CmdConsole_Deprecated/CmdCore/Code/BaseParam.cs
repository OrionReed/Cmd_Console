using System;

namespace CmdConsole
{
    public abstract class BaseParam : IParam
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public string ValueString()
        {
            return "";
        }
    }
    public abstract class BaseParam<T> : IParam<T>
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public T ParseValue(string parseString)
        {
            return parseString.Parse<T>();
        }
        public bool CanParseValue(string parseString)
        {
            if (parseString.CanParse(typeof(T)))
                return true;
            else
                return false;
        }
        public string ValueString()
        {
            return "";
        }
    }
}
