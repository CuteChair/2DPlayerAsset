# Slime Encounter

## Overview
Slime Encounter is a gameplay prototype inspired by Terraria-style combat and movement.
The project focuses on player movement feel, enemy readability, and encounter setup rather than complete combat systems.

This prototype serves as a foundation for a larger project planned for future development.

---

## Gameplay Features
- Player-controlled character with responsive movement
- Giant slime enemy that tracks the player and performs jumping attacks
- Enemy attack telegraphing through charge-up timing and visual cues
- Projectile spawning on enemy ground impact

---

## Technical Highlights

### Player Controller
- Custom physics-based movement
- Acceleration and deceleration handling
- Increased gravity during continuous fall
- Variable jump height based on input duration
- Coyote jump and pogo jump support
- Frictionless rigidbody setup for precise control
- Clear state separation to handle different jump and movement behaviors

### Enemy Behavior
- Player tracking system with trajectory calculation
- Jumping attack logic targeting player position
- Charge-up phase to improve attack readability
- Projectile spawning on impact

### Combat Foundations
- Layer-based box cast interaction system
- Collision groundwork designed to support future health and damage systems

---

## Controls
- WASD – Move
- Space – Jump

---

## Project Status
This project is a gameplay prototype.
Full combat mechanics (health, damage, attack resolution) are not yet implemented, but the existing interaction systems are designed to support them.

Planned improvements include additional enemy attack patterns and more varied behavior logic to improve encounter depth and unpredictability.

---

## How to Run
Playable build can be found on my Itch page : https://themightychair.itch.io/slime-encounter

---

## Built With
- Unity
- C#
