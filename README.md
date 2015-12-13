# WoWCreaturePathMapper
This can be used for plotting the paths of creatures and NPCs for World of Warcraft private servers.
There are two parts to this tool:

1) World of Warcraft Addon: CoordinatesRecorder

2) Convert to SQL tool: WoWCoordsToSQLScript

### TOOL BUILT ON/WITH
---------------
Programming language:

WoW Addon: LUA script

Convert to SQL tool: C#

IDE/Editors: Visual Studio 2013 and Notepad++

O/S: MS Windows 7

### REASON FOR BEING
---------------
Plotting paths for creatures and NPCs in World of Warcraft can be a time consuming and very, very mundane experience, 
so I decided to create a tool to significantly improve the situation.

Now, with this tool, you can move freely along the path you wish to set out for the creature or NPC, clicking on a 
button as you go to record the points of the path. Then once done, you just load it up into the to SQL converter to
create script that can be used to update the relevant table for creature movement (e.g. MaNGOS: creature_movement).
