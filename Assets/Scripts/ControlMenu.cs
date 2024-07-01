using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlMenu : MonoBehaviour
{
    [SerializeField] private GameObject panelMenu;               // Referencia al panel menu
    [SerializeField] private GameObject panelInstrucciones;      // Referencia al panel instrucciones

    private void Start()
    {
        // Configura paneles
        panelMenu.SetActive(true);
        panelInstrucciones.SetActive(false);
    }
    // Gestiona el bot�n Jugar
    public void BotonJugar()
    {
        // Reinicia las variables del kuego
        GameManager.gameManager.ReiniciaJuego();

        // Carga la escena 1
        SceneManager.LoadScene(1);

    }

    // Gestiona el cierre de la aplicaci�n
    public void SaleJuego()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Gestiona el bto�n Instrucciones
    public void BotonInstrucciones()
    {
        panelMenu.SetActive(false);
        panelInstrucciones.SetActive(true);
    }

    // Gestiona el bot�n Volver
    public void BotonVolver()
    {
        panelMenu.SetActive(true);
        panelInstrucciones.SetActive(false);
    }
}
