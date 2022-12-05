using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Empire
{
    public int Gold = 0; // Total empire gold
    public int Mana = 0; // Total empire mana
    public List<City> Cities;
}
