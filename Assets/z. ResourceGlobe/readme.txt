How to use:

First you need a canvas and image object.
Example Materials or any material using GlobeShader can be attached to that image object as material.
Or you can directly drop prefabs to your scene after you create your canvas. You might need to align them, though.
There are a few properties of GlobeShader materials.

<Color Settings>
If you are using Linear Color Space, you need to toggle Linear Color Space setting. Using Linear Color Space is suggested.
If you are using Gamma Color Space, don't toggle this setting. However you might consider switching to Linear Color Space for better results.

<Texture Settings>
You need 5 textures to make the shader work. The first one is the primary texture and the other two are put as particles.
Different combinations will result in different looks, you need to go a little experimental there.
You also need a mask texture and a texture for fake 3D effect (normal map). I have included Circle mask and a sphere normal map which you can use.
WaterLine texture is included in the Textures. You can use it or create yours.
Main Brightness is an important property because when the textures are multiplied, the brightness will decrease. You may need to set this value to a large number (like 10000) if you have darker textures.

<Health Settings>
There, you can set maximum health and the current health value. The percentage will be used to show how much of the globe is filled.

<Water Settings>
Rotation Speed is the rotation speed of the main texture.
Particle Rotation speed is the second texture's rotation speed.
You can enable Waves and WaterLine.
Crop around line will set how much of the effect shown above the water line.
Water line color will be generally 1, 1, 1, 0.4 (a white transparent color). But you can modify it to make it look in different colors. Values are R G B A.
Water line boldness will affect how bold the line is.