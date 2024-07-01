using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControladorUI : MonoBehaviour
{
    
    [Header("PanelJuego")]
    [SerializeField] private TextMeshProUGUI rebotes;       // Referencia al componente Text de los rebotes
    [SerializeField] private TextMeshProUGUI nivel;         // Referencia al componente Text del nivel actual
    [SerializeField] private TextMeshProUGUI puntos;        // Referencia al componente Text de los puntos
    [SerializeField] private TextMeshProUGUI textoMensaje;  // Referencia al componente Text del mensaje

    [SerializeField] private GameObject menuNivel;          // Referencia al game object panel Menu Nivel

    private bool ganasteNivel;      // Referencia a la variable ganaste el nivel
    private bool juegoPausado;      // Referencia a la variable juego pausado


    private void Start()
    {
        // Define la variable que verifica si se ha ganado el nivel
        ganasteNivel = GameManager.gameManager.ObtieneEstadoGanasteNivel();

        // Define la variable que verifica si el juego está pausado
        juegoPausado = GameManager.gameManager.ObtieneEstadoJuegoPausado();

        // Actualiza el panel del juego
        menuNivel.SetActive(false);
        ActualizaPanelJuego();
    }

    private void Update()
    {
        // Actualiza el panel del juego
        ActualizaPanelJuego();

    }

      
    private void ActualizaPanelJuego()
    {
        // Define la variable que verifica si se ha ganado el nivel
        ganasteNivel = GameManager.gameManager.ObtieneEstadoGanasteNivel();

        // Define la variable que verifica si el juego está pausado
        juegoPausado = GameManager.gameManager.ObtieneEstadoJuegoPausado();

        // Actualiza el número de nivel jugado en el panel UI
        nivel.text = GameManager.gameManager.ObtieneEscenaActual().ToString();

        // Actualiza el texto de los rebotes alcanzados
        rebotes.text = GameManager.gameManager.ObtieneRebotes().ToString();

        // Actualiza el texto de los puntos alcanzados
        puntos.text = GameManager.gameManager.ObtienePuntos().ToString();

        // Si el juego está pausado juegoPausado = true => el jugador ganó o perdió
        if (juegoPausado)
        {
            // Si activa el Menu Nivel
            menuNivel.SetActive(true);

            // Si ganasteNivel = true => cambia el mensaje
            if (ganasteNivel)
            {
                // Cambia el título de menú
                textoMensaje.text = "GANASTE NIVEL";
            }
            else
            {
                // Cambia el título de menú
                textoMensaje.text = "PERDISTE NIVEL";
            }

        }
        else
        { 
            // Actualiza el panel del juego
            menuNivel.SetActive(false);
        }

    }

    // Gestiona la accción del botón Menú
    public void BotonMenu()
    {
        // Setea gameOver = true para iniciar variables del juego
        GameManager.gameManager.IniciaMenu();

    }

    public void SaleJuego()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Gestiona la accción del botón Continuar
    public void BotonContinuar()
    {
        if (ganasteNivel)
        {
            // Pasa al siguiente nivel = nueva escena o nivel
            GameManager.gameManager.SiguienteNivel();

        }
        else
        {
            // Se reinicia el nivel
            GameManager.gameManager.ReiniciaNivel();
        }

    }


}


