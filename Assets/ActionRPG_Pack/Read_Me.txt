/*
0. !!! MOVE "Resources" folder from "\Assets\ActionRPG_Pack" to "\Assets" !!! 

00. If you import this package for the first time mechanim animator transitions might be messed up. To fix this close Unity and open it again.

1. General
To quickly setup this framework think of this package as prefab driven. 
Place onto the level mesh following prefabs: player, SCRIPTS, camera, zombie, MainCanvas, PlayerRespawnPoint.
To register movement you will also need to place a TargetingPlane prefab(obviously it needs to be of a size of a level - can be bigger, doesn't need to be of a shape of a level, can be a simple quad). Set it's height so it would be on the level with the ground of the level.
There is also TargetingPlaneToShoot prefab which registers input for skills, it has to be place similar to the TargetingPlan but higher - on the level with Player shoulders.
Then you will need to setup Layers(read demo section), bake Nav Mesh and delete MainCamera(since you alread placed a camera prefab and there can be only one camera).

Also please note that:
 a. Walls should be in the Obstacles layer if not zombies will see throught them.
 b. PlayerSpawnPoint prefab is just an empty game object. Place it onto the scene and when player character will proceed to the next scene he will be placed where this prefab is located.
 c. Waypoint prefab - Place this prefab onto the scene. Once you click it while player is close to it it will open level menu.
    You will also need to setup "Scenes in build" in File/Build settings to use waypoint. Mind the strings in waypoint scripts.	

...or you can just run the demo:

2.Demo and project setup:

Before you can run the demo you'll need to setup Layers and Layers Collision Mastrix in the edit/project settings/physics.
To help you with that I've included picture of how it supposed to look like:

	2.a. Layers:
	Setup layers in layers position starting from position 20 in the same order as on picture or listed below:
	20 Player
	21 Enemies
	22 EnemiesMouseCollider
	23 TargetingPlane
	24 Obstacles
	25 InteractiveObject
	26 Bullets
	27 TargetingPlaneToShoot
	28 Ground
	29 EnemyProjectiles
	30 DoT

	2.b.Layers Collision Mastrix:
	please see .jpg attached in this package to find out how to setup layers matrix.

	2.c. To use waypoint you also have to setup build settings.
	     To do that go to File/Build settings and add scenes from the demo to the build.

	2.d. In Unity5 you also have to setup tags: Please see attached picture.
	     - all children of "DoorPositionToNavigate" should have door tag
	     - MainCanvas should have MainCanvas tag
	     - TargetingPlane and TargetingPlaneToShoot should have targetingPlane tag
	     - Zombie and its child ColliderMouse should have enemy tag, same for ZombieBare
	     - Ray prefab should have DoT tag.
	     - It's easy to spot missing tag in a prefab: missing tag is called "Undefined"

#also please note that demo 2.Level_Doors and 3.Level_Large are accesible runtime only from 1.Level_Small waypoint.

3.Animations:

Animations included in this package are redistributable and are provided by Unity or 
are a part of Carnegie Mellon University Motion Capture Database(http://mocap.cs.cmu.edu/).

While Unity animations are high quality and are just fine for game production CMU animations are far from perfect. 

CMU animations have bad timing and often have unnecessary leg movement included. This make using some skills appear to a beginner like broken, 
but if you use quality animations from some recommended provider it will work great

I've decided to include them to give you chance to play this pack just after purchase.

Firstly you should change the "Crowbar" animation for player character. I use Mixamo "stable slash" for a web demo, 
but on the asset store you can find many other quality animations providers.

Other CMU animations that you would want to replace are zombie animations and open door and cast spell for player character.

4. Particles/VFX/SkillEffects

All skill effects were created using Particle Playground(https://www.assetstore.unity3d.com/en/#!/content/13325), top notch software I recommend.

5. C# & JavaScript

Starting from version 1.2 I'm focusing on C#, for now JS version stays in its 1.1 form. I might update JS version at some point in future. 
I decided to do this because most of people are using C# and dropping JS for now, allows me to introduces changes more quickly.
If you are an JS user let me know by email(is there any out there?:]), this might motivate me to do the JS update ;].

Be carefull not to mix C# & JS scripts, they are the same, so if you already have a language preference you might as well delete C#/JavaScript folder.

6. Input

You can switch input in aRPG_Input script.
Mouse and keyboard input is MouseInput and ActionButtonsInput bools set to true.
ActionButtonsInput set to false means that skills are executed by touching them or by mouse clicking in editor.

7. Mobile/WASD

To test/simulate mobile input on desktop I suggest using Input setup with WASD keys for movement and simulating touch input(for the purpose of launching skills) with mouse. 
This means you need to setup aRPG_Input bools so that only WASD bool is true. 

Doors open automatically(on collision with door collider) in mobile mode.

Waypoint mechanics in the demo dont work with WASD/Mobile input in editor, but it does work on Mobile device - when you actually build .apk and install it on your device. I'll address this later.

8. UI
Starting from 1.2 you can now add new buttons without a need to code.
To add new action button, just copy the existing one and move it somewhere. Same with buttons that you press to change skills or weapons.

9. Scriptable objects(skills, weapons, etc.) notes
In Resources you have three folders, each contains respectively weapons, skills and AIs Scriptable Objects(SO), you can duplicate these SOs to add new skills etc..
Weapons are for weapons that a player uses. Duplicate a weapon and change variables in SO to make it unique, if you want to add unique look your new weapon add your model to a player bone where other models are, setup reference for it
in the Pickup script and setup weaponModelName in the SO.

AI are used in the EnemyMovement script attached to each enemy. Most important thing to change here is to setup whether an enemy is melee or a spellcaster and to choose a skill(most of skills in the skills folder can be used by enemies)

Skills - most important thing to change here is to setup a unique name and choose an archetype of a skill in inspector. Then you can setup and change respective variables to create new unique skills.
Once skill archetype is defined defined only work with variables under coresponding header, other variables dont affect skill.
When changing VFX of skill check out existing one and how it is constructed, often vfx requires coping over a script and some components like collider or a rigidbody. 
When adding new skills by duplicating existing make sure you made UNIQUE_skillName variable value unique. 

10. Naming - "@"
Often you might find a string or an object name that has "@" symbol next to it, this means that this string/name is in the code somewhere
An example is "@medkit" string in the UNIQUE_skillName variable in the Skill Scriptable Object, this is connected to the name of the prefab that pickup a medkit to increase its quantity. So if you want to change that
search the code for that string to find out to which object/variable it is connected and change both names and strings in the code. 

100. This documentation is under construction I'll be slowly improving it with each consequent update. In the meantime feel free to email me if you have any questions/problems or if you'd want to share your experience with this asset.

Outofmymindgames,
Regards.