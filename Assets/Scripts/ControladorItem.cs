using System;
using UnityEngine;

public class ControladorItem : MonoBehaviour
{
    [SerializeField] private int puntosItem_02 = 5;     // Referencia a la variable que establece los puntos que otorga el ítem_02
    [SerializeField] private int puntosItem_03 = 10;    // Referencia a la variable que establece los puntos que otorga el ítem_03


    // Obtiene los puntos del ítem_02
    public int ObtienepuntosItem_02()
    {
        return puntosItem_02;
    }

    // Obtiene los puntos del ítem_03
    public int ObtienepuntosItem_03()
    {
        return puntosItem_03;
    }
}
