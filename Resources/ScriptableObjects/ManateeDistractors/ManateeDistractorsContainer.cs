using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManateeGame Distractor Container", menuName = "ScriptableObjects/ManateeGame Distractor Container", order = 1)]
public class ManateeDistractorsContainer : ScriptableObject
{
    // Start is called before the first frame update
    public string Letter;
    public List<ManateeImageObject> DistractorsList;
}
