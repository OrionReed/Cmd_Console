using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CmdConsole;
public class CommandClear : BaseCommand
{
    public CommandClear()
    {
        Name = "clear";
        Description = "clear the console";
    }

    public override CmdMessage ProcessDefault()
    {
        //        Console.instance.ClearLog();
        return null;
    }
    public override CmdMessage ProcessArgs(string parameter, List<string> args)
    {
        return ProcessDefault();
    }
}
