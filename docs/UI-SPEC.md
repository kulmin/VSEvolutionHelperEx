# Tooltip UI specification

This document defines the behavior shared by the IL2CPP and Mono builds.

## Runtime boundary

The two builds share feature code and differ only at runtime boundaries:

- plugin lifecycle and BepInEx base class
- managed collection and nullable types
- Unity event delegates
- access to non-public game fields
- JSON implementation

Runtime-specific code belongs in the adapter files or compile-time aliases. Feature patches must not select a runtime at execution time.

## Lifecycle

Initialization must:

1. bind configuration
2. create one Harmony instance
3. apply enabled patches
4. initialize the tooltip state

`ItemTooltipsMod.Update` owns delayed actions, scene transitions, hover state, controller navigation, and popup cleanup. A scene change clears references to destroyed Unity objects.

## Registration

A tooltip entry contains:

- a stable owner object
- an optional hit target
- title, description, sprite, and rows
- popup placement
- optional providers for data that can change after registration
- optional click forwarding

Registration is idempotent by Unity instance ID. Rebinding a recycled row replaces its previous entry.

Use the smallest reliable hit target. Do not attach a new `EventTrigger` to a selectable root when doing so would replace the game’s click handling. Register visual children for pointer input and map the root separately for keyboard or controller navigation.

## Pointer and selection input

Mouse input uses Unity raycasts. Keyboard and controller input follows the active selected object and walks its ancestors until it finds a registered entry.

Pointer exit may schedule a delayed close so movement from a source icon into its popup does not collapse the popup stack. Each delayed close must confirm that it still owns the active popup before changing state.

When a tooltip overlays a clickable game control, forward the click once to the original control. Never invoke both the child and ancestor handler manually.

## Canvases

Prefer an active game-owned safe-area canvas when it can draw the requested popup. Otherwise use the plugin overlay canvas.

The overlay canvas must:

- use screen-space overlay mode
- inherit the active UI layer
- sort one step above the highest active game canvas
- omit a `GraphicRaycaster` unless the popup contains interactive elements
- survive scene transitions without retaining destroyed page objects

Nested canvases that contain interactive tooltip targets need their own `GraphicRaycaster`. Full-screen faders above those targets must not block raycasts when inactive.

## Placement

Popup coordinates are relative to the selected canvas. Every placement specifies both an anchored position and a pivot.

Docked panels use fixed edges so content growth moves away from the game controls. Long arcana lists may continue in a second panel. Other lists remain in one panel and report omitted rows explicitly.

Do not cache a canvas transform before the layout pass that activates it. Recheck scale and active state when creating the popup.

## Content

Prefer text already rendered by the game. Use localized game data when rendered text is unavailable. Reject localization keys, placeholder masks, and sentinel values from player-facing output.

Locked or undiscovered content follows its feature configuration:

- secrets: `SecretSpoilers`
- Bestiary: `BestiarySpoilers`
- music: `MusicSpoilers`

An empty title, description, and row set must not create a panel. A title-only entry is allowed only when the feature explicitly supplies that state.

## Sprites

Sprite lookup order is:

1. sprite already rendered by the row
2. typed game data
3. indexed loaded sprites
4. requested DLC atlas

Atlas requests are asynchronous. Cache successful lookups. Cache a miss only until the sprite generation changes, then allow another lookup.

## Patching

Patch concrete methods declared by the target type. Avoid inherited base methods unless every derived page is an intended target.

When a UI type exposes several bind overloads, patch each compatible overload. Postfixes should read the bound instance after the game has updated it. Use reflection for non-public fields shared by the Mono and IL2CPP representations.

Do not patch methods whose argument marshalling is unstable under IL2CPP. Register those views from a bounded scene scan after the page populates.

Patch failures must name the feature and method. One optional feature failure must not prevent unrelated patches from loading.

## Data refresh

Use deferred providers when data changes while a page stays open, including:

- power-up prices after a purchase or refund
- asynchronously loaded sprites
- arcana descriptions populated by the info panel
- page rows recycled for new records

Reset per-scene caches when their owning game objects are destroyed. Keep catalog data cached while its `DataManager` remains valid.

## Verification

Both runtime projects must compile before release. The native Linux smoke test must show:

```text
Loading [VS Evolution Helper 1.15.0]
VS Evolution Helper initialized.
Chainloader startup complete
```

Run the interaction checks in [SMOKE-TEST.md](SMOKE-TEST.md) for a release candidate.
