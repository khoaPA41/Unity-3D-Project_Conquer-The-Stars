<p align="center">
  <img src="https://github.com/khoaPA41/Unity-3D-Project_Conquer-The-Stars/blob/main/Assets/Third_party%20Assets/UI/Demo/Player_I.gif" width="32%" />
  <img src="https://github.com/khoaPA41/Unity-3D-Project_Conquer-The-Stars/blob/main/Assets/Third_party%20Assets/UI/Demo/Player_II.gif" width="32%" />
  <img src="https://github.com/khoaPA41/Unity-3D-Project_Conquer-The-Stars/blob/main/Assets/Third_party%20Assets/UI/Demo/Player_III.gif" width="32%" />
</p>

# Conquer The Stars

> A 3D turn-based combat game focused on tactical decision-making, character abilities, and reusable gameplay systems.

**Conquer The Stars** is a personal Unity project developed to explore and implement turn-based combat systems, enemy decision-making, reusable gameplay architecture, and data-driven design.

The project is currently **In Development**.

## Overview

The combat system is built around a dynamic turn-order system where character speed affects the order of actions. Players can select skills, targets, allies, and defensive actions, while enemies evaluate available targets and make decisions based on combat conditions.

The project also includes reusable systems for items, temporary buffs, object pooling, character statistics, save data, and boss phases.

## Features

* Turn-based combat with dynamic turn order
* Speed-based turn manipulation
* Player skill, target, ally, and item selection
* Dodge and parry mechanics
* Enemy target evaluation system
* Boss phases with health-based attack changes
* Temporary buffs and stat modifications
* Item system using Factory Pattern
* Reusable object pooling system
* Data-driven character and item configuration using ScriptableObjects
* Battle statistics and post-battle results
* Save / Load system
* Cinemachine-based battle camera transitions
* Unity Input System integration

## Technical Highlights

### Hierarchical State Machine

The battle flow and character behaviors are separated into multiple state machines.

```text
Battle State Machine
├── Setup
├── Start Turn
├── Player Turn
├── Action Selection
├── Action Execution
├── Enemy Turn
├── Enemy Execution
├── Resolve
└── Battle Result
```

Player and enemy combat behaviors use dedicated state machines to manage actions such as attacking, defending, dodging, blocking, taking damage, dying, and victory states.

### Enemy Target Evaluation

Enemy AI evaluates available targets using weighted combat factors:

```text
Target Score =
    HP Weight
  + Threat Weight
  + Defense Weight
  + Status Weight
```

The evaluation strategy is configured through ScriptableObjects, allowing different enemy behaviors to use different priorities.

### Data-Driven Gameplay

ScriptableObjects are used to separate gameplay data from runtime logic.

Examples include:

* Character statistics
* Player attacks and skills
* Enemy attacks
* Enemy team configurations
* Item data
* Enemy evaluation strategies

### Item & Buff System

Items are implemented through an interface-based Factory Pattern.

```text
ItemData
    ↓
ItemType
    ↓
ItemFactory
    ↓
IItem
    ↓
Use()
```

The system supports healing, mana recovery, temporary stat buffs, revival, and other item effects.

### Object Pooling

A custom object pooling system is used for frequently spawned objects such as enemies, VFX, and UI elements.

```text
ObjectPoolingManager
        ↓
Dictionary<string, Stack<PooledObject>>
        ↓
Get / Release
```

This reduces repeated object instantiation during gameplay and provides reusable object lifecycle management.

### Combat Statistics

Battle statistics are tracked during combat and displayed on the result screen, including:

* Highest damage
* Damage dealt
* Damage received
* Successful parries
* Successful dodges
* Battle duration
* Defeated enemies

## Architecture

The project is organized around gameplay responsibilities and reusable systems:

```text
Scripts/
├── Fight/
├── Input/
├── Managers/
├── Pattern/
│   ├── Factory/
│   ├── Object Pooling/
│   └── State Machine/
├── Physics/
├── Save/
├── ScriptableObject/
├── Stats/
├── UI/
└── VFX/
```

## Tech Stack

* **Unity 6**
* **C#**
* **Universal Render Pipeline**
* **Unity Input System**
* **Cinemachine**
* **ScriptableObject**
* **JSON Save System**

## Project Status

**In Development**

Current work includes gameplay iteration, progression systems, additional content, and system refinement.

### Planned Improvements

* Character progression and experience system
* Additional rewards and progression content
* More items and combat effects
* Additional enemy behaviors
* Further balancing and combat polish
* Additional graphics and audio settings

## Controls

| Action         | Input     |
| -------------- | --------- |
| Move           | WASD      |
| Select Target  | A / D     |
| Confirm        | Enter     |
| Open Settings  | Tab       |
| Dodge          | Space     |
| Block          | F         |

## Development Notes

This project is primarily focused on **gameplay programming and system architecture**, with an emphasis on building reusable and maintainable gameplay components rather than maximizing content scope.

## Credits

Developed by **Phạm Anh Khoa**

