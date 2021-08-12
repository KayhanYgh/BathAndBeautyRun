using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreativeMenu : MonoBehaviour
{
    public SkyClass[] skies;
    public InputField inputField;
    public GameObject[] uis;

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
    public Toggle hideUiToggle;

    public void HideUi()
    {
        foreach (var item in uis)
        {
            if (hideUiToggle.isOn)
            {
                item.SetActive(false);
            }
            else
            {
                item.SetActive(true);
            }
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
