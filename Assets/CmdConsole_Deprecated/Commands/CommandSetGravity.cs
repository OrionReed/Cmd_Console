using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using CmdConsole;
public class CommandSetGravity : BaseCommand
{
    public CommandSetGravity()
    {
        Name = "setgravity";
        Description = "Sets the players gravity";
        DefaultType = typeof(bool);
    }

    public override CmdMessage ProcessDefault()
    {
        return new CmdMessage(Name + " requires one argument of type <bool>");
    }

    public override CmdMessage ProcessArgs(string parameter, List<string> args)
    {
        CmdMessage message = new CmdMessage();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            obj.GetComponent<Rigidbody>().useGravity = parameter.Parse<bool>();
        }
        message.AddLine(new CMLine()
        {
            {"Set gravity to ",CMStyle.Default},
            { parameter.Parse<bool>().ToString(), CMStyle.Emphasis}
        });
        return message;
    }
}
