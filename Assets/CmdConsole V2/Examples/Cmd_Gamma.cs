using System.Collections.Generic;
using UnityEngine;
using CmdConsole;

public class Cmd_Gamma : CommandBase
{
    public Cmd_Gamma()
    {
        Name = "Gamma";
    }

    public override CmdMessage ExecuteDefault()
    {
        Debug.Log("Executed Gamma!");
        return new CmdMessage("Executed Gamma!");
    }

    public override CmdMessage ExecuteWithArguments(List<object> arguments)
    {
        return new CmdMessage("No Args Needed...");
    }
}
