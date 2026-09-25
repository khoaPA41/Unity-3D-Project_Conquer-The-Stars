# Conquer The Stars

<p align="center">
  <img src="https://github.com/khoaPA41/Unity-3D-Project_Conquer-The-Stars/blob/main/Assets/Third_party%20Assets/UI/Demo/Player_I.gif" width="32%" />
  <img src="https://github.com/khoaPA41/Unity-3D-Project_Conquer-The-Stars/blob/main/Assets/Third_party%20Assets/UI/Demo/Player_II.gif" width="32%" />
  <img src="https://github.com/khoaPA41/Unity-3D-Project_Conquer-The-Stars/blob/main/Assets/Third_party%20Assets/UI/Demo/Player_III.gif" width="32%" />
</p>

## 🎮 Demo
- [Download](https://pakbot4124.itch.io/conquer-the-stars)
- [Gameplay Video](https://youtu.be/g3vkVFKuGm8)

> A 3D turn-based combat game focused on tactical decision-making, character abilities, and reusable gameplay systems.

**Conquer The Stars** is a personal Unity project developed to explore and implement turn-based combat systems, enemy decision-making, reusable gameplay architecture, and data-driven design.

## Overview

**Conquer The Stars** is a 3D turn-based combat project focused on tactical decision-making, dynamic turn order, enemy AI, and reusable gameplay systems.

Players can select skills, targets, allies, items, and defensive actions during combat, while enemies evaluate targets based on combat conditions.

The project is currently **In Development**.

# 🛠️ Maintenance
## 📝 [Developer Logs](https://app.notion.com/p/3-Week-Engineering-Polish-Sprint-25-09-15-10-2026-3e5fa5c6075a8168aa4ec2b897999959?source=copy_link)
**D01:** ***Find And Note Bug inventory***
Tracked during code review / Phase A. Status: **Fixed** | **Open**.

| # | Issue | Severity | Status | Notes |
|---|--------|----------|--------|--------|
| 1 | TouchSwipe half-screen uses `Screen.height` instead of `Screen.width` | Medium | **Fixed** | Compare `startPos.x` against `Screen.width * 0.5f` |
| 2 | BossPhase overwrites attack by list order; does not pick a single clear phase | Low–Med | Open | Same HP% can yield different attacks if Inspector list order changes |
| 3 | `CharacterStatsManagers.OnEnable` fully re-inits stats / HP / death | Medium | Open | Risky with pool re-enable or accidental `SetActive(true)` |
| 4 | `EnemyEvaluation.EvaluateStatus` always returns `0` + leftover `Debug.Log` | Low | Open | Status weight unused in AI scoring |
| 5 | `ResolveState` mixes buff, camera, death cleanup, turn steal, win check | Low (design) | Open | Harder to test/maintain; not a runtime crash |
| 6 | Object pool: string keys + parent lookup via `Substring`/`Contains` | Low | Open | Fragile on rename/typo; silent null if key wrong |
| 7 | Typos / naming (`Nomalized`, `Playerexecuted`, `pooledObect`, …) | Low | Open | Consistency pass |
| 8 | Empty stubs (`TeamManagers`, `UseItem`); `Target` is empty marker | Low | Open | Delete unused; keep `Target` only if still used as marker |
## Credits

Developed by [Phạm Anh Khoa](https://github.com/khoaPA41)

