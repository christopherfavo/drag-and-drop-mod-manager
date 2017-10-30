# Dark Souls Drag and Drop Mod Manager
A program that makes installing mods for Dark Souls easier.

Requires the .NET framework to work properly.

File types that work with DSDAD:
* FSB (sound mods)
* MSB (enemy and/or object placement mods)

Any DCX files found in the game:
* *.PARAMBND.DCX (parameter mods, such as [Aggression mod](http://www.nexusmods.com/darksouls/mods/1265/?), [Custom Starting Classes](http://www.nexusmods.com/darksouls/mods/1215/?), [All enemies are Gravelord](http://www.nexusmods.com/darksouls/mods/1176/))
* *.EMEVD.DCX (event mods, such as HotPocketRemix's [Estus Quest](https://github.com/HotPocketRemix/DSEventScriptTools/tree/master/Mods/EstusQuest))
* *.PARTSBND.DCX (modelswap mods, such as [Crown of Dusk Replacer](https://www.nexusmods.com/darksouls/mods/1344/?))
* *.MSGBND.DCX (text mods, such as [Improved texts](https://www.nexusmods.com/darksouls/mods/1198/?), or custom translations.) 
* *.ANIBND.DCX (animation mods)

for example.

Changlog:
v1.0.1
------
* Fixed an issue where text mods would only show up in-game if your language was set to English.
* Fixed a similar issue for the following files: "DSFont24.ccm.dcx", "DSFont24.tpf.dcx", "TalkFont24.ccm.dcx", "TalkFont24.tpf.dcx" and "menu_local.tpf.dcx".
* Added an option to load mods from "DATA\dadmod" even if the Mod Setup isn't empty.
* Added support for DSFix texture mods. Note that "enableTextureOverride" still has to be set manually, for the mods to show up.

v1.0.2
------
* Fixed a minor error where special case mods loaded from "DATA\dadmod" would not be handled as special cases.
* Revamped the crash handling: auto-backup is safer, and restoration of the bdt archives is much faster.
