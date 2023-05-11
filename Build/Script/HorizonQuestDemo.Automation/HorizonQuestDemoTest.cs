// Copyright 1998-2019 Epic Games, Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gauntlet;
using System.IO;
using System.IO.Compression;

namespace UnrealGame
{

	public class HorizonQuestDemoTestConfig : UnrealTestConfiguration
	{
		/// <summary>
		/// Map to use
		/// </summary>

		
		[AutoParam]
		public string AdditionalCommandLine = "";
		
		[AutoParam]
		public string ExecCmds = "automation RunTests StartsWith:Plugin+StartsWith:_Game+StartsWith:Project.Functional Tests;Quit";
        

        /// <summary>
        /// Applies these options to the provided app config
        /// </summary>
        /// <param name="AppConfig"></param>
        public override void ApplyToConfig(UnrealAppConfig AppConfig, UnrealSessionRole ConfigRole, IEnumerable<UnrealSessionRole> OtherRoles)
		{
			base.ApplyToConfig(AppConfig, ConfigRole, OtherRoles);

			AppConfig.CommandLine += " -ExecCmds=\"" + ExecCmds + "\" -FORCELOGFLUSH";
			AppConfig.CommandLine += AdditionalCommandLine;
			
	
		}
	}
	public class HorizonQuestDemoTest : UnrealTestNode<HorizonQuestDemoTestConfig>
	{
		public HorizonQuestDemoTest(UnrealTestContext InContext) : base(InContext)
		{
		}

		public override HorizonQuestDemoTestConfig GetConfiguration()
		{
			// just need a single client
			HorizonQuestDemoTestConfig Config = base.GetConfiguration();
			Config.MaxDuration = 5 * 600;		// 5min should be plenty
			int ClientCount = Context.TestParams.ParseValue("numclients", 1);
			bool WithServer = Context.TestParams.ParseParam("server");

			if (ClientCount > 0)
			{
				Config.RequireRoles(UnrealTargetRole.Client, ClientCount);
			}

			if (WithServer)
			{
				Config.RequireRoles(UnrealTargetRole.Server, 1);
			}

			UnrealTestRole ClientRole = Config.RequireRole(UnrealTargetRole.Client);

			//ClientRole.Controllers.Add("HorizonQuestDemo");
			return Config;
		}

	}

}
