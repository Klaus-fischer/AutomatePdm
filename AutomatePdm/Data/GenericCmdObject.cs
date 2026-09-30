// <copyright file="GenericCmdObject.cs" company="SIM Automation">
// Copyright (c) SIM Automation. All rights reserved.
// </copyright>

namespace AutomatePdm.Data;

using EPDM.Interop.epdm;

public class GenericCmdObject : BaseCmdObject
{
    public int ObjectID1 => this.CmdData.mlObjectID1;

    public int ObjectID2 => this.CmdData.mlObjectID2;

    public int ObjectID3 => this.CmdData.mlObjectID3;

    public int ObjectID4 => this.CmdData.mlObjectID4;

    public string StrData1 => this.CmdData.mbsStrData1 ?? string.Empty;

    public string StrData2 => this.CmdData.mbsStrData2 ?? string.Empty;

    public string StrData3 => this.CmdData.mbsStrData3 ?? string.Empty;

    public int LongData1 => this.CmdData.mlLongData1;

    public int LongData2 => this.CmdData.mlLongData2;

    public int LongData3 => this.CmdData.mlLongData3;

    public object Extra => this.CmdData.mpoExtra;
}

public class TaskRunCmdObject : BaseCmdObject
{
    public IEdmTaskInstance Instance => (IEdmTaskInstance)this.CmdData.mpoExtra;

    public int SelectedObjectId => this.CmdData.mlObjectID1;

    public int ParentFolderId => this.CmdData.mlObjectID2;

    public string FullPath => this.CmdData.mbsStrData1;

    public string ConfigurationName => this.CmdData.mbsStrData2;

    public EdmObjectType ObjectType => (EdmObjectType)this.CmdData.mlLongData1;

    public int LocalVersionNumber => this.CmdData.mlLongData2;
}

public class TaskSetupCmdObject : GenericCmdObject
{
    public IEdmTaskProperties Properties => (IEdmTaskProperties)this.CmdData.mpoExtra;

    public bool SupportsScheduling
    {
        get => ((EdmTaskFlag)this.Properties.TaskFlags).HasFlag(EdmTaskFlag.EdmTask_SupportsScheduling);
        set
        {
            if (value)
            {
                this.Properties.TaskFlags |= (int)EdmTaskFlag.EdmTask_SupportsScheduling;
            }
            else
            {
                this.Properties.TaskFlags &= ~(int)EdmTaskFlag.EdmTask_SupportsScheduling;
            }
        }
    }
}