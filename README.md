
![DesignQuestGraph](./ScreenShot/HorizonQuest_ScreenShot_DesignQuestGraph.png)  



Note: 

main branch may be unstable since it is in development, please switch to tags, for example: editor/4.27.0.1

  
----------------------------------------------
               HorizonQuestDemo
                 4.27.0
          	dorgonman@hotmail.com
----------------------------------------------
   
-----------------------
System Requirements
-----------------------

Supported UnrealEngine version: 4.27
 

-----------------------
Installation Guide
-----------------------

If you want to use plugins in C++, you should add associated module to your project's 
YOUR_PROJECT.Build.cs:
PublicDependencyModuleNames.AddRange(new string[] { "HorizonQuest", "HorizonQuestFlag" });

-----------------------
User Guide: Quick Start
-----------------------

1. Create DataTable with FHorizonQuestTableRow and Add Quest to it.

  ![Create Quest DataTable](./ScreenShot/HorizonQuest_ScreenShot_CreateDataTable.png)  

  ![Design Quest DataTable](./ScreenShot/HorizonQuest_ScreenShot_DesignDataTable.png) 

2. Create QuestGraph and Design Quest Dependency.

![CreateQuestGraph](./ScreenShot/HorizonQuest_ScreenShot_CreateQuestGraph.png)  

![DesignQuestGraph](./ScreenShot/HorizonQuest_ScreenShot_DesignQuestGraph.png)  

3. Add HorizonQuestManager and HorizonQuestFlagManager Component to your PlayerState.

![AddManager1](./ScreenShot/HorizonQuest_ScreenShot_AddManager1.png)  

![AddManager2](./ScreenShot/HorizonQuest_ScreenShot_AddManager2.png)  

4. Then you can Accept/Complete Quest or Add QuestFlag using managers.

![AddFlag](./ScreenShot/HorizonQuest_ScreenShot_AcceptCompleteQuest_AddFlag.png)  


Following screenshot is some Functions in QuestManagers.

![QuestManagerFunctions](./ScreenShot/HorizonQuest_ScreenShot_QuestManager1.png)  
![QuestManagerFunctions](./ScreenShot/HorizonQuest_ScreenShot_QuestManager2.png)  


Following screenshot is some Functions in QuestFlagManagers.

![QuestFlagManager](./ScreenShot/HorizonQuest_ScreenShot_QuestFlagManager.png)  


-----------------------
User Guide: How to define QuestRequirement
-----------------------

1. Create Blueprint inheritted from HorizonQuestRequirement. 

2. Override BP_CheckRequirement and Implement Check Logic: Here we check if flag count large than expected amount that defined in RequirementData.

![RequirementLogic](./ScreenShot/HorizonQuest_ScreenShot_QuestRequirement.png)  

3. Assign Requirement and RequirementData to Quest defined in Quest DataTable: Here we Set CompleteRequirement will check if Flag_SQ002_Step1_1 count larger than 5.

![RequirementData](./ScreenShot/HorizonQuest_ScreenShot_QuestRequirement_Data.png) 

-----------------------
User Guide: How to define QuestReward
-----------------------

1. Create Blueprint inheritted from HorizonQuestReward. 

2. Override BP_AcceptReward and Implement Accept Logic: Here we add player score by the amount that defined in RewardData.

![RewardLogic](./ScreenShot/HorizonQuest_ScreenShot_QuestReward.png)  

3. Assign Reward and RewardData to Quest defined in Quest DataTable: Here we Add Score 1 when Quest Accepted and add score 100 when Quest success.

![RewardData](./ScreenShot/HorizonQuest_ScreenShot_QuestReward_Data.png) 


-----------------------
User Guide: How extends QuestGraphNode with C++ and Add Game Specific UPROPERTY
-----------------------

1. In {YourGameProject}.Build.cs, add HorizonQuest to  PublicDependencyModuleNames.

2. Create New C++ that extends from UHorizonQuestGraphNode, ex: UHorizonQuestGameDemoQuestGraphNode

3. Add your Game Specific UPROPERTY, ex: Integrate QuestSystem with HorizonDialogueScene

        
          UCLASS()
          class HORIZONQUESTDEMO_API UHorizonQuestGameDemoQuestGraphNode : public UHorizonQuestGraphNode
          {
            GENERATED_BODY()
            
          public:  
         
                UPROPERTY(EditAnywhere, BlueprintReadOnly, meta = (MetaClass = "HorizonDialogueScene"), Category = "_Game")
                TArray<FSoftClassPath> PreAcceptDialogueSceneClassPathList;
            
                UPROPERTY(EditAnywhere, BlueprintReadOnly, meta = (MetaClass = "HorizonDialogueScene"), Category = "_Game")
                TArray<FSoftClassPath> InprogressDialogueSceneClassPathList;
            
                UPROPERTY(EditAnywhere, BlueprintReadOnly, meta = (MetaClass = "HorizonDialogueScene"), Category = "_Game")
                TArray<FSoftClassPath> AcceptFrom_InprogressDialogueSceneClassPathList;
            
                UPROPERTY(EditAnywhere, BlueprintReadOnly, meta = (MetaClass = "HorizonDialogueScene"), Category = "_Game")
                TArray<FSoftClassPath> CompleteBy_InprogressDialogueSceneClassPathList;
            
                UPROPERTY(EditAnywhere, BlueprintReadOnly, meta = (MetaClass = "HorizonDialogueScene"), Category = "_Game")
                TArray<FSoftClassPath> PreCompleteDialogueSceneClassPathList;

                UPROPERTY(EditAnywhere, BlueprintReadOnly, meta = (MetaClass = "HorizonDialogueScene"), Category = "_Game")
                TArray<FSoftClassPath> AcceptedDialogueSceneClassPathList;

                UPROPERTY(EditAnywhere, BlueprintReadOnly, meta = (MetaClass = "HorizonDialogueScene"), Category = "_Game")
                TArray<FSoftClassPath> CompletedDialogueSceneClassPathList;
            
          };

4. In QuestGraph, change NodeClass to UHorizonQuestGameDemoQuestGraphNode you just created.

![QuestNode Customization](./ScreenShot/HorizonQuest_ScreenShot_QuestNode_Customization.png) 

5. Now you can edit UPROPERTY you just add in QuestGraph.

![DialoguePlugin Integration](./ScreenShot/HorizonQuest_ScreenShot_DialoguePlugin_Integration.png) 

Note: You can also customize node using BP with similar process, you just need to create new BP type that extend from HorizonQuestGraphNode.

![BP_QuestNode Customization](./ScreenShot/HorizonQuest_ScreenShot_BP_QuestNode_Customization.png) 
-----------------------
User Guide: More Advanced Usage
-----------------------

Check Automation Tests in Plugin for more advanced usage:
HorizonQuest\Source\HorizonQuest\Private\Test\HorizonQuest.spec.cpp

-----------------------
Technical Details
-----------------------

This plugin is inspired by GDC talk [Building Non-Linear Narratives in Horizon: Zero Dawn](https://www.youtube.com/watch?v=ykPZcG8_mPU) and the graph editor is modified from [GenericGraph, MIT License](https://github.com/jinyuliao/GenericGraph).

The goal of this plugin is to provoide a general purpose Quest System that can support non-linear story telling. 

Although this plugin didn't implement a Dialogue System with it, but it was designed in mind with flexibility for customization with game project, so can be integrated with any other systems you like. 

Features:

1. QuestGraph systems that can define non-linear depencency of each Quests.

2. Support Accept/Complete QuestRequirement and QuestReward.

3. Flexiable callbacks that can be used to customize your games. ex: OnAcceptQuestEvent, OnCompleteQuestEvent, OnAcceptQuestRewardEvent, OnQuestStateChangedEvent, OnDropQuestEvent
  
4. Additional QuestFlagSystem that help designer record any gameplay flag that can be used with QuestRequirement to check if a Quest can be Accepted/Completed.

5. QuestTreeView Menu Widgets and Debug UI: Example implementation for showing Quests in UMG Widgets.

Code Modules:

HorizonQuest (Runtime)
HorizonQuestEditor (Editor)


Network Replicated: False  

Supported Development Platforms: Win64, Mac, Linux  

Supported Target Build Platforms: All Platforms  

Tested Platform: Win64  

Documentation: 

Example Project: 



[DemoVideo](https://youtu.be/ju3tL1lv7lU)  
[QuickStartVideo](https://youtu.be/mANMYY476mM)

-----------------------
What does your plugin do/What is the intent of your plugin
-----------------------  

The goal of this plugin is to provide a General Quest Graph system for Games.

-----------------------
Contact and Support
-----------------------

email: dorgonman@hotmail.com

-----------------------
 Version History
-----------------------

*4.27.0  

        NEW: First Version including core features.  