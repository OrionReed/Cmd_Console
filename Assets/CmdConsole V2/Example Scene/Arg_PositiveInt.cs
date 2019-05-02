using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CmdConsole;

public class Arg_PositiveInt : ArgBase
{
    public Arg_PositiveInt()
    {
        Parts = 1;
        Type = typeof(int);
    }

    public override void Init() { }

    public override SortedList<string, object> GetOptions()
    {
        if (Input.CanParse(typeof(int)) && (int)Input.Parse(typeof(int)) > 0)
        {
            Options.Clear();
            Options.Add(Input, Input.Parse(typeof(int)));
            return Options;
        }
        else
        {
            Options.Clear();
            return Options;
        }
    }
}