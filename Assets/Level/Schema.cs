using ProtoBuf;
using System.Collections.Generic;

namespace WesGoof.GrabStudio.Schema
{
    [ProtoContract]
    public class Level
    {
        [ProtoMember(1)]
        public uint formatVersion = 22;

        [ProtoMember(2)]
        public string title;

        [ProtoMember(3)]
        public string creators;

        [ProtoMember(4)]
        public string description;

        [ProtoMember(5)]
        public uint complexity;

        [ProtoMember(6)]
        public List<LevelNodeWrapper> levelNodes = new();

        [ProtoMember(7)]
        public int maxCheckpointCount = 10;

        [ProtoMember(8)]
        public AmbienceSettings ambienceSettings;

        [ProtoMember(9)]
        public List<string> tags = new();

        [ProtoMember(10)]
        public ulong defaultSpawnPointID;

        [ProtoMember(11)]
        public bool unlisted = false;
    }

    [ProtoContract]
    public class AmbienceSettings
    {
        [ProtoMember(1)] public Color skyZenithColor;   // was wrongly skyHorizonColor
        [ProtoMember(2)] public Color skyHorizonColor;  // was wrongly skyZenithColor
        [ProtoMember(3)] public float sunAltitude;
        [ProtoMember(4)] public float sunAzimuth;
        [ProtoMember(5)] public float sunSize;
        [ProtoMember(6)] public float fogDensity;       // was missing entirely
    }

    [ProtoContract]
    public class Color
    {
        [ProtoMember(1)] public float r;
        [ProtoMember(2)] public float g;
        [ProtoMember(3)] public float b;
        [ProtoMember(4)] public float a = 1.0f;
    }

    [ProtoContract]
    public class LevelNodeWrapper
    {
        [ProtoMember(1)] public LevelNodeStart levelNodeStart;
        [ProtoMember(2)] public LevelNodeFinish levelNodeFinish;
        [ProtoMember(3)] public LevelNodeStatic levelNodeStatic;
        [ProtoMember(4)] public LevelNodeSign levelNodeSign;
        [ProtoMember(6)] public bool isLocked = false;
    }

    [ProtoContract]
    public class Vector3
    {
        [ProtoMember(1)] public float x;
        [ProtoMember(2)] public float y;
        [ProtoMember(3)] public float z;
    }

    [ProtoContract]
    public class Quaternion
    {
        [ProtoMember(1)] public float x;
        [ProtoMember(2)] public float y;
        [ProtoMember(3)] public float z;
        [ProtoMember(4)] public float w;
    }

    [ProtoContract]
    public class LevelNodeStatic
    {
        [ProtoMember(1)] public int shape;
        [ProtoMember(2)] public int material;
        [ProtoMember(3)] public Vector3 position;
        [ProtoMember(4)] public Vector3 scale;
        [ProtoMember(5)] public Quaternion rotation;
        [ProtoMember(6)] public Color color;
        [ProtoMember(7)] public bool isNeon;
        [ProtoMember(8)] public bool isTransparent;
        [ProtoMember(9)] public bool isGrabbable;
        [ProtoMember(10)] public bool isGrapplable;
        [ProtoMember(11)] public bool isPassable;
    }

    [ProtoContract]
    public class LevelNodeSign
    {
        [ProtoMember(1)] public Vector3 position;
        [ProtoMember(2)] public Quaternion rotation;
        [ProtoMember(3)] public string text;
    }

    [ProtoContract]
    public class LevelNodeStart
    {
        [ProtoMember(1)] public Vector3 position;
        [ProtoMember(2)] public Quaternion rotation;
        [ProtoMember(3)] public float radius;
        [ProtoMember(4)] public string name;
    }

    [ProtoContract]
    public class LevelNodeFinish
    {
        [ProtoMember(1)] public Vector3 position;
        [ProtoMember(2)] public Quaternion rotation;
        [ProtoMember(3)] public float radius;
    }
}