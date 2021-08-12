using UnityEngine;

public class ChangeMaterialListener : MonoBehaviour
{
    public MaterialClass[] materials;
    public MeshRenderer mesh;

    public void ChangeMaterial(string id)
    {
        foreach (var item in materials)
        {
            if (item.id == id)
            {
                mesh.material = item.material;
                mesh.materials[0] = item.material;
            }
        }
    }

    [System.Serializable]
    public class MaterialClass
    {
        public string id;
        public Material material;
    }


}
