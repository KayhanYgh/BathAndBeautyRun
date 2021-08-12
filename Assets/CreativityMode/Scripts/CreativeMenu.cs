using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreativeMenu : MonoBehaviour
{
    public SkyClass[] skies;
    public InputField inputField;

    public void ChangeGroundMaterial(string id)
    {
        foreach (var item in FindObjectsOfType<ChangeMaterialListener>())
        {
            item.ChangeMaterial(id);
        }
    }

    public void ChangeSkyMaterial(string id)
    {
        foreach (var item in skies)
        {
            if (item.id == id)
            {
                RenderSettings.skybox = item.material;
                RenderSettings.fogColor = item.fogColor;
            }
        }
    }

    public void LoadScene()
    {
        try
        {
            SceneManager.LoadScene(int.Parse(inputField.text));
        }
        catch
        {

            //  ignore
        }
    }


    [System.Serializable]
    public class SkyClass
    {
        public string id;
        public Material material;
        public Color fogColor;
    }
}
