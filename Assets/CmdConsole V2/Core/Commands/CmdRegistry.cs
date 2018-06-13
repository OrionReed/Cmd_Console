using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;

namespace CmdConsole
{
    public class CmdRegistry
    {
        public static Dictionary<string, Command> Commands = new Dictionary<string, Command>();

        public static void AddCommand(Command command)
        {
            Commands.Add(command.Name, command);
        }

        public static void RegisterCommands()
        {
            AddCommand(new Cmd_Alpha());
            AddCommand(new Cmd_Allepo());
            AddCommand(new Cmd_Beta());
            AddCommand(new Cmd_Gamma());
            AddCommand(new Cmd_Get());

        }

        public static CmdMessage ValidateCommands()
        {
            CmdMessage message = new CmdMessage();
            if (Commands.Count > 0)
            {
                int validatedCount = 0;
                foreach (KeyValuePair<string, Command> c in Commands)
                {
                    message.AddLine(new CMLine()
                    {
                        {"Successfuly loaded command ", CMStyle.Default},
                        {c.Key, CMStyle.Emphasis}
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
                    {"0 ", CMStyle.Emphasis},
                    {"commands to load.",CMStyle.Warning}
                });
            }
            return message;
        }

    }
}
