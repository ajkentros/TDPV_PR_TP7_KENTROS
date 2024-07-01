using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlFinal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tituloFinal;     // Referencia variable al texto del t�tulo
    [SerializeField] private TextMeshProUGUI totalPuntos;     // Referencia variable al texto de los puntos finales alcanzados
    [SerializeField] private TextMeshProUGUI totalItems;      // Referencia variable al texto de cantidad de items coleccionados
    [SerializeField] private TextMeshProUGUI totalRebotes;    // Referencia variable al texto de cantidad de rebotes efectuados


    private bool gameOver;

    // Update is called once per frame
    void Update()
    {
        
        // Define la variable que verifica si el juego termin�
        gameOver = GameManager.gameManager.ObtieneEstadoGameOver();

        // Actualiza el men� final si el juego ha terminado
        if (gameOver)
        {
            ActualizaUIMenuFinal();
        }


    }

    // Actuliza el canvas del men� final
    private void ActualizaUIMenuFinal()
    {
        
        // Pausa el juego
        //GameManager.gameManager.PausarJuego();

        // Cambia el t�tulo de men�
        tituloFinal.text = "GANASTE EL JUEGO";


        // Actualiza el texto de los puntos alcanzados
        totalPuntos.text = GameManager.gameManager.ObtieneTotalPuntos().ToString();

        // Actualiza el texto de la cantidad de items coleccionados
        totalItems.text = GameManager.gameManager.ObtieneTotalItems().ToString();

        // Actualiza el texto de rebotes efectuados
        totalRebotes.text = GameManager.gameManager.ObtieneTotalRebotes().ToString();
    }
    
      
    // Gestiona la accci�n del bot�n Men�
    public void BotonMenu()
    {
        AudioManager.Instance.StopEfectoSonoro();
        // Setea gameOver = true para iniciar variables del juego
        GameManager.gameManager.IniciaMenu();


    }
}
