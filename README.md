# Arduino-Object-Detection
# Aim
The aim of this project is to enable the mini pan to detect the black spot in the area shown by the camera and position it relative to that point, and to shoot a laser at the center of that point.

# Steps
Our project is a black dot shot with a laser. For this we need 2 servo motors,1 mini pan/tilt camera and a laser. First, we performed the installation of the Mini Pan. We fixed the subsystem with a screwdriver. We put the first two parts together and put the servo motor between them. On top of it, we again combined the two parts and added another servo motor. We glued our laser with silicone so that it was parallel to the top.The image of the system is ready.

![Resim2](https://user-images.githubusercontent.com/79202352/159037962-bc16c462-e930-49aa-8e24-865860adca69.jpg)

It's time to connect these systems with arduino. We used 6 cables, female to male, for the servo motor. 3 Wires were used for each motor. The brown cable is connected to the ground, the red cable is connected to the 5V, and the orange cables are connected to the pins we want to signal. We have set the connections to Analog. When it came to the laser connection, 3 female-to-male cables were used for this. The circuit part of the Arduino has been completed.

![Resim3](https://user-images.githubusercontent.com/79202352/159038483-fd7b8428-06fc-4dbe-831f-51fc43261a33.jpg)

Then the writing of the Arduino code was started. For the use of Servo Motor Servo.h library was used. Servo motors (M1, M2) and Th1 and Th2 variables were defined to specify the angles. The connection between the Arduino and the computer has been established. The engines are brought to the starting position. 90 degrees was assigned. Since the code will run over image processing, a waiting time has been defined. Th1 and Th2 are defined in the engine so that the angles are transmitted to the engine. Then the necessary codes were written for the flashing of the laser. If it is "1", the laser is enabled to turn on. A short waiting period was given. It was set to "0" and the laser was turned off. In this way, the laser became blinking. After selecting the required port, I uploaded the code. Our project is ready. The engines turned 90 degrees. Now we are ready for the image processing stage.

![Resim4](https://user-images.githubusercontent.com/79202352/159038754-8d628194-e5dc-4e54-bc85-98842a1e9e24.jpg)

I started writing C# code as a final process. I have defined the camera pointer to the system. I created a class and assigned an object to it. Since I want the camera to take 3 images. A color image was requested via Bgr. The other 2 were identified as class Gray. I have defined the Scale variable so that the system can take an image and process it. This value is known as cm/Px. First of all, I calculated the distance between the computer camera and the wall and divided it by the width of the screen.  This is my Scale.

![Resim5](https://user-images.githubusercontent.com/79202352/159039051-cb916fcd-6610-481f-91a8-65cf9f1a09ea.jpg)
