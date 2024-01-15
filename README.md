# PRogramAR: An end-user augmented reality trigger-action robot programming system #

This project provides reviewers with a simulation of our system PRogramAR. Users can run our system in Unity while interacting with a simulated robot running in ROS. If the reviewer has a HoloLens, they may also run our Unity program using the holographic remoting application on the HoloLens device.

# Requirements #
To run our program reviewers will need to have the following.
ROS Noetic
Unity 2020.3.38f1
git downloaded on windows

# Setting up ROS #
Setup and run the ROS portion of PRogramAR using the following repository: https://github.com/hri-ironlab/fetch_mtc/tree/PRogramAR

# Setting up Unity #
## Step 1 ##
Download this repository using the following command in git bash:
```
git clone https://github.com/bryceikeda/PRogramAR.git
```

## Step 2 ##
Open Unity Hub then click "Open" in the top right. Then, navigate to the cloned unity project and press open.

## Step 3 ##
If the project does not automatically open with the demo scene, navigate to Assets -> ProgramAR -> Scenes and click demo_scene

## Step 4 ##
Get the IP address of the computer running ROS. Then, navigate to the Unity Hierarchy, click "ROSConnectionPrefab," and update the ROS IP Address. Finally, press the Unity play button.  

Note: If the ROS IP in the top left of the game view is red rather than a blue/green color, you may need to stop Unity, relaunch the ROS files from the previous Step 5, then replay Unity.

## Step 5 ##
Once the system is running, you can hold down the right click button on the mouse and move around the scene using the wasd keys. To click on buttons or move zones around the scene, use the left clicker on your mouse.

# Using the HoloLens #
If you would like to use your HoloLens with the demo simulation, first download and print off the QR code provided with the files, put it on a table, then follow these steps.

## Step 1 ##
Turn on your HoloLens and put it on. Then download the application called Holographic Remoting. Once downloaded, navigate to the application and run it.

## Step 2 ## 
Make sure the HoloLens is on the same network as your computer running Unity. Then write down the IP Address shown in the Holographic Remoting app.

## Step 3 ##
Go back to the Unity application then navigate to the tabs at the top and click Mixed Reality -> Remoting -> Holographic Remoting for Play Mode. Then type in the IP Address into the "Remote Host Name". Finally, click Enable Holographic Remoting for Play Mode.

## Step 4 ##
Go to the Hierarchy on the left side of the application and click "QRManager". Then, click the three checkboxes associated with each attached component.

## Step 5 ##
Press play on the Unity application then look down at the QR code. Wait for the HoloLens to display the demo and move freely around the scene to interact with our system.