using System.Collections;
using TMPro;
using UnityEngine;

// Ce script est a placer dans la hiérarchie (ou dans le canva directement)
// Il contient une fonction qui peut etre appelee partout s'il a la reference de se script 
// Il faut lui passer les parammtres : Textes a ecrire, emplacement du texte, secondes entre chaque caracteres
public class TypeSentence : MonoBehaviour
{
   // Parametres 
   private TMP_Text _textPlace;
   private string _textToShow;
   private float _timeBetweenChar; // Temps en Seconde

    public void WriteMachinEffect(string _currentTextToShow, TMP_Text _currentTextPlace, float _currentTimeBetweenChar) // Fonction à appeler depuis un autre script
    {
        _textToShow = _currentTextToShow;
        _textPlace = _currentTextPlace;
        _timeBetweenChar = _currentTimeBetweenChar;
        StartCoroutine(TypeCurrentSentence(_textToShow, _textPlace));
    }
    IEnumerator TypeCurrentSentence(string sentence, TMP_Text place)
    {
        foreach (char letter in sentence.ToCharArray())
        {
            yield return new WaitForSeconds(_timeBetweenChar);
            place.text += letter;
            yield return null;
        }
    }
}
