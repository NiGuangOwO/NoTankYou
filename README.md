# No Tank You
[![Download count](https://img.shields.io/endpoint?url=https://qzysathwfhebdai6xgauhz4q7m0mzmrf.lambda-url.us-east-1.on.aws/NoTankYou)](https://github.com/MidoriKami/NoTankYou)

Plugin for XivLauncher/Dalamud, shows a warning banner ingame you or your party members are forgetting to do role-specific actions.

Why would this be useful you ask?

Ever charged headfirst into a dungeon following you tank as a DPS, and all of a sudden realize a lot of aggro is changing? And then realize moments later that you are building up a lot of aggro? Then moments later... you are laying on the ground dead, thinking "oh, tank forgot their stance".

Teaching a friend that is just picking up Scholar? Now you can see if they accidentally dismiss their Faerie mid-combat.

Playing as a DPS, and want those **big numbers** but your Dancer doesn't realize they have Dance Partner, now you have a notice!

The primary focus of this plugin is helping to inform you that **others** may not be doing role critical things that they should be doing in all situations.

Alternatively, you can use this plugin to remind yourself to do these role specific tasks!

## How does it work?
When enabled, if a party member with a role-specific action hasn't done that role specific action, you will see a warning displayed on your screen.
Only you can see this warning, and no messages are sent to other party members. 

Warnings will not show if there are no players with a role-specific action in the party.
(Ex. If there are no summoners, you will not see the Summoner Pet Warning)

## How to use it
Using the ingame command `/nty` will bring up the main configuration window as seen below.
Configuration is split into three categories, Display, Modules, Settings.

You can use the command `/nty help` to view a list of all available commands.

![image](https://github.com/MidoriKami/NoTankYou/assets/9083275/d9091131-c857-42a4-b707-ad21002d6406)

## Overlays
NoTankYou has two overlays you can use to display the warnings, you can go for the subtle approach by integrating with the PartyList user interface, or you can go with the classic in-your-face warning banner in the middle of your screen.

Party List Overlay Appearance            |  Warning Banner Appearance
:-------------------------:|:-------------------------:
![YoloMouse_JVgwSjNQUg](https://user-images.githubusercontent.com/9083275/177447974-c2bfc46d-ac92-4bfe-a1bf-42f413074d02.gif) | ![image](https://user-images.githubusercontent.com/9083275/194427783-45b4d2f2-3778-4dfd-8f90-5e8b5cf53b03.png)

# Blacklist

NoTankYou includes a zone blacklist that allows you to explicity state which zones you do not want **Any** warnings to appear. This is particularly useful for areas such as Bozja/Zadnor/Eureka

Note: NoTankYou is entirely disabled while you are in PvP automatically

![image](https://github.com/MidoriKami/NoTankYou/assets/9083275/d1e45580-ee14-4300-a3a6-d8ab716831ed)

