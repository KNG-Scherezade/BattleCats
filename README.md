# BattleCats
Comp 376 project

## Stations
Stations all run on an abstract station StationAbstract. Using composite design pattern each station is built off of StationGenericScript which contains the basic functions. This to avoid inheritance hiding messiness. 
* StationAbstract is an abstract station class
* StationController is a class to perform functions on all stations
* StationGeneric as a station is simply an object a player can enter into. Provides base station functionality to all stations inheriting from StationAbstract.
* StationNav allows the player to control the ball
