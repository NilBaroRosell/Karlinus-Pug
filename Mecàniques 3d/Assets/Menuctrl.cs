using UnityEngine;


public class Menuctrl : MonoBehaviour {

    void Start()
    {

    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        MainMenu.Instance.disableMainMenu();
        if (!misions.Instance.resetGameFile && MainMenu.Instance.state == MainMenu.states.MAIN_MENU)
        {
            MainMenu.Instance.state = MainMenu.states.TRAVELLING_TO_PLAYER;

            StartCoroutine(MainMenu.Instance.TravellingTime(5.0f));

            GameObject.Find("Jugador").GetComponent<Controller>().state = Controller.playerState.HITTING;
        }
        else
        {
            MainMenu.Instance.state = MainMenu.states.PLAYING;
            if (misions.pauseMenu)
            {
                misions.pauseMenu = false;
                MainMenu.Instance.showMenu = true;
                misions.Instance.ActualMision = misions.Misions.NONE;
            }
            loadScreen.Instancia.CargarEscena(sceneName);
        }
    }

    public void ExitGameBtn()
    {
        Application.Quit();
    }
}
