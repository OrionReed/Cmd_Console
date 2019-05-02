using System.Collections;
using UnityEditor;
using UnityEngine;

public class Metrics : EditorWindow
{

    static string selectedPath = "/Assets";
    static System.Text.StringBuilder strStats;
    static int totalLines;
    static int totalFiles;
    static Metrics window;

    struct File
    {
        public string name;
        public int nbLines;

        public File (string name, int nbLines)
        {
            this.name = name;
            this.nbLines = nbLines;
        }
    }

    [MenuItem ("Window/Metrics")]

    static void Init ()
    {
        window = (Metrics) EditorWindow.GetWindow (typeof (Metrics));
        window.Show ();
        window.DoCountLines ();
    }

    void OnGUI ()
    {
        GUILayout.Label ("Number of Lines", EditorStyles.boldLabel);
        GUILayout.Label (totalLines.ToString ());
        GUILayout.Label ("Number of Files", EditorStyles.boldLabel);
        GUILayout.Label (totalFiles.ToString ());
        GUILayout.Label ("Selected Folder", EditorStyles.boldLabel);
        GUILayout.Label (selectedPath);

        EditorGUILayout.HelpBox (strStats.ToString (), MessageType.None);

        if (GUILayout.Button ("Select Folder"))
        {
            selectedPath = EditorUtility.OpenFolderPanel ("Specify Folder", "/Assets", "");
            int index = selectedPath.IndexOf ("/Assets");
            selectedPath = selectedPath.Substring (index, selectedPath.Length - index);
        }
        if (GUILayout.Button ("Update Metrics"))
        {
            DoCountLines ();
        }
    }

    void DoCountLines ()
    {
        string strDir = System.IO.Directory.GetCurrentDirectory ();
        strDir += @selectedPath;
        int iLengthOfRootPath = strDir.Length;
        ArrayList stats = new ArrayList ();
        ProcessDirectory (stats, strDir);

        int iTotalNbLines = 0;
        foreach (File f in stats)
        {
            iTotalNbLines += f.nbLines;
        }

        strStats = new System.Text.StringBuilder ();
        totalFiles = stats.Count;
        totalLines = iTotalNbLines;

        foreach (File f in stats)
        {
            strStats.Append (f.name.Substring (iLengthOfRootPath + 1, f.name.Length - iLengthOfRootPath - 1) + " --> " + f.nbLines + "\n");
        }
    }

    static void ProcessDirectory (ArrayList stats, string dir)
    {
        string[] strArrFiles = System.IO.Directory.GetFiles (dir, "*.cs");
        foreach (string strFileName in strArrFiles)
            ProcessFile (stats, strFileName);

        strArrFiles = System.IO.Directory.GetFiles (dir, "*.js");
        foreach (string strFileName in strArrFiles)
            ProcessFile (stats, strFileName);

        string[] strArrSubDir = System.IO.Directory.GetDirectories (dir);
        foreach (string strSubDir in strArrSubDir)
            ProcessDirectory (stats, strSubDir);
    }

    static void ProcessFile (ArrayList stats, string filename)
    {
        System.IO.StreamReader reader = System.IO.File.OpenText (filename);
        int iLineCount = 0;
        while (reader.Peek () >= 0)
        {
            reader.ReadLine ();
            ++iLineCount;
        }
        stats.Add (new File (filename, iLineCount));
        reader.Close ();
    }
}