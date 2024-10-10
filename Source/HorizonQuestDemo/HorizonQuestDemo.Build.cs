// Copyright Epic Games, Inc. All Rights Reserved.
using System;
using System.IO;
using UnrealBuildTool;

public class HorizonQuestDemo : ModuleRules
{
	public HorizonQuestDemo(ReadOnlyTargetRules Target) : base(Target)
	{
		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

		PublicDependencyModuleNames.AddRange(new string[] { 
			"Core", "CoreUObject", "Engine", "InputCore",
			"HorizonQuest"});
		string ProjectPath = Path.GetFullPath(Path.Combine(ModuleDirectory, "../../"));
		if(Target.ProjectFile != null)
		{
			ProjectPath = Path.GetDirectoryName(Target.ProjectFile.ToString());

		}
         // https://docs.unrealengine.com/en-US/Platforms/Mobile/UnrealPluginLanguage/index.html
        AdditionalPropertiesForReceipt.Add("AndroidPlugin", Path.Combine(ProjectPath, "Source", "Game_UPL.xml"));
	}
}
