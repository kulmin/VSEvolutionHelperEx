# VS Evolution Helper

Tooltips for evolutions, arcanas, weapons, items, stages, characters, adventures, secrets, achievements, enemies, power-ups, and music in Vampire Survivors.

Version `1.15.0` targets Vampire Survivors `1.16.x` and Unity `6000.0.62f1`.

## Choose the correct build

| Game build | BepInEx package | Plugin artifact |
| --- | --- | --- |
| Windows | `Unity.IL2CPP-win-x64` | `VSEvolutionHelper-Unity.IL2CPP-win-x64` |
| Windows through Proton | `Unity.IL2CPP-win-x64` | `VSEvolutionHelper-Unity.IL2CPP-win-x64` |
| Native Linux | `Unity.Mono-linux-x64` | `VSEvolutionHelper-Unity.Mono-linux-x64` |

The host operating system does not determine the runtime. Proton runs the Windows IL2CPP build; the native Linux depot uses Mono. Do not mix the two BepInEx packages or plugin DLLs.

The current builds use [BepInEx `6.0.0-be.788`](https://builds.bepinex.dev/projects/bepinex_be).

## Install

### Windows

1. Extract `BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.788+5b766a3.zip` into the Vampire Survivors directory.
2. Start the game once and close it after BepInEx creates `BepInEx/LogOutput.log`.
3. Extract the Windows IL2CPP plugin artifact into the game directory.

The plugin should end up at:

```text
BepInEx/plugins/VSEvolutionHelper/VSEvolutionHelper.dll
```

### Proton

Use the Windows instructions and set this Steam launch option to enable the `winhttp` DLL override:

```text
WINEDLLOVERRIDES="winhttp=n,b" %command%
```

### Native Linux

1. Extract `BepInEx-Unity.Mono-linux-x64-6.0.0-be.788+5b766a3.zip` into the Vampire Survivors directory.
2. Make the loader executable:

   ```sh
   chmod u+x run_bepinex.sh
   ```

3. Extract the native Linux Mono plugin artifact into the game directory.
4. Set the Steam launch option:

   ```text
   ./run_bepinex.sh %command%
   ```

### Verify

Open `BepInEx/LogOutput.log` and check for:

```text
Loading [VS Evolution Helper 1.15.0]
VS Evolution Helper initialized.
Chainloader startup complete
```

## Features

- Evolution and union recipes for weapons and passives
- Related arcanas and affected weapons
- Level-up, equipment, merchant, and weapon-selector tooltips
- Collection, Grimoire, and pause-map tooltips
- Character and adventure summaries
- Stage relic tooltips and the Stage Guide panel
- Secret and achievement rewards
- Bestiary stats, resistances, skills, and stage locations
- Power-up prices and remaining upgrade cost
- Music credits and unlock conditions
- Mouse, keyboard, and controller navigation

Spoiler controls are available for secrets, the Bestiary, and music.

## Configuration

BepInEx creates the configuration file after the first successful load:

```text
BepInEx/config/com.nihil.vsevolutionhelper.cfg
```

See [docs/USER-GUIDE.md](docs/USER-GUIDE.md) for the complete option and control reference.

## Build

Install a .NET SDK and extract both BepInEx packages outside the repository.

Build Windows IL2CPP:

```sh
dotnet build VSEvolutionHelper.BepInEx/VSEvolutionHelper.BepInEx.csproj \
  --configuration Release \
  --property:BepInExPath=/path/to/extracted/BepInEx
```

Build native Linux Mono:

```sh
dotnet build VSEvolutionHelper.BepInEx/VSEvolutionHelper.BepInEx.Mono.csproj \
  --configuration Release \
  --property:BepInExPath=/path/to/extracted/BepInEx
```

Outputs:

```text
VSEvolutionHelper.BepInEx/bin/Release/il2cpp/VSEvolutionHelper.dll
VSEvolutionHelper.BepInEx/bin/Release/mono/VSEvolutionHelper.dll
```

Pushes to `main` and pull requests build both variants and upload separate artifacts.

## Updating build references

After a Vampire Survivors update, launch both game builds once so the Windows BepInEx installation refreshes its `interop` directory. Then run:

```sh
./tools/update-references.sh \
  "/path/to/native-linux/Vampire Survivors" \
  "/path/to/windows/Vampire Survivors/BepInEx/interop"
```

Rebuild both projects before committing the updated files under `VSEvolutionHelper.BepInEx/lib/`.

## Troubleshooting

| Symptom | Check |
| --- | --- |
| No `BepInEx/LogOutput.log` | The matching BepInEx loader is not starting. Recheck the selected game build and installation directory. |
| Log reports no plugins | Confirm the plugin path and artifact variant. |
| Native Linux reports `BepInEx.Unity.IL2CPP` errors | Replace the IL2CPP package and DLL with the Mono variants. |
| Proton starts without BepInEx | Confirm the `winhttp` override and use the Windows IL2CPP package. |

## Documentation

- [User guide](docs/USER-GUIDE.md)
- [UI specification](docs/UI-SPEC.md)
- [Smoke-test checklist](docs/SMOKE-TEST.md)
- [Changelog](CHANGELOG.md)

## Credits

- [NihilXD](https://github.com/NihilXD/VSEvolutionHelper), original VS Evolution Helper and VS Item Tooltips author
- [ashimpure](https://www.nexusmods.com/vampiresurvivors/users/80031423), unofficial 1.14 update
- [n3rdyguy](https://github.com/n3rdyguy/VSEvolutionHelperEx), BepInEx IL2CPP port and current feature work

Keep these credits when redistributing the plugin.
