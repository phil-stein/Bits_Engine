# Unnamed Game Project

*(I know it currently is named Be_Safe, but the name is still TBD)*

**All Art is still Placeholder**

Game-Project I am working on to test out the engines capabilities.



## Gameplay

top-down / birds-eyes-view perspective, tile-based, puzzle game about pushing boxes around. 


## Todo
	
### Game
- [ ] make/find texture for hero_defense_char (color palette texture used in "Hero Defense")

- [x] make a .md file for the be_safe page
	
- [x] make a map and player tiles based on that not just a square
- [x] make the player be abled to walk the new map
- [x] check the player leaving out the left/right side of the map
- [ ] make it be abled to move between sections
- [x] make the EnvGen. be abled to have multiple strings/imgs as layers for the map, i.e. grass -> wall -> vines
- [ ] make dif. models for each side of a water-facing tile 

- [ ] make the cliff-sides as ground-types
- [ ] make the maploader / envcontroller surround the map with cliff / water
- [ ] make rough models for the walls / etc.
- [ ] make a chance for an empty grass tile to have pebbles spawn on it


- [x] define the structure for tile-layout i.e. GroundType -> Structure -> Deco
- [x] load maps from file [made loader, still have to make the map-creation in the EnvController]
		-> @UNSURE: make a file-loading-util class in bits core ??? 

- [x] make pushables
- [x] @REFACTOR: make a base-class for player and pushables / all other dynamic map-entities
- [x] make the tile-check based on the type of object, i.e. so that pushable objects can be pushed into the water
- [ ] @UNSURE: move pushable logic from EnvController to TileObject
		-> make a interaction function in the TileObject base-class and call that each from EnvGen to have the reactions in the objects classes
- [ ] make a button / pressure-plate you can push objects onto 
- [ ] make the button / pressure-plate open a door

- [x] add seperate text for current-tile [x] / player-pos[x] / draw-calls [x]

### GameDesign
- [ ] find a name
- [ ] define basic visual theme
- [ ] define art style
- [ ] make basic design concept
- [ ] design level using the push mechanics

### Engine
- [ ] make the textured materials have tint-colors for the albedo and specular maps
- [ ] make the blank textures, primitives and shaders part of the BitsCore projects asset folder and copy them
- [ ] add a tween-utils class that can move/rotate/scale objects smoothly over time
- [ ] add structures/constructs/groups/etc. just a couple of GO's grouped together (prefabs)
- [ ] add simple particle-system
- [ ] add simple batch-rendering


## Buggs

- [x] floor is lit differently than the other objects
- [x] movement "wraps" when moving out of bound to the left or right
- [ ] rotate char in direction of key-press even if that move if blocked
- [ ] AssetManager error message when wrong asset-name
- [ ] moving into pushable object on the left & right
