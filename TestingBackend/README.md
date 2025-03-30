# testing backend

This web application is built using the ASP.NET Core web framework.

### initialization

TestingBackend was initialized with the command: `dotnet new web -o TestingBackend -f net8.0`

### run

Locally: 

- Launch PSQL (in wsl)
`sudo service postgresql start`
`sudo nano /etc/postgresql/<version>/main/postgresql.conf`



- Start Web App
`dotnet run` 
    Find Host location within ./Properties/launchSettings.json (currently http://localhost:5271[/swagger])

