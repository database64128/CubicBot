# 🤖 Cubic Bot

[![Build](https://github.com/database64128/CubicBot/workflows/Build/badge.svg)](https://github.com/database64128/CubicBot/actions?query=workflow%3ABuild)
[![Release](https://github.com/database64128/CubicBot/workflows/Release/badge.svg)](https://github.com/database64128/CubicBot/actions?query=workflow%3ARelease)

A stupid and annoying chatbot for your group chats.

## Features

- Modular architecture: Disable any module you don't need.
- Adaptive design: Detect bot privacy mode and automatically disable incompatible features.
- Easy maintenance: Register enabled commands on startup.

## Modules

### Commands

#### 1. Common

- `/chant`: 🗣 Say it out loud!
- `/drink`: 🥤 I'm thirsty!
- `/thank`: 🦃 Reply to or mention the name of the person you would like to thank.
- `/thanks`: 😊 Say thanks to me!
- `/vax`: 💉 Gen Z also got the vax!

#### 2. Dice

- `/dice`: 🎲 Dice it!
- `/dart`: 🎯 Oh shoot!
- `/basketball`: 🏀 404 Basket Not Found
- `/soccer`: ⚽ It's your goal!
- `/roll`: 🎰 Feeling unlucky as hell?
- `/bowl`: 🎳 Can you knock them all down?

#### 3. Consent Not Needed

- `/cook`: 😋 Who cooks the best food in the world? Me!
- `/force`: ☮️ Use of force not recommended.
- `/touch`: 🥲 No touching.
- `/fuck`: 😍 Feeling horny as fuck?

#### 4. Not A Vegan

- `/eat`: ☃️ Do you want to eat a snowman?

#### 5. Law Enforcement

- `/call_cops`: 📞 Hello, this is 911. What's your emergency?
- `/arrest`: 🚓 Do I still have the right to remain silent?
- `/guilty_or_not`: 🧑‍⚖️ Yes, your honor.

#### 6. Public Services

- `/call_ambulance`: 🚑 Busy saving lives?
- `/call_fire_dept`: 🚒 The flames! Beautiful, aren't they?

#### 7. Chinese

- `/interrogate`: 🔫 开门，查水表！

#### 8. Chinese Tasks

- `/ok`: 👌 好的，没问题！
- `/assign`: 📛 交给你了！
- `/unassign`: 💢 不干了！

#### 9. Query Stats

- `/my_stats`: 🤳 View your stats in this chat.
- `/leaderboard_command`: ⌨️ View command usage rankings in this chat.
- `/leaderboard_grass`: 🍀 View grass growth rankings in this chat.

### Stats

#### 1. Grass

The Chinese charater "草" is commonly seen in Chinese text messages. It could mean "Damn.", "Shit.", "Fuck!", or simply "Awesome!".

This stats counter counts each individual's usage of "草" in group chats and generates usage rankings. Say it when you feel like it to unlock achievements.

The counter also recognizes common typos like "cao", "艹", "c奥", "c嗷", etc.

## Build

Prerequisites: .NET 5 SDK

Note for packagers: The application by default uses executable directory as config directory.
To use user's config directory, define the constant `PACKAGED` when building.

```bash
# Build with Release configuration
$ dotnet build -c Release

# Publish as framework-dependent
$ dotnet publish CubicBot.Telegram -c Release

# Publish as self-contained for Linux x64
$ dotnet publish CubicBot.Telegram -c Release \
    -p:PublishReadyToRun=true \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true \
    -p:TrimMode=link \
    -p:DebuggerSupport=false \
    -p:EnableUnsafeBinaryFormatterSerialization=false \
    -p:EnableUnsafeUTF7Encoding=false \
    -p:InvariantGlobalization=true \
    -r linux-x64 --self-contained

# Publish as self-contained for packaging on Linux x64
$ dotnet publish CubicBot.Telegram -c Release \
    -p:DefineConstants=PACKAGED \
    -p:PublishReadyToRun=true \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true \
    -p:TrimMode=link \
    -p:DebuggerSupport=false \
    -p:EnableUnsafeBinaryFormatterSerialization=false \
    -p:EnableUnsafeUTF7Encoding=false \
    -p:InvariantGlobalization=true \
    -r linux-x64 --self-contained
```

## Usage

```bash
# See usage guide.
$ cubic-bot-telegram --help

# Set bot token in config.
$ cubic-bot-telegram config set --bot-token "1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy"

# Disable all stats.
$ cubic-bot-telegram config set --enable-stats-mod false

# Check config.
$ cubic-bot-telegram config get

# Start bot.
$ cubic-bot-telegram
```

## License

- This project is licensed under [GPLv3](LICENSE).
- The icons are from [Material Design Icons](https://materialdesignicons.com/) and are licensed under the [Pictogrammers Free License](https://dev.materialdesignicons.com/license).
- [`System.CommandLine`](https://github.com/dotnet/command-line-api) is licensed under the MIT license.
- `System.Linq.Async` and `System.Interactive.Async` are from [dotnet/reactive](https://github.com/dotnet/reactive). They are licensed under the MIT license.
- [`Telegram.Bot`](https://github.com/TelegramBots/Telegram.Bot) and [`Telegram.Bot.Extensions.Polling`](https://github.com/TelegramBots/Telegram.Bot.Extensions.Polling) are licensed under the MIT license.

© 2021 database64128
