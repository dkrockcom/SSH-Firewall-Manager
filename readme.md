# SSH Firewall Manager

SSHService - It is .Net Web Service application which is use for manage Linux Server Firewall rules

## Features!
  - Web Api for handle another server http request.
  - Create/Delete Firewall rules based on http request.
  
## Tech

* [.Net Core] - .NET Core is a new version of .NET Framework, which is a free, open-source, general-purpose development platform maintained by Microsoft. It is a cross-platform framework that runs on Windows, macOS, and Linux operating systems.
.NET Core Framework can be used to build different types of applications such as mobile, desktop, web, cloud, IoT, machine learning, microservices, game, etc.

## Prerequisite
  - Install [.Net Core] https://dotnet.microsoft.com/download/dotnet/6.0
  - [Checkout] SshService
  - Visual Studio Code
  - Linux OS
  - 27000 Enable port for HTTP Access of this application
  - 10000-20000 Default port which is used by your server
  - If We use azure so we need to open port according to our requirement
  - Tested on Ubuntu 20.04.3 LTS
  - Open SSH Serevr shluld be installed
  - Change sshd server config "/etc/ssh/sshd_config"

```
GatewayPorts yes
```

## Installation

SSH Service requires .Net Core to run

1. Change Appsettings.Json > location of authorized_keys file for read write

```
 "AuthorizedKeysFile": "/home/azureuser/.ssh/authorized_keys"
```

## For production build
```
dotnet publish -c release -r linux-x64 --self-contained true
```

## API Requests

### API /AddRule - Api for add rule in firewall
```
Method: "POST"
Params: {
  IpAddress: string,
  Port: Number
}
```

### API /DeleteRule - Api for delete rule from firewall
```
Method: "POST"
Params: {
  Port: Number
}
```

### API /Status - Api for get firewall status
```
Method: "GET"
```

### API /GenerateKey - Generate SSH-Key for Authentication
```
Method: "GET"
```

## Service Installation on linux Server
```
sudo apt-get update
apt-get install curl libunwind8 gettext -y;
mkdir /SshService
COPY ALL item from .net build and paste it here 
cd /SshService
sudo cp SshService.service /etc/systemd/system
sudo systemctl daemon-reload
sudo systemctl reset-failed
sudo systemctl enable SshService
sudo systemctl start SshService
```

### Important Links
 1. [Dotnet core SDK and Runtimes](https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-3.1.409-windows-x64-installer)
 2. [Git](https://git-scm.com/downloads)