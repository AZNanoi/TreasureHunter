# TreasureHunter

Ah Zau Marang (am223es@student.lnu.se)


Treasure Hunter


My game is called Treasure Hunter. In the game the player finds a treasure chest on an island going through 2 villages in the forest and fighting against armed evils on the way. The player kills an evil by slashing with sword. Evils are equipped with sword as well, which they use it for killing the player. 

Game Objectives


The objective of the player is without being killed to find the treasure by opening a treasure chest with a key. The key can be found in a village. The game is over when the play opens the treasure chest with the key or is killed by evils. 

Feature List


The game will have the following features:

Player
The player can walk by pressing the arrow keys and run by holding S(Shift) key.
The player can rotate by holding the Ctrl key and pressing the left/right arrow key.
The player swings the sword by pressing the “Space“ key.
The player has to wait some time for each slash.
The player can not do any damage to other objects except evils.
A sound effect shall also be played each time the player slashes an evil.
The player starts with 100% health, which is reduced by 5% each time an evil slashes the player.
The player picks up the powerup particles by colliding them.
Each time the player picks up an powerup particle a pickup sound effect is played.
When the player is slashed by an evil a sound effect and pain animation is played.
When the player is killed a dead animation and sound effect is played.
When the player comes close to the treasure chest, a text shall be displayed saying to press the U key to unlock the chest or “You are missing the key. Get the key first!” if the player does not have the key.

Evil/enemy
The evils are spawned at random places on the road and around the houses. More evils shall be spawned in villages and most of them around the treasure.
The evils cannot overlap each other.
When an evil is killed it spawns a powerup particle for the player’s health or weapon.
The evils are walking around the place where they were spawned unless they detect the player.
When the evils detect the player within a certain distance, they walk towards the player and attack when they get close to the player.
All the evils have the same limited rate for slashing the player. 
The evils cannot attack the player while they are being slashed by the player.
When the player escapes, the evils stop moving towards the player and go back to the place where they were spawned and walk around the place.
The evils start with 100% health and the health can be seen over their head.
The evils’ health are reduced by 25% each time the player slashes them.
The evils cannot pick up any powerup particle.
When an evil is slashed by the player a sound effect and pain animation is played.
The evils are killed/destroyed if their health gets down to 0%.
When an evil is killed a sound effect and dead animation is played.

Powerup particle
A powerup particle with green color and “+” sign increases the player’s health by 5%.
A powerup particle with blue color increases the strength of player’s sword by 20%.
A powerup particle is floating at the place where it was spawned.
The PowerUps never disappear.

Sword Power Bar
When the player has reached 100% power of sword, the player can press the S(P) key to increase the damage/strength of sword by 25% for 1 minute.
Once the power is used, it shall be set back to 0%.

Camera
The player shall be followed by the camera from behind and a bit above.

Game Controller
Before the game starts a text with the title of the game is shown, saying the player to press the “Space” key to start the game or the “I” key to read/display the instruction for how to play the game. When the instruction is displayed, the player shall also see the text saying to press the “Space” key to start the game.
When the game is ended a “Game Over! Press R to restart” text is displayed, and the player can restart the game by pressing the R key.
The player health and sword power bar are displayed on the top left of the screen. The color of the health bar changes from green to yellow to red depending on how much health the player has left.
When the player have picked up the key, it will be shown on the top right of the screen.

Game World
The game world is an island surrounded by water.
The player cannot go into water and the shore shall be the edge of the game world.
The game world has mountains, trees, grass, bushes, ground, houses, a bridge, stones, campfire, sky and furnitures.



Prototype of Game World


Prototype of Game View



Assets

I will be using the following free assets from Unity Asset Store in my game:
Female Warrior Princess for the player
https://www.assetstore.unity3d.com/en/#!/content/44041
Fantasy Monster-Skeleton for the evil
	https://www.assetstore.unity3d.com/en/#!/content/35635
PowerUp particles for the powerup particle and the pickup sound effect
	https://www.assetstore.unity3d.com/en/#!/content/16458
The Blacksmith: Environments for the mountains, trees, houses and bridges
	https://www.assetstore.unity3d.com/en/#!/content/39948
Campfire for the campfire
	https://www.assetstore.unity3d.com/en/#!/content/45038
Classic Treasure Box for the treasure chest
	https://www.assetstore.unity3d.com/en/#!/content/8952
Handpainted Keys for the key to unlock the treasure chest
	https://www.assetstore.unity3d.com/en/#!/content/42044
Realistic Tree 9 for the tree, bushes, grass and ground
	https://www.assetstore.unity3d.com/en/#!/content/54622
Medieval Gold for the treasure
	https://www.assetstore.unity3d.com/en/#!/content/14162 
Middle Age - Medieval Action Sound FX Pack for arrows and swords sound effects and the background music
	https://www.assetstore.unity3d.com/en/#!/content/54030
Mangled Screams for human and evil voices
	https://www.assetstore.unity3d.com/en/#!/content/41754
Sky FX Pack for the sky
	https://www.assetstore.unity3d.com/en/#!/content/19242

