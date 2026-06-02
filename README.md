# cLocked In - Office Stealth Game

A 2D top-down stealth game built in Unity where you play as an office worker trying to survive the work day without getting caught by the boss. Complete daily tasks, manage your inventory, interact with NPCs, and avoid detection to make it through each day.

---

## Table of Contents

- [About the Game](#about-the-game)
- [How to Play](#how-to-play)
- [Running the Game](#running-the-game)
- [Building from Source](#building-from-source)
- [Dependencies](#dependencies)
- [Authors](#authors)

---

## About the Game

cLocked In is a 2D top-down stealth game set in an office environment. Each in-game day runs from 9AM to 5PM. Your goal is to complete your daily task on the office computer before the end of the day — or risk getting fired. Meanwhile, the boss patrols the office and will chase you if you enter his vision cone. Use items, hide in lockers, and distract the boss to survive.

**Key Features:**
- Boss AI with patrol, chase, and investigate states driven by a vision cone detection system
- Inventory and hotbar system with drag and drop, item stacking, and item descriptions
- Daily task system — complete a Snake minigame on the office computer each day
- Interactable objects — printers, computers, lockers
- NPC dialogue and quest system
- Day and time counter scaling from 9AM to 5PM
- Save and load system
- Difficulty settings — Intern, Senior, CEO
- Item pickup and throwing mechanics

---

## How to Play

| Action | Control |
|---|---|
| Move | WASD |
| Sprint | Left Shift |
| Move to mouse | Left Click (hold) |
| Interact | E |
| Toggle flashlight | F |
| Use hotbar item | 1 - 0 keys |
| Throw item (from hotbar) | Drag item out of hotbar |

**Tips:**
- Watch the detection meter — if it hits 100% and stays there, you'll be caught
- Throw items to distract the boss and send him to investigate
- Hide in lockers to avoid detection entirely
- Complete the daily Snake task on the computer before 5PM or you're fired
- The Snake game gets harder each day — higher score required and faster speed

---

## Running the Game

The release build is available on the **Release branch** of the GitHub repository inside the `UnityGame` folder.

1. Go to the **Release branch** on GitHub
2. Navigate to the `UnityGame` folder
3. Download the folder contents
4. Run `Learning.exe`

> Make sure the `Learning_Data` folder and all other files are in the **same folder** as `Learning.exe` — the exe will not run without them.

---

## Building from Source

**Requirements:**
- Unity 6 (6000.4.0f1)
- Unity Input System package
- Unity AI Navigation package (NavMesh)
- TextMeshPro package
- Universal Render Pipeline (URP)

**Steps:**
1. Clone the repository
2. Open the project in Unity 6
3. Open **File → Build Settings**
4. Add all scenes to the **Scenes In Build** list in this order:
   - `MainMenu`
   - `AudioSettingsManager`
   - `AccessibilityMenu`
   - `AudioMenu`
   - `GameSettingsMenu`
   - `GamePlay`
     
5. Select **Windows (x86_64)** as the target platform
6. Click **Build** and select an output folder
7. Run the generated `.exe` file

---

## Dependencies

| Dependency | Purpose |
|---|---|
| Unity Input System | Player movement and interaction input |
| Unity AI Navigation | Boss NavMesh pathfinding |
| TextMeshPro | UI text rendering |
| Universal Render Pipeline | 2D lighting and flashlight |
| Visual Paradigm Community | UML class diagram |

---

## Authors

This game was developed as a team project for Software Development Practice at AUT.

- Jayden Marsh
- Christian Cantos
- Matty Luriz
- Mohammed Yaacoub Abou Chlih

---

## Notes

- Save data is stored in `Application.persistentDataPath/saveData.json`
- To reset your save, delete `saveData.json` or use the **New Game** option from the main menu
- Difficulty is selected from the main menu and affects boss speed, vision, and detection rate
