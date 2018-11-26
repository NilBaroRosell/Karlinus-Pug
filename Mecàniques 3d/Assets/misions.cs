using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class misions : MonoBehaviour {


    public enum Misions { M1, M2, M3, M4, SM_1 = 0, SM_2 = 0, SM_3 = 0, SM_4 = 1, SM_5 = 1, SM_6 = 1, NONE = 10 };
    public Misions ActualMision;
    public GameObject Jugador;
    private static misions Instance;
    [System.Serializable]
    public struct MisionPoint
    {
        public GameObject pointObject;
        public Vector3 playerDistance;
        public bool[] MisionsCompleted;
    }
    public MisionPoint PrincipalMision;
    public MisionPoint RatHood;
    public MisionPoint RebelCat;
    public MisionPoint ScaryDog;


    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyObject(gameObject);
        }
        MisionPoint PrincipalMision = new MisionPoint();
        PrincipalMision.MisionsCompleted = new bool[4];
        MisionPoint RatHood = new MisionPoint();
        RatHood.MisionsCompleted = new bool[2];
        MisionPoint RebelCat = new MisionPoint();
        RebelCat.MisionsCompleted = new bool[2];
        MisionPoint ScaryDog = new MisionPoint();
        ScaryDog.MisionsCompleted = new bool[2];
        hideMisionPoints();
    }
    // Use this for initialization
    void Start() {
        if (GameObject.Find("Jugador") != null)
            Jugador = GameObject.Find("Jugador");
        switch (ActualMision)
        {
            case Misions.NONE:
                showMisionPoints();
                break;
            case Misions.M2:
                break;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    void showMisionPoints()
    {
        if (PrincipalMision.MisionsCompleted[(int)Misions.M3] == false)
        {
            if (PrincipalMision.MisionsCompleted[(int)Misions.M2])
            {
                PrincipalMision.pointObject.SetActive(true);
                PrincipalMision.pointObject.transform.position = new Vector3(80.73f, -31.81266f, 189.97f) + transform.position;
            }
            else
            {
                {
                    PrincipalMision.pointObject.SetActive(true);
                    PrincipalMision.pointObject.transform.position = new Vector3(-14.1f, -31.81266f, -12.6f) + transform.position;
                }
            }
            if (RatHood.MisionsCompleted[(int)Misions.SM_1] == false)
            {
                RatHood.pointObject.SetActive(true);
                RatHood.pointObject.transform.position = new Vector3(13.17202f, -31.81266f, -18.64425f) + transform.position;
            }
            if (RebelCat.MisionsCompleted[(int)Misions.SM_2] == false)
            {
                RebelCat.pointObject.SetActive(true);
                RebelCat.pointObject.transform.position = new Vector3(-49.41798f, -31.81266f, 48.72575f) + transform.position;
            }
            if (ScaryDog.MisionsCompleted[(int)Misions.SM_3] == false)
            {
                ScaryDog.pointObject.SetActive(true);
                ScaryDog.pointObject.transform.position = new Vector3(-95.95798f, -31.81266f, 5.595755f) + transform.position;
            }
        }
        else
        {
            PrincipalMision.pointObject.SetActive(true);
            PrincipalMision.pointObject.transform.position = new Vector3(78.94f, -31.81266f, 275.4f) + transform.position;
            if (RatHood.MisionsCompleted[(int)Misions.SM_1] == false)
            {
                RatHood.pointObject.SetActive(true);
                RatHood.pointObject.transform.position = new Vector3(13.17202f, -31.81266f, -18.64425f) + transform.position;
            }
            else if (RatHood.MisionsCompleted[(int)Misions.SM_4] == false)
            {
                RatHood.pointObject.SetActive(true);
                RatHood.pointObject.transform.position = new Vector3(-14.7f, -31.81266f, 178.6f) + transform.position;
            }
            if (RebelCat.MisionsCompleted[(int)Misions.SM_2] == false)
            {
                RebelCat.pointObject.SetActive(true);
                RebelCat.pointObject.transform.position = new Vector3(-49.41798f, -31.81266f, 48.72575f) + transform.position;
            }
            else if (RebelCat.MisionsCompleted[(int)Misions.SM_5] == false)
            {
                RebelCat.pointObject.SetActive(true);
                RebelCat.pointObject.transform.position = new Vector3(-96.1f, -31.81266f, 133.5f) + transform.position;
            }
            if (ScaryDog.MisionsCompleted[(int)Misions.SM_3] == false)
            {
                ScaryDog.pointObject.SetActive(true);
                ScaryDog.pointObject.transform.position = new Vector3(-95.95798f, -31.81266f, 5.595755f) + transform.position;
            }
            else if (ScaryDog.MisionsCompleted[(int)Misions.SM_6] == false)
            {
                ScaryDog.pointObject.SetActive(true);
                ScaryDog.pointObject.transform.position = new Vector3(-18.41f, -31.81266f, 293.47f) + transform.position;
            }
        }
    }

    void hideMisionPoints()
    {
        RatHood.pointObject.SetActive(false);
        ScaryDog.pointObject.SetActive(false);
        RebelCat.pointObject.SetActive(false);
        PrincipalMision.pointObject.SetActive(false);
    }

    void OnLevelWasLoaded()
    {
        Start();
    }
}
