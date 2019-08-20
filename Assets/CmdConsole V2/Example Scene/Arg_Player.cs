using System;
using System.Collections.Generic;
using System.Linq;
using CmdConsole;
using UnityEngine;

public class Arg_Player : ArgBase {
    private static SortedList<string, Player> allPlayers;
    public Arg_Player () {
        Parts = 1;
        Type = typeof (Player);
    }

    public override void Init () {
        var findPlayers = GameObject.FindObjectsOfType<Player> ()
            .OrderBy (p => p.Name)
            .ToDictionary (p => p.Name, p => p);
        allPlayers = new SortedList<string, Player> (findPlayers);
    }

    public override SortedList<string, object> GetOptions () {
        var searchedPlayers = allPlayers.Where (p => p.Key.StartsWith (Input, StringComparison.InvariantCultureIgnoreCase)).ToList ();
        if (searchedPlayers.Count > 0) {
            Options.Clear ();
            for (int i = 0; i < searchedPlayers.Count; i++) {
                Options.Add (searchedPlayers[i].Key, searchedPlayers[i].Value);
            }
            return Options;
        } else {
            Options.Clear ();
            return Options;
        }
    }
}