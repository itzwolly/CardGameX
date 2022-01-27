# Card Game X #

## Rush effect ##
[![Watch the video](https://img.youtube.com/vi/OyhJCGYyFyI/hqdefault.jpg)](https://youtu.be/OyhJCGYyFyI)

## Damage To Adjacent Effect ##
[![Watch the video](https://img.youtube.com/vi/Alu8CzdfGlk/hqdefault.jpg)](https://youtu.be/Alu8CzdfGlk)

## Empower Effect ##
[![Watch the video](https://img.youtube.com/vi/kcZhvurNGVU/hqdefault.jpg)](https://youtu.be/kcZhvurNGVU)

## Spell Card Target Effect ##
[![Watch the video](https://img.youtube.com/vi/nQ-OVpeFC7w/hqdefault.jpg)](https://youtu.be/nQ-OVpeFC7w)

## Spell Card Multi-Target ##
[![Watch the video](https://img.youtube.com/vi/B3oNk27r-DI/hqdefault.jpg)](https://youtu.be/B3oNk27r-DI)

## Spell Card ##
[![Watch the video](https://img.youtube.com/vi/LYzgmiJqECo/hqdefault.jpg)](https://youtu.be/LYzgmiJqECo)

## Server ##

The server is setup using Photon as its base and has been extended to be an authoritative server. 
The server handles the majority of the validations as we don't want the client to be able to cheat, 
by knowing what cards are going to be drawn or play cards that aren't in the player's hand. 

The server also contains a scripting engine which is the backbone of the gameplay. 
The scripting engine allows anyone to design specific card mechanics using LUA. 
The scripting engine is event based and allows designers to use the currently available methods to extend the functionality of cards.
The avialable methods can be extended to include more mechanics aswell.

The card data, such as: name, cost, description, etc. is saved in a database to easily modify when neccessary.

## Client ##

The client is made in Unity and connects to the server using Photon's library.

## Game Launcher ##

Card Game X also includes a custom game launcher which allows the user to login and also update their game files,
by pressing the Play button. The launcher is made in C# Windows Presentation Forms and it contains secure login using
threads. The user information is saved in a database to also ensure security.

# Made By #
Volkan Baykal - Programmer, Lead Game Designer\
Oleksandr Mazur - Audio Designer, Game Designer\
Tim Joosten - 3D/2D Artist, Game Designer\
