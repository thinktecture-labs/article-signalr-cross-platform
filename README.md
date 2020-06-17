# Cross-platform SignalR demo

## Initial Setup

### Angular Frontend
* Switch into folder `frontend/angular-signalr-sample`
* run `npm install` to install all dependencies
* run `npm start` to start the web server with the address `http://localhost:4200`
* to build the ios app to an output folder run `npm run build-ios` (after this, XCode will run if this is available)

### Blazor Frontend
* Switch into folder `frontend/BlazorSignalRSample`
* run `dotnet restore` to install all dependencies
* Switch into folder `frontend/BlazorSignalRSample/Client`
* run `dotnet run` to start the web server with the address `https://localhost:6001`

### .NET Core Backend
* Switch into folder `backend`
* run `dotnet restore` to install all dependencies

#### IdentityServer
* Switch into folder `SignalRSample.IdentityServer`
* run `dotnet run` to start the web server with the address `http://localhost:5000`

#### API
* Switch into folder `SignalRSample.Api`
* run `dotnet run` to start the web server with the address `http://localhost:5002`