# Multiplayer The Grand Library of 「Mirrored」 Patch

A RimWorld Multiplayer compatibility patch for [The Grand Library of 「Mirrored」](https://steamcommunity.com/sharedfiles/filedetails/?id=3782667656).

This mod is designed to improve multiplayer synchronization when playing with the [The Grand Library of 「Mirrored」](https://steamcommunity.com/sharedfiles/filedetails/?id=3782667656) mod and RimWorld Multiplayer.

## Features

- Adds multiplayer compatibility for [The Grand Library of 「Mirrored」](https://steamcommunity.com/sharedfiles/filedetails/?id=3782667656).
- Syncs shared story progress (scenario completion, unlocks, incident/quest/marker reports, branch activation) across all clients.
- Relays dialogue option selections so external storyline effects run on every client.
- Keeps wealth-based story triggers deterministic by using the same home map wealth for all clients.
- Isolates chat typing-indicator randomness from shared RNG.
- Closes eliminated branch windows and resets the scenario playback queue consistently on every client.

## Requirements

- RimWorld
- [Harmony](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077)
- RimWorld Multiplayer
  - [GitHub version](https://github.com/rwmt/Multiplayer) or [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=2606448745) version
- [The Grand Library of 「Mirrored」](https://steamcommunity.com/sharedfiles/filedetails/?id=3782667656)

The host and every connected player must use compatible versions of all required mods.

## Installation

### Steam Workshop

Subscribe to the required mods and add them to your RimWorld mod list in the following order:

1. Harmony
2. Core
3. Royalty, Ideology, Biotech, and Anomaly, if applicable
4. RimWorld Multiplayer
5. [The Grand Library of 「Mirrored」](https://steamcommunity.com/sharedfiles/filedetails/?id=3782667656)
6. [Multiplayer The Grand Library of 「Mirrored」 Patch](https://github.com/Keullaeseu/Multiplayer-The-Grand-Library-of-Mirrored-Patch/releases/latest)

The patch should load after both RimWorld Multiplayer and [The Grand Library of 「Mirrored」](https://steamcommunity.com/sharedfiles/filedetails/?id=3782667656).

### Manual Installation

1. Download the latest release from the [**Releases**](https://github.com/Keullaeseu/Multiplayer-The-Grand-Library-of-Mirrored-Patch/releases/latest) section.
2. Extract the mod folder into your RimWorld `Mods` directory.
3. Enable the required mods in RimWorld.
4. Use the recommended load order listed above.
5. Make sure every multiplayer player has the same mod list, configuration, and load order.

## Multiplayer Usage

All players should have the following mods installed and enabled:

- RimWorld Multiplayer
- [The Grand Library of 「Mirrored」](https://steamcommunity.com/sharedfiles/filedetails/?id=3782667656)
- [Multiplayer The Grand Library of 「Mirrored」 Patch](https://github.com/Keullaeseu/Multiplayer-The-Grand-Library-of-Mirrored-Patch/releases/latest)
- All required The Grand Library of 「Mirrored」 dependencies

The host and all connected clients should use the same:

- RimWorld version
- RimWorld Multiplayer version
- The Grand Library of 「Mirrored」 version
- Multiplayer The Grand Library of 「Mirrored」 Patch version
- Mod configuration
- Mod load order

Do not add, remove, update, or reorder mods while players are connected to the same multiplayer session.

## Compatibility

This patch is intended to provide multiplayer compatibility for [The Grand Library of 「Mirrored」](https://steamcommunity.com/sharedfiles/filedetails/?id=3782667656).

It does not replace:

- [RimWorld Multiplayer](https://steamcommunity.com/sharedfiles/filedetails/?id=2606448745)
- [The Grand Library of 「Mirrored」](https://steamcommunity.com/sharedfiles/filedetails/?id=3782667656)

## Known Limitations

- Chat window playback, typing indicators, and chat history remain per-client and may differ between players; only trigger-relevant story progress is synced.
- Compatibility may be affected by future RimWorld updates.
- Compatibility may be affected by future updates to RimWorld Multiplayer or The Grand Library of 「Mirrored」.

## Credits

- [RimWorld Multiplayer on GitHub](https://github.com/rwmt/Multiplayer)
- [RimWorld Multiplayer on Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=2606448745)
- [The Grand Library of 「Mirrored」 on Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3782667656)
- [Multiplayer The Grand Library of 「Mirrored」 Patch](https://github.com/Keullaeseu)
