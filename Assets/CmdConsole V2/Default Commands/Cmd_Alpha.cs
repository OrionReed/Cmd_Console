using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CmdConsole;

public class Cmd_Alpha : Command
{
    public Cmd_Alpha()
    {
        Name = "Alpha";
        Variables = new List<IVariable>
        {
            new Variable("Alpha Float", typeof(float)),
            new Variable("Any Int", typeof(int)),
            new GlobalVar_Transform("ObjectOfInterest"),
        };
    }

    public override CmdMessage Execute()
    {
        return new CmdMessage("Processed Default Alpha");
    }
    public override CmdMessage ExecuteWithArgs(List<string> args)
    {
        float result = Variables[0].Parse<float>() + Variables[1].Parse<int>() + Variables[3].Parse<Transform>().position.y;
        return new CmdMessage("Float + Int + ObjectOfInterest.Y = " + result);
    }
}
