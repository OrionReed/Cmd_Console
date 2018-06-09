using System.Collections.Generic;
using CmdConsole;

public class CommandHelp : BaseCommand
{
    public CommandHelp()
    {
        Name = "help";
        Alias = "?";
        Description = "lists available commands & parameters";
        DefaultType = typeof(float);
        //        Arguments.Add(new Param("command", "...", typeof(ICommand)));
        //  Arguments.Add(new Param("parameter", "...", typeof(IParam)));
    }

    public override CmdMessage ProcessDefault()
    {
        CmdMessage message = new CmdMessage();
        message.AddLineBreak();
        message.AddLine("Command Help Menu", CMStyle.Confirm);
        foreach (ICommand command in ConsoleCommands.Commands)
        {
            message.AddLine(new CMLine(command.Name, CMStyle.Emphasis));
            message.AddLine(new CMLine(command.Description));
        }
        return message;
    }
    public override CmdMessage ProcessArgs(string parameter, List<string> args)
    {
        return ProcessDefault();
        /*ConsoleMessage message = new ConsoleMessage(CMSender.User);
        switch ()
        if (args[0] == Arguments[0].Name)
        {
            return ProcessDefault();
        }
        else if (Arguments[0].Options.Contains(args[0]))
        {
            message.Add("[Parameter Help Menu:;P]");
            foreach (IParam param in ConsoleCommands.Parameters)
            {
                message.Add("[" + param.Name + ":;E]");
                message.Add(param.Description);
            }
            return message;
        }
        else
        {
            ICommand foundCommand;
            foundCommand = ConsoleCommands.FindCommand(args[0]);
            if (foundCommand != null)
            {
                message.Add("[" + foundCommand.Name + ";P" + " Help");
                message.Add(foundCommand.Description);
                return message;
            }
            else
            {
                message.Add("No command found.", CMLineType.Warning);
                return message;
            }
        }*/
    }
}

