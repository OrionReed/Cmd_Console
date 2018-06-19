using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CmdConsole;

public class Cmd_PlayerHealth : CommandBase
{
    public Cmd_PlayerHealth()
    {
        Name = "PlayerHealth";
        Variables = new List<IVar>
        {
            new Var<Arg_Player>("Player"),
            new Var<float>("HealthBoost")
        };
    }

    public override CmdMessage ExecuteDefault()
    {
        return new CmdMessage("Command Needs Arguments");
    }

    public override CmdMessage ExecuteWithArguments(List<object> arguments)
    {
        Player player = (Player)arguments[0];
        player.Health += (float)arguments[1];
        Debug.Log(
            "Added <b>" +
            (float)arguments[1] +
            "</b> Health to <b>" +
            player.Name +
            "</b>");
        return new CmdMessage(" ... ");
    }
}
