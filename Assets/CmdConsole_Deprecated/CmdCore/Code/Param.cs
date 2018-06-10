using System;

namespace CmdConsole
{
    public class Param : BaseParam
    {
        public Param(string name = "", string description = "")
        {
            Name = name;
            Description = description;
        }
    }
    public class Param<T> : BaseParam<T>
    {
        public Param(string name = "", string description = "")
        {
            Name = name;
            Description = description;
        }
    }
}
