using System.Text.Json.Serialization;
using Json.Schema;

namespace root.src.house_bot.Misc
{
    public class JsonModels
    {
        public class Meta
        {
            public required string StartRoom { get; set; }
            public int MovementTimeSeconds { get; set; }
            public List<string>? AccessibleByRolesIds { get; set; }
        }

        public class Room
        {
            public required string DisplayName { get; set; }
            public List<string>? AccessibleByRoleIds { get; set; }
            public string? EntranceMessage { get; set; }
            public string? TeaserMessage { get; set; }
            public string? AmbientNoise { get; set; }
            public bool Lockable { get; set; } = false;
            public required List<Connection> ConnectedTo { get; set; }
        }

        public class Connection
        {
            public required string Id { get; set; }
            public Direction DirectionFromHere { get; set; }
        }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum Direction
        {
            N,
            S,
            E,
            W,
            NE,
            NW,
            SE,
            SW,
            up,
            down
        }
        public readonly JsonSchema MapSchema = JsonSchema.FromText("""
            {
            "$schema": "http://json-schema.org/draft-07/schema#",
            "title": "Game Map Configuration",
            "description": "Schema for defining game maps, rooms, their properties, and connections, based on the provided C# class structure.",
            "type": "object",
            "properties": {
                "meta": {
                "$ref": "#/$defs/meta"
                }
            },
            "required": [
                "meta"
            ],
            "additionalProperties": {
                "$ref": "#/$defs/mapContainer"
            },
            "$defs": {
                "meta": {
                    "type": "object",
                "description": "Metadata for the entire map configuration.",
                "properties": {
                        "startRoom": {
                            "type": "string",
                    "description": "The ID of the room where the player starts."
                        },
                    "movementTimeSeconds": {
                            "type": "integer",
                    "description": "The time in seconds it takes to move between rooms.",
                    "minimum": 0
                    },
                    "AccessibleByRolesIds": {
                            "type": "array",
                    "description": "Optional list of role IDs that can access this entire map configuration.",
                    "items": {
                                "type": "string"
                    }
                        }
                    },
                "required": [
                    "startRoom",
                    "movementTimeSeconds"
                ],
                "additionalProperties": false
                },
                "mapContainer": {
                    "type": "object",
                "description": "A container for a set of interconnected rooms, such as 'main_map' or 'sewer_system'. The keys are room IDs.",
                "additionalProperties": {
                        "$ref": "#/$defs/room"
                }
                },
                "room": {
                    "type": "object",
                "description": "Defines a single location or area in the game.",
                "properties": {
                        "displayName": {
                            "type": "string",
                    "description": "The human-readable name of the room."
                        },
                    "AccessibleByRoleIds": {
                            "type": "array",
                    "description": "Optional list of role IDs that can access this specific room.",
                    "items": {
                                "type": "string"
                    }
                        },
                    "entranceMessage": {
                            "type": "string",
                    "description": "The message displayed to the player upon entering the room."
                    },
                    "teaserMessage": {
                            "type": "string",
                    "description": "A short message describing this room when viewed from an adjacent, connected room."
                    },
                    "ambientNoise": {
                            "type": "string",
                    "description": "The filename of the ambient noise audio to play in the room."
                    },
                    "lockable": {
                            "type": "boolean",
                    "description": "Indicates if the room can be locked.",
                    "default": false
                    },
                    "connectedTo": {
                            "type": "array",
                    "description": "A list of connections to other rooms.",
                    "items": {
                                "$ref": "#/$defs/connection"
                    }
                        }
                    },
                "required": [
                    "displayName",
                    "connectedTo"
                ],
                "additionalProperties": false
                },
                "connection": {
                    "type": "object",
                "description": "Defines a one-way link from the current room to another.",
                "properties": {
                        "id": {
                            "type": "string",
                    "description": "The ID of the room to connect to."
                        },
                    "directionFromHere": {
                            "$ref": "#/$defs/direction"
                    }
                    },
                "required": [
                    "id",
                    "directionFromHere"
                ],
                "additionalProperties": false
                },
                "direction": {
                    "type": "string",
                "description": "A cardinal or relative direction.",
                "enum": [
                    "N",
                    "S",
                    "E",
                    "W",
                    "NE",
                    "NW",
                    "SE",
                    "SW",
                    "up",
                    "down"
                ]
                }
            }
            }
            """);
    }
}