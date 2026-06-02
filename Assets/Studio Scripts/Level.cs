namespace WesGoof.GrabStudio
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;
    using System.IO;
    using ProtoBuf;
    using WesGoof.GrabStudio.Schema;
    using Newtonsoft.Json;

    public class Level : MonoBehaviour
    {
        [Header("Map Info")]
        public string mapTitle = "GRAB Studio";
        public string mapCreator = "WesGoof";
        public int mapCheckpoints;
        [TextArea(2, 5)]
        public string mapDescription = "This level was made with GRAB Studio.";

        [Header("Ambience Settings")]
        public UnityEngine.Color skyZenithColor = new UnityEngine.Color(0.28f, 0.476f, 0.73f, 1);
        public UnityEngine.Color skyHorizonColor = new UnityEngine.Color(0.916f, 0.9574f, 0.9574f, 1);
        public float sunAltitude = 45f;
        public float sunAzimuth = 315f;
        public float sunSize = 1f;
        public float fogDensity = 0f;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Level))]
    public class LevelEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Level script = (Level)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Compile"))
            {
                Compile(script);
            }
        }

        public static void Compile(Level info)
        {
            string path = EditorUtility.SaveFilePanel(
                "Save GRAB Level",
                "",
                info.mapTitle,
                "level"
            );

            if (string.IsNullOrEmpty(path))
                return;

            var levelData = new Schema.Level
            {
                title = info.mapTitle,
                creators = info.mapCreator,
                description = info.mapDescription,
                maxCheckpointCount = info.mapCheckpoints,

                ambienceSettings = new AmbienceSettings
                {
                    skyHorizonColor = UnityToProtoColor(info.skyHorizonColor),
                    skyZenithColor = UnityToProtoColor(info.skyZenithColor),
                    sunAltitude = info.sunAltitude,
                    sunAzimuth = info.sunAzimuth,
                    sunSize = info.sunSize,
                    fogDensity = info.fogDensity
                },

                levelNodes = GetBinaryNodes()
            };

            using (var file = File.Create(path))
            {
                Serializer.Serialize(file, levelData);
            }

            string jsonPath = Path.ChangeExtension(path, ".json");

            string jsonContent = JsonConvert.SerializeObject(
                levelData,
                Formatting.Indented
            );

            File.WriteAllText(jsonPath, jsonContent);

            Debug.Log(
                $"<color=green>SUCCESS!</color>\n" +
                $"Binary: {path}\n" +
                $"JSON Debug: {jsonPath}"
            );
        }

        private static List<Schema.LevelNodeWrapper> GetBinaryNodes()
        {
            var nodeList = new List<LevelNodeWrapper>();

            Node[] sceneNodes = Object.FindObjectsOfType<Node>(true);

            foreach (var node in sceneNodes)
            {
                if (node == null)
                    continue;

                var wrapper = new LevelNodeWrapper
                {
                    isLocked = false
                };

                var pos = new Schema.Vector3
                {
                    x = node.transform.position.x,
                    y = node.transform.position.y,
                    z = node.transform.position.z
                };

                var rot = new Schema.Quaternion
                {
                    x = node.transform.rotation.x,
                    y = node.transform.rotation.y,
                    z = node.transform.rotation.z,
                    w = node.transform.rotation.w
                };

                var scale = new Schema.Vector3
                {
                    x = node.transform.localScale.x,
                    y = node.transform.localScale.y,
                    z = node.transform.localScale.z
                };

                bool validNode = true;

                switch (node.nodeType)
                {
                    case GrabNodeType.Static:
                        wrapper.levelNodeStatic = new LevelNodeStatic
                        {
                            shape = node.shape,
                            material = node.material,
                            position = pos,
                            rotation = rot,
                            scale = scale,
                            color = UnityToProtoColor(node.color1),
                            isNeon = node.isNeon,
                            isTransparent = node.isTransparent,
                            isGrabbable = node.isGrabbable,
                            isGrapplable = node.isGrapplable,
                            isPassable = node.isPassable
                        };
                        break;

                    case GrabNodeType.Sign:
                        wrapper.levelNodeSign = new LevelNodeSign
                        {
                            text = node.signText ?? string.Empty,
                            position = pos,
                            rotation = rot
                        };
                        break;

                    case GrabNodeType.Start:
                        wrapper.levelNodeStart = new LevelNodeStart
                        {
                            position = pos
                        };
                        break;

                    case GrabNodeType.Finish:
                        wrapper.levelNodeFinish = new LevelNodeFinish
                        {
                            position = pos
                        };
                        break;

                    case GrabNodeType.Crumbling:
                    case GrabNodeType.Gravity:
                        Debug.LogWarning(
                            $"Unsupported node type '{node.nodeType}' on '{node.name}'. Skipping."
                        );
                        validNode = false;
                        break;

                    default:
                        Debug.LogWarning(
                            $"Unknown node type '{node.nodeType}' on '{node.name}'. Skipping."
                        );
                        validNode = false;
                        break;
                }

                if (validNode)
                {
                    nodeList.Add(wrapper);
                }
            }

            return nodeList;
        }

        private static Schema.Color UnityToProtoColor(UnityEngine.Color c)
        {
            return new Schema.Color
            {
                r = c.r,
                g = c.g,
                b = c.b,
                a = c.a
            };
        }
    }
#endif
}
