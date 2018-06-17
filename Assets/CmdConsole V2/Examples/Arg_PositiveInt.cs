using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CmdConsole;

public class Arg_PositiveInt : ArgBase
{
    public Arg_PositiveInt()
    {
        PartCount = 1;
        Type = typeof(int);
    }

    public override void Init() { }

    public override SortedList<string, object> GetOptions()
    {
        if (InputString.CanParse(typeof(int)) && (int)InputString.Parse(typeof(int)) > 0)
        {
            Options.Clear();
            Options.Add(InputString, InputString.Parse(typeof(int)));
            return Options;
        }
        else
        {
            Options.Clear();
            return Options;
        }
    }
}