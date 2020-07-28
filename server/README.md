# Project Title

One Paragraph of project description goes here

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

What things you need to install the software and how to install them

- [Install .Net Core 3.1](https://www.microsoft.com/net/download/windows)

### Installing

A step by step series of examples that tell you have to get a development env running

1 - Change "DefaultConnection" in /appsettings.json file

```
{
  "AppSettings": {
    "Secret": "ASWE!@#$@",
    "TokenValidDays": 7,
    "DefaultConnection": "Data Source=[change IP];Initial Catalog=[change DB];Integrated Security=False;User [change user]=sa;Password=[change pwd];MultipleActiveResultSets=True"
  },
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Warning"
    }
  }
}

```

Run the follow commands from the project directory:

2 - Restore packages

```
dotnet restore
```

3 - Install migrations

```
dotnet ef database update
```

End with an example of getting some data out of the system or using it for a little demo

## Running the tests

Run the follow commands from the project directory:

```
dotnet run
```

### Random String

```
https://www.random.org/strings/?num=2&len=10&upperalpha=on&loweralpha=on&unique=on&format=html&rnd=new
```

### Break down into end to end tests

Explain what these tests test and why

```
Give an example
```

### And coding style tests

Explain what these tests test and why

```
Give an example
```

## Deployment

run:
dotnet publish -c Release

https://docs.microsoft.com/en-us/dotnet/core/deploying/deploy-with-cli

## Built With

- [ASP.NET Core 3.1](https://docs.microsoft.com/en-us/aspnet/core/getting-started/) - The web framework used
- [Entity Framework Core](https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro) - Code first framework used
- [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?tabs=visual-studio%2Caspnetcore2x) - User Management
- [Token Base Authentication](https://jwt.io/) - Authenticacation
- [Maven](https://maven.apache.org/) - Dependency Management
- [ROME](https://rometools.github.io/rome/) - Used to generate RSS Feeds

## Contributing

Please read [CONTRIBUTING.md](https://gist.github.com/PurpleBooth/b24679402957c63ec426) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/your/project/tags).

## Authors

- **Gustavo Gomez** - _Initial work_

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

- Hat tip to anyone who's code was used
- Inspiration
- etc
