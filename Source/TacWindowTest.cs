using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tac;
using KSP.IO;
using UnityEngine;

// A sample usage of TacLib to create a window that remembers state and position ?per-ship? and features a "Show on startup" option

public class TacWindowTest : PartModule
{
    private MyWindow mainWindow = new MyWindow();

    public override void OnAwake()
    {
        base.OnAwake();
        Debug.Log("[TWT] Awake");

    }

    public override void OnLoad(ConfigNode node)
    {
        base.OnLoad(node);
        mainWindow.Load(node);
    }

    public override void OnSave(ConfigNode node)
    {
        base.OnSave(node);
        mainWindow.Save(node);
    }

}

/**
 * Instructions for use:
 *  (1) Create an instance of this class somewhere, preferrably where it will not be deleted/garbage collected.
 *  (2) Call SetVisible(true) to start showing it.
 *  (3) Call Load/Save if you want it to remember its position and size between runs.
 */
class MyWindow : Window<MyWindow>
{
    UIStatus uistatus;

    public class UIStatus
    {
        public bool ShowOnStartup = false;
    }
    //private UIStatus uistatus = new UIStatus();

    //private bool ShowOnStartup = false;


    public MyWindow()
        : base("My Window")
    {
        this.uistatus = new UIStatus();
        Debug.Log("[TWT] Constructor");
    }

    protected override void DrawWindow()
    {
        //Example feature - prevent the window from being shown if the vessel is not prelaunch or landed.
        if (FlightGlobals.fetch && FlightGlobals.fetch.activeVessel != null)
        {
            var situation = FlightGlobals.fetch.activeVessel.situation;
            if (situation == Vessel.Situations.PRELAUNCH || situation == Vessel.Situations.LANDED)
            {
                base.DrawWindow();
            }
        }
    }

    protected override void ConfigureStyles()
    {
        base.ConfigureStyles();

        // Initialize your styles here (optional)
    }

    protected override void DrawWindowContents(int windowId)
    {
        // Put your stuff here
        GUILayout.BeginVertical();
        GUILayout.Box("Hello World");
        GUILayout.EndVertical();
    }

    public override void Load(ConfigNode node)
    {
        Debug.Log("[TWT] Load");
        // Load base settings from global
        var configFilename = IOUtils.GetFilePathFor(this.GetType(), "TacWindowTest.cfg");
        ConfigNode config = ConfigNode.Load(configFilename);

        // Merge with per-ship settings
        config.CopyTo(node);

        // Apply settings
        base.Load(node);

        // Set uistatus.ShowOnStartup according to setting
        
        if (node.HasNode(GetConfigNodeName()))
        {
            var tmp = node.GetNode(GetConfigNodeName());

            //ShowOnStartup = Utilities.GetValue(tmp, "showonstartup", ShowOnStartup);
            uistatus.ShowOnStartup = Utilities.GetValue(tmp, "showonstartup", uistatus.ShowOnStartup);
            Debug.Log("[TWT] uistatus: " + uistatus.ShowOnStartup);
            SetVisible(uistatus.ShowOnStartup);     // Null Reference exception on this line but it prints a valid value to the log - why???
        }

        // Make the UI visible
        //SetVisible(ShowOnStartup);
        //SetVisible(uistatus.ShowOnStartup);

    }

    public override void Save(ConfigNode node)
    {
        // Start with fresh node
        var configFilename = IOUtils.GetFilePathFor(this.GetType(), "TacWindowTest.cfg");
        ConfigNode config = new ConfigNode();

        // Add Window information to node
        base.Save(config);

        // Add custom info to the WINDOW settings
        //config.GetNode(GetConfigNodeName()).AddValue("showonstartup", ShowOnStartup);
        config.GetNode(GetConfigNodeName()).AddValue("showonstartup", uistatus.ShowOnStartup);

        // Save global settings
        config.Save(configFilename);

        // Save Per-Ship settings
        config.CopyTo(node);
    }
}

