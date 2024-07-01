using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;     // Referencia instancia estática del GameManager para acceder desde otros scripts

    private int puntos;         // Referencia variable puntos por nivel
    private int totalPuntos;    // Referencia variable acumula puntos totales en el juego
    private int items;          // Referencia variable items coleccionados
    private int itemsEnNivel;   // Referencia variable calcula items en el nivel que se juega
    private int totalItems;     // Referencia variable acumula itmes totales en el juego
    private int rebotes;        // Referencia variable cuanta cantidad de rebotes por nivel
    private int totalRebotes;   // Referencia variable acumula cantidad de rebotes totales en el juego

    private int nivel;      // Contador de dimensiones jugadas (escena o niveles)

    private bool ganasteNivel;      // Referencia variable controla el estado del nivel (ganaste el nivel)
    private bool gameOver;          // Referencia variable controla el gameOver del juego (si termina o no el juego)
    private bool juegoPausado;      // Referencia variable controla si el juego está pausado o no


    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            
            // Evita que el objeto GameManager se destruya al cargar una nueva escena.
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        IniciaStart();
    }
    private void Update()
    {
        // Si el juego no termino y no está pausado => 
        if (!gameOver)
        {          
            // Verifica el estado del juego 
            VerificaEstadoJuego();
        }

        VerificaEscape();
    }

    // Gestiona el inicio de las variables
    private void IniciaStart()
    {
        puntos = 0;
        totalPuntos = 0;
        items = 0;
        totalItems = 0;
        nivel = 0;
        rebotes = 0;
        totalRebotes = 0;
        ganasteNivel = false;
        gameOver = false;
        juegoPausado = false;
        Time.timeScale = 1f;
        AudioManager.Instance.PlayMusicaFondo();
        AudioManager.Instance.StopEfectoSonoro();
        //Debug.Log("Juego iniciado, Time.timeScale establecido a 1");

    }
    
    // Gestiona el estado de los puntos y la escena actual
    private void VerificaEstadoJuego()
    {     
        // Verifica la cantidad de items coleccionados
        VerificaItems();

        // Verifica la escena actual
        VerificaEscenaActual();

        // Verifica cantidad de Rebotes
    }

    // Verifica la cantidad de items en la escena
    private void VerificaItems()
    {
        // Llama al método que calcula la cantidad de items en la escena
        CalculaItemsEnNivel();

        // Si el nivel o escena >=1 && se coleccionaron todos los ítems =>
        if (nivel >= 1 && items == itemsEnNivel)
        {
            // Cambia banderas (ganaste el nivel)
            ganasteNivel = true;

            // Pausa el juego
            PausarJuego();
        }
    }

    // Verifica escena actual
    private void VerificaEscenaActual()
    {
        // Si la escena = a la última => el juego terminó (ganaste)
        if (nivel == SceneManager.sceneCountInBuildSettings - 1)
        {
            // Cambia banderas (se termina el juego)
            gameOver = true;

            // Detener la Musica de fondo
            AudioManager.Instance.StopMusicaFondo();

            // Reproduce efecto de sonido al colisionar con el Obstaculo_01
            AudioManager.Instance.PlayEfectoSonoro(5);

            // Pausa el juego
            PausarJuego(); 
        }
    }

    private void VerificaEscape()
    {
        // Verificar si la tecla Escape es presionada
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                juegoPausado = false;
                Time.timeScale = 1f;
            }
            else
            {
                PausarJuego();
                
            }
        }
    }
    // Gestiona la pausa del jeugo
    public void PausarJuego()
    {
        // Detiene el tiempo
        Time.timeScale = 0f;
        
        //Debug.LogWarning("Juego pausado, Time.timeScale establecido a 0");
        
        // Cambia banderas (se pausa el juego)
        juegoPausado = true; 
    }

    // Gestiona el reinicio del juego
    public void ReiniciaJuego()
    {
        IniciaStart();
    }

    // Gestiona el inicio del menú incial
    public void IniciaMenu()
    {
        //
        IniciaStart();

        // Carga la primera escena del juego (escena 0)
        SceneManager.LoadScene(0);

        //Debug.Log("Cargando menú, Time.timeScale establecido a 1");
    }

    // Gestiona el reinicio del nivel cuando 
    public void ContinuaNivel()
    {
        // Asegura que el tiempo se reanuda al reiniciar el nivel
        Time.timeScale = 1f;

        juegoPausado = false;
        
        //Debug.Log("Nivel reiniciado, Time.timeScale establecido a 1");
    }

    // Gestiona la carga del siguiente nivel del juego
    public void SiguienteNivel()
    {
        // Llama al método que calcula y carga la siguiente escena
        IncrementaEscenaActual();

        // Reestablece los puntos - items - rebotes
        totalPuntos += puntos;
        puntos = 0;

        totalItems += items;
        items = 0;

        totalRebotes += rebotes;
        rebotes = 0;

        // Asegura que el tiempo se reanuda al pasar al siguiente nivel
        Time.timeScale = 1f;

        juegoPausado = false;
        ganasteNivel = false;

        //Debug.Log("Siguiente nivel cargado, Time.timeScale establecido a 1");
    }

    // Obtiene el puntaje actual
    public int ObtienePuntos()
    {
        return puntos;
    }

    // Agrega puntos
    public void AgregaPuntos(int _puntos)
    {
        puntos += _puntos;

    }

    // Quita puntos
    public void QuitaPuntos(int _puntos)
    {
        puntos -= _puntos;

    }

    // Agrega items coleccionados
    public void AgregaItems()
    {
        items ++;
    }

    // Agrega rebotes
    public void AgregaRebotes()
    {
        rebotes += 1;
    }

    // Obtiene la cantidad de rebotes
    public int ObtieneRebotes()
    {
        return rebotes;
    }

    // Calcula la cantidad de items por nivel
    public void CalculaItemsEnNivel()
    {
        // Obtiene el valor de la escena actual
        nivel = SceneManager.GetActiveScene().buildIndex;

        // Calcula la cantidad de items en el nivel
        itemsEnNivel = nivel * 2;
        
        //Debug.Log("nivelItems =" + itemsEnNivel);
    }

    // Incrementa el contador de noveles o escenas jugadas
    public void IncrementaEscenaActual()
    {
        // Obtiene el índice de la escena activa
        nivel = SceneManager.GetActiveScene().buildIndex;

        // Incrementa la escena o nivel actual a 1, y el operador módulo (%) = resto, asegura que si el nivel > índice de la última escena => vuelve a 0 (la primera escena)
        nivel = (nivel + 1) % SceneManager.sceneCountInBuildSettings;

        // Carga la nueva escena o nivel.
        SceneManager.LoadScene(nivel);

    }

    // Obtiene el número de nivel o escena actual
    public int ObtieneEscenaActual()
    {
        return nivel;
    }

    // Obtiene el valor de la bandera ganaste
    public bool ObtieneEstadoGanasteNivel()
    {
        return ganasteNivel;
    }

    // Gestiona el cambio de la bandera ganaste
    public void CambiaEstadoGanasteNivel(bool _ganasteNivel)
    {
        ganasteNivel = _ganasteNivel;
    }

    // Obtiene el valor de la bandrea gameOver
    public bool ObtieneEstadoGameOver()
    {
        return gameOver;
    }

    // Gestiona el cambio de la bandera gameOver
    public void CambiaEstadoGameOver(bool _gameOver)
    {
        gameOver = _gameOver;
    }

    // Obtiene el valor de la bandrea gameOver
    public bool ObtieneEstadoJuegoPausado()
    {
        return juegoPausado;
    }

    // Gestiona el cambio de la bandera gameOver
    public void CambiaEstadoJuegoPausado(bool _juegoPausado)
    {
        juegoPausado = _juegoPausado;
    }

    // Obtien total de puntos de todo el juego
    public int ObtieneTotalPuntos()
    {
        return totalPuntos;
    }

    // Obtien total de ítems coleccionados de todo el juego
    public int ObtieneTotalItems()
    {
        return totalItems;
    }

    // Obtiene el total de rebotes de todo el juego
    public int ObtieneTotalRebotes()
    {
        return totalRebotes;
    }
}
