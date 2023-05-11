// Created by dorgon, All Rights Reserved. 
// email: dorgonman@hotmail.com
// blog: dorgon.horizon-studio.net

#pragma once

// Engine
#include "Camera/CameraComponent.h"
#include "Components/CapsuleComponent.h"
#include "Components/InputComponent.h"
#include "Engine/EngineTypes.h"
#include "Engine/World.h"
#include "GameFramework/CharacterMovementComponent.h"
#include "GameFramework/Controller.h"
#include "GameFramework/SpringArmComponent.h"
#include "Kismet/GameplayStatics.h"
#include "Kismet/KismetSystemLibrary.h"

// Core
#include "Logging/LogMacros.h"
#include "Algo/Reverse.h"
#include "Algo/AnyOf.h"


// HorizonQuest
#include "HorizonQuestLibrary.h"
#include "HorizonQuestManagerComponent.h"





// GameSaveSlot0 is for debug purpose, it will not be checked in normal save/load process.
#define MAX_SAVE_SLOT 3