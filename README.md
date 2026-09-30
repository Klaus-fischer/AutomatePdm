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
