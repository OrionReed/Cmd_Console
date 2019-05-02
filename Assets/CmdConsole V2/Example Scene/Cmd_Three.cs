using System.Collections.Generic;
using UnityEngine;
using CmdConsole;

public class Cmd_Three : CommandBase
{
    public Cmd_Three()
    {
        Name = "PlayThree";
    }

    public override void ExecuteDefault()
    {
        CmdLog.Log(new CmdMessage("Executed PlayThree"));
    }

    public override void ExecuteWithArguments(List<object> arguments)
    {
        CmdLog.Log(new CmdMessage("Executed PlayThree with Args"));
    }
}
