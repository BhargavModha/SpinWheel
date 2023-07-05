# Spin Wheel

Created a spin wheel feature with custom drop rates for each item that is easy to use and highly scalable. In order to achieve this, the core aspects are data-driven with the help of scriptable objects.

![Spin Wheel Demo](/Images/SpinDemo.gif)

---
## Scriptable Objects

Each item in the wheel is stored as a "**SpinWheelItem**" Scriptable Object that contains
- Name of the item
- Quantity 
- Image
- Drop rate (In percentage)

Each of the items is then stored in the "**SpinWheelContainer**" Scriptable Object that only contains
- The list of all possible items that can be won
 
![Scriptable Objects](/Images/SpinWheelSOs.png)

---
## Spin Wheel (Script)

This script holds the primary logic for this feature.  
It exposes multiple settings which can fine-tune the spin animation
- Spin Clockwise (The wheel can spin either clockwise or anti-clockwise)
- Min Rotations
- Max Rotations
- Spin Duration (Time for each spin) 

The script creates a **weighted list** based on the SpinWheelContainer and randomly picks an item along with a random amount of rotations.  

![Spin Wheel Script](/Images/SpinWheelScript.png)

## Unit Test
In order to test if the drop rates work as intended, there also exists a function in the SpinWheel script that emulates 1000 drops and then groups the result by the prize. To run the test, click the "**RunDropTest**" in this script's context menu.  
You can run the test in the editor without playing the game.  
This creates a text file called "**DropRateTest.txt**" inside the Resources folder.

![Unit test](/Images/UnitTest.png)

---

## Misc

In order to make this scalable, the code was decoupled, where I created different scripts to handle specific functions and made it straightforward for designers to use and edit easily.  

- **SpinWheelManager** (Singleton): Handles all game events.
- **SpinWheelAnimations**: Takes care of all timelines animations and listens to events.
- **SpinWheelPopulateData**: Updates information for the items in the spin wheel and validates the data every time the data in the scriptable object changes.

---
---
