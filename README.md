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

#### VisualsHandler (client)
Command | Value | Description
------- | ----- | -----------
BlackScreen | (none) | Turns the screen of the client off.
TextBox | "textToDisplay" | Shows a message box on the clients screen
ShowImage | "base64Image" | Shows an image on the clients screen
OpenSite | "url" | Opens a website on the users screen
SetBackground | "base64Image" | Sets the provided image as the users background (also disables wallpaper engine)

### Data headers
guidelines for the received data:
first byte is check:
```
0x1A: Response  
0x1B: Command  
0x1C: Data
```
second 2 bytes is data length:
```
0x0001: 1x 2029 bytes sets  
0x001B: 27x 2029 bytes sets  
0x0022: 34x 2029 bytes sets  
0x012C: 300x 2029 bytes sets etc...  
```
fourth byte is series:
```
0x01: 1/N byte set  
0x1A: 26/N byte set etc...  
```
the next 16 bytes are unique id:
```
0x00112233445566778899AABBCCDDFF00
```
