
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

You can have two types of QuestDependency: Accept(Ⓐ Icon) and Complete(Ⓒ Icon). 
The Image above describe following relationship:

+ Accept(Ⓐ) Dependency: means you can only accept MQ002 and SQ002 only after MQ001 is Completed.  
+ Complete(Ⓒ) Dependency: means you can only complete SQ002 only after SQ001 is Complete.
+ No Dependency: Like MQ001 and SQ001, you can accept and complete the quests at any time when QuestRequirement is meet.


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

QuestRequirement is defined using Blueprint that can encapsulate any gameplay logics for Quest Completion test.

1. Create Blueprint inheritted from HorizonQuestRequirement. 

2. Override BP_CheckRequirement and Implement Check Logic: Here we check if flag count large than expected amount that defined in RequirementData.

![RequirementLogic](./ScreenShot/HorizonQuest_ScreenShot_QuestRequirement.png)  

3. Assign Requirement and RequirementData to Quest defined in Quest DataTable: Here we Set CompleteRequirement will check if Flag_SQ002_Step1_1 count larger than 5.

![RequirementData](./ScreenShot/HorizonQuest_ScreenShot_QuestRequirement_Data.png) 


Here has two types of QuestRequirement:

1. AcceptRequirements: A Quest should pass all requirements assigned in DataTable before the quest can be accepted.
2. CompleteRequirements: A Quest should pass all requirements assigned in DataTable before the quest can be completed.



-----------------------
User Guide: How to define QuestReward
-----------------------

QuestReward is defined using Blueprint that can encapsulate any quest reward logics.

1. Create Blueprint inheritted from HorizonQuestReward. 

2. Override BP_AcceptReward and Implement Accept Logic: Here we add player score by the amount that defined in RewardData.

![RewardLogic](./ScreenShot/HorizonQuest_ScreenShot_QuestReward.png)  

3. Assign Reward and RewardData to Quest defined in Quest DataTable: Here we Add Score 1 when Quest Accepted and add score 100 when Quest success.

![RewardData](./ScreenShot/HorizonQuest_ScreenShot_QuestReward_Data.png) 

Here has three types of QuestReward:

1. Quest Accepted: You can give player critial items, for example, a key to open dungeon boss's door.
2. Quest Completed with Success State.
3. Quest Completed with Failed State: You can give player any punishment here.

-----------------------
User Guide: QuestContext
-----------------------

QuestContext is used for Check/Querying if a Quest can be Accept/Completed, 
Your UObject should implement IHorizonQuestContextInterface in order to be used as QuestContext.

Currently Plugin has two types of QuestContext you can assign to Quests in Quest DataTable:

![QuestContext](./ScreenShot/HorizonQuest_ScreenShot_QuestContext.png) 

1. QuestContext_AcceptFrom:  MQ001 can be Accepted from BP_NPC_Demo1, when you trying to Accept the Quest using QuestManager, you should pass BP_NPC_Demo1.
2. QuestContext_CompletedBy: MQ001 can be Completed by BP_NPC_Demo2, when you trying to Complete the Quest using QuestManager, you should pass BP_NPC_Demo2.



-----------------------
User Guide: QuestManager and FlagManager
-----------------------

The best place to put HorizonQuestManager and HorizonQuestFlagManager Component is PlayerState, but if you need to shared Quest progress between multiple player, put Components in GameState or GameMode is another choice. But since GameMode can't be accessed from client in Networked games, so it is depends on game design.

In order to Accept/Complete a Quest, it should pass Dependency(With Optional Quest support), Requirement, and QuestContext checks.


-----------------------
User Guide: QuestStep
-----------------------

QuestStep is just another QuestGraph that can assign to every QuestNode, which means you can recursively defined QuestStep for each QuestNode.
We usually didn't want to designed a Quest that has too much step depth, most quest system I know only support one depth step, but it is depend on game design, this plugin didn't limit depth of QuestStep.

Another hint about QuestStep is you may want to auto accept QuestStep when a Quest begin and start next step when previous step completed.
This plugin support such use case by following two flags in each QuestNode:

AutoAcceptQuestStepOnQuestAccepted: Enabled by default, when a Quest is accepted, it also accept all acceptable steps use given QuestContext.
AutoAcceptNextQuestOnQuestCompleted: Disabled by default, when a Quest is completed, it also accept all acceptable Quests use given QuestContext. You usaually want to enable this in QuestSteps but not in MainQuest.

![QuestStep_AutoAcceptNext](./ScreenShot/HorizonQuest_ScreenShot_QuestStep_AutoAcceptNext.png) 

-----------------------
User Guide: Integrate Quest with Dialogue System
-----------------------

Usually we didn't use QuestSystem alone, it is nessary to provide some extendability so we can integrate with other systems.

This section will show how to extend QuestGraphNode, so we use additional information to integrate with Dialogue System. Here we use HorizonDialogue as example.


1. Extend QuestGraphNode using C++

+ In {YourGameProject}.Build.cs, add HorizonQuest to  PublicDependencyModuleNames.

+ Create New C++ that extends from UHorizonQuestGraphNode, ex: UHorizonQuestGameDemoQuestGraphNode

+ Add your Game Specific UPROPERTY, ex: Integrate QuestSystem with HorizonDialogueScene

        
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

+ In QuestGraph, change NodeClass to UHorizonQuestGameDemoQuestGraphNode you just created.

![QuestNode Customization](./ScreenShot/HorizonQuest_ScreenShot_QuestNode_Customization.png) 

+ Now you can edit UPROPERTY in QuestGraph.

![DialoguePlugin Integration](./ScreenShot/HorizonQuest_ScreenShot_DialoguePlugin_Integration.png) 


+ Enqueue Dialogue in OnInteractStarted or in QuestManager's callbacks: OnAcceptQuestEvent, OnCompleteQuest, etc.

![DialoguePlugin Enqueue](./ScreenShot/HorizonQuest_ScreenShot_IntegrateWithDialogue_EnqueueDialogue.png) 

2. Extend QuestGraphNode using Blueprint

+ The process is similar with C++ version, instead, you only need to create new BP type that extend from HorizonQuestGraphNode and change NodeClass in QuestGraph.

![BP_QuestNode Customization](./ScreenShot/HorizonQuest_ScreenShot_BP_QuestNode_Customization.png) 


-----------------------
HorizonQuest: TestCase
-----------------------
HorizonQuest\Source\HorizonQuest\Private\Test\HorizonQuest.spec.cpp

```
 
static UWorld* GetAnyGameWorld()
{
	UWorld* TestWorld = nullptr;
	const TIndirectArray<FWorldContext>& WorldContexts = GEngine->GetWorldContexts();
	for (const FWorldContext& Context : WorldContexts)
	{
		if (((Context.WorldType == EWorldType::PIE) || (Context.WorldType == EWorldType::Game)) && (Context.World() != NULL))
		{
			TestWorld = Context.World();
			break;
		}
	}

	return TestWorld;
}



BEGIN_DEFINE_SPEC(FHorizonQuestSpec,
	"Plugin.HorizonQuest",
	EAutomationTestFlags::ProductFilter | EAutomationTestFlags::ApplicationContextMask)
	FSoftObjectPath HorizonQuestDemoMapPath {"World'/Game/_HorizonQuestDemo/Map/HorizonQuestDemoMap.HorizonQuestDemoMap'"};
	FSoftObjectPath QuestGraph_Main {"HorizonQuestGraph'/Game/_HorizonQuestDemo/_Subsystem/Quest/Graph/QuestGraph_Main.QuestGraph_Main'"};
	FSoftClassPath	CharacterClassPath_NPC_Demo1{ TEXT("Blueprint'/Game/_HorizonQuestDemo/Character/NPC/BP_NPC_Demo1.BP_NPC_Demo1''") };
	FSoftClassPath	CharacterClassPath_NPC_Demo2{ TEXT("Blueprint'/Game/_HorizonQuestDemo/Character/NPC/BP_NPC_Demo2.BP_NPC_Demo2'") };
	FSoftClassPath	CharacterClassPath_NPC_Demo3{ TEXT("Blueprint'/Game/_HorizonQuestDemo/Character/NPC/BP_NPC_Demo3.BP_NPC_Demo3'") };
	FSoftClassPath	CharacterClassPath_NPC_Demo4{ TEXT("Blueprint'/Game/_HorizonQuestDemo/Character/NPC/BP_NPC_Demo4.BP_NPC_Demo4'") };
	FSoftClassPath	CharacterClassPath_NPC_Demo5{ TEXT("Blueprint'/Game/_HorizonQuestDemo/Character/NPC/BP_NPC_Demo5.BP_NPC_Demo5'") };
	FSoftClassPath	CharacterClassPath_NPC_Demo6{ TEXT("Blueprint'/Game/_HorizonQuestDemo/Character/NPC/BP_NPC_Demo6.BP_NPC_Demo6'") };

	FSoftClassPath	TriggerButtonClassPath_NPC_Demo5{ TEXT("Blueprint'/Game/_HorizonQuestDemo/_Subsystem/Interact/Blueprint/BP_QuestTriggerButton.BP_QuestTriggerButton'") };

	//

	TScriptInterface<IHorizonQuestContextInterface> NPC_Demo1;
	TScriptInterface<IHorizonQuestContextInterface> NPC_Demo2;
	TScriptInterface<IHorizonQuestContextInterface> NPC_Demo3;
	TScriptInterface<IHorizonQuestContextInterface> NPC_Demo4;
	TScriptInterface<IHorizonQuestContextInterface> NPC_Demo5;
	TScriptInterface<IHorizonQuestContextInterface> NPC_Demo6;

	TWeakObjectPtr<UHorizonQuestManagerComponent> QuestManagerWeakPtr;


	TScriptInterface<IHorizonQuestContextInterface> BP_QuestTriggerButton_SQ002_Step1_1;
	TScriptInterface<IHorizonQuestContextInterface> BP_QuestTriggerButton_SQ002_Step1_2;
	TScriptInterface<IHorizonQuestContextInterface> BP_QuestTriggerButton_MQ002_Success;
	
	//TWeakObjectPtr<AHorizonQuestTriggerVolume> TriggerVolume_QuestAll;

	FDelegateHandle PostLoadMapWithWorldHandle;

END_DEFINE_SPEC(FHorizonQuestSpec)

void FHorizonQuestSpec::Define()
{
	FHorizonQuestInfo QuestInfo_MQ001;
	QuestInfo_MQ001.QuestRowHandle.RowName = "MQ001";

	FHorizonQuestInfo QuestInfo_MQ002;
	QuestInfo_MQ002.QuestRowHandle.RowName = "MQ002";

	FHorizonQuestInfo QuestInfo_SQ001;
	QuestInfo_SQ001.QuestRowHandle.RowName = "SQ001";

	FHorizonQuestInfo QuestInfo_SQ002;
	QuestInfo_SQ002.QuestRowHandle.RowName = "SQ002";


	FHorizonQuestInfo QuestInfo_SQ002_Step1_1;
	QuestInfo_SQ002_Step1_1.QuestRowHandle.RowName = "SQ002_Step1_1";

	FHorizonQuestInfo QuestInfo_SQ002_Step1_2;
	QuestInfo_SQ002_Step1_2.QuestRowHandle.RowName = "SQ002_Step1_2";

	FHorizonQuestInfo QuestInfo_SQ002_Step2;
	QuestInfo_SQ002_Step2.QuestRowHandle.RowName = "SQ002_Step2";

	Describe(HorizonQuestDemoMapPath.GetLongPackageName(), [=]()
	{
		LatentBeforeEach([=](const FDoneDelegate& Done)
		{
			
			AutomationOpenMap(HorizonQuestDemoMapPath.GetLongPackageName());
			UWorld* pCurrentWorld = GetAnyGameWorld();
			auto pCurrentWorldPacakge = pCurrentWorld->GetPackage();
			auto sCurrentWorldPath = pCurrentWorldPacakge->FileName.ToString();
			auto sTestWorldPath = HorizonQuestDemoMapPath.GetLongPackageName();
			if (sTestWorldPath != sCurrentWorldPath)
			{
				PostLoadMapWithWorldHandle = FCoreUObjectDelegates::PostLoadMapWithWorld.AddLambda([pCurrentWorld, Done](UWorld* InLoadedWorld)
				{
					Done.Execute();
				});
			}
			else
			{
				Done.Execute();
			}
			
		});

		BeforeEach([=]()
		{

			auto pWorld = GetAnyGameWorld();

			TArray<AActor*> actors;
			UGameplayStatics::GetAllActorsWithInterface(pWorld, UHorizonQuestContextInterface::StaticClass(), actors);




			for (auto& it : actors)
			{
				FSoftClassPath currentClassPath{ it->GetClass() };
				auto currentClassPathPackageName = currentClassPath.GetLongPackageName();
				auto actorName = it->GetName();
				if (currentClassPathPackageName == CharacterClassPath_NPC_Demo1.GetLongPackageName())
				{
					NPC_Demo1 = it;
				}
				else if (currentClassPathPackageName == CharacterClassPath_NPC_Demo2.GetLongPackageName())
				{
					NPC_Demo2 = it;
				}
				else if (currentClassPathPackageName == CharacterClassPath_NPC_Demo3.GetLongPackageName())
				{
					NPC_Demo3 = it;
				}
				else if (currentClassPathPackageName == CharacterClassPath_NPC_Demo4.GetLongPackageName())
				{
					NPC_Demo4 = it;
				}
				else if (currentClassPathPackageName == CharacterClassPath_NPC_Demo5.GetLongPackageName())
				{
					NPC_Demo5 = it;
				}
				else if (currentClassPathPackageName == CharacterClassPath_NPC_Demo6.GetLongPackageName())
				{
					NPC_Demo6 = it;
				}
				else if (actorName == "BP_QuestTriggerButton_SQ002_Step1_1")
				{
					BP_QuestTriggerButton_SQ002_Step1_1 = it;
				}
				else if (actorName == "BP_QuestTriggerButton_SQ002_Step1_1")
				{
					BP_QuestTriggerButton_SQ002_Step1_2 = it;
				}
				else if(actorName == "BP_QuestTriggerButton_MQ002_Success")
				{
					BP_QuestTriggerButton_MQ002_Success = it;
				}
	

			}
			auto pPlayerController = UGameplayStatics::GetPlayerControllerFromID(pWorld, 0);
			QuestManagerWeakPtr = UHorizonQuestLibrary::GetQuestManager(pPlayerController);

			TestNotNull("NPC_Demo1", NPC_Demo1.GetObject());
			TestNotNull("NPC_Demo2", NPC_Demo2.GetObject());
			TestNotNull("NPC_Demo3", NPC_Demo3.GetObject());
			TestNotNull("NPC_Demo4", NPC_Demo4.GetObject());
			TestNotNull("NPC_Demo5", NPC_Demo5.GetObject());
			TestNotNull("NPC_Demo6", NPC_Demo6.GetObject());
			//TestTrue("TriggerVolume_QuestAll", TriggerVolume_QuestAll.IsValid());
			TestNotNull("QuestManager", QuestManagerWeakPtr.Get());
			if (QuestManagerWeakPtr.IsValid())
			{
				QuestManagerWeakPtr->ClearQuestGraphSaveGameData();

			}
		});


		It(QuestGraph_Main.GetLongPackageName() + ".Check if we can accept and finish a quest", [=]()
		{
			auto pQuestGraph = Cast<UHorizonQuestGraph>(QuestGraph_Main.TryLoad());
			TestTrue("pQuestGraph", pQuestGraph != nullptr);
			QuestManagerWeakPtr->SetQuestGraph(pQuestGraph);
			TestTrue("Quest QuestInfo_MQ001 is a valid Quest", QuestManagerWeakPtr->GetQuestState(QuestInfo_MQ001) == EHorizonQuestState::Init);
			TestTrue("Quest QuestInfo_MQ001 Should be acceptable", QuestManagerWeakPtr->AcceptQuest(QuestInfo_MQ001, NPC_Demo1));
			TestTrue("Quest QuestInfo_MQ001 is in progress", QuestManagerWeakPtr->IsQuestInProgress(QuestInfo_MQ001));
			TestTrue("Quest QuestInfo_MQ001 is completed", QuestManagerWeakPtr->CompleteQuest(QuestInfo_MQ001, NPC_Demo2));
			TestTrue("Quest QuestInfo_MQ001 is success", QuestManagerWeakPtr->IsQuestSuccess(QuestInfo_MQ001));

		});


		It(QuestGraph_Main.GetLongPackageName() + ".Check Accept/Complete Context", [=]()
		{
			auto pQuestGraph = Cast<UHorizonQuestGraph>(QuestGraph_Main.TryLoad());
			TestTrue("pQuestGraph", pQuestGraph != nullptr);
			QuestManagerWeakPtr->SetQuestGraph(pQuestGraph);
	
			// Because we provide wrong context, the quest should failed to be accepted, so it should not be in progress state.	
			QuestManagerWeakPtr->AcceptQuest(QuestInfo_MQ001, nullptr);
			QuestManagerWeakPtr->AcceptQuest(QuestInfo_MQ001, NPC_Demo2);
			QuestManagerWeakPtr->AcceptQuest(QuestInfo_MQ001, NPC_Demo3);
			QuestManagerWeakPtr->AcceptQuest(QuestInfo_MQ001, NPC_Demo4);
			QuestManagerWeakPtr->AcceptQuest(QuestInfo_MQ001, NPC_Demo5);
			TestFalse("WrongContext Test: Quest QuestInfo_MQ001 IsQuestInProgress", 
					QuestManagerWeakPtr->IsQuestInProgress(QuestInfo_MQ001));
			

			// Now we accept Quest with correct Quest Context, so the quest should be change it state to InProgress
			QuestManagerWeakPtr->AcceptQuest(QuestInfo_MQ001, NPC_Demo1);
			TestTrue("CorrectContext Test: Quest QuestInfo_MQ001 IsQuestInProgress",
				QuestManagerWeakPtr->IsQuestInProgress(QuestInfo_MQ001));

			// Because we provide wrong context, the quest should failed to be completed
			QuestManagerWeakPtr->CompleteQuest(QuestInfo_MQ001, nullptr, true);
			QuestManagerWeakPtr->CompleteQuest(QuestInfo_MQ001, NPC_Demo1, true);
			QuestManagerWeakPtr->CompleteQuest(QuestInfo_MQ001, NPC_Demo3, true);
			QuestManagerWeakPtr->CompleteQuest(QuestInfo_MQ001, NPC_Demo4, true);
			QuestManagerWeakPtr->CompleteQuest(QuestInfo_MQ001, NPC_Demo5, true);
			TestFalse("WrongContext Test: Quest QuestInfo_MQ001 IsQuestCompleted", QuestManagerWeakPtr->IsQuestCompleted(QuestInfo_MQ001));

			// Now we Complete Quest with correct Quest Context, so the quest should be change it state to Success
			{
				QuestManagerWeakPtr->CompleteQuest(QuestInfo_MQ001, NPC_Demo2, true);
				TestTrue("CorrectContext Test: Quest QuestInfo_MQ001 IsQuestCompleted", QuestManagerWeakPtr->IsQuestSuccess(QuestInfo_MQ001));
			}
		});

		It(QuestGraph_Main.GetLongPackageName() + ".Check if save load quest works", [=]()
		{
			auto pQuestGraph = Cast<UHorizonQuestGraph>(QuestGraph_Main.TryLoad());
			TestTrue("pQuestGraph", pQuestGraph != nullptr);
			QuestManagerWeakPtr->SetQuestGraph(pQuestGraph);

			// Save Current Quest State
			FHorizonQuestArchiveData data;
			QuestManagerWeakPtr->GetArchiveData(data);

			// Before Quest Completed, all quest should not be completed
			TestFalse("Quest QuestInfo_MQ001 IsQuestSuccess", QuestManagerWeakPtr->IsQuestCompleted(QuestInfo_MQ001));
			TestFalse("Quest QuestInfo_SQ001 IsQuestSuccess", QuestManagerWeakPtr->IsQuestCompleted(QuestInfo_SQ001));
			TestFalse("Quest QuestInfo_MQ002 IsQuestSuccess", QuestManagerWeakPtr->IsQuestCompleted(QuestInfo_SQ002));

			// Force Complete all Quests and it's dependence
			QuestManagerWeakPtr->ForceAcceptCompleteQuestAndDependence(QuestInfo_SQ002, true);

			// Now all quest are completed
			TestTrue("Quest QuestInfo_MQ001 IsQuestSuccess", QuestManagerWeakPtr->IsQuestCompleted(QuestInfo_MQ001));
			TestTrue("Quest QuestInfo_SQ001 IsQuestSuccess", QuestManagerWeakPtr->IsQuestCompleted(QuestInfo_SQ001));
			TestTrue("Quest QuestInfo_MQ002 IsQuestSuccess", QuestManagerWeakPtr->IsQuestCompleted(QuestInfo_SQ002));

			// Load Previous saved Quest State
			QuestManagerWeakPtr->SetArchiveData(data);

			// After Load, all quest should not be completed
			TestFalse("Quest QuestInfo_MQ001 IsQuestSuccess", QuestManagerWeakPtr->IsQuestCompleted(QuestInfo_MQ001));
			TestFalse("Quest QuestInfo_SQ001 IsQuestSuccess", QuestManagerWeakPtr->IsQuestCompleted(QuestInfo_SQ001));
			TestFalse("Quest QuestInfo_MQ002 IsQuestSuccess", QuestManagerWeakPtr->IsQuestCompleted(QuestInfo_SQ002));

		});


		It(QuestGraph_Main.GetLongPackageName() + ".Check SQ002 steps Auto Accept/Complete ", [=]()
		{
			auto pQuestGraph = Cast<UHorizonQuestGraph>(QuestGraph_Main.TryLoad());
			TestTrue("pQuestGraph", pQuestGraph != nullptr);
			QuestManagerWeakPtr->SetQuestGraph(pQuestGraph);

			// Complete all SQ002's dependency
			{
				QuestManagerWeakPtr->AcceptQuest(QuestInfo_MQ001, nullptr, true);
				QuestManagerWeakPtr->CompleteQuest(QuestInfo_MQ001, nullptr, true, true);
				QuestManagerWeakPtr->AcceptQuest(QuestInfo_SQ001, nullptr, true);
				QuestManagerWeakPtr->CompleteQuest(QuestInfo_SQ001, nullptr, true, true);
			}
			// Check if bAutoAcceptQuestStepOnQuestAccepted works
			{
				QuestManagerWeakPtr->AcceptQuest(QuestInfo_SQ002, NPC_Demo4);
				TestTrue("Quest QuestInfo_SQ002_Step1_1 is in progress",
					QuestManagerWeakPtr->IsQuestInProgress(QuestInfo_SQ002_Step1_1));
				TestTrue("Quest QuestInfo_SQ002_Step1_2 is in progress",
					QuestManagerWeakPtr->IsQuestInProgress(QuestInfo_SQ002_Step1_2));
			}

			// Trigger Quest Flag 5 time will complete Step1_1, the logic are implemented in 
			// /Game/_HorizonQuestDemo/_Subsystem/Interact/Blueprint/BP_QuestTriggerButton
			for (int32 i = 0; i < 5; ++i)
			{
				IHorizonQuestContextInterface::Execute_TriggerQuest(BP_QuestTriggerButton_SQ002_Step1_1.GetObject());
			}
			//QuestManagerWeakPtr->CompleteQuest(QuestInfo_SQ002_Step1_1, BP_QuestTriggerButton_SQ002_Step1_1, true, false);

			// Check if bAutoAcceptNextQuestOnQuestCompleted works
			TestTrue("Quest QuestInfo_SQ002_Step2 is in progress",
				QuestManagerWeakPtr->IsQuestInProgress(QuestInfo_SQ002_Step2));


			// Try complete SQ002's Step
			QuestManagerWeakPtr->CompleteQuest(QuestInfo_SQ002_Step2, NPC_Demo4, true);

			// If all Quest Step are completed, SuperQuest should also in completed state
			TestTrue("Quest QuestInfo_SQ002 is Completed",
				QuestManagerWeakPtr->IsQuestCompleted(QuestInfo_SQ002));

		});

		It(QuestGraph_Main.GetLongPackageName() + ".Force Complete All Quest.Success", [=]()
		{
			auto pQuestGraph = Cast<UHorizonQuestGraph>(QuestGraph_Main.TryLoad());
			TestTrue("pQuestGraph", pQuestGraph != nullptr);
			QuestManagerWeakPtr->SetQuestGraph(pQuestGraph);

			QuestManagerWeakPtr->ForceAcceptCompleteAllQuest(true);

			TestTrue("QuestGraphis Completed",
				QuestManagerWeakPtr->IsQuestGraphCompleted());
			TestTrue("QuestGraphis Success",
				QuestManagerWeakPtr->IsQuestGraphSuccess());

		});

		It(QuestGraph_Main.GetLongPackageName() + ".Force Complete All Quest.Failed", [=]()
		{
			auto pQuestGraph = Cast<UHorizonQuestGraph>(QuestGraph_Main.TryLoad());
			TestTrue("pQuestGraph", pQuestGraph != nullptr);
			QuestManagerWeakPtr->SetQuestGraph(pQuestGraph);

			QuestManagerWeakPtr->ForceAcceptCompleteAllQuest(false);

			TestTrue("QuestGraphis Completed",
				QuestManagerWeakPtr->IsQuestGraphCompleted());

			TestTrue("QuestGraphis failed",
				QuestManagerWeakPtr->IsQuestGraphFailed());

		});


		It(QuestGraph_Main.GetLongPackageName() + ".DropQuest", [=]()
		{
			auto pQuestGraph = Cast<UHorizonQuestGraph>(QuestGraph_Main.TryLoad());
			TestTrue("pQuestGraph", pQuestGraph != nullptr);
			QuestManagerWeakPtr->SetQuestGraph(pQuestGraph);

			QuestManagerWeakPtr->ForceAcceptCompleteAllQuest(true);
			TestTrue("QuestGraphis Completed",
				QuestManagerWeakPtr->IsQuestCompleted(QuestInfo_MQ001));
			QuestManagerWeakPtr->DropQuest(QuestInfo_MQ001);
			TestFalse("QuestGraphis Completed",
				QuestManagerWeakPtr->IsQuestCompleted(QuestInfo_MQ001));

			TestEqual("GetQuestState(QuestInfo_MQ001)", QuestManagerWeakPtr->GetQuestState(QuestInfo_MQ001), EHorizonQuestState::Init);

		});

		It(QuestGraph_Main.GetLongPackageName() + ".Check Quest progress", [=]()
		{
			auto pQuestGraph = Cast<UHorizonQuestGraph>(QuestGraph_Main.TryLoad());
			TestTrue("pQuestGraph", pQuestGraph != nullptr);
			QuestManagerWeakPtr->SetQuestGraph(pQuestGraph);


			{
				TArray<FHorizonQuestInfo> allAcceptableQuest;
				QuestManagerWeakPtr->GetAllAcceptableQuest(NPC_Demo1, true, allAcceptableQuest);
				TestEqual("GetAllAcceptableQuest(NPC_Demo1)", allAcceptableQuest.Num(), 1);
			}


			// Test After Accept QuestInfo_MQ001
			{
				TArray<FHorizonQuestInfo> allAcceptableQuest;
				QuestManagerWeakPtr->AcceptQuest(QuestInfo_MQ001, NPC_Demo1);
				QuestManagerWeakPtr->AcceptQuest(QuestInfo_SQ001, NPC_Demo3);
				QuestManagerWeakPtr->GetAllAcceptableQuest(NPC_Demo1, true, allAcceptableQuest);
				TestEqual("GetAllAcceptableQuest(NPC_Demo1)", allAcceptableQuest.Num(), 0);


				TArray<FHorizonQuestInfo> allInprogressQuest_AcceptFrom;
				QuestManagerWeakPtr->GetAllInprogressQuestWithContext_AcceptedFrom(NPC_Demo1, true, allInprogressQuest_AcceptFrom);
				TestEqual("GetAllInprogressQuestWithContext_AcceptedFrom(NPC_Demo1)", allInprogressQuest_AcceptFrom.Num(), 1);
				TArray<FHorizonQuestInfo>  allInprogressQuest_CompletedBy;
				QuestManagerWeakPtr->GetAllInprogressQuestWithContext_CompleteBy(NPC_Demo2, true, allInprogressQuest_CompletedBy);
				TestEqual("GetAllInprogressQuestWithContext_CompleteBy(NPC_Demo2)", allInprogressQuest_CompletedBy.Num(), 1);
				TestEqual("GetAllInprogressQuestWithContext_CompleteBy(NPC_Demo2)", allInprogressQuest_CompletedBy.Num(), 1);
				TArray<FHorizonQuestInfo> allInprogressQuest;
				QuestManagerWeakPtr->GetAllInprogressQuest(true, allInprogressQuest);
				TestEqual("GetAllInprogressQuest", allInprogressQuest.Num(), 2);
			}

			{
				TArray<FHorizonQuestInfo> allCompletableQuest;
				QuestManagerWeakPtr->GetAllCompletableQuest(NPC_Demo2, true, allCompletableQuest);
				TestEqual("GetAllCompletableQuest(NPC_Demo2)", allCompletableQuest.Num(), 1);
				TArray<FHorizonQuestInfo> allCompletedQuest;
				QuestManagerWeakPtr->GetAllCompletedQuestWithContext(NPC_Demo2, true, allCompletedQuest);
				TestEqual("GetAllCompletableQuest(NPC_Demo2)", allCompletedQuest.Num(), 0);
			}

			// Test After Complete QuestInfo_MQ001
			{
				TArray<FHorizonQuestInfo> allCompletableQuest;
				QuestManagerWeakPtr->CompleteQuest(QuestInfo_MQ001, NPC_Demo2);
				QuestManagerWeakPtr->GetAllCompletableQuest(NPC_Demo2, true, allCompletableQuest);
				TestEqual("GetAllCompletableQuest(NPC_Demo1)", allCompletableQuest.Num(), 0);
				TArray<FHorizonQuestInfo> allCompletedQuest;
				QuestManagerWeakPtr->GetAllCompletedQuestWithContext(NPC_Demo2, true, allCompletedQuest);
				TestEqual("GetAllCompletableQuest(NPC_Demo2)", allCompletedQuest.Num(), 1);

				TestTrue("IsQuestSuccess(QuestInfo_MQ001)", QuestManagerWeakPtr->IsQuestSuccess(QuestInfo_MQ001));
				TestFalse("IsQuestFailed(QuestInfo_MQ001)", QuestManagerWeakPtr->IsQuestFailed(QuestInfo_MQ001));
				TestTrue("IsQuestCompleted(QuestInfo_MQ001)", QuestManagerWeakPtr->IsQuestCompleted(QuestInfo_MQ001));
			}

			// Check all not completed quests
			{
				TArray<FHorizonQuestInfo> allNotCompletedQuest_includeStep;
				QuestManagerWeakPtr->GetAllNotCompletedQuest(true, allNotCompletedQuest_includeStep);
				TestEqual("allNotCompletedQuest_includeStep", allNotCompletedQuest_includeStep.Num(), 6);

				TArray<FHorizonQuestInfo> allNotCompletedQuest;
				QuestManagerWeakPtr->GetAllNotCompletedQuest(false, allNotCompletedQuest);
				TestEqual("GetAllNotCompletedQuest", allNotCompletedQuest.Num(), 3);

				TArray<FHorizonQuestInfo> allNotCompletedQuest_NPC_Demo4;
				QuestManagerWeakPtr->GetAllNotCompletedQuestWithContext(NPC_Demo4, true, allNotCompletedQuest_NPC_Demo4);

				TestEqual("GetAllNotCompletedQuestWithContext(NPC_Demo2)", allNotCompletedQuest_NPC_Demo4.Num(), 2);
			}

			// Check all success quests
			{
				TArray<FHorizonQuestInfo> allSuccessQuest;
	
				QuestManagerWeakPtr->GetAllSuccessQuest(true, allSuccessQuest);
				TestEqual("GetAllSuccessQuest", allSuccessQuest.Num(), 1);

				TArray<FHorizonQuestInfo> allSuccessQuest_Demo3;
				QuestManagerWeakPtr->GetAllSuccessQuestWithContext(NPC_Demo3, true, allSuccessQuest_Demo3);
				TestEqual("GetAllSuccessQuest(NPC_Demo3)", allSuccessQuest_Demo3.Num(), 0);
			}
			
		});

		It(QuestGraph_Main.GetLongPackageName() + ".Test Repeat Quest", [=]()
		{
			auto pQuestGraph = Cast<UHorizonQuestGraph>(QuestGraph_Main.TryLoad());
			TestTrue("pQuestGraph", pQuestGraph != nullptr);
			QuestManagerWeakPtr->SetQuestGraph(pQuestGraph);
			auto pPlayerState = Cast<APlayerState>(QuestManagerWeakPtr->GetOwner());
			pPlayerState->SetScore(0);
			{
				TestTrue("UHorizonQuestLibrary::IsQuestInfoValid(QuestInfo_MQ001)", UHorizonQuestLibrary::IsQuestInfoValid(QuestInfo_MQ001));
				TestTrue("UHorizonQuestLibrary::IsQuestInfoValid(QuestInfo_MQ002)", UHorizonQuestLibrary::IsQuestInfoValid(QuestInfo_MQ002));
				QuestManagerWeakPtr->ForceAcceptCompleteQuestAndDependence(QuestInfo_MQ001);
				int32 numRepeat =  100;
				for(int32 i = 0; i < numRepeat; ++i)
				{ 
					UE_HORIZONQUEST_LOG("numRepeat: %d", i);
					QuestManagerWeakPtr->AcceptQuest(QuestInfo_MQ002, NPC_Demo6);
					QuestManagerWeakPtr->CompleteQuest(QuestInfo_MQ002, BP_QuestTriggerButton_MQ002_Success);
				}

				bool bIsDataExists_MQ002 = false;
				FHorizonQuestGraphNodeData graphNodeData_MQ002 = QuestManagerWeakPtr->BP_GetQuestData(QuestInfo_MQ002, bIsDataExists_MQ002);
				auto pGraphNode = QuestManagerWeakPtr->GetQuestGraphNode(QuestInfo_MQ002);
				TestNotNull("GetQuestGraphNode(QuestInfo_MQ002)", pGraphNode);
				TestTrue("BP_GetQuestData(QuestInfo_MQ002)", bIsDataExists_MQ002);

				TestEqual("graphNodeData_MQ002.SuccessCount", graphNodeData_MQ002.SuccessCount, numRepeat);
				
				{
					int32 currentScore = pPlayerState->GetScore();
					// currentScore = AcceptReward + CompleteReward
					TestEqual("currentScore", currentScore, 1 * numRepeat + 100 * numRepeat);
				}
			}
		});
		AfterEach([this]()
		{
			FCoreUObjectDelegates::PostLoadMapWithWorld.Remove(PostLoadMapWithWorldHandle);
		});


	});
}


```




-----------------------
HorizonQuestFlag: TestCase
-----------------------

HorizonQuest\Source\HorizonQuestFlag\Private\Test\HorizonQuestFlag.spec.cpp

```


BEGIN_DEFINE_SPEC(FHorizonQuestFlagSpec,
	"Plugin.HorizonQuestFlag",
	EAutomationTestFlags::ProductFilter | EAutomationTestFlags::ApplicationContextMask)
	FSoftObjectPath HorizonQuestDemoMapPath{ "World'/Game/_HorizonQuestDemo/Map/HorizonQuestDemoMap.HorizonQuestDemoMap'" };
	FSoftObjectPath QuestGraph_Main{ "HorizonQuestGraph'/Game/_HorizonQuestDemo/_Subsystem/Quest/Graph/QuestGraph_Main.QuestGraph_Main'" };
	TWeakObjectPtr<AHorizonQuestFlagTriggerVolume> QuestFlagTriggerVolume_SQ002_Step1_1;
	TWeakObjectPtr<AHorizonQuestFlagTriggerVolume> QuestFlagTriggerVolume_SQ002_Step1_2;
	FDelegateHandle PostLoadMapWithWorldHandle;
	TWeakObjectPtr<UHorizonQuestFlagManagerComponent> QuestFlagManagerWeakPtr;

END_DEFINE_SPEC(FHorizonQuestFlagSpec)

void FHorizonQuestFlagSpec::Define()
{
	

	Describe(HorizonQuestDemoMapPath.GetLongPackageName(), [=]()
	{
		LatentBeforeEach([=](const FDoneDelegate& Done)
		{
			AutomationOpenMap(HorizonQuestDemoMapPath.GetLongPackageName());
			UWorld* pCurrentWorld = GetAnyGameWorld();
			auto pCurrentWorldPacakge = pCurrentWorld->GetPackage();
			auto sCurrentWorldPath = pCurrentWorldPacakge->FileName.ToString();
			auto sTestWorldPath = HorizonQuestDemoMapPath.GetLongPackageName();
			if (sTestWorldPath != sCurrentWorldPath)
			{
				PostLoadMapWithWorldHandle = FCoreUObjectDelegates::PostLoadMapWithWorld.AddLambda([pCurrentWorld, Done](UWorld* InLoadedWorld)
				{
					Done.Execute();
				});
			}
			else
			{
				Done.Execute();
			}
			
		});

		BeforeEach([=]()
		{

			auto pWorld = GetAnyGameWorld();

			TArray<AActor*> actors;
			UGameplayStatics::GetAllActorsOfClass(pWorld, AHorizonQuestFlagTriggerVolume::StaticClass(), actors);

			for (auto& it : actors)
			{
				auto actorName =  it->GetName();
				if (actorName == "QuestFlagTriggerVolume_SQ002_Step1_1")
				{
					QuestFlagTriggerVolume_SQ002_Step1_1 = Cast<AHorizonQuestFlagTriggerVolume>(it);
				}
				if (actorName == "QuestFlagTriggerVolume_SQ002_Step1_2")
				{
					QuestFlagTriggerVolume_SQ002_Step1_2 = Cast<AHorizonQuestFlagTriggerVolume>(it);
				}

			}
			TestNotNull("QuestFlagTriggerVolume_SQ002_Step1_1", QuestFlagTriggerVolume_SQ002_Step1_1.Get());
			TestNotNull("QuestFlagTriggerVolume_SQ002_Step1_2", QuestFlagTriggerVolume_SQ002_Step1_2.Get());
			auto pPlayerController = UGameplayStatics::GetPlayerControllerFromID(pWorld, 0);
			QuestFlagManagerWeakPtr = UHorizonQuestFlagLibrary::GetQuestFlagManager(pPlayerController);
			TestNotNull("QuestFlagManagerWeakPtr", QuestFlagManagerWeakPtr.Get());
			if (QuestFlagManagerWeakPtr.IsValid())
			{
				QuestFlagManagerWeakPtr->ClearQuestFlagData();

			}
		});

		It( "Check QuestFlag count", [=]()
		{
			TestEqual("QuestFlagManagerWeakPtr->GetQuestFlagCount",
				QuestFlagManagerWeakPtr->GetQuestFlagCount(QuestFlagTriggerVolume_SQ002_Step1_1->QuestFlagTag), 0);

			{
				for (int32 i = 0; i < 5; ++i)
				{
					TestTrue("QuestFlagTriggerVolume_SQ002_Step1_1->TriggerQuestFlag",
						QuestFlagTriggerVolume_SQ002_Step1_1->TriggerQuestFlag(QuestFlagManagerWeakPtr.Get()));
				}

				TestEqual("QuestFlagManagerWeakPtr->GetQuestFlagCount",
					QuestFlagManagerWeakPtr->GetQuestFlagCount(QuestFlagTriggerVolume_SQ002_Step1_1->QuestFlagTag), 5);


			}


			{

				QuestFlagManagerWeakPtr->RemoveQuestFlag(FGameplayTag::RequestGameplayTag("QuestFlag.SQ002.Step1.Optional1"));
		
				TestEqual("QuestFlagManagerWeakPtr->GetQuestFlagCount",
					QuestFlagManagerWeakPtr->GetQuestFlagCount(QuestFlagTriggerVolume_SQ002_Step1_1->QuestFlagTag), 0);
			}
		});


		It("Save/Load Serialization", [=]()
		{
			FHorizonQuestFlagArchiveData archiveData;
			if (QuestFlagManagerWeakPtr.IsValid())
			{
				for (int32 i = 0; i < 1; ++i)
				{
					TestTrue("QuestFlagTriggerVolume_SQ002_Step1_2->TriggerQuestFlag",
						QuestFlagTriggerVolume_SQ002_Step1_2->TriggerQuestFlag(QuestFlagManagerWeakPtr.Get()));
				}
				TestEqual("QuestFlagManagerWeakPtr->GetQuestFlagCount",
					QuestFlagManagerWeakPtr->GetQuestFlagCount(QuestFlagTriggerVolume_SQ002_Step1_2->QuestFlagTag), 1);

				QuestFlagManagerWeakPtr->GetArchiveData(archiveData);

				for (int32 i = 0; i < 2; ++i)
				{
					TestTrue("QuestFlagTriggerVolume_SQ002_Step1_2->TriggerQuestFlag",
						QuestFlagTriggerVolume_SQ002_Step1_2->TriggerQuestFlag(QuestFlagManagerWeakPtr.Get()));
				}
				TestEqual("QuestFlagManagerWeakPtr->GetQuestFlagCount",
					QuestFlagManagerWeakPtr->GetQuestFlagCount(QuestFlagTriggerVolume_SQ002_Step1_2->QuestFlagTag), 3);

				QuestFlagManagerWeakPtr->SetArchiveData(archiveData);


				for (int32 i = 0; i < 2; ++i)
				{
					TestTrue("QuestFlagTriggerVolume_SQ002_Step1_2->TriggerQuestFlag",
						QuestFlagTriggerVolume_SQ002_Step1_2->TriggerQuestFlag(QuestFlagManagerWeakPtr.Get()));
				}
				TestEqual("QuestFlagManagerWeakPtr->GetQuestFlagCount",
					QuestFlagManagerWeakPtr->GetQuestFlagCount(QuestFlagTriggerVolume_SQ002_Step1_2->QuestFlagTag), 3);

			}
		});

		AfterEach([this]()
		{
			FCoreUObjectDelegates::PostLoadMapWithWorld.Remove(PostLoadMapWithWorldHandle);
		});


	});
}
```




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