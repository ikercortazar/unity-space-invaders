using UnityEngine;
using System.Collections;

public class ControlAlien : MonoBehaviour
{
	// Conexión al marcador, para poder actualizarlo
	public GameObject marcador;
    int sentido = 0;
    int contador = 0;
    private float velocidad = 2f;
   

    // Por defecto, 100 puntos por cada alien
    public int puntos = 100;

	// Use this for initialization
	void Start ()
	{
        // Localizamos el objeto que contiene el marcador
        marcador = GameObject.Find ("Marcador");
	}
	
	// Update is called once per frame
	void Update ()
	{
        // Calculamos la anchura visible de la cámara en pantalla

        float distanciaHorizontal = Camera.main.orthographicSize * Screen.width / Screen.height;
        //int sentido = 0;

    // Calculamos el límite izquierdo y el derecho de la pantalla
    float limiteIzq = -1.0f * distanciaHorizontal;
        float limiteDer = 1.0f * distanciaHorizontal;

        // Tecla: Izquierda
        if (sentido == 0)
        {

            // Nos movemos a la izquierda hasta llegar al límite para entrar por el otro lado
            if (transform.position.x > limiteIzq)
            {
                transform.Translate(Vector2.left * velocidad * Time.deltaTime);
            }
            else {
                contador++;
                if (contador == 7)
                {
                    transform.position = new Vector2(limiteIzq, transform.position.y - 4);
                    transform.Translate(Vector2.right * velocidad * Time.deltaTime);
                    contador = 0;
                    sentido = 1;
               }
            }
        }
        else {
            if (transform.position.x < limiteDer)
            {
                transform.Translate(Vector2.right * velocidad * Time.deltaTime);
            }
            else {
                contador++;
                if (contador == 7)
                {
                    transform.position = new Vector2(limiteDer, transform.position.y - 4);
                    transform.Translate(Vector2.left * velocidad * Time.deltaTime);
                    contador = 0;
                    sentido = 0;
                }
            }
        }
    }

    void OnCollisionEnter2D (Collision2D coll)
	{
		// Detectar la colisión entre el alien y otros elementos

		// Necesitamos saber contra qué hemos chocado
		if (coll.gameObject.tag == "disparo") {

			// Sonido de explosión
			GetComponent<AudioSource> ().Play ();

			// Sumar la puntuación al marcador
			marcador.GetComponent<ControlMarcador> ().puntos += puntos;

			// El disparo desaparece (cuidado, si tiene eventos no se ejecutan)
			Destroy (coll.gameObject);

			// El alien desaparece (hay que añadir un retraso, si no, no se oye la explosión)

			// Lo ocultamos...
			GetComponent<Renderer> ().enabled = false;
			GetComponent<Collider2D> ().enabled = false;

			// ... y lo destruímos al cabo de 5 segundos, para dar tiempo al efecto de sonido
			Destroy (gameObject, 5f);
		}
	}
}
