using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

namespace CmdConsole
{
    public class ConsoleCommands
    {
        public static IList<ICommand> Commands { get; private set; } = new List<ICommand>();
        public static IList<IParam> Parameters { get; private set; } = new List<IParam>();

        public static void AddParam(IParam parameter) => Parameters.Add(parameter);
        public static void AddCommand(ICommand command) => Commands.Add(command);
        public static void RemoveParam(IParam parameter) => Parameters.Remove(parameter);
        public static void RemoveCommand(ICommand command) => Commands.Remove(command);
        public static void SortParameters() => Parameters = Parameters.OrderBy(f => f.Name).ToList();
        public static void SortCommands() => Commands = Commands.OrderBy(f => f.Name).ToList();
        public static ICommand FindCommand(string search) { return Commands.First(c => c.Name.StartsWith(search)); }
        public static IParam FindParameter(string search) { return Parameters.First(p => p.Name.StartsWith(search)); }
        public static void RegisterParameters()
        {
        }
        public static void RegisterCommands()
        {
            //Commands = InterfaceFinder.FindObjects<ICommand>() as IList<ICommand>;
            AddCommand(new CommandHelp());
            AddCommand(new CommandClear());
            AddCommand(new CommandTime());
            AddCommand(new CommandSetGravity());
        }

        public static CmdMessage ValidateCommands()
        {
            CmdMessage message = new CmdMessage();
            if (Commands.Count > 0)
            {
                int validatedCount = 0;
                foreach (ICommand command in ConsoleCommands.Commands)
                {
                    message.AddLine(new CMLine()
                    {
                        {"Successfuly loaded command ", CMStyle.Default},
                        {command.Name, CMStyle.Emphasis}
                    });
                    validatedCount++;
                }
                message.AddLine(new CMLine()
                {
                    {"Successfully loaded ", CMStyle.Confirm},
                    {validatedCount.ToString(), CMStyle.Emphasis},
                    {" commands",CMStyle.Confirm}
                });
            }
            else
            {
                message.AddLine(new CMLine()
                {
                    {"Found ", CMStyle.Warning},
                    {"0", CMStyle.Emphasis},
                    {" commands to load.",CMStyle.Warning}
                });
            }
            return message;
        }

    }
}
