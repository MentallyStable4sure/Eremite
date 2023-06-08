# C# Discord Bot with MySQL DB on DSharpPlus [.NET 8]
 2nd version of an [old mini-games Genshin bot](https://github.com/dentalmisorder/genshin-eremite-discordbot). .NET 5+ is suggested. Pull requests are welcomed.
***
> [Documentation WIP]

> [Invite Bot](https://discord.com/api/oauth2/authorize?client_id=739487241469952000&permissions=8&scope=bot) or mess with the bot in [our server #bot-commands](https://discord.gg/mentallystable4sure)
***

# How to setup
  <img src=https://github.com/MentallyStable4sure/Eremite/blob/main/content/img_pull2.png width='450'></img>

  - Open startup_config.json and edit token from [your applications developer portal](https://discord.com/developers/applications)
  - Open dbconfig.json and edit with your mysql database credentials (you can use MAMP if you just want to test it locally or phpmyadmin)

  > CONFIGS NOTE: If you want to test server build navigate to [releases](https://github.com/MentallyStable4sure/Eremite/releases) since build already has all the configs in json, but if u want to start it as a project (in debug mode for example), you gotta move this juicy config folder into /bin/debug(or release)/net8.0/

### Want a custom DB?

 > CUSTOM DB NOTE: If you want to use something other beside MySQL (MSSQL/PostgreSQL/REST/etc.) feel free to modify [DbConnector.cs](Eremite/Services/DbConnector.cs) and [DatabaseConfig.cs](Eremite/Data/DatabaseConfig.cs) since those are the connector and the db credentials model

  <img src=https://github.com/MentallyStable4sure/Eremite/blob/main/content/img_profile.png width='450'></img>


***

# Credits

> Thanks to [Enka.Network](https://github.com/EnkaNetwork) for [API](https://github.com/EnkaNetwork/API-docs) provided, and [DevilTakoyaki](https://twitter.com/deviltakoyaki) for materials provided.
