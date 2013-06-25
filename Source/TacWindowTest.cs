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
        Debug.Log("[Tac Window Test] OnAwake");
        mainWindow.SetVisible(true);
        ConfigNode config = new ConfigNode();
        mainWindow.Load(config);
    }

    public override void OnSave(ConfigNode node)
    {
        base.OnSave(node);

        Debug.Log("[Tac Window Test] OnSave");

        ConfigNode config = new ConfigNode();
        config.AddValue("test", 1);
        mainWindow.Save(config);
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
    public MyWindow()
        : base("My Window")
    {
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
        base.Load(node);
        //Q: Does the TacLib Save method actually save anything or just assemble the data for saving?
        //Q: Is it designed for per-ship saves?

        // Load mod-wide settings from config.xml
        PluginConfiguration config = PluginConfiguration.CreateForType<TacWindowTest>();
        config.load();
        windowPos = config.GetValue<Rect>("Window Position");

        //SetVisible(true);
    }

    public override void Save(ConfigNode node)
    {
        base.Save(node);

        // Save to config.xml
        PluginConfiguration config = PluginConfiguration.CreateForType<TacWindowTest>();
        config.SetValue("Window Position", windowPos);
        //config.SetValue("Show Build Menu on StartUp", 1);
        config.save();
    }
}

