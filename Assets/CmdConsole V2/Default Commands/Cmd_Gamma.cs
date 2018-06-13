using System.Collections.Generic;
using CmdConsole;

public class Cmd_Gamma : Command
{
    public Cmd_Gamma()
    {
        Name = "Gamma";
        Variables = new List<IVariable>
        {
            new Variable("Alpha Float", typeof(float)),
            new Variable("Any Int", typeof(int)),
            new GlobalVar_Transform("ObjectOfInterest"),
        };
    }

    public override CmdMessage Execute()
    {
        return new CmdMessage("Processed Default");
    }
    public override CmdMessage ExecuteWithArgs(List<string> args)
    {
        return new CmdMessage();
    }
}
