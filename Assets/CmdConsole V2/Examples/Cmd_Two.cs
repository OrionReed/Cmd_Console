using System.Collections.Generic;
using UnityEngine;
using CmdConsole;

public class Cmd_Two : CommandBase
{
    public Cmd_Two()
    {
        Name = "PlayTwo";
    }

    public override CmdMessage ExecuteDefault()
    {
        Debug.Log("Executed PlayTwo");
        return new CmdMessage("Executed PlayTwo");
    }

    public override CmdMessage ExecuteWithArguments(List<object> arguments)
    {
        return new CmdMessage("No Args Needed...");
    }
}
