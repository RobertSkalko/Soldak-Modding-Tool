# Soldak-Modding-Tool

This is a tool i created that allows me to more easily mod Soldak games.

It uses Unity Engine(I learned Unity's UI so it was the best choice for me) and C#.

Use instructions:
* setup all the required data in input fields (they are saved so you don't have to input again on restart if it's the same)
* place all .gdb files in the FileEditPath you specified (can be in folder or in zip, doesn't matter the tool should get them all)
* Click a button

That's it! Your result is outputed to the output path folder. 

Do note that currently there's not much functionality, but if you have a specific use you can always write a ToolButton yourself!

Dev Use instructions:
* Open project in unity
* Create a new .cs script and derive from ToolButton, make sure you're in the same namespace SoldakModdingTool
* Add functionality to Action() 

Done! Everything that derives from ToolButton is auto added to the buttons list using reflection.

If you created any Tools or added functionality, please post a link to it. I hope that as we create more and more general
purpose tools, this Modding Tool will be more and more useful!

