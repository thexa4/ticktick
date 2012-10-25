TickTick
===========

AudioManager
------------
You can use the AudioManager to load and play sounds. The AudioManager has its 
own ContentManager, with its root set at "Content/Audio" so you can directly 
load assets from there. In your LoadContent functions, use 

	AudioManager.Load(asset, instance) 
	
to load a new sound. If the instancename does already exists, you will get it 
back instead. You can also set pitch and volume with overloads of that 
function. Simply use the Play function to start playing a sound. 

Remember that songs in XNA are not seemlessly loopable, so background music is
best to be selected in the content project, content processor set to 
SoundEffect and open up the properties to set the compression if you want to.

FontCache
------------
Simple component to cache fonts. Just load them in your LoadContent function 
and use anywhere in your game after it is loaded. Uses the game default
ContentManager. 

InputManager
------------
Handles, captures and processes all input for your game. Has two properties 
(but more may be added) which are Keyboard and Mouse and both refer to their
respective InputHandling counterparts. For each Key or Button you can ask the
InputManager if it is currently:

	Pressed 	: key was pressed down this update call
	Released 	: key was released up this update call
	Down		: key is being held down
	Up			: key is being released
	Triggered	: key was Pressed, or Hold down. Just press a key on your
				  keyboard in a text editor to see what triggered is.
				  
The InputManager also works cross-threads.

ScreenManager
------------
Main Game State/Screen State management component. It might seem a bit over-
whelming but here are the important parts:

	AddScreen( a GameScreen )
	
	AudioManager property
	InputManager property
	ContentManager property
	SpriteBatch property
	
This component makes sure only the most RECENT gamescreen is visible. Use the
exit method on a gamescreen object to remove it from the component. Use the
properties to get access to the audio, input and contentManager and to get
access to a spritebatch. Each screen however will get its OWN contentmanager
so if you can, just use that one. This way, each screen can be completelty
unloaded when you are done with it, released all the assets used by that screen
without the need of reloading game-wide contents (like fonts). 

A GameScreen has an initialize, update, draw and loadcontent function. Just
override those in your subclass if you want to add functionality to it.
	
	
				  