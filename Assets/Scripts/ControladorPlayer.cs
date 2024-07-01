using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;


public class ControladorPlayer : MonoBehaviour
{
    [SerializeField] private float maxForce = 20f;        // Referencia a la variable fuerza máxima que el jugador puede acumular
    
    private float currentForce = 0f;        // Referencia a la variable fuerza actual
    private Vector2 targetPosition;         // Referencia a la variable posición actual del player
    private Rigidbody2D rb;                 // Referencia al rigidbody del player
    private bool isCharging = false;        // Referencia a la variable que controla la carga de la fuerza

    private int puntos;                 // Referencia a la variable que calcula la cantidad de puntos por colección de ítems
    private float reboteFactor = 1.0f;  // Referencia a la variable factor de rebote

    private ControladorObstaculos controladorObstaculos;        // Referencia al script ControladorObstaculos
    private ControladorItem controladorItem;                    // Referencia al script ControladorItem




    void Start()
    {
        // Inciializa el player
        rb = GetComponent<Rigidbody2D>();
        // Busca y encuentra los componentes
        controladorObstaculos = FindObjectOfType<ControladorObstaculos>();
        controladorItem = FindObjectOfType<ControladorItem>();
        //Debug.Log("Start() ejecutado");
    }

    void Update()
    {
        //Debug.Log("Update() ejecutado");
        //Debug.Log($"Time.timeScale: {Time.timeScale}");
        
        // Actualizar la posición del mouse
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Rota al player para que apunte hacia el mouse
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Ajusta el ángulo según la orientación del sprite
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); 

        // Si clic en la tecla Space => acumula fuerza cuando se mantiene presionada la tecla space
        if (Input.GetKey(KeyCode.Space))
        {
            //Debug.Log("clic space");
            isCharging = true;
            currentForce += Time.deltaTime * maxForce;
            //Debug.Log($"Time.deltaTime: {Time.deltaTime}");
            //Debug.Log($"Fuerza acumulada: {currentForce}");
            currentForce = Mathf.Clamp(currentForce, 0f, maxForce);
            //Debug.Log($"Fuerza acumulada: {currentForce}");
        }

        // Si suelta la tecla space para aplicar la fuerza => 
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //Debug.Log("suelta space");
            isCharging = false;
            // Asegura de que el player esté quieto antes de aplicar la nueva fuerza
            rb.velocity = Vector2.zero;
            // Detiene cualquier rotación existente
            rb.angularVelocity = 0f; 
            Vector2 launchDirection = (targetPosition - (Vector2)transform.position).normalized;
            rb.AddForce(launchDirection * currentForce, ForceMode2D.Impulse);
            //Debug.Log($"Dirección de lanzamiento: {launchDirection}, Fuerza aplicada: {currentForce}");
            // Resetea la fuerza acumulada
            currentForce = 0f; 
        }
    }

    void FixedUpdate()
    {
        // Si se está cargando fuerza => el jugador no debe moverse
        if (isCharging)
        {
            // Detiene cualquier movimiento
            rb.velocity = Vector2.zero;
            // Detiene cualquier rotación
            rb.angularVelocity = 0f; 
        }
    }

    // Gestiona las colisiones del player con otros
    void OnCollisionEnter2D(Collision2D collision)
    {
 
        // Si es con la pared =>
        if (collision.gameObject.CompareTag("Pared"))
        {
            // Calcula el factor de rebote
            reboteFactor = 1 + collision.collider.sharedMaterial.bounciness;
            // Llama al método que cuenta la cantida de rebotes
            GameManager.gameManager.AgregaRebotes();
            // Llama al método que calcula el rebote
            CalculaRebote(collision);
            // Reproduce efecto de sonido al colisionar con la pared
            AudioManager.Instance.PlayEfectoSonoro(3);
            //Debug.Log("Pared");
        }
        else if (collision.gameObject.CompareTag("Obstaculo"))
        {
            // Calcula el factor de rebote
            reboteFactor = controladorObstaculos.ObtieneRebotFactor();
            // Llama al método que cuenta la cantida de rebotes
            GameManager.gameManager.AgregaRebotes();
            // Llama al método que genera nuevos tiles
            controladorObstaculos.HandleTileCollision(collision);
            // Llama al método que calcula el rebote
            CalculaRebote(collision);
        }
        else if (collision.gameObject.CompareTag("Item_02"))
        {
            // Calcula los puntos según el item
            puntos = controladorItem.ObtienepuntosItem_02();
            // Llama al método que cuenta los puntos
            GameManager.gameManager.AgregaPuntos(puntos);
            // LLama al método que cuenta los ítems
            GameManager.gameManager.AgregaItems();
            // Establece el factor de rebote
            reboteFactor = 2.5f;
            // Llama al método que calcula el rebote
            CalculaRebote(collision);
            // Reproduce efecto de sonido al colisionar con el item_02
            AudioManager.Instance.PlayEfectoSonoro(4);
            // Desturye el item después de calcular el rebote
            Destroy(collision.gameObject); 

            //Debug.Log("Item");
        }
        else if (collision.gameObject.CompareTag("Item_03"))
        {
            // Calcula los puntos según el item
            puntos = controladorItem.ObtienepuntosItem_03();
            // Llama al método que cuenta los puntos
            GameManager.gameManager.AgregaPuntos(puntos);
            // LLama al método que cuenta los ítems
            GameManager.gameManager.AgregaItems();
            // Establece el factor de rebote
            reboteFactor = 3.8f;
            // Llama al método que calcula el rebote
            CalculaRebote(collision);
            // Reproduce efecto de sonido al colisionar con el item_03
            AudioManager.Instance.PlayEfectoSonoro(4);
            // Desturye el item después de calcular el rebote
            Destroy(collision.gameObject);

            //Debug.Log("Item");
        }

    }

    // Gestiona el cálculo del rebote
    private void CalculaRebote(Collision2D collision)
    {
        // Calcula la dirección de rebote
        //Vector2 normal = collision.contacts[0].normal;
        Vector2 normal = collision.GetContact(0).normal;
        Vector2 inDirection = rb.velocity.normalized;
        Vector2 reflectDirection = Vector2.Reflect(inDirection, normal);

        // Mantiene la magnitud de la velocidad después del rebote ajustada por el factor de rebote
        float speed = rb.velocity.magnitude * reboteFactor;

        //Debug.Log($"Rebote - Dirección: {reflectDirection}, Velocidad: {speed}");

        // Rota al player hacia la nueva dirección
        float angle = Mathf.Atan2(reflectDirection.y, reflectDirection.x) * Mathf.Rad2Deg;
        
        // Ajusta el ángulo según la orientación del sprite
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        // Aplica la fuerza de rebote
        rb.velocity = reflectDirection * speed;
    }
}
