# Remote-Access-Trollkit
Development project voor het vak C# 2 van de 4e periode in jaar 2

## Wat is de trollkit?
Dit is een remote access tool waarmee je 'grappige' acties kunt uitvoeren op een andere pc.


## Vaste waarden

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

#### VisualsHandler (client)
Command | Value | Description
------- | ----- | -----------
BlackScreen | (none) | Turns the screen of the client off.
TextBox | "textToDisplay" | Shows a message box on the clients screen
ShowImage | "base64Image" | Shows an image on the clients screen
OpenSite | "url" | Opens a website on the users screen
SetBackground | "base64Image" | Sets the provided image as the users background (also disables wallpaper engine)

### Data headers
In order to keep perfect track of data that is being sent and data that is received we thought of a clever way to devide the byte streams.  
The first 21 bytes of each 2048 byte packet is made up of 'header' data that tells the program what to expect of the next piece of data.

guidelines for the received data are:

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
