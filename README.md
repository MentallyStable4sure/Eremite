# C# Discord Bot with MySQL DB on DSharpPlus [.NET 7]
 2nd version of an [old mini-games Genshin bot](https://github.com/dentalmisorder/genshin-eremite-discordbot). .NET 7+ is required. Pull requests are welcomed.
***
> [Documentation](https://mentally-stable.gitbook.io/eremite/)

> [Invite Bot](https://discord.com/api/oauth2/authorize?client_id=739487241469952000&permissions=8&scope=bot) or mess with the bot in [our server #bot-commands](https://discord.gg/mentallystable4sure)
***
# This bot is a basically a DEMO-version (limited)
Actual [Eremite APP](https://github.com/MentallyStable4sure/Eremite-App) is avaliable in our Launcher.
Bot functions:
- Gift Welkin Moon in Genshin Impact
- Pull characters
- Sacrifice characters
- Earn pills/primogems/mora
- Set main character/receive buffs
- Go on an Adventures
- Do dailies

# How to setup
  <img src=https://github.com/MentallyStable4sure/Eremite/blob/main/content/img_pull2.png width='450'></img>

  - Open startup_config.json and edit token from [your applications developer portal](https://discord.com/developers/applications)
  - Open dbconfig.json and edit with your mysql database credentials (you can use MAMP if you just want to test it locally or phpmyadmin)

  > CONFIGS NOTE: If you want to test server build navigate to [releases](https://github.com/MentallyStable4sure/Eremite/releases) since build already has all the configs in json, but if u want to start it as a project (in debug mode for example), you gotta move this juicy config folder into /bin/debug(or release)/net8.0/

### Want a custom DB?

 > CUSTOM DB NOTE: If you want to use something other beside MySQL (MSSQL/PostgreSQL/REST/etc.) feel free to modify [DbConnector.cs](Eremite/Services/DbConnector.cs), [QueryHandler.cs](Eremite/Services/QueryHandler.cs) and [DatabaseConfig.cs](Eremite/Data/DatabaseConfig.cs) since those are the connector and the db credentials model

  <img src=https://github.com/MentallyStable4sure/Eremite/blob/main/content/img_profile.png width='450'></img>


***

# Credits

> Thanks to [@Escartem](https://github.com/Escartem) for French localization.
> Thanks to [Enka.Network](https://github.com/EnkaNetwork) for [API](https://github.com/EnkaNetwork/API-docs) provided, and [DevilTakoyaki](https://twitter.com/deviltakoyaki) for materials provided.
