using System.Collections.Generic;
using UnityEngine;
using CmdConsole;
using System.Linq;
public class CommandTime : BaseCommand
{
    public CommandTime()
    {
        Name = "time";
        Description = "Get the time since the level loaded (in seconds).";

        Parameters.Add(new Param("neg", "shows negative time..."));
        Parameters.Add(new Param<bool>("bool", "add an amount of time"));
        Parameters.Add(new Param<float>("float", "add an amount of time"));
    }

    public override CmdMessage ProcessDefault()
    {
        return new CmdMessage("Time since startup: " + Time.realtimeSinceStartup.ToString("F2") + "s");
    }

    public override CmdMessage ProcessArgs(string parameter, List<string> args)
    {
        CmdMessage message = new CmdMessage();
        switch (parameter)
        {
            case "neg":
                message.AddLine("Param is 'neg'");
                break;
            case "bool":
                message.AddLine("Param is 'bool'");
                break;
            case "float":
                message.AddLine("Param is 'float'");
                // IParam<float> f = Parameters.First(x => x.Name == "float") as IParam<float>;
                // message.AddLine("f = " + f);
                // message.AddLine("10 + Float: " + (10 + (int)args[0]));
                break;
        }
        return message;
    }
}
