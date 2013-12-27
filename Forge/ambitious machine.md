#An Ambitious Fabri Machine

I'm in a blue-sky mood, as usual. It's Christmas and no force on Earth can compel me to write text that must compile and run. 

Let's talk about the kind of thing we'd like to write with Fabri. Once it's really up and running. 

The robot itself is a SCARA arm traveling on a polished round rod, with two robust points of contact. It has an elbow with two degrees of freedom, a wrist with one, and three fingers. The work surface, which is round, has two such robots: the fingers may reach to the edge of the work environment. The base itself, containing both rods, may pivot on its own axis. There are microphones, cameras in visible and infrared, and a couple time-of-flight lasers, to construct the virtual environment. 

Here's what we can expect to work with: A laptop, running some kind of Unix. Our seat of power, and yet a virtual environment. Can't be helped, we all need documentation and weird foreign support tools and Hacker News breaks. This simply has to be done with the inspection and control computer. We don't even want it to be in the running loop, because we want to detach it and have things run. I put my laptop in my backpack and leave the area, frequently, whatever area we're talking about. 

For boss computers it will probably suffice to run a minimal Posix system with realtime guarantees. QNX, I believe. It will run its own version of Forge and Fabri code, supported by whatever arcane C libraries we need to make it all hum. 

These will have lieutenants who do specialized sensor processing, such as constructing a multileveled virtual environment from the inputs. Conceivably we'll use a Parallela to tie this all together, and perhaps a graphics card for physical modeling. 

 Then there are the microcontrollers, where we can certainly afford one per servo, so that force feedback sensors are hooked up directly to intelligence to control motor movement.  

What we're going to do, of course, is teach this confabulation to move around in its own environment and manipulate useful tools in useful ways. 

How we'll do this is also very ordinary: we'll design a nice user interface for the servos and do things like chopping vegetables via remote control. This should establish useful priors, we can then use the usual machine learning techniques to refine execution. 

3-d printing is a bit of a dead end. 3-d basketweaving has potential. 3-d pottery throwing, 3-d carpentry, 3-d food preparation. A pair of hands that can work in concert with a human being or even two to get things done together. 

This kind of gestalt system is...nontrivial to program. The sensors should train their own simulated world, with considerable input and guidance from the operators. If the robot is not sure whether something is a cucumber or a zucchini, it should ask. The most important thing for our robot to understand: what a human is, how they move, and that they are never to be touched with tools or arms under any circumstances. 

Many entertaining hours of playing snatch-and-grab with a robot armed with a toy knife lie ahead!

With parallel computers we can do intelligent things like simulate the robots behavior, analyse the result, then execute the behavior, while checking to see if we're having acceptably similar outcomes. That's learning in essence. 

We can of course look at the simulation, and decorate it in various ways. That's the best way to tell if some knowledge has made it into the robot's environment. 

This is a machine learning problem, sure, but mostly it's a simulation problem. When you can generate dense clouds of voxels, use them to precisely locate your cameras in space, and then use that to correlate the camera data, you have a pretty rich environment, so saying "this is a knife" and "this is a cucumber" is not as hard as it was. Surely, such a creation could carefully observe the effect of certain knife movements on a cucumber, and learn from that. 

Going from hand directly to machine is certainly a hard problem. Harder, anyway, than developing expertise at waldo manipulation. It's often easiest to meet the machines halfway. 
