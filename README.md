# ITU-BDSA23-GROUP19
## Group members
- Annabell Philip Nørdam - <apno@itu.dk>   
- Eva Afzelius - <evaf@itu.dk>  
- Patrick Handberg Jessen - <patj@itu.dk>  
- Rebekka Mia Pahus Pedersen - <rebp@itu.dk>  
- Simon Benjamin Sander Rasmussen - <besr@itu.dk>

## Commit with co-authors
Use the following lines to assign co-authors to a commit.  
- Co-authored-by: Annabell <apno@itu.dk>  
- Co-authored-by: Eva <evaf@itu.dk>  
- Co-authored-by: Patrick <patj@itu.dk>  
- Co-authored-by: Rebekka <rebp@itu.dk>  
- Co-authored-by: Simon <besr@itu.dk>
  
+ Co-authored-by: ChatGPT

<br>

# Chirp.Razor
## Running the application
### Locally
- Change directory to `Chirp/src/Chirp.Razor`.  
- Use `dotnet run`, this will start the application locally.  
- Open your browser and go to the following URL.  
http://localhost:5273/

### Remotely
- Open your browser and go to the following URL.  
https://bdsagroup19chirprazor.azurewebsites.net/

<br>

# Chirp.CLI
## Running the application
- Change directory to `Chirp/src/Chirp.CLI`.  
- Use the following commands to interact with the application.  
`dotnet run cheep <message>` - Post a cheep.  
`dotnet run read` - Retrieve all cheeps.  
`dotnet run read <limit>` - Retrieve an amount of cheep.


## Access the CSV database remotely
- Open your browser and go to the following URL.  
https://bdsagroup19chirpremotedb.azurewebsites.net/  

- Add one of the following lines to the URL.  
`cheeps` - Retrieve all cheep  
`cheeps?limit=<limit>` - Retrieve an amount of cheep.