# ITU-BDSA23-GROUP19
## Group members
- Annabell Philip Nørdam (apno)  
  - ITU-mail: <apno@itu.dk>  
  - Github account: https://github.com/zqueamish
  
+ Eva Afzelius (evaf)
  + ITU-mail: <evaf@itu.dk>
  + Github account: https://github.com/evafz
  
- Patrick Handberg Jessen (patj)
  - ITU-mail: <patj@itu.dk>
  - Github account: https://github.com/PatrickHJessen
  
+ Rebekka Mia Pahus Pedersen (rebp)
  + ITU-mail: <rebp@itu.dk>
  + Github account: https://github.com/RebekkaHeart
  
- Simon Benjamin Sander Rasmussen (besr)
  - ITU-mail: <besr@itu.dk>
  - Github account: https://github.com/Wrenaeris

## Commit with co-authors
Use the following lines to assign co-authors to a commit.  
- Co-authored-by: Annabell <apno@itu.dk>  
- Co-authored-by: Eva <evaf@itu.dk>  
- Co-authored-by: Patrick <patj@itu.dk>  
- Co-authored-by: Rebekka <rebp@itu.dk>  
- Co-authored-by: Simon <besr@itu.dk>
  
+ Co-authored-by: ChatGPT

<br>

# Chirp.CLI
## Running the application
- Change directory to `Chirp/src/Chirp.CLI`.  
- Use the following commands to interact with the application.  
`dotnet run cheep <message>` - Post a cheep.  
`dotnet run read` - Retrieve all cheeps.  
`dotnet run read <limit>` - Retrieve an amount of cheep.


## Access the CSV database remotely
- Open a browser and go to the following URL.  
https://bdsagroup19chirpremotedb.azurewebsites.net/  

- Add one of the following lines to the end of the URL.  
`cheeps` - Retrieve all cheep  
`cheeps?limit=<limit>` - Retrieve an amount of cheep.