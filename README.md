# Conquer The Stars

<p align="center">
  <img src="https://github.com/khoaPA41/Unity-3D-Project_Conquer-The-Stars/blob/main/Assets/Third_party%20Assets/UI/Demo/Player_I.gif" width="32%" />
  <img src="https://github.com/khoaPA41/Unity-3D-Project_Conquer-The-Stars/blob/main/Assets/Third_party%20Assets/UI/Demo/Player_II.gif" width="32%" />
  <img src="https://github.com/khoaPA41/Unity-3D-Project_Conquer-The-Stars/blob/main/Assets/Third_party%20Assets/UI/Demo/Player_III.gif" width="32%" />
</p>

> A 3D turn-based combat game focused on tactical decision-making, character abilities, and reusable gameplay systems.

**Conquer The Stars** is a personal Unity project developed to explore and implement turn-based combat systems, enemy decision-making, reusable gameplay architecture, and data-driven design.

The project is currently **In Development**.

## Overview

The combat system is built around a dynamic turn-order system where character speed affects the order of actions. Players can select skills, targets, allies, and defensive actions, while enemies evaluate available targets and make decisions based on combat conditions.

The project also includes reusable systems for items, temporary buffs, object pooling, character statistics, save data, and boss phases.

## Features

* Turn-based combat with dynamic and speed-based turn order
* Skill, target, ally, item, and defensive action selection
* Dodge and parry mechanics
* Enemy AI with weighted target evaluation
* Boss phases with health-based behavior changes
* Item and temporary buff system
* Battle statistics and post-battle results
* Save / Load system


## Technical Highlights

### Hierarchical State Machine

Implemented a hierarchical state-machine architecture to manage battle flow and character behaviors.

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

Dedicated state machines are also used for player and enemy combat behaviors, including attacking, defending, dodging, blocking, taking damage, dying, and victory states.

### Dynamic Turn Order

Implemented a speed-based turn-order system where character speed determines action priority.

The battle system also supports turn manipulation, allowing certain combat conditions to influence the order in which characters act.

### Enemy AI & Target Evaluation

Implemented a weighted target evaluation system that allows enemies to prioritize targets based on combat factors such as:

```text
HP
Threat
Defense
Status
```

Different evaluation strategies can be configured through ScriptableObjects, allowing enemy behaviors to be adjusted without modifying the core AI logic.

### Data-Driven Gameplay

Used ScriptableObjects to separate gameplay data from runtime logic.

```text
Character Stats
Player Attacks
Enemy Attacks
Enemy Teams
Items
AI Evaluation Strategies
```

This allows gameplay parameters and configurations to be modified directly in the Unity Inspector.

### Item & Buff System

Implemented an interface-based item system using the Factory Pattern.

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

The system supports reusable effects such as healing, mana recovery, temporary stat buffs, revival, and other combat effects.

### Object Pooling

Developed a reusable object-pooling system for frequently spawned gameplay objects such as enemies, VFX, and UI elements.

```text
Object Pool
    ↓
Get()
    ↓
Use
    ↓
Release()
```

This reduces repeated runtime instantiation and provides centralized object lifecycle management.

### Battle Statistics

Implemented a combat statistics system to track and display battle performance, including:

* Damage dealt
* Damage received
* Highest damage
* Successful dodges
* Successful parries
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

Developed by Developed by [Phạm Anh Khoa](https://github.com/khoaPA41)

