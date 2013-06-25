using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tac;
using KSP.IO;
using UnityEngine;

// A sample usage of TacLib to create a window that remembers state and position ?per-ship? and features a "Show on startup" option

public class TWT : PartModule
{
    private MyWindow buildWindow = new MyWindow();

    public override void OnAwake()
    {
        base.OnAwake();
        Debug.Log("[Tac Window Test] OnAwake");
        buildWindow.SetVisible(true);
        ConfigNode config = new ConfigNode();
        buildWindow.Load(config);
    }

    public override void OnSave(ConfigNode node)
    {
        base.OnSave(node);

        Debug.Log("[Tac Window Test] OnSave");

        ConfigNode config = new ConfigNode();
        config.AddValue("test", 1);
        buildWindow.Save(config);
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
        //Q: Does the TacLib Save method actually save anything or just assemble the data for saving?
        //Q: Is it designed for per-ship saves?

        //if (node.HasNode("testnode"))
        //{
        //ConfigNode windowConfig = node.GetNode("testnode");

        /*
        windowPos.x = Utilities.GetValue(windowConfig, "x", windowPos.x);
        windowPos.y = Utilities.GetValue(windowConfig, "y", windowPos.y);
        windowPos.width = Utilities.GetValue(windowConfig, "width", windowPos.width);
        windowPos.height = Utilities.GetValue(windowConfig, "height", windowPos.height);

        bool newValue = Utilities.GetValue(windowConfig, "visible", true);
        */
        PluginConfiguration config = PluginConfiguration.CreateForType<TWT>();

        Rect wp = config.GetValue<Rect>("Window Position");
        windowPos.x = wp.xMin;
        windowPos.y = wp.y;
        windowPos.width = wp.width;
        windowPos.height = wp.height;

        SetVisible(true);
        //}
    }

    public override void Save(ConfigNode node)
    {
        base.Save(node);

        ConfigNode windowConfig;
        if (node.HasNode("testnode"))
        {
            windowConfig = node.GetNode("testnode");
        }
        else
        {
            windowConfig = new ConfigNode("testnode");
            node.AddNode(windowConfig);
        }

        windowConfig.AddValue("visible", true);
        windowConfig.AddValue("x", windowPos.x);
        windowConfig.AddValue("y", windowPos.y);
        windowConfig.AddValue("width", windowPos.width);
        windowConfig.AddValue("height", windowPos.height);

        PluginConfiguration config = PluginConfiguration.CreateForType<TWT>();
        config.SetValue("Window Position", new Rect(windowPos.x, windowPos.y, windowPos.width, windowPos.height));
        config.SetValue("Show Build Menu on StartUp", 1);
        config.save();
    }
}

