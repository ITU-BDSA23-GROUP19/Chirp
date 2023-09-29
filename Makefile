.PHONY: all build clean

all: build

bin/Release/net7.0/linux-x64/publish/Chirp: src/Chirp.CLI/Chirp.CLI.csproj
	dotnet publish --runtime "linux-x64" --self-contained false

bin/Release/net7.0/osx-x64/publish/Chirp: src/Chirp.CLI/Chirp.CLI.csproj
	dotnet publish --runtime "osx-x64" --self-contained false

bin/Release/net7.0/osx-arm64/publish/Chirp: src/Chirp.CLI/Chirp.CLI.csproj
	dotnet publish --runtime "osx-arm64" --self-contained false

bin/Release/net7.0/win-x64/publish/Chirp.exe: src/Chirp.CLI/Chirp.CLI.csproj
	dotnet publish --runtime "win-x64" --self-contained false

build: bin/Release/net7.0/linux-x64/publish/Chirp bin/Release/net7.0/osx-x64/publish/Chirp bin/Release/net7.0/osx-arm64/publish/Chirp bin/Release/net7.0/win-x64/publish/Chirp.exe

clean:
	rm -r ./bin/ ./obj/