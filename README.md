[Marketplace](https://www.unrealengine.com/marketplace/en-US/horizon-quest) 

[![Build Status](https://dev.azure.com/hsgame/UE4HorizonPlugin/_apis/build/status/HorizonQuest/HorizonQuestDemo-Shipping-CI?repoName=HorizonQuestDemo&branchName=main)](https://dev.azure.com/hsgame/UE4HorizonPlugin/_build/latest?definitionId=51&repoName=HorizonInteractDemo&branchName=main)

public feed: nuget.org  

[![nuget.org package in feed in ](https://img.shields.io/nuget/v/HorizonQuestDemo.svg)](https://www.nuget.org/packages/HorizonQuestDemo/)
  

Note: 

main branch may be unstable since it is in development, please switch to tags, for example: editor/4.26.0.1

How to Run Demo Project before purchase:(Only for Win64 editor build, no source code)
1. Double click install_game_package_from_nuget_org.cmd, and check if UE4Editor-*.dll are installed to Binaries\Win64 and Plugins\HorizonInteractDemo\Binaries\Win64\
2. Double click HorizonQuestDemo.uproject  

  
----------------------------------------------
              HorizonQuest
                 4.26.0
         http://dorgon.horizon-studio.net
          	dorgonman@hotmail.com
----------------------------------------------
   
-----------------------
System Requirements
-----------------------

Supported UnrealEngine version: 4.26
 

-----------------------
Installation Guide
-----------------------

If you want to use plugins in C++, you should add associated module to your project's 
YOUR_PROJECT.Build.cs:
PublicDependencyModuleNames.AddRange(new string[] { "HorizonQuest"});

-----------------------
User Guide: Quick Start
-----------------------


  
-----------------------
Technical Details
-----------------------

Features:


Code Modules: (Please include a full list of each Plugin module and their module type (Runtime, Editor etc.))

 HorizonQuest (Runtime)


Network Replicated: False  

Supported Development Platforms: Win64, Mac, Linux  

Supported Target Build Platforms: All Platforms  

Tested Platform: Win64  

Documentation: https://github.com/dorgonman/HorizonQuestDemo  

Example Project: https://github.com/dorgonman/HorizonQuestDemo  



-----------------------
What does your plugin do/What is the intent of your plugin
-----------------------  



[DemoVideo](https://youtu.be/wdclGx1IIwQ)  
[TutorialVideo](https://www.youtube.com/watch?v=l-WCsGpg_fo&feature=youtu.be)
	
-----------------------
Contact and Support
-----------------------

email: dorgonman@hotmail.com  

-----------------------
 Version History
-----------------------

*4.26.0  

        NEW: 