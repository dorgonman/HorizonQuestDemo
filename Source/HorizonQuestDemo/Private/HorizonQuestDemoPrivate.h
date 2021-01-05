// Created by dorgon, All Rights Reserved. 
// email: dorgonman@hotmail.com
// blog: dorgon.horizon-studio.net

#pragma once

// Engine

#include "Engine/EngineTypes.h"
#include "Kismet/KismetSystemLibrary.h"
#include "Kismet/GameplayStatics.h"

// Core
#include "Logging/LogMacros.h"
#include "Algo/Reverse.h"
#include "Algo/AnyOf.h"


// HorizonQuest
#include "HorizonQuestLibrary.h"
#include "HorizonQuestManagerComponent.h"





// GameSaveSlot0 is for debug purpose, it will not be checked in normal save/load process.
#define MAX_SAVE_SLOT 3