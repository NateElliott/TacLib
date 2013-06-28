A library from Thunder Aerospace Corporation (TAC), designed by Taranis Elsu.
Forked by Clive Galway (evilc@evilc.com) to make a Template for PartModule modding
For use with the Kerbal Space Program, http://kerbalspaceprogram.com/

This library is made available under the Attribution-NonCommercial-ShareAlike 3.0 (CC BY-NC-SA
3.0) creative commons license. See the LICENSE.txt file.

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
