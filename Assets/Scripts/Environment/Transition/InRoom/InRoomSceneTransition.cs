using UnityEngine;
using UnityEngine.SceneManagement;
public class InRoomSceneTransition : MonoBehaviour
{
   
   //Once the player bumps into the trigger, load the next part of the room 
   //in the update function, check if player is for enough away to unload the last part of the room
   //either use an index or make a Scene array for every multi part room, load the next scene and do array stuff 
   //put the trigger far back in room 1! the player should never see loading
   //Remember to use loadsceneasync
   /*
   Example: Room 1 is loaded
   Player hits trigger to load room 2 
   room 1 and room 2 is loaded 
   Player hits trigger for room 3
   Room 2 and 3 is loaded
   Room 1 is unloaded
   Player goes back to trigger in room 2
   Room 2 and 1 is loaded
   Room 3 is unloaded 
   etc 
   */
}
