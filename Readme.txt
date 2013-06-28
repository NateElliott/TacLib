A library from Thunder Aerospace Corporation (TAC), designed by Taranis Elsu.
Forked by Clive Galway (evilc@evilc.com) to make a Template for PartModule modding
For use with the Kerbal Space Program, http://kerbalspaceprogram.com/

This library is made available under the Attribution-NonCommercial-ShareAlike 3.0 (CC BY-NC-SA
3.0) creative commons license. See the LICENSE.txt file.

Features:
* A basic template that lets you start coding KSP PartModule UIs with ease.
* Demonstrates adding an option to the right-click context menu of a part, and how to make it toggle on/off.
* Demo menu item opens a window to serve as the GUI for your mod.
* Window position and size stored on two levels: per-mod and per-craft (within a savegame).
* This means new craft use the same position as was used in the last save, but each craft in a save remembers its own position.
* Window shown/hidden appropriately. Disappears during Escape menu, in editor, when you switch to another craft etc.
* Cycling through craft in a game with [ and ] opens and closes the window as appropriate (Multiple copies of same mod GUI with different states).
* Demonstrations to help you get your head around the quirky way KSP UIs work.
* Window can be resized, and can be limited to horizontal or vertical only resizing.

Usage:
1) Set up Visual Studio as per the instructions here:
http://wiki.kerbalspaceprogram.com/wiki/Plugins

2) Make folder structure in KSP folder
a) Make TacWindowTest folder in your KSP Gamedata folder
b) inside that, make Plugins and Parts folders

3) Download this project
a) Open the sln in Visual Studio and set references as explained in step 1
b) Project > Properties > Build Events, in the Post-Build box, paste something like this:
copy "$(TargetPath)" "C:\Games\KSP\GameData\TacWindowTest\Plugins"
(Change the path to KSP as appropriate)

4) Edit a part to make it use the mod
a) Copy the "RCS block" folder from GameData\Squad\Parts\Utility to GameData\TacWindowTest\Parts
b) Edit the part.cfg
Change name field at start to anything unique
Change title to something also as that is what it will be called in editor
At the end, BEFORE the last }, paste the following:
MODULE
{
  name = TacWindowTest
	debug = True
}

5) Hit "Build" in Visual Studio, if you did step 3b right, the DLL should appear in GameData\TacWindowTest\Plugins

6) Start KSP, build a ship using your modified part, and launch - the window should appear.

7) If so, congratulations - start editing TacWindowTest and have a play!
