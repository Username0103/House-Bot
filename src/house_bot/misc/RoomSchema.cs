using System.Text.Json.Nodes;
using Json.Schema;

namespace root.src.house_bot.Misc
{
    public static class RoomSchema
    {
        public static JsonSchema GetSchema()
        {
            return JsonSchema.FromText("""
            {
            "$schema": "http://json-schema.org/draft-07/schema#",
            "title": "Text Adventure Map Configuration",
            "description": "A schema for defining interconnected rooms, their properties, and global metadata for a text-based game.",
            "type": "object",
            "properties": {
                "meta": {
                "$ref": "#/definitions/meta"
                }
            },
            "patternProperties": {
                "^(?!meta$).*": {
                "title": "Channel",
                "description": "A container for a set of interconnected rooms, representing a distinct map or area. The key is the channel's unique ID.",
                "type": "object",
                "additionalProperties": {
                    "$ref": "#/definitions/room"
                }
                }
            },
            "required": [
                "meta"
            ],
            "definitions": {
                "meta": {
                "title": "Metadata",
                "description": "Global settings and metadata for the map configuration.",
                "type": "object",
                "properties": {
                    "startRoom": {
                    "description": "The unique ID of the room where players start.",
                    "type": "string"
                    },
                    "movementTimeSeconds": {
                    "description": "The default time, in seconds, it takes to move between rooms.",
                    "type": "integer",
                    "minimum": 0
                    }
                },
                "required": [
                    "startRoom",
                    "movementTimeSeconds"
                ]
                },
                "room": {
                "title": "Room",
                "description": "Defines a single location within a channel.",
                "type": "object",
                "properties": {
                    "displayName": {
                    "description": "The human-readable name of the room.",
                    "type": "string"
                    },
                    "accessibleByRoles": {
                    "description": "An array of role IDs that can access this room. Use ['all'] for public access.",
                    "type": "array",
                    "items": {
                        "type": "string"
                    }
                    },
                    "entranceMessage": {
                    "description": "The message displayed to a user upon entering the room.",
                    "type": "string"
                    },
                    "teaserMessage": {
                    "description": "A short message shown when looking at this room's connection from another room.",
                    "type": "string"
                    },
                    "ambientNoise": {
                    "description": "An identifier for a looping background sound to play while in the room.",
                    "type": "string"
                    },
                    "lockable": {
                    "description": "Whether the room can be locked, preventing entry.",
                    "type": "boolean",
                    "default": false
                    },
                    "connectedTo": {
                    "description": "An array of connections to other rooms.",
                    "type": "array",
                    "items": {
                        "$ref": "#/definitions/connection"
                    }
                    }
                },
                "required": [
                    "displayName",
                    "accessibleByRoles",
                    "connectedTo"
                ]
                },
                "connection": {
                "title": "Connection",
                "description": "Defines a one-way path from the current room to another.",
                "type": "object",
                "properties": {
                    "id": {
                    "description": "The unique ID of the room to connect to. This must match a room key in the same channel.",
                    "type": "string"
                    },
                    "directionFromHere": {
                    "description": "The direction of travel from this room to the connected room.",
                    "type": "string",
                    "enum": [
                        "north",
                        "south",
                        "east",
                        "west",
                        "northeast",
                        "northwest",
                        "southeast",
                        "southwest",
                        "up",
                        "down"
                        ]
                    }
                },
                "required": [
                    "id",
                    "directionFromHere"
                ]
                }
            }
            }
            """);
        }
    }
}