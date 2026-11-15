# FarinasIago_RA3

## Descripció del joc

Joc de **plataformes 2D** amb **vista lateral**. El jugador avança per nivells cap a la sortida i, en lloc de fer un salt tradicional, **inverteix la gravetat**: en prémer el botó de salt, el personatge cau cap amunt (o cap avall) i pot caminar pel sostre com si fos el terra.

- **Gènere:** plataformes 2D / puzle de gravetat.
- **Visualització:** 2D, vista lateral amb càmera que segueix el jugador i fons amb efecte *parallax*.
- **Objectiu:** arribar al final de cada nivell (4 nivells + pantalla de comiat) sense morir.
- **Elements del joc:**
  - Obstacles mortals (el jugador mor en tocar-los i reapareix).
  - Punts de control (*checkpoints*) on es reapareix després de morir.
  - Enemics **Walker** (patrullen la plataforma) i **Torretes** que disparen projectils.
  - Zones que canvien l'estat del nivell al ser creuades.

## Instruccions de joc (controls)

| Acció | Teclat | Comandament |
|---|---|---|
| Moure's a l'esquerra / dreta | `A` / `D` o fletxes `←` / `→` | Joystick esquerre o creueta (D-Pad) |
| Invertir la gravetat (saltar) | `Espai`, `W` o fletxa `↑` | Botó A / Sud (`buttonSouth`) |
| Pausa | `Esc` | Botó Select |

**Notes:**
- Només es pot invertir la gravetat quan el personatge **toca una superfície** (terra o sostre).
- Durant la pausa, en morir o en completar el nivell, els controls queden desactivats.

## Com jugar

1. Des del menú principal, inicia la partida.
2. Mou-te amb `A`/`D` i inverteix la gravetat amb `Espai` per esquivar obstacles i enemics.
3. Activa els punts de control pel camí: si mors, reapareixeràs al darrer.
4. Arriba a la sortida del nivell per passar al següent.

## Estructura del projecte

- `Assets/Scenes/` — Menú principal, nivells 01–04 i pantalla final.
- `Assets/_Project/Scripts/` — Codi del jugador, enemics, nivell i interfície.
- `Assets/_Project/` — Sprites, animacions, sons, tiles i prefabs.
