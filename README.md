
# Godot Basic Dynamic Pool

I wrote this Dynamic Objects Pooling C# code to make it easier for other programmers to pool their objects and nodes.

The code is not optomized yet, but hopefully I'll make it as optimized as I can by next update.
## Demo

Here's a demo of how objects are pooling:

![](https://github.com/q8geek/Godot-4-Basic-Dynamic-Pool/blob/main/demo.gif)
## Features

- A dictionary for PackedScenes and their queues
- Boolean value to post debug messages in Godot's console
- Store and retreive objects


## Installation

To use this script:

1. Copy `Pool.cs` to your project's folder
2. Create a C# script with an array of PackedScenes
3. In that script, write `public Pool pool = new Pool();` inside `public override void _EnterTree();`
4. You'd have to initialize the pool by passing the PackedScene's array and a node to act as a parent for Instantiated Nodes: `pool.Innit(PackedScenesArray, parentNode);`
5. Have fun!

Please make sure you attach materials to your liking and preferences.
## Usage/Examples

Its preferrable to initialize the script inside `_Ready()` with the following calls:

```
Pool pool = New Pool();
pool.Innit(PackedSceneArray, parentNode);
```

`Pool.Innit()` would create the static instance `Pool.instance` if it was empty and unassigned so you don't have to reference a pool at any script that needs a poot, just refer to the static instance for pooling. 

Where `PackedSceneArray` is a predefined array of the PackedScenes you'd like to pool, and `parentNode` is the node that would be all Instantiated initial parent node.

### Get a node from the pool
To get a `Node` from the pool, you can simply do the following:

```
Node node = Pool.instance.GetPooled("NAME");
```

Where `NAME` is the `PackedScene`'s root node's name (Case sensitive).

### Store a node inside the pool
To store a node back in the pool, you can simply do the following:

```
Pool.instance.StoreObject(NODE);
```

Where `NODE` is the node you want to store in the pool.
NOTE: Make sure that you hide the stored object (Usually by `.visible = false`).
## License

[The Unlicense](https://choosealicense.com/licenses/unlicense/)