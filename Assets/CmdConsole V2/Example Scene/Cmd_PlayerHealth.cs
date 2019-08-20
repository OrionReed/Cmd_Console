using System.Collections;
using System.Collections.Generic;
using CmdConsole;
using UnityEngine;

public class Cmd_PlayerHealth : CommandBase {
    public Cmd_PlayerHealth () {
        Name = "PlayerHealth";
        Variables = new List<IVar> {
            new Var<Arg_Player> ("Player"),
            new Var<float> ("HealthBoost"),
            new Var<Vector3> ("Vector3")
        };
    }

    public override void ExecuteDefault () {
        CmdLog.Log (new CmdMessage ("Dis cummand nids argooments"));
    }

    public override void ExecuteWithArguments (List<object> arguments) {
        Player player = (Player) arguments[0];
        player.Health += (float) arguments[1];
        CmdMessage message = new CmdMessage () { { "Added ", CMStyle.Default }, {
                ((float) arguments[1]).ToString (), CMStyle.Emphasis }, { " health to ", CMStyle.Default }, { player.Name, CMStyle.Emphasis }
        };
        CmdLog.Log (message);
    }
}