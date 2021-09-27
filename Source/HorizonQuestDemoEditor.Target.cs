// Copyright Epic Games, Inc. All Rights Reserved.

using UnrealBuildTool;
using System.Collections.Generic;

public class HorizonQuestDemoEditorTarget : TargetRules
{
	public HorizonQuestDemoEditorTarget(TargetInfo Target) : base(Target)
	{
		Type = TargetType.Editor;
		DefaultBuildSettings = BuildSettingsVersion.V2;
		//bOverrideBuildEnvironment = true;
		//GlobalDefinitions.Add("HORIZON_PLUGIN_ENABLE_FAST_ARRAY_REPLICATION=1");
		ExtraModuleNames.Add("HorizonQuestDemo");
	}
}
