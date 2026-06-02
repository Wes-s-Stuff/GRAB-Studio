namespace WesGoof.GrabStudio
{
    using UnityEngine;

    [ExecuteAlways]
    public class Node : MonoBehaviour
    {
        public GrabNodeType nodeType = GrabNodeType.Static;
        public int shape = 1000;
        public int material = 0;

        public Color color1 = Color.white;
        public bool isNeon = false;
        public bool isTransparent = false;

        [Header("Physics/Interaction")]
        public bool isGrabbable = false;
        public bool isGrapplable = false;
        public bool isPassable = false;

        public string signText;

#if UNITY_EDITOR
        private MaterialPropertyBlock _propBlock;

        private void OnValidate()
        {
            UpdateVisuals();
            UpdateModel();
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                UpdateVisuals();
                UpdateModel();
            }
        }

        private void UpdateModel()
        {
            MeshFilter filter = GetComponent<MeshFilter>();

            if (nodeType == GrabNodeType.Start || nodeType == GrabNodeType.Finish)
            {
                Mesh mesh = Resources.Load<Mesh>($"Models/SE");
                filter.mesh = mesh;
            }
            else if (nodeType == GrabNodeType.Sign)
            {
                Mesh mesh = Resources.Load<Mesh>($"Models/S");
                filter.mesh = mesh;
            }
            else
            {
                Mesh mesh = Resources.Load<Mesh>($"Models/{shape}");
                filter.mesh = mesh;
            }
        }

        private void UpdateVisuals()
        {
            Renderer ren = GetComponent<Renderer>();
            if (ren == null) return;

            if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

            if (nodeType == GrabNodeType.Start)
            {
                Material mat = Resources.Load<Material>($"Textures/nan");
                ren.material = mat;
                ren.GetPropertyBlock(_propBlock);
                _propBlock.SetColor("_Color", new Color(0, 0, 0));
                ren.SetPropertyBlock(_propBlock);
            }
            else if (nodeType == GrabNodeType.Finish)
            {
                Material mat = Resources.Load<Material>($"Textures/nan");
                ren.material = mat;
                ren.GetPropertyBlock(_propBlock);
                _propBlock.SetColor("_Color", new Color(1, 0, 0));
                ren.SetPropertyBlock(_propBlock);
            }
            else if (nodeType == GrabNodeType.Sign)
            {
                Material mat = Resources.Load<Material>($"Textures/4");
                ren.material = mat;
                ren.GetPropertyBlock(_propBlock);
                _propBlock.SetColor("_Color", new Color(1, 1, 1));
                ren.SetPropertyBlock(_propBlock);
            }
            else if (material == 8)
            {
                Material mat = Resources.Load<Material>("Textures/8");
                ren.material = mat;
                ren.GetPropertyBlock(_propBlock);
                _propBlock.SetColor("_Color", color1);
                ren.SetPropertyBlock(_propBlock);
            }
            else if (material == 3)
            {
                Material mat = Resources.Load<Material>("Textures/3");
                ren.material = mat;
                ren.GetPropertyBlock(_propBlock);
                _propBlock.SetColor("_Color", new Color(1, 1, 0));
                ren.SetPropertyBlock(_propBlock);
            }
            else
            {
                Material mat = Resources.Load<Material>($"Textures/{material}");
                ren.material = mat;
                ren.GetPropertyBlock(_propBlock);
                _propBlock.SetColor("_Color", new Color(1, 1, 1));
                ren.SetPropertyBlock(_propBlock);
            }
        }
#endif
    }

    public enum GrabNodeType { Static, Start, Finish, Crumbling, Sign, Gravity }
}