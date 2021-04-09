using System;

using SRML.Console;
using Console = SRML.Console.Console;
using System.Collections.Generic;

using UnityEngine;
using System.Linq;

namespace ElementalElectricTree.Other
{
    class PrintComponentsCommand : ConsoleCommand
    {
        // The command's command ID; how someone using the console will call your command.
        public override string ID => "print_components";

        // What will appear when the command is used incorrectly or viewed via the help command.
        // Remember, <> is a required argument while [] is an optional one.
        public override string Usage => "PrintComponent <argument>";

        // A description of the command that will appear when using the help command.
        public override string Description => "Prints Components of something";

        // The code that the command runs. Requires you to return a bool.
        public override bool Execute(string[] args)
        {
            // Checks if the code has enough arguments.
            if (args == null || args.Length > 1)
            {
                Console.LogError("Incorrect amount of arguments!", true);
                return false;
            }

            Identifiable.Id id;
            GameObject prefab;

            try
            {
                id = (Identifiable.Id)Enum.Parse(typeof(Identifiable.Id), args[0], true);
                prefab = GameContext.Instance.LookupDirector.GetPrefab(id);
            }
            catch
            {
                Console.LogError("Invalid ID!");
                return false;
            }

            Console.Log("Starting printing: ", true);

            prefab.PrintComponent();

            return true;
        }

        // A list that the autocomplete references from. You must return a List<string>.
        public override List<string> GetAutoComplete(int argIndex, string argText)
        {
            return GameContext.Instance.LookupDirector.identifiablePrefabs.Select(x => Identifiable.GetId(x).ToString()).ToList();
        }
    }
}