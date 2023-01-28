# DataForge
A toolset for working with data in a procedural generation context.

⚠ This is a hobby project (for now). This means I'll work on it whenever I feel like it and I might introduce breaking changes if I feel like it. My wish is to release it one day, but for now I've made this just public because there's no reason not to.

## What is this?

I've worked with a lot of procedural generation in the past decade. I've implemented what feels like a dozen graph structures, noise functions, height map operations and formal grammars. Instead of always re-implementing the stuff, I decided to write a library containig some generic data structures for procedural content generation.

I've started with a graph module for now, but I plan on writing other modules, too. This is a list of things I may or may not implement in the future:
* Grammars (text-only and generic)
* Height Maps
* Custom Random Number Generation (based on [this GDC talk](https://www.youtube.com/watch?v=LWFzPP8ZbdU))
* Cellular Automata ontop of the Graph module
* Interpolation Methods
* WFC

## What can I do with this?

It is meant to be a library for use in procedural generation contexts. You can find some inspiration about what you can do with procedural generation [here](https://procgen.space/resources).
