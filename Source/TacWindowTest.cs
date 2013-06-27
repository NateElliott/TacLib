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
    // Set debug according to setting in part.cfg
    [KSPField]
    public bool debug = false;

    private MyWindow mainWindow = new MyWindow();

    // Fired first - this is at KSP load-time (When the loading bar hits a part with this mod)
    public override void OnAwake()
    {
        base.OnAwake();
        if (debug) Debug.Log("[TWT] Awake");
    }

    // Fires at multiple times, but mainly when scene loads - node contains scene ConfigNode data (all craft in savegame)
    // IMPORTANT! This also fires at KSP load-time. DO NOT try and start the GUI here.
    public override void OnLoad(ConfigNode node)
    {
        base.OnLoad(node);
        if (debug) Debug.Log("[TWT] Load");
        // Load settings for mainWindow
        mainWindow.Load(node);
    }

    // Fired when scene containing part Saves (Ends)
    public override void OnSave(ConfigNode node)
    {
        base.OnSave(node);
        if (debug) Debug.Log("[TWT] Save");
        mainWindow.Save(node);
    }

    // Fired once, when a scene starts containing the part
    public void Start()
    {
        if (debug) Debug.Log("[TWT] Start");
        mainWindow.SetVisible(mainWindow.uistatus.ShowOnStartup);
    }

    // Fires ?every frame? while the GUI is active
    public void OnGUI()
    {

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
    public class UIStatus
    {
        public bool ShowOnStartup = true;
    }
    public UIStatus uistatus = new UIStatus();


    public MyWindow()
        : base("My Window")
    {
        // Force default size
        windowPos = new Rect(60, 60, 400, 400);
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
        uistatus.ShowOnStartup = GUILayout.Toggle(uistatus.ShowOnStartup, "Show on StartUp");
        GUILayout.EndVertical();
    }

    public override void Load(ConfigNode node)
    {
        // Load base settings from global
        var configFilename = IOUtils.GetFilePathFor(this.GetType(), "TacWindowTest.cfg");
        ConfigNode config = ConfigNode.Load(configFilename);

        // Merge with per-ship settings
        if (config != null) config.CopyTo(node);
 
        // Apply settings
        base.Load(node);

        // Set uistatus.ShowOnStartup according to setting
        if (node.HasNode(GetConfigNodeName()))
        {
            var tmp = node.GetNode(GetConfigNodeName());

            uistatus.ShowOnStartup = Utilities.GetValue(tmp, "showonstartup", uistatus.ShowOnStartup);
        }
    }

    public override void Save(ConfigNode node)
    {
        // Start with fresh node
        var configFilename = IOUtils.GetFilePathFor(this.GetType(), "TacWindowTest.cfg");
        ConfigNode config = new ConfigNode();

        // Add Window information to node
        base.Save(config);

        // Add custom info to the WINDOW settings
        config.GetNode(GetConfigNodeName()).AddValue("showonstartup", uistatus.ShowOnStartup);

        // Save global settings
        config.Save(configFilename);

        // Save Per-Ship settings
        config.CopyTo(node);
    }
}

