# VS Evolution Helper user guide

Installation and runtime selection are covered in the [README](../README.md).

## Controls

### Mouse

- Hover a supported icon or row to open its tooltip.
- Hover an icon inside a tooltip to open nested details.
- Click an interactive formula or arcana icon to keep navigating.
- Move outside the tooltip stack to close it.

### Keyboard and controller

| Action | Keyboard | Controller |
| --- | --- | --- |
| Move selection | Arrow keys or WASD | Stick or D-pad |
| Enter tooltip navigation | Tab | Y |
| Open selected nested tooltip | Space or Enter | A |
| Go back or close | Backspace | B |
| Switch Music and Guide tabs | Q and E | LB and RB |

Tooltips open after the configured controller dwell delay. Moving the mouse returns control to pointer input.

Pause-map icons require a mouse because they are not selectable game controls.

## In-run tooltips

### Weapons and passives

Equipment and level-up tooltips can show:

- localized name and description
- every matching evolution or union recipe
- required passives and maximum-level requirements
- resulting weapon
- related arcanas

Formula icons open nested weapon, item, or arcana details.

### Level up

The tooltip opens only after hovering or dwelling on a choice. It closes when the choice is no longer targeted and does not cover other choices automatically.

### Pause map

Hover a registered relic, pickup, or merchant icon for its name, description, or inventory.

### Weapon selectors

Weapon-selection screens, including Arma Dio and Penshin Fatcha, show the same weapon details. The original selection controls remain active.

### Arcana cards

Arcana cards show their description and affected weapons or passives. Face-down cards remain hidden. Long affected-item lists continue in a second panel.

Hover affected-weapon icons in the arcana information panel to view their evolutions and unions.

## Menu tooltips

### Collection and Grimoire

Weapon, passive, arcana, and evolution cells show localized details. Locked collection cells show an unlock hint when the game provides one.

### Characters

Character cards can show:

- localized name and description
- portrait
- starting weapon
- evolution path
- other outfit starting weapons
- notable stat changes

### Adventures and Ascension

Adventure cards summarize their cast and weapons. Ascension controls show the bonus, assigned points, per-point value, and available points.

### Secrets and achievements

Secret and achievement rows show their rewards. Disable `SecretSpoilers` to hide rewards for undiscovered secrets.

### Bestiary

Enemy rows can show health, damage, speed, experience, knockback, resistances, skills, and stage locations. Disable `BestiarySpoilers` to limit details to encountered enemies.

### Power Up

Power-up rows show the owned level, next price, and remaining cost when the game data supports a reliable projection. The panel refreshes after purchases and refunds.

### Music

Track rows show composer, source, and unlock condition. Disable `MusicSpoilers` to preserve the game’s mask for locked tracks.

## Stage selection

Hover the relic list for item details. The right-side panel adds Music and Guide tabs.

Guide content may include:

- localized stage name
- curated stage notes
- game-provided tips
- stage-specific restrictions and events
- coffin, arcana, treasure, boss, and merchant information
- interactive relic rows

The Guide scrolls when its content exceeds the panel height. Leaving stage selection restores the Music panel.

## Configuration

Edit `BepInEx/config/com.nihil.vsevolutionhelper.cfg` while the game is closed.

### Debug

| Key | Default | Effect |
| --- | ---: | --- |
| `VerboseLogging` | `false` | Write detailed registration, lookup, and hover diagnostics. |

### Tooltips

| Key | Default | Effect |
| --- | ---: | --- |
| `HoverDelay` | `0.4` | Delay for collection, map, relic, character, and adventure tooltips. |
| `LevelUpHoverDelay` | `0.15` | Delay for level-up choices. |
| `ControllerDwellDelay` | `0.5` | Delay before a selected control opens its tooltip. |

### Features

| Key | Default | Effect |
| --- | ---: | --- |
| `MapTooltips` | `true` | Pause-map tooltips. |
| `StageGuide` | `true` | Music and Guide tabs on stage selection. |
| `StageGuideDefaultToGuide` | `false` | Open stage selection on the Guide tab. |
| `LevelUpTooltips` | `true` | Level-up choice tooltips. |
| `CharacterTooltips` | `true` | Character-selection tooltips. |
| `AdventureTooltips` | `true` | Adventure and Ascension tooltips. |
| `WeaponSelectionTooltips` | `true` | Weapon-selector tooltips. |
| `SecretTooltips` | `true` | Secret reward tooltips. |
| `SecretSpoilers` | `true` | Reveal undiscovered secret rewards. |
| `BestiaryTooltips` | `true` | Bestiary tooltips. |
| `BestiarySpoilers` | `true` | Reveal unencountered enemy details. |
| `AchievementTooltips` | `true` | Achievement reward tooltips. |
| `PowerUpTooltips` | `true` | Power-up cost tooltips. |
| `ArcanaCardTooltips` | `true` | Arcana card and affected-weapon tooltips. |
| `MusicTooltips` | `true` | Music credit and unlock tooltips. |
| `MusicSpoilers` | `true` | Reveal locked track names. |

Restart the game after changing configuration.

## Compatibility

- Vampire Survivors `1.16.x`
- Unity `6000.0.62f1`
- BepInEx 6 IL2CPP on Windows and Proton
- BepInEx 6 Mono on native Linux

Other mods that replace the same menu pages may conflict. Online and co-op behavior has not been fully tested.

## Troubleshooting

| Problem | Check |
| --- | --- |
| Plugin does not load | Confirm the platform table and plugin path in the README. |
| DLL does not update | Close the game before replacing it. |
| Raw localization key appears | Record the screen, item, language, and plugin version. |
| Guide tabs are absent | Enable `StageGuide` and select a stage. |
| Character tooltip is absent | Enable `CharacterTooltips` and target a grid card. |
| Repeated exceptions appear | Enable `VerboseLogging`, reproduce once, and attach `BepInEx/LogOutput.log`. |

Use [SMOKE-TEST.md](SMOKE-TEST.md) for release validation.
