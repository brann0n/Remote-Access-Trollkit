# Remote-Access-Trollkit
Development project voor het vak C# 2 van de 4e periode in jaar 2

## Wat is de trollkit?
Dit is een remote access tool waarmee je 'grappige' acties kunt uitvoeren op een andere pc.


## Vaste waarden

### Pipeline Commands

#### TaskHandler (client)
	"DeleteTask" - Removes the task from the local task scheduler in Windows.

### Data headers
guidelines for the received data:
first byte is check:
			0x1A: Response			
			0x1B: Command			
			0x1C: Data
			
second byte is data length:
			0x01: 1x 2029 bytes sets			
			0x1B: 27x 2029 bytes sets		
			0x22: 34x 2029 bytes sets etc...
			
third byte is series:
			0x01: 1/N byte set	
			0x1A: 26/N byte set etc...
			
the next 16 bytes are unique id:
			0x00112233445566778899AABBCCDDFF00
