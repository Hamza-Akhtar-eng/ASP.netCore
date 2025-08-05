#Game Store

##Starting SQL Server docker container
```powershell
$sa_password = "password here"
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$sa_password" -p 1433:1433 -d -v sqlvolume:/var/opt/mssql --rm --name mssql mcr.microsoft.com/mssql/server:2022-latest
```
##Setting the connection string in Secret Manager
```powershell
$sa_password = "set password here"
dotnet user-secrets set "ConnectionStrings:GameStoreContext" "Server=localhost; Database=GameStore; User Id=sa; Password=$sa_password;TrustServerCertificate=True"
```
