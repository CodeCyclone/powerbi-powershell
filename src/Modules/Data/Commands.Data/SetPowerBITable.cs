﻿/*
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

using System;
using System.Management.Automation;
using Microsoft.PowerBI.Common.Api.Datasets;
using Microsoft.PowerBI.Common.Api.Workspaces;
using Microsoft.PowerBI.Common.Client;

namespace Microsoft.PowerBI.Commands.Data
{
    [Cmdlet(CmdletVerb, CmdletName, DefaultParameterSetName = DatasetIdParameterSetName)]
    [OutputType(typeof(Dataset))]
    public class SetPowerBITable : PowerBIClientCmdlet
    {
        public const string CmdletVerb = VerbsCommon.Set;
        public const string CmdletName = "PowerBITable";

        #region ParameterSets
        private const string DatasetParameterSetName = "Dataset";
        private const string DatasetIdParameterSetName = "DatasetId";
        #endregion

        #region Parameters

        [Parameter(Mandatory = true)]
        public Table Table { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = DatasetIdParameterSetName)]
        public Guid DatasetId { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = DatasetParameterSetName)]
        public Dataset Dataset { get; set; }

        [Alias("GroupId")]
        [Parameter(Mandatory = false, ParameterSetName = DatasetIdParameterSetName)]
        [Parameter(Mandatory = false, ParameterSetName = DatasetParameterSetName)]
        public Guid WorkspaceId { get; set; }

        [Alias("Group")]
        [Parameter(Mandatory = false, ParameterSetName = DatasetIdParameterSetName, ValueFromPipeline = true)]
        [Parameter(Mandatory = false, ParameterSetName = DatasetParameterSetName, ValueFromPipeline = true)]
        public Workspace Workspace { get; set; }

        #endregion

        #region Constructors
        public SetPowerBITable() : base() { }

        public SetPowerBITable(IPowerBIClientCmdletInitFactory init) : base(init) { }
        #endregion

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        public override void ExecuteCmdlet()
        {
            if (this.Dataset != null)
            {
                this.DatasetId = this.Dataset.Id;
            }

            if (this.Workspace != null)
            {
                this.WorkspaceId = this.Workspace.Id;
            }

            object result;
            using (var client = this.CreateClient())
            {
                if (this.WorkspaceId != default)
                {
                    result = client.Datasets.UpdateTable(this.Table, this.DatasetId, this.WorkspaceId);
                }
                else
                {
                    result = client.Datasets.UpdateTable(this.Table, this.DatasetId);
                }
            }
            
            this.Logger.WriteObject(result, true);
        }
    }
}
