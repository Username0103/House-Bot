# House-Bot
## Space simulation bot in Discord made in C#.

- Simulate a space where each channel is a room and you can move around. Each user can only be in one room at once.
- You can move between rooms with !move \<roomName>.
- Moving rooms takes time, and people in the target room will be alerted when you're coming in. Or maybe moving is instant but there's a customizable cooldown.
- Allow admins to de-admin themselves (and get a "de-adminned" role which they can redeem to become an admin again) so they can enjoy moving around too.
- Voice channel (that only you can access, and the bot is there) where sound effects play, based on the environment (maybe sound input from adjacent rooms too but muffled because of the distance?). Example of sound would be footsteps when you go into another room.
  
- Probably needs to be hosted by the owner of the server or something. I'm too poor to host this myself.
- For this to work, disable non-admin channel access by default, then specifically allow users to enter the new channel and remove old channel access at the same time. The access gets removed when they're AFK for too long.
- Allow users to use short names for rooms (like stra instead of strawberry room) when moving, assuming there is no ambiguity.
- Eventually expand RP-oriented features like items, an inventory, etc.
- The space you move around in (the "house") will be defined with a JSON file.
- Since the house will probably be reset a lot, Message logging to a database would be a good idea.
- The bot generates the channels and category and sets everything up with the JSON.
- Verify JSON with schemas.
- JSON file might be like:
```json
{
  "Channel1": {
    "room1": {
      "displayName": "Room One",
      "accessibleByRoles": [
        "all"
      ],
      "entranceMessage": "Example room named room1",
      "teaserMessage": "Endless possibilities lie awaiting...",
      "ambientNoise": "exampleSound1",
      "lockable": false,
      "connectedTo": [
        {
          "id": "room2",
          "directionFromHere": "north"
        },
        {
          "id": "room3",
          "directionFromHere": "south"
        }
      ]
    },
    "room2": {
      
    }
  },
  "meta": {
    "startChannel": "room1",
    "movementTimeSeconds": 3
  }
}
```
\> Notes:
- Use Discord's 'ephemeral message' feature to send messages only visible to one person in a channel once the user arrives.  
- Make sure to warn during setup that ambientNoise needs to repeat well.  
- Lockable is an optional property.  
  
Entrance message will be like (for room1 from the JSON):
```
*Example room named room1*

You can move to:
    <room2 display name> <room 2 teaser message>
    <room3 display name> <room 3 teaser message>

(only visible to you)
```
\> Possible commands:  
/look > displays entranceMessage  
/move > or maybe /go  
/view_current_json > admin only. Displays the JSON definition of the current room.  
/edit_json > admin only. Sends a message with the current JSON and reads your next code block for the new JSON. Also accepts attachments.  
/undo > go back to the previous room.  
/who > displays people in the current room.  
/shout > shout things to people in nearby rooms. Needs an option to disable this. Some obfuscation with \*. Output would be like:  
> *You hear a distant voice* "TH** IS AN E*AM**E"  

/lock > lock the current room if lockable, unlock the current room if locked.  
/announce > Send message to all rooms. Admin only.