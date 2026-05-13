---
id: HR-TSK-0004
uid: 019d6e05-f22b-7002-8df1-58d1b60b92c3
type: Task
title: Add test coverage for HorizonQuestFlagManagerComponent
state: Proposed
priority: P2
parent: ~
owner: ~
area: general
iteration: backlog
created: 2026-04-09
updated: 2026-04-09
external:
  {}
links:
  relates:
    []
  blocks:
    []
  blocked_by:
    []
decisions:
  []
tags:
  []
---

# Context

HorizonQuestFlagManagerComponent is a core system used by quest requirements to track flag counts. It has zero automated test coverage. A regression in flag add/remove/query could silently break all quest completion logic.

# Goal

Add HorizonQuestFlag.spec.cpp with comprehensive tests for AddFlag/RemoveFlag/QueryFlag/Count operations, covering typical use and edge cases.

# Approach

Follow the existing HorizonQuest.spec.cpp pattern (open map, resolve actors, run It() blocks). Add BeforeEach to clear flag state. Cover: AddFlag, RemoveFlag, GetFlagCount, HasFlag, ClearFlags, archive/load cycle.

# Acceptance Criteria

All new It() blocks pass; no regression in existing spec; build passes

# Risks / Dependencies

FlagManager requires a PlayerState or similar context - may need BP_QuestTriggerButton actor resolution pattern from existing spec.

# Worklog

2026-04-09 00:56 [agent=sisyphus] Created item
2026-04-09 00:56 [agent=sisyphus] [model=unknown] Updated Ready fields: Context, Goal, Approach, Acceptance Criteria, Risks / Dependencies
