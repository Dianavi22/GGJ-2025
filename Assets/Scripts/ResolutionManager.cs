using UnityEngine;

// Script permettant de fixer le jeu
// une resolution
// Placer le script sur la scene via un gameObject
// Choisissez la resolution dirrectement via l'inspectorci-besoin (1920x1080 par defaut)

// Les UI son affectes et donc ne bougeent plus peu importe la resolution de l'ecran (plus besoin d'adapter les ancres) 
// Seul inconvenient : IL peut y avoir des petites bandes noires autour ddu jeu mais c'est negligeable
public class ResolutionManager : MonoBehaviour 
{
    [SerializeField] int targetWidth = 1920;
    [SerializeField] int targetHeight = 1080;

    void Start()
    {
        Resolution[] resolutions = Screen.resolutions;

        foreach (Resolution res in resolutions)
        {
            if (res.width == targetWidth && res.height == targetHeight)
            {
                Screen.SetResolution(targetWidth, targetHeight, true);
                break;
            }
        }
    }
}

