# Project
This Project is for developing an Plugin for Solidworks-PDM to enable advanced automation.

The Addin should use configurable Pipelines to automate the behaviour.
Trigger->Condition->Command

It uses [Hooks](https://help.solidworks.com/2024/english/api/epdmapi/EPDM.Interop.epdm~EPDM.Interop.epdm.IEdmCmdMgr5~AddHook.html), 
[Menu commands](https://help.solidworks.com/2024/english/api/epdmapi/EPDM.Interop.epdm~EPDM.Interop.epdm.IEdmCmdMgr5~AddCmd.html), 
and [Tasks](https://help.solidworks.com/2024/english/api/epdmapi/Tasks.htm?id=4.5.9) as Trigger.

Pipeline steps for 
- enriches Information (such as variable values and Status) of an item. 
- to filter items from execution.
- to find depencencies (parent or Child).
- Execute Tasks, Change State, Prevent Changing State...

The order should be changeable.

Everything should be configureable over an GUI inside the PDM Adminconsole.

# Use Cases

- Change State to related Files
1. File Changes from Release to Review
2. Get Dependendent Files (*PDF and other neutral formats*)
3. Change their state too

- Change State from neutral formats after commit.
1. File was commited
2. Filter: only neutral formats
3. Filter: source file must be in state release (*if neutral file server tooks too long and source file is in Review again*)
4. Change state of neutral format to release

- Set variables to project based of properties of the **Main Assembly** from the project. 
1. File was commited
2. Filename ends with the project name [^1]
3. Set Project properties *name*, *comment*, *State*...

[^1]: this is how we structure our construction projects, we dont use generated filenames
