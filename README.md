# customSailSkins
Provides you the ability to assign custom sail skins to players by steamID.

**WIP Project, will contain more data when it is ready. (no guarantee to work as of yet)**

## Setup

If you have not setup the modloader before, read from [here](#initial-setup).
Otherwise you can skip to [here](#mod-setup).

### Initial setup

Fetch the **Assembly-CSharp.dll**, **0Harmony.dll** and **BWModLoader.dll** files from the latest release version.

Place these files inside your **Steam\steamapps\common\Blackwake\Blackwake_Data\Managed** folder and run the game.

**Note: You will have to overwrite the existing Assembly-CSharp.dll with the one provided.**

On running, and closing once you reach the main menu, you will notice a new folder named **Mods** has appeared inside **Blackwake_Data\Managed**.

Once you have verified that you have generated the Mods folder, you have finished setting up the modloader. See [here](#mod-setup) for the next stage.

### Mod setup

Place the attached **customSailSkins.dll** file (can be found on the latest release) inside your **Mods** folder. Run the game and wait until you reach the main menu, the mod will generate the folder(s)/file(s) required for you.

To assign custom sails, you do the following:

1. Place the sail skins you wish to use **(Native resolution is 2048x2048)**, inside **/Managed/Mods/Assets/customSails/** .
   - You must use a **.png** image.

2. Inside the auto-generated **steamID.txt** file, inside **/Managed/Mods/Assets/customSails/** you enter the steamID's you wish to link to specific users.
Example:
```
768522222333=Viking
768493028463=Black
758434647561=Emboss
```

3. When complete you should have a folder structure like this (assuming 2 sails assigned):

**/Managed/Mods/Assets/customSails/Viking.png**
**/Managed/Mods/Assets/customSails/Black.png**
**/Managed/Mods/Assets/customSails/steamID.txt**

Where **steamID.txt** contains:
```text
768522222333=Viking
768493028463=Black
```