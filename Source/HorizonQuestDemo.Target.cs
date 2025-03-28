// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;
using System.Collections.Generic;

public class HorizonQuestDemoTarget : TargetRules
{
	public HorizonQuestDemoTarget(TargetInfo Target) : base(Target)
	{
		Type = TargetType.Game;
		DefaultBuildSettings = BuildSettingsVersion.Latest;
		IncludeOrderVersion = EngineIncludeOrderVersion.Latest;
        //bOverrideBuildEnvironment = true;
        //GlobalDefinitions.Add("HORIZON_PLUGIN_ENABLE_FAST_TARRAY_REPLICATION=1");
        ExtraModuleNames.Add("HorizonQuestDemo");
        //{
        //    bUsePCHFiles = false;
        //    bUseSharedPCHs = false;
        //    bUseUnityBuild = false;
        //}
    }
}
