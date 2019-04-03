using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadScreen : MonoBehaviour
{

    public static loadScreen Instancia { get; private set; }

    public Image imageDeCarga;
    public Image imageDeCarga2;
    public Image imageDeCarga3;
    public Image[] pisadas;
    public Image pantallaFinal;
    [Range(0.01f, 0.1f)]
    public float velocidadAparecer = 0.5f;
    [Range(0.01f, 0.1f)]
    public float velocidadOcultar = 0.5f;

    void Awake()
    {
        DefinirSingleton();
    }

    private void DefinirSingleton()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(this);
            imageDeCarga.gameObject.SetActive(false);
            imageDeCarga2.gameObject.SetActive(false);
            imageDeCarga3.gameObject.SetActive(false);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CargarEscena(string nombreEscena)
    {
        if(!imageDeCarga.gameObject.activeSelf) StartCoroutine(MostrarPantallaDeCarga(nombreEscena));
    }


    private IEnumerator MostrarPantallaDeCarga(string nombreEscena)
    {
        imageDeCarga.gameObject.SetActive(true);
        imageDeCarga2.gameObject.SetActive(true);
        imageDeCarga3.gameObject.SetActive(true);
        Color c = imageDeCarga.color;
        c.a = 0.0f;
        //Mientras no esté totalmente visible va aumentando su visibilidad
        while (c.a < 1)
        {
            imageDeCarga.color = c;
            c.a += velocidadAparecer;
            yield return null;
        }
        imageDeCarga3.color = c;
        c.a = 1.0f;
        c = imageDeCarga2.color;
        c.a = 0.0f;
        while (c.a < 1)
        {
            imageDeCarga2.color = c;
            c.a += velocidadAparecer;
            yield return null;
        }

        //Carga la escena
        //StartCoroutine(loadScreenAnimation(nombreEscena));
        AsyncOperation operation = SceneManager.LoadSceneAsync(nombreEscena);
        float sumAlphaValue = -0.01f;
        c = imageDeCarga2.color;
        c.a = 1.0f;
        int indx = 0;
        while (!operation.isDone)
        {
            
            if (sumAlphaValue == -0.01f)
            {
                c.a += sumAlphaValue;
                if (c.a <= 0.5f)
                {
                    sumAlphaValue = 0.01f;
                    c.a = 0.5f;
                    if (indx < pisadas.Length)
                    {
                        Debug.Log(operation.progress);
                        Color aux = pisadas[indx].color;
                        aux.a = 1.0f;
                        pisadas[indx].color = aux;
                        pisadas[indx].gameObject.SetActive(true);
                        indx++;
                    }
                    yield return null;
                }
            }
            if (sumAlphaValue == 0.01f)
            {
                c.a += sumAlphaValue;
                if (c.a >= 1.0f)
                {
                    c.a = 1.0f;
                    sumAlphaValue = -0.01f;
                    if (indx < pisadas.Length)
                    {
                        Debug.Log(operation.progress);
                        Color aux = pisadas[indx].color;
                        aux.a = 1.0f;
                        pisadas[indx].color = aux;
                        pisadas[indx].gameObject.SetActive(true);
                        indx++;
                    }
                    yield return null;

                }

            }

        }
        //Espera a que haya cargado la nueva escena
        while (!nombreEscena.Equals(SceneManager.GetActiveScene().name))
        {
            
            yield return null;
        }
        //Mientras la imagen de carga siga visible va desvaneciéndola
        for (int i = 0; i < pisadas.Length; i++)
        {
            Color aux = pisadas[i].color;
            aux.a = 0.0f;
            pisadas[i].color = aux;
            pisadas[i].gameObject.SetActive(false);
        }
        c = imageDeCarga3.color;
        c.a = 0.0f;
        imageDeCarga3.color = c;
        imageDeCarga3.gameObject.SetActive(false);
        pantallaFinal.gameObject.SetActive(true);
        c = pantallaFinal.color;
        c.a = 1.0f;
        while (c.a > 0)
        {
            pantallaFinal.color = c;
            c.a -= velocidadOcultar;
            yield return null;
        }
        c = imageDeCarga2.color;
        c.a = 1.0f;
        while (c.a > 0)
        {
            imageDeCarga2.color = c;
            c.a -= velocidadOcultar;
            yield return null;
        }
        c = imageDeCarga.color;
        c.a = 1.0f;
        while (c.a > 0)
        {
            imageDeCarga.color = c;
            c.a -= velocidadOcultar * 2;
            yield return null;
        }
        imageDeCarga.gameObject.SetActive(false);
        imageDeCarga2.gameObject.SetActive(false);
        pantallaFinal.gameObject.SetActive(false);
    }
}