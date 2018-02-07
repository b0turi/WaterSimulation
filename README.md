# WaterSimulation
A project made for EECS 466: Graduate Computer Graphics

# About
As a main component of the graduate version of Computer Graphics at Case Western Reserve University, studens work on a semester long project of their own choosing. Students could choose any number of topics discussed in class to showcase in their project and show a greater understanding and independent research into those areas. I chose to focus on an applied case of environment mapping in relation to the simulation of water. Simulating water in a realistic way is a complex problem which I tackled using many approximations.

To create a lifelike feel of motion to the water, I used a distortion map, an image that uses its red and green channels to store X and Y coordinates that dictate an offset amount in the U and V coordinates used at interpolated points on the faces of a 3D object. I captured the image seen by the camera in a Frame Buffer Object (FBO) and used this as the refractive component's texture, and also translated and rotated the camera to a point where the camera is under the surface of the water with its inverse X rotation and at the same relative distance from the water's surface to capture the point of view of the reflective FBO, which was also mixed into the surface of the quad that simulates the water. The amount of the reflective channel used and the amount of the refracive channel used was determined by an approximation of the Fresnel effect, using the dot product of the view vector and the normal of the quad to determine the percentage of the final image that should be reflected or refracted. This means Fresnel values closer to 1 would be almost entirely reflected and values closer to 0 would be almost entirely refracted.

The terrain the water surrounds is created using a height map which uses a grayscale image from #000000 to #FFFFFF with higher values indicating higher elevation. The height map was sampled to create the slightly low-poly terrain shown in the final product.

# Tools Used

I wrote this application using OpenTK, an open source framework that wraps OpenGL and OpenAL for .NET. On top of OpenTK, I developed a small game engine that could spawn objects at given points, and wrote basic shaders to implement simple lighting on most objects and a complex shader for the water.

I created the 3D models for the trees and the boats using Blender and used Photoshop to create their textures. 

The height map, distortion map, and normal map were all assets found in a tutorial series online by ThinMatrix.
