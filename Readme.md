# Test Task for Modsen
To run the program you must enter the following commands:

docker-compose up -d

dotnet ef database update -s TestTaskModsen.Api -p TestTaskModsen.Persistence

cd ./TestTaskModsen.API

dotnet run --launch-profile "https"

To test Api click here: [swagger](https://localhost:7255/swagger/index.html)

