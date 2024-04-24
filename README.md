# <img alt="Cubic Bot Logo" src="icons/robot-love.svg" width="40" /> Cubic Bot

[![Build](https://github.com/database64128/CubicBot/actions/workflows/build.yml/badge.svg)](https://github.com/database64128/CubicBot/actions/workflows/build.yml)
[![Release](https://github.com/database64128/CubicBot/actions/workflows/release.yml/badge.svg)](https://github.com/database64128/CubicBot/actions/workflows/release.yml)
[![AUR badge for cubic-bot-telegram-git](https://img.shields.io/aur/version/cubic-bot-telegram-git?label=AUR%20cubic-bot-telegram-git)](https://aur.archlinux.org/packages/cubic-bot-telegram-git/)

A stupid and annoying chatbot for your group chats.

## Features

- Modular architecture: Disable any module you don't need.
- Adaptive design: Detect bot privacy mode and automatically disable incompatible features.
- Easy maintenance: Register enabled commands on startup.

## Modules

### Commands

#### 0. Personal

- `/add_pronouns`: ➕ Add pronouns.
- `/remove_pronouns`: ➖ Remove pronouns.
- `/get_pronouns`: ❤️ Get someone's pronouns by replying to someone's message, or display your own pronoun settings.
- `/set_default_pronouns`: 📛 Set or unset default pronouns for all chats.
- `/set_preferred_pronouns`: 🕶️ Set or unset preferred pronouns for this chat.

#### 1. Common

- `/apologize`: 🥺 Sorry about last night.
- `/chant`: 🗣 Say it out loud!
- `/drink`: 🥤 I'm thirsty!
- `/me`: 🤳 What the hell am I doing?
- `/thank`: 🦃 Reply to or mention the name of the person you would like to thank.
- `/thanks`: 😊 Say thanks to me!
- `/vax`: 💉 This ain't Space Needle!

#### 2. Dice

- `/dice`: 🎲 Dice it!
- `/dart`: 🎯 Oh shoot!
- `/basketball`: 🏀 404 Basket Not Found
- `/soccer`: ⚽ It's your goal!
- `/roll`: 🎰 Feeling unlucky as hell?
- `/bowl`: 🎳 Can you knock them all down?

#### 3. Consent Not Needed

- `/cook`: 😋 Who cooks the best food in the world? Me!
- `/throw`: 🥺 Throw me a bone.
- `/catch`: 😏 Catch me if you can.
- `/force`: ☮️ Use of force not recommended.
- `/touch`: 🥲 No touching.
- `/fuck`: 😍 Feeling horny as fuck?

#### 4. Not A Vegan

- `/eat`: ☃️ Do you want to eat a snowman?

#### 5. Law Enforcement

- `/call_cops`: 📞 Hello, this is 911. What's your emergency?
- `/arrest`: 🚓 Do I still have the right to remain silent?
- `/guilty_or_not`: 🧑‍⚖️ Yes, your honor.
- `/overthrow`: 🏛️ Welcome to Capitol Hill!

#### 6. Public Services

- `/call_ambulance`: 🚑 Busy saving lives?
- `/call_fire_dept`: 🚒 The flames! Beautiful, aren't they?

#### 7. Chinese

- `/interrogate`: 🔫 开门，查水表！

#### 8. Chinese Tasks

- `/ok`: 👌 好的，没问题！
- `/assign`: 📛 交给你了！
- `/unassign`: 💢 不干了！

#### 9. Systemd

- `/systemctl`: `➡️ systemctl <command> [unit]`

#### 10. Query Stats

- `/get_stats`: 📅 View your stats in this chat, or reply to a message to view the sender's stats in this chat.
- `/leaderboard_grass`: 🍀 View grass growth rankings in this chat.
- `/leaderboard_demanding`: 👉 Who's the most demanding person in this chat?
- `/leaderboard_talkative`: 🗣️ Who's the most talkative person in this chat?

### Stats

#### 1. Grass

The Chinese charater "草" is commonly seen in Chinese text messages. It could mean "Damn.", "Shit.", "Fuck!", or simply "Awesome!".

This stats counter counts each individual's usage of "草" in group chats and generates usage rankings. Say it when you feel like it to unlock achievements.

The counter also recognizes common variants and typos like "cao", "艹", "c奥", "c嗷", etc.

#### 2. Command Usage Stats

Counts command usage and generates the demanding leaderboard.

#### 3. Message Counter

Counts text messages and generates the talkative leaderboard.

## Build

Prerequisites: .NET 8 SDK

### Build with Release configuration

```bash
dotnet build -c Release
```

### Publish as framework-dependent

```bash
dotnet publish CubicBot.Telegram.App -c Release
dotnet publish CubicBot.Telegram.Tool -c Release
```

### Publish as self-contained for Linux x64

```bash
dotnet publish CubicBot.Telegram.App -c Release \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true \
    -p:DebuggerSupport=false \
    -p:EnableUnsafeBinaryFormatterSerialization=false \
    -p:EnableUnsafeUTF7Encoding=false \
    -p:InvariantGlobalization=true \
    -r linux-x64 --self-contained
dotnet publish CubicBot.Telegram.Tool -c Release \
    -p:PublishSingleFile=true \
    -p:PublishTrimmed=true \
    -p:DebuggerSupport=false \
    -p:EnableUnsafeBinaryFormatterSerialization=false \
    -p:EnableUnsafeUTF7Encoding=false \
    -p:InvariantGlobalization=true \
    -r linux-x64 --self-contained
```

## Usage

```bash
# See usage guide.
$ cubic-bot-telegram-tool --help

# Set bot token in config.
$ cubic-bot-telegram-tool config set --bot-token "1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy"

# Disable all stats.
$ cubic-bot-telegram-tool config set --enable-stats-mod false

# Check config.
$ cubic-bot-telegram-tool config get

# Start bot.
$ cubic-bot-telegram-app
```

## License

- This project is licensed under [GPLv3](LICENSE).
- The icons are from [Material Design Icons](https://materialdesignicons.com/) and are licensed under the [Pictogrammers Free License](https://dev.materialdesignicons.com/license).
- [`System.CommandLine`](https://github.com/dotnet/command-line-api) is licensed under the MIT license.
- `System.Linq.Async` and `System.Interactive.Async` are from [dotnet/reactive](https://github.com/dotnet/reactive). They are licensed under the MIT license.
- [`Telegram.Bot`](https://github.com/TelegramBots/Telegram.Bot) and [`Telegram.Bot.Extensions.Polling`](https://github.com/TelegramBots/Telegram.Bot.Extensions.Polling) are licensed under the MIT license.

© 2024 database64128
