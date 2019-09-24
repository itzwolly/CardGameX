# Card Game X

## Server

The server is setup using Photon as its base and has been extended to be an authoritative server. 
The server handles the majority of the validations as we don't want the client to be able to cheat, 
by knowing what cards are going to be drawn or play cards that aren't in the player's hand. 

The server also contains a scripting engine which is the backbone of the gameplay. 
The scripting engine allows anyone to design specific card mechanics using LUA. 
The scripting engine is event based and allows designers to use the currently available methods to extend the functionality of cards.
The avialable methods can be extended to include more mechanics aswell.

The card data, such as: name, cost, description, etc. is saved in a database to easily modify when neccessary.

## Client

The client is made in Unity and connects to the server using Photon's library.

## Game Launcher

Card Game X also includes a custom game launcher which allows the user to login and also update their game files,
by pressing the Play button. The launcher is made in C# Windows Presentation Forms and it contains secure login using
threads. The user information is saved in a database to also ensure security.

