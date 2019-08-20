using System.Collections.Generic;
using CmdConsole;
using UnityEngine;

public class Cmd_Gamma : CommandBase {
    public Cmd_Gamma () {
        Name = "Gamma";
    }

    public override void ExecuteDefault () {
        Debug.Log ("Executed Gamma!");
        CmdLog.Log (new CmdMessage ("Executed Gamma!"));
    }

    public override void ExecuteWithArguments (List<object> arguments) {
        CmdLog.Log (new CmdMessage ("No Args Needed"));
    }
}