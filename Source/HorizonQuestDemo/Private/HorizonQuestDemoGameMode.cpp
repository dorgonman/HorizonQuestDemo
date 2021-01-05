// Copyright Epic Games, Inc. All Rights Reserved.

#include "HorizonQuestDemoGameMode.h"
#include "HorizonQuestDemoCharacter.h"
#include "UObject/ConstructorHelpers.h"

AHorizonQuestDemoGameMode::AHorizonQuestDemoGameMode()
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnBPClass(TEXT("/Game/ThirdPersonCPP/Blueprints/ThirdPersonCharacter"));
	if (PlayerPawnBPClass.Class != NULL)
	{
		DefaultPawnClass = PlayerPawnBPClass.Class;
	}
}
