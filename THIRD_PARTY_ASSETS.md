# Third-Party Assets & Credits

Inventory checked against the local project on **2026-10-12**. Thank you to the creators of the art, animation, audio, UI, fonts, and effects used while developing Conquer The Stars.

This is a package/source-group inventory, not a list of every texture or prefab. It covers all immediate asset-pack folders in `Assets/Third_party Assets/`, package-origin metadata elsewhere in `Assets/`, TextMesh Pro content, and declared Unity packages. **21 distinct Asset Store product IDs** were recovered from local `.meta` files. Other folders do not retain enough information to identify their exact original package or license.

## Reading the inventory

- Paths in the tables are relative to `Assets/Third_party Assets/`, unless explicitly prefixed with `Assets/`.
- **R** = a static GUID dependency was found from the enabled Start, Main, Battle, End, or Loading scene, or from content under a `Resources` folder. This can include indirect dependencies and demo/editor content; it is not proof that the asset appears during gameplay or ships in the APK.
- **—** = no dependency was found by that scan. This does not prove the asset is unused: copied clips, binary assets, shader graphs, and other runtime loading paths require separate checks.
- Versions and product IDs below come from local import metadata, not the latest online release. Package-origin metadata can survive copying; it does not prove authorship of every bundled file.
- Linked package/creator pages provide source references. For entries without a verified product link, use the exact name and product ID in Unity Asset Store/My Assets. Do not substitute a similarly named pack or an unofficial mirror.

## Environment, characters, weapons & animation

| Local folder / asset | Original package or source | Creator / identification status | Local version / product ID | Ref. |
|---|---|---|---|---|
| `Map/SciFi Warehouse Kit` | [Sci-Fi Construction Kit (Modular)](https://assetstore.unity.com/packages/3d/environments/sci-fi/sci-fi-construction-kit-modular-159280) | Sickhead Games | 1.1.0 / 159280 | R |
| `Models/Biomechanical_Mutant` | Free.Biomechanical Mutant | Package identified; publisher still to confirm | 1.0 / 166330 | R |
| `Models/Sci_fi_Drones` | [Sci fi Drones](https://assetstore.unity.com/packages/3d/characters/robots/sci-fi-drones-90326) | Lukas Bobor | 1.0 / 90326 | R |
| `Models/Sci_Fi_Character_08` | [Sci-Fi Character 08 (Renegade)](https://assetstore.unity.com/packages/3d/characters/humanoids/sci-fi/sci-fi-character-08-renegade-208640) | Maksim Bugrimov | 1.0 / 208640 | — |
| `Models/Sci_Fi_Character_03` | Candidate: [Sci-Fi Character 03 (Yakuza)](https://assetstore.unity.com/packages/3d/characters/humanoids/sci-fi/sci-fi-character-03-yakuza-194608) | Maksim Bugrimov's listing matches the name; confirm original download before treating this as an exact match | Not retained | R |
| `Models/Blink` | Stylized swords, including `Sword4_3` | Folder suggests Blink; exact pack and license unconfirmed | Not retained | R |
| `Models/Marauder` | Marauder model and textures | Original creator, download URL, and license needed | Not retained | R |
| `Models/Weapons` | Sci-fi weapon assets, including a rocket launcher | Original creator, download URL, and license needed | Not retained | R |
| `Animation/Combat animations - Kung fu V1` | Combat animations - Kung fu V1 | Original creator, download URL, and license needed | Not retained | — |
| `Animation/Unity` | Imported animation content | Folder name alone does not establish Unity authorship | Not retained | — |
| `Animation/Unity_2` | Imported animation content | Folder name alone does not establish Unity authorship | Not retained | — |
| `Assets/Animation/Boss/Boss_II/Inplace/Sword2Blades_Transform_01_Inplace.FBX` | [TransformAnimation SXB](https://assetstore.unity.com/packages/3d/animations/transformanimation-sxb-159419) | WM_Animset; metadata found outside the ignored asset folder | 2 / 159419 | Not separately classified |

`Assets/Animation/Enemy/` and `Assets/Animation/Player/` also contain FBX files and extracted `.anim` clips. Their source relationships need confirmation before assigning author credits or redistribution permissions. Do not infer a Mixamo origin from an animation name alone. Animator controllers and integration work are distinct from ownership of the underlying motion data.

## UI, icons & fonts

| Local folder | Original package or source | Creator / identification status | Local version / product ID | Ref. |
|---|---|---|---|---|
| `UI/Dynamic Floating Text` | [Dynamic Floating Text](https://assetstore.unity.com/packages/tools/particles-effects/dynamic-floating-text-227063) | Austin Windsor | 1.0.2 / 227063 | R |
| `UI/FlexUnit/SpaceGameIconSet` | [Space Game Icon Set](https://flexunit.itch.io/space-game-icon-set) | FlexUnit | 1.0.0 / 290437 | — |
| `UI/Free RPG Icons Pack` | Free RPG Icons Pack | Package identified; publisher still to confirm | 2.0.0 / 267155 | R |
| `UI/Free_FantasySkillIcon` | Free Fantasy RPG Skill Icon And Portrait | Package identified; publisher still to confirm | 1.0 / 377694 | — |
| `UI/Modern GDR - Free icons pack` | Modern GDR - Free icons pack | Bundled README signed Jennifer; publisher profile still to confirm | 1.0 / 264706 | — |
| `UI/Sci-Fi UI` | [Sci-fi GUI skin](https://marketplace.unity.com/packages/2d/gui/sci-fi-gui-skin-15606) | 3d.rina | 1.2 / 15606 | R |
| `UI/Sci-fi Hud Gui` | Sci-fi UI HUD - Custom GUI HUD | Title from bundled documentation; creator and original download needed | Not retained | R |
| `UI/Strategic Warfare UI` | Strategic Warfare Sci-fi UI Starter Pack | Package identified; publisher still to confirm from original listing/receipt | 1.0 / 391120 | R |
| `UI/Wenrexa Interface UI KIT #4` | [Assets Free Interface UI KIT #4](https://wenrexa.itch.io/ui-different02) | Wenrexa | Not retained | R |
| `UI/DaFont/Cynatar` | [Cynatar](https://www.dafont.com/cynatar.font) | Leonard Posavec / LeoSupply.co | See license action below | R |
| `UI/DaFont/Batman Forever` | Batman Forever solid and outline fonts (`batmfa__`, `batmfo__`) | Font files and TMP derivatives present; original author/license still to confirm. DaFont is a hosting site, not the creator | Not retained | R at folder level |
| `UI/ChatGPT` | UI images, icons, logo, panels, and loading assets | Tool-named folder; generation history, source images, edits, and rights need owner confirmation | Not applicable | R |
| `UI/Gemini` | UI/VFX images, including handles and targeting elements | Tool-named folder; generation history, source images, edits, and rights need owner confirmation | Not applicable | R |

`Assets/Scenes/FunctionalUI-Demo.unity` also retains product ID **391120**. An imported demo scene outside the ignored folder is not automatically project-original content.

The ChatGPT/Gemini folder names are not evidence that every image was generated from scratch or that source-image rights have been cleared. No original-human-art claim is made for these assets.

## Audio

| Local folder | Original package or source | Creator / identification status | Local version / product ID | Ref. |
|---|---|---|---|---|
| `Sound/Deadly Kombat Free version` | [Free Deadly Kombat](https://assetstore.unity.com/packages/audio/sound-fx/free-deadly-kombat-228835) | Daniel SoundsGood | 1.0 / 228835 | R |
| `Sound/Free Sci-Fi Music Collection` | Free Sci-Fi Music Collection | [Phat Phrog Studio](https://www.phatphrogstudio.com/); creator named in local track filenames | 1.0 / 287462 | R |
| `Sound/TII_SoundLibrary_3Steps/SCI-FI` | TII 3 Steps Audio(SCI FI Package) | TII; retain the bundled usage guidelines and logo | 1.0 / 384690 | R |
| `Sound/ULTRA FORGE STUDIO/Low-Fi Sci-Fi Starter Pack` | Free Ambient & Lo-Fi Sci-Fi Music Starter Pack + Music Players | ULTRA FORGE STUDIO; credit specified in bundled README | 1.0 / 355874 | R |
| `Sound/Bandlad mix` | Edited/mixed audio: `Sword Parry.mp3`, `MartialArtParry.mp3`, `Gun.mp3`, `Dodge.mp3` | Original recordings, samples, creator, and applicable license needed; folder name does not establish a BandLab license | Not retained | R |
| `Sound/Pixapay` | Sword-slash and hit sound effects | Original clip URLs and creators needed; do not assume Pixabay provenance or terms from this folder name | Not retained | R |

Static references include **Ironblood Frontier - LOOP** from Phat Phrog Studio's Mecha Conflict tracks, **Shadow Circuit_Full** from ULTRA FORGE STUDIO, and several **TII_SCIWeap_SCIFIArrowShot** clips. These names help identify the actual source audio when checking credits and licenses.

## Visual effects

| Local folder | Original package or source | Creator / identification status | Local version / product ID | Ref. |
|---|---|---|---|---|
| `VFX/Hovl Studio/Magic effects pack` | [Magic Effects FREE](https://marketplace.unity.com/packages/vfx/particles/spells/magic-effects-free-247933) | Hovl Studio | 1.6 / 247933 | R |
| `VFX/Matthew Guz/Hits Effects FREE` | [Hit Effects FREE](https://assetstore-fallback.unity.com/packages/vfx/particles/hit-effects-free-284613) | Matthew Guz | 1.2.0 / 284613 | — |
| `VFX/Mirza/Distortion Shockwaves VFX` | [Distortion Shockwaves VFX](https://mirzabeig.notion.site/Distortion-Shockwave-VFX-395163b830404359b5653c5eb1a49a83) | Mirza Beig | 1.1.0 / 273644 | R |
| `VFX/Vefects/Easy Shockwaves VFX URP` | Easy Shockwaves VFX - URP | [Vefects](https://vefects.com/); Ge Lush named in bundled documentation | 1.0.2 / 306143 | R |
| `VFX/Vefects/Wind VFX URP` | Wind VFX - URP | [Vefects](https://vefects.com/); Ge Lush named in bundled documentation | 1.0 / 398182 | R |

## TextMesh Pro, bundled samples & Unity packages

TextMesh Pro content is kept separately under `Assets/TextMesh Pro/`. Some files retain Dynamic Floating Text import metadata; that does **not** make Austin Windsor the author of the bundled fonts or emoji.

| Included content | Attribution / local notice |
|---|---|
| Liberation Sans | Google / Red Hat copyright notices; SIL OFL 1.1 in `Assets/TextMesh Pro/Fonts/LiberationSans - OFL.txt` |
| EmojiOne sprite sample | EmojiOne; `Assets/TextMesh Pro/Sprites/EmojiOne Attribution.txt` points to the provider's terms; confirm the license for the bundled version before distributing it |
| Oswald, Bangers, Anton example fonts | Vernon Adams; corresponding OFL notices in `Assets/TextMesh Pro/Examples & Extras/Fonts/` |
| Unity example font | Unity; `Unity - OFL.txt` in the same folder |
| Roboto Bold example font | `Roboto-Bold - License.txt` specifies Apache License 2.0; retain accompanying notices in the same folder |
| TextMesh Pro shaders, scripts, resources, and examples | Unity/TextMesh Pro distribution; retain applicable package notices |

The example assets are inventoried because they exist locally, not because all are confirmed to appear in the game.

Unity-provided dependencies are recorded in [manifest.json](Packages/manifest.json) and [packages-lock.json](Packages/packages-lock.json). These include URP, Input System, Cinemachine, Unity UI/TextMesh Pro, AI Navigation, Timeline, 2D Sprite, Test Framework, Memory Profiler, Device Simulator device definitions, editor integrations, and Unity engine modules. Presence in the manifest is not a claim that every package is used by gameplay; consult each installed package's license and third-party notices.

## License and provenance actions still needed

Credits are acknowledgements, **not** a license grant or a certification that all assets are cleared for redistribution. Check each asset's acquisition terms and bundled notices; do not apply the [Unity Asset Store EULA](https://unity.com/legal/as-terms) automatically to fonts, outside downloads, or assets with separate terms.

1. **Cynatar:** the [author's download page](https://www.dafont.com/cynatar.font) labels the free font for personal use only and requests a license for commercial use. Confirm permission for the intended game/demo distribution, obtain an appropriate license, or replace it. This inventory does not classify a public portfolio or free download as automatically permitted. TMP font conversion does not resolve that question.
2. **TII audio:** the locally bundled `Sound/TII_SoundLibrary_3Steps/SCI-FI/README.md` requires **TII_Logo in the final product's ending credits**. A text mention here is not a substitute. It also disallows redistributing/repackaging/reselling the raw audio and AI training. Verify the game's credits and the applicable package terms before the next release; this documentation update does not add the logo in-game.
3. **Unidentified sources:** recover download receipts/URLs for the entries marked unconfirmed, particularly Marauder, weapons, animation packs/extracted clips, Sci-fi HUD, Batman Forever fonts, and the two edited/downloaded audio folders. Verify source-image provenance for tool-named image folders. A missing notice is not evidence of unrestricted use.
4. **Other observed notices:** ULTRA FORGE STUDIO's bundled README permits game/trailer use and makes attribution optional; this project credits it regardless. Wenrexa and FlexUnit's linked itch.io pages currently identify their downloads as CC0; confirm the version/source actually acquired rather than assuming all editions or stores share identical terms.
5. **Repository boundaries:** ignoring `Assets/Third_party Assets/` does not remove older commits or cover copied material elsewhere. Review the animation files, imported demo scene, and font/sample notices outside that directory before treating the public repository as cleared. No Git history cleanup or asset removal was performed as part of this inventory.

## Restoring the project after cloning

1. Resolve Unity packages using the checked-in manifests and the Editor version listed in the README.
2. Obtain the identified asset packs from their original creators or Unity Asset Store, under licenses that cover your use. The tables record local imported versions; newer releases can change contents and GUIDs.
3. Restore assets with their original `.meta` files/GUIDs and the folder layout expected by this project. Merely creating folders with matching names does not restore serialized references. Modified or extracted assets may require the project owner's mapping or manual reassignment; do not overwrite project files blindly.
4. Resolve the unidentified sources with the project owner or substitute properly licensed assets and reassign all affected references. The inventory is not yet a one-click reproducible asset manifest.
5. Check scenes, prefabs, ScriptableObjects, materials, fonts, animation controllers, and audio references in Unity. Then run Play Mode and an Android build; no clean-clone build or runtime validation was performed for this documentation update.

Keep required copyright/license notices with redistributed material. Do not upload excluded source packs just to make a clone run unless their licenses permit that distribution.
