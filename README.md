# Delivery-drone-AI with RL
A reinforcement learning project using unity to train drones on delivering parcels.

When training or using tensorboard, go to the working directory then into assets then training, and then run this command on your conda env to start the training:

mlagents-learn .\Droneconfig.yaml --run-id="Run _________" --torch-device "cpu"

(instead of __________ write a number like 5 so "Run 5")

To use tensorboard, run this command:

tensorboard --logdir results


--Tasks:

- (optional) new drone controls. so only take the models. [DONE]

- Adding trails for drones.

- Adding skybox and world border and parcel assets. [Done]


- positive rewards : {distance to drop zone, delivering parcel} [DONE]

- negative rewards : {collision for hitting environment and world border, time penatly} [DONE]

  
- Having multiple drones in each episode, but multiple episodes are the same time ghosting since the environment is constant. (under thought)

- mechanics needed : {drop zone random generation [Done] }
  

