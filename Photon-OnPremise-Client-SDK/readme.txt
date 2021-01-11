
Photon Unity3D SDK
------------------------------------------

This SDK implements a lean networking framework, LoadBalancing API, offering programmers detailed control.
As alternative, use Photon Unity Networking (PUN) which integrates more tightly with Unity's development approach.


Package Contents
------------------------------------------

license.txt                                   - the license terms
Photon-DotNet-Client-Documentation_v*.chm     - compiled HTML documentation
readme.txt                                    - this readme text
release_history.txt                           - release history
/PhotonAssets                                 - Asset folder used to install Photon in Unity3D

/demo-particle-unity:
The demo client connects to a master server and shows the random matchmaking mechanism, how to create a room and how to send and receive events within a running game.
Players move 'their' blocks around and the positions are updated in realtime between clients. The UI shows areas of interest when activated.

/demo-photon-chat-uGUI:
This is a simple chat client implementation using the Photon Chat API.

/demo-turnbased-memory:
The demo contains the client and the server-side reference implementation for an asynchronous "memory" game using Photon Realtime. See Memory Demo tutorial for additional info.

/demo-turnbased-sandbox:
A basic testbed demo to show how to take turns, store gamestate and work with Photon's persistency.


LoadBalancing Documentation
------------------------------------------

The API reference is included in this package as CHM file.
Find the current manuals, tutorials, API reference and more online:
http://doc.photonengine.com


Running the Demos
------------------------------------------

Our demos are built for the Photon Cloud for convenience (no initial server setup needed).
The service is free for development and signing up is instant and without obligation.

Every game title on the Cloud gets it's own AppId string which must be copied into the clients.
The demos use a property "AppId" in the source files. Set it's value before you build them.

Sign In:             https://www.photonengine.com/en/Account/SignIn
AppId in Dashboard:  https://www.photonengine.com/dashboard

Each application type has it's own AppId (e.g Realtime and Chat).
You will find specific sections of the Dashboard per application type.

Alternatively you can host a "Photon Cloud" yourself. The AppId is not used in that case.
Download the server SDK here:
https://www.photonengine.com/OnPremise/Download

How to start the server:
http://doc.photonengine.com/en/onpremise/current/getting-started/photon-server-in-5min


Chat Documentation
------------------------------------------

    http://doc.photonengine.com/en/chat
    http://forum.photonengine.com


Implementing Photon Chat
------------------------------------------

    Photon Chat is separated from other Photon Applications, so it needs it's own AppId.
    Our demos usually don't have an AppId set.
    In code, find "<your appid>" to copy yours in. In Unity, we usually use a component
    to set the AppId via the Inspector. Look for the "Scripts" GameObject in the scenes.

    Register your Chat Application in the Dashboard:
    https://www.photonengine.com/en/Chat/Dashboard

    The class ChatClient and the interface IChatClientListener wrap up most important parts
    of the Chat API. Please refer to their documentation on how to use this API.
    More documentation will follow.

    If you use Unity, copy the source from the ChatApi folder into your project.
    It should run in most cases (unless your Photon assembly is very old).


Unity Notes
------------------------------------------

If you don't use Unity, skip this chapter.


We assume you're using Unity 5.x. 
The demos are prepared to export for Standalone, not WSA or other.

The SDK contains a "PhotonAssets" folder. To import Photon into your project, copy it's content 
into your project's Asset folder. You may need to setup the DLLs in Unity 5 Inspector to export 
for your platform(s).

Currently supported export platforms are:
    Standalone (Windows, OSx and Linux)
    Web (Windows and MacOS)
    WebGL
    iOS         (Unity 4 needs iOS Pro Unity license)
    Android     (Unity 4 needs Android Pro Unity license)
    Windows Store 8.1 and 10    (including Phone and UWP with one library)
    PS3, PS4 and XBox           (certified developers should get in contact with us on demand)


All Unity projects must use ExitGames.Client.Photon.Hashtable!
This provides compatibility across all exports.
Add this to your code (at the beginning), to resolve the "ambiguous Hashtable" declaration:
using Hashtable = ExitGames.Client.Photon.Hashtable;


Web players do a policy-file request to port TCP 843 before they connect to a remote server.
The Photon Cloud and Server SDK will handle these requests.
If you host the server, open the additional "policy request" port: TCP 843. If you configure
your server applications, run "Policy Application" for webplayers.


How to add Photon to your Unity project:
1) The Unity SDK contains a "PhotonAssets" folder. 
   Copy the content of PhotonAssets\ into your project's Assets folder.
2) Make sure to have the following line of code in your scripts to make it run in background:
   Application.runInBackground = true; //without this Photon will loose connection if not focussed
3) Add "using Hashtable = ExitGames.Client.Photon.Hashtable;" to your scripts. Without quotation.
4) iOS build settings (Edit->Project Settings->Player)
   "iPhone Stripping Level" to "Strip Bytecode" and use
   "DotNet 2.0 Subset".
5) If you host Photon, change the server address in the client. A default of "localhost:5055" won't work on device.
6) Implement OnApplicationQuit() and disconnect the client when the app stops.


Windows Store Notes
------------------------------------------

To run demos on a smartphone or simulator, you need to setup your server's network address or
use the Photon Cloud.

We assume you run a Photon Server on the same development machine, so by default, our demos have a
address set to "127.0.0.1:5055" and use UDP.
Demos for LoadBalancing or the Cloud will be set to: "ns.exitgames.com:5055" and use UDP.

Search the code and replace those values to run the demos on your own servers or in a simulator.
The Peer.Connect() method is usually used to set the server's address.

A smartphone has it's own IP address, so 127.0.0.1 will contact the phone instead of the machine
that also runs Photon.