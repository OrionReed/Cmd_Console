using System.Collections.Generic;
using UnityEngine;
using CmdConsole;

public class Cmd_Two : CommandBase
{
    public Cmd_Two()
    {
        Name = "PlayTwo";
    }

    public override void ExecuteDefault()
    {
        CmdLog.Log(new CmdMessage("Executed PlayTwo"));
    }

    public override void ExecuteWithArguments(List<object> arguments)
    {
        CmdLog.Log(new CmdMessage("No Args Needed..."));
    }
}
