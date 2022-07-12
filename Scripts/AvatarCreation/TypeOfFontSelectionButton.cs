using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeOfFontSelectionButton :MonoBehaviour
{
    public TypeOfChosenLetterFont typeOfFont;

    public delegate void OnTypeOfFontSelectionButtonClicked(TypeOfChosenLetterFont typeOfFont);

    public static OnTypeOfFontSelectionButtonClicked onSelectFonTypeButtonClicked;

    public void SelectFontType()
    {
        onSelectFonTypeButtonClicked.Invoke(typeOfFont);
    }

}
