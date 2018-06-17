using System.Collections.Generic;
using UnityEngine;
using CmdConsole;

public class Cmd_Three : CommandBase
{
    public Cmd_Three()
    {
        Name = "PlayThree";
    }

    public override CmdMessage ExecuteDefault()
    {
        Debug.Log("Executed PlayThree");
        return new CmdMessage("Executed PlayThree");
    }

    public override CmdMessage ExecuteWithArguments(List<object> arguments)
    {
        return new CmdMessage("No Args Needed...");
    }
}
