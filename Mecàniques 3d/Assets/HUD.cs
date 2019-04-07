using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SeenEnemy
{
    public GameObject enemy;
    public GameObject triangle;
    public SeenEnemy(GameObject _enemy, GameObject _triangle)
    {
        enemy = _enemy;
        triangle = _triangle;
    }
}

public class Mission
{
    public GameObject mission;
    public GameObject triangle;
    public Mission(GameObject _mission, GameObject _triangle)
    {
        mission = _mission;
        triangle = _triangle;
    }
}

public class HUD : MonoBehaviour
{

    public static float startTime;
    public static float finalTime;
    public static int timeUntilDisapear;
    public bool started;
    public GameObject Objective;
    private GameObject Objective_text;
    public GameObject Helps;
    private GameObject Helps_text;
    public GameObject Dialog;
    private GameObject Dialog_text;
    public GameObject M1;
    public GameObject M2;
    public GameObject M3;
    public GameObject M4;
    public GameObject SM_1;
    public List<SeenEnemy> enemiesSeen;
    public GameObject triangle;
    public Sprite RedT;
    public Sprite YellT;
    public Sprite OrangeT;
    public Sprite BlueT;
    public Sprite GreenT;
    public float DetectionDistance;
    private List<Mission> Missions;
    public GameObject missionSprite;
    public GameObject InteractSprite;
    private Sprite interact;
    public bool finalDialog = false;
    public GameObject NPC_Dialog;

    public static GameObject canvasHUD;
    //private const float objectiveY = 120.0f;
    //private const float HelpsY = 200.0f;
    private float objectiveY = Screen.height / 5;
    private float HelpsY = Screen.height / 4;
    private float DialogY = Screen.height / 4; //canviar valor(/4)
    public Vector3 DialogPos;

    // Use this for initialization
    void Awake()
    {
        Objective_text = Objective.transform.GetChild(0).gameObject;
        Helps_text = Helps.transform.GetChild(0).gameObject;
        Dialog_text = Dialog.transform.GetChild(0).gameObject;
        M1.SetActive(false);
        M2.SetActive(false);
        M3.SetActive(false);
        SM_1.SetActive(false);
        Objective.SetActive(false);
        Helps.SetActive(false);
        Dialog.SetActive(false);
        startTime = Time.frameCount;
        started = false;
        NPC_Dialog.SetActive(false);
    }

    private void Start()
    {
        canvasHUD = GameObject.Find("Canvas");
        enemiesSeen = new List<SeenEnemy>();
        Missions = new List<Mission>();
        MissionHUD();
        loadIntreractSprite();
    }

    // Update is called once per frame
    void Update()
    {
        finalDialog = false;
        finalTime = Time.frameCount;
        if (finalTime - startTime > timeUntilDisapear)
        {
            if (Objective.activeSelf && Objective.GetComponent<RectTransform>().position.y > -381.4f / 10) Objective.GetComponent<RectTransform>().position =
                    new Vector3(Objective.GetComponent<RectTransform>().position.x, Objective.GetComponent<RectTransform>().position.y - 40, Objective.GetComponent<RectTransform>().position.z);
            else Objective.SetActive(false);
            if (Helps.activeSelf && Helps.GetComponent<RectTransform>().position.y > -426.0f / 10) Helps.GetComponent<RectTransform>().position =
                    new Vector3(Helps.GetComponent<RectTransform>().position.x, Helps.GetComponent<RectTransform>().position.y - 40, Helps.GetComponent<RectTransform>().position.z);
            else Helps.SetActive(false);
            if (Dialog.activeSelf && Dialog.GetComponent<RectTransform>().position.y > -20000 / 10) Dialog.GetComponent<RectTransform>().position = //DialogPos;// canviar valors (-426.0f)
                      new Vector3(Dialog.GetComponent<RectTransform>().position.x, -20000/ 10, Dialog.GetComponent<RectTransform>().position.z); // canviar valors (40)
            else 
            {
                Dialog.SetActive(false);
                finalDialog = true;
            }
        }
        else
        {
            objectiveY = Screen.height / 4;
            HelpsY = Screen.height / 2.25f;
            DialogY = Screen.height / 3.125f;
            if (Objective.activeSelf && Objective.GetComponent<RectTransform>().position.y < objectiveY) Objective.GetComponent<RectTransform>().position =
                    new Vector3(Objective.GetComponent<RectTransform>().position.x, Objective.GetComponent<RectTransform>().position.y + 40, Objective.GetComponent<RectTransform>().position.z);
            if (Helps.activeSelf && Helps.GetComponent<RectTransform>().position.y < HelpsY) Helps.GetComponent<RectTransform>().position =
                    new Vector3(Helps.GetComponent<RectTransform>().position.x, Helps.GetComponent<RectTransform>().position.y + 40, Helps.GetComponent<RectTransform>().position.z);
            if (Dialog.activeSelf && Dialog.GetComponent<RectTransform>().position.y < DialogY) Dialog.GetComponent<RectTransform>().position = //new Vector3(DialogPos.x, //DialogPos.y + 40, DialogPos.z);
                     new Vector3(Dialog.GetComponent<RectTransform>().position.x, Dialog.GetComponent<RectTransform>().position.y + 40, Dialog.GetComponent<RectTransform>().position.z);
        }
        if (MainMenu.Instance == null || MainMenu.Instance.state == MainMenu.states.PLAYING)
        {
            ShowEnemies();
            ShowMission();
        }
    }

    public void showM1Objective(int text_to_show, int font_to_set = 50)
    {
        M1.SetActive(true);
        Objective.SetActive(true);
        if (Objective_text.GetComponent<Text>().text != M1.transform.GetChild(0).GetChild(text_to_show).gameObject.GetComponent<Text>().text)
        {
            Objective_text.GetComponent<Text>().text = M1.transform.GetChild(0).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
            Objective_text.GetComponent<Text>().fontSize = font_to_set;
            Objective.GetComponent<RectTransform>().position =
            new Vector3(Objective.GetComponent<RectTransform>().position.x, -381.4f / 10, Objective.GetComponent<RectTransform>().position.z);
            startTime = Time.frameCount;
            timeUntilDisapear = 300;
        }
        M1.SetActive(false);
    }
    public void showM1Helps(int text_to_show, int font_to_set = 50)
    {
        M1.SetActive(true);
        Helps.SetActive(true);
        if (Helps_text.GetComponent<Text>().text != M1.transform.GetChild(1).GetChild(text_to_show).gameObject.GetComponent<Text>().text)
        {
            Helps_text.GetComponent<Text>().text = M1.transform.GetChild(1).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
            Helps_text.GetComponent<Text>().fontSize = font_to_set;
            Helps.GetComponent<RectTransform>().position =
                        new Vector3(Helps.GetComponent<RectTransform>().position.x, -426.0f / 10, Helps.GetComponent<RectTransform>().position.z);
            startTime = Time.frameCount;
            timeUntilDisapear = 300;
        }
        M1.SetActive(false);
    }

    public void showM2Objective(int text_to_show, int font_to_set = 50)
    {
        M2.SetActive(true);
        Objective.SetActive(true);
        if (Objective_text.GetComponent<Text>().text != M2.transform.GetChild(0).GetChild(text_to_show).gameObject.GetComponent<Text>().text)
        {
            Objective_text.GetComponent<Text>().text = M2.transform.GetChild(0).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
            Objective_text.GetComponent<Text>().fontSize = font_to_set;
            Objective.GetComponent<RectTransform>().position =
                new Vector3(Objective.GetComponent<RectTransform>().position.x, -381.4f / 10, Objective.GetComponent<RectTransform>().position.z);
            startTime = Time.frameCount;
            timeUntilDisapear = 300;
        }
        M2.SetActive(false);
    }
    public void showM2Helps(int text_to_show, int font_to_set = 50)
    {
        M2.SetActive(true);
        Helps.SetActive(true);
        if (Helps_text.GetComponent<Text>().text != M2.transform.GetChild(1).GetChild(text_to_show).gameObject.GetComponent<Text>().text)
        {
            Helps_text.GetComponent<Text>().text = M2.transform.GetChild(1).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
            Helps_text.GetComponent<Text>().fontSize = font_to_set;
            Helps.GetComponent<RectTransform>().position =
                        new Vector3(Helps.GetComponent<RectTransform>().position.x, -426.0f / 10, Helps.GetComponent<RectTransform>().position.z);
            startTime = Time.frameCount;
            timeUntilDisapear = 300;
        }
        M2.SetActive(false);
    }

    public void showM3Objective(int text_to_show, int font_to_set = 50)
    {
        M3.SetActive(true);
        Objective.SetActive(true);
        if (Objective_text.GetComponent<Text>().text != M3.transform.GetChild(0).GetChild(text_to_show).gameObject.GetComponent<Text>().text)
        {
            Objective_text.GetComponent<Text>().text = M3.transform.GetChild(0).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
            Objective_text.GetComponent<Text>().fontSize = font_to_set;
            Objective.GetComponent<RectTransform>().position =
                new Vector3(Objective.GetComponent<RectTransform>().position.x, -381.4f / 10, Objective.GetComponent<RectTransform>().position.z);
            startTime = Time.frameCount;
            timeUntilDisapear = 300;
        }
        M3.SetActive(false);
    }
    public void showM3Helps(int text_to_show, int font_to_set = 50)
    {
        M3.SetActive(true);
        Helps.SetActive(true);
        if (Helps_text.GetComponent<Text>().text != M3.transform.GetChild(1).GetChild(text_to_show).gameObject.GetComponent<Text>().text)
        {
            Helps_text.GetComponent<Text>().text = M3.transform.GetChild(1).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
            Helps_text.GetComponent<Text>().fontSize = font_to_set;
            Helps.GetComponent<RectTransform>().position =
                        new Vector3(Helps.GetComponent<RectTransform>().position.x, -426.0f / 10, Helps.GetComponent<RectTransform>().position.z);
            startTime = Time.frameCount;
            timeUntilDisapear = 300;
        }
        M3.SetActive(false);
    }

    public void showM4Objective(int text_to_show, int font_to_set = 50)
    {
        M4.SetActive(true);
        Objective.SetActive(true);
        if (Objective_text.GetComponent<Text>().text != M4.transform.GetChild(0).GetChild(text_to_show).gameObject.GetComponent<Text>().text)
        {
            Objective_text.GetComponent<Text>().text = M4.transform.GetChild(0).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
            Objective_text.GetComponent<Text>().fontSize = font_to_set;
            Objective.GetComponent<RectTransform>().position =
                new Vector3(Objective.GetComponent<RectTransform>().position.x, -381.4f / 10, Objective.GetComponent<RectTransform>().position.z);
            startTime = Time.frameCount;
            timeUntilDisapear = 300;
        }
        M4.SetActive(false);
    }
    public void showM4Helps(int text_to_show, int font_to_set = 50)
    {
        M4.SetActive(true);
        Helps.SetActive(true);
        if (Helps_text.GetComponent<Text>().text != M4.transform.GetChild(1).GetChild(text_to_show).gameObject.GetComponent<Text>().text)
        {
            Helps_text.GetComponent<Text>().text = M4.transform.GetChild(1).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
            Helps_text.GetComponent<Text>().fontSize = font_to_set;
            Helps.GetComponent<RectTransform>().position =
                        new Vector3(Helps.GetComponent<RectTransform>().position.x, -426.0f / 10, Helps.GetComponent<RectTransform>().position.z);
            startTime = Time.frameCount;
            timeUntilDisapear = 300;
        }
        M4.SetActive(false);
    }

    public void showZoneWarning()
    {
        if (!Helps.activeSelf)
        {
            Helps.SetActive(true);
            Helps_text.GetComponent<Text>().text = "Warning, you're moving away from the mission area";
            Helps_text.GetComponent<Text>().fontSize = 50;
            Helps.GetComponent<RectTransform>().position =
                        new Vector3(Helps.GetComponent<RectTransform>().position.x, -426.0f / 10, Helps.GetComponent<RectTransform>().position.z);
            startTime = Time.frameCount;
            timeUntilDisapear = 300;
        }
    }

    public void showSM_1Objective(int text_to_show)
    {
        SM_1.SetActive(true);
        Objective.SetActive(true);
        Objective_text.GetComponent<Text>().text = SM_1.transform.GetChild(0).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
        SM_1.SetActive(false);
        Objective.GetComponent<RectTransform>().position =
            new Vector3(Objective.GetComponent<RectTransform>().position.x, -381.4f / 10, Objective.GetComponent<RectTransform>().position.z);
        startTime = Time.frameCount;
        timeUntilDisapear = 300;
    }
    public void showSM_1Helps(int text_to_show, int font_to_set)
    {
        SM_1.SetActive(true);
        Helps.SetActive(true);
        Helps_text.GetComponent<Text>().text = SM_1.transform.GetChild(1).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
        Helps_text.GetComponent<Text>().fontSize = font_to_set;
        SM_1.SetActive(false);
        Helps.GetComponent<RectTransform>().position =
                    new Vector3(Helps.GetComponent<RectTransform>().position.x, -426.0f / 10, Helps.GetComponent<RectTransform>().position.z);
        startTime = Time.frameCount;
        timeUntilDisapear = 300;
    }
    public void showSM_1Dialog(int text_to_show, int font_to_set, float posX, float posY, int time)
    {
        SM_1.SetActive(true);
        Dialog.SetActive(true);
        Dialog_text.GetComponent<Text>().text = SM_1.transform.GetChild(2).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
        Dialog_text.GetComponent<Text>().fontSize = font_to_set;
        SM_1.SetActive(false);
        DialogPos = new Vector3(posX, posY, Dialog.GetComponent<RectTransform>().position.z);
        Dialog.GetComponent<RectTransform>().position = DialogPos;
        //new Vector3(Dialog.GetComponent<RectTransform>().position.x, -403.0f / 10, Dialog.GetComponent<RectTransform>().position.z); // canviar valors (-426.0f)
        startTime = Time.frameCount;
        timeUntilDisapear = time;
    }

    public void rememberObjective()
    {
        if (!Objective.activeSelf && misions.Instance.ActualMision != misions.Misions.NONE)
        {
            Objective.SetActive(true);
            Objective.GetComponent<RectTransform>().position =
                new Vector3(Objective.GetComponent<RectTransform>().position.x, -381.4f / 10, Objective.GetComponent<RectTransform>().position.z);
            startTime = Time.frameCount;
            timeUntilDisapear = 300;
        }
    }

    public void MissionCompleted()
    {
            Objective.SetActive(true);
        Objective_text.GetComponent<Text>().text = "MISSION COMPLETED";
        Objective_text.GetComponent<Text>().fontSize = 50;
        Objective.GetComponent<RectTransform>().position =
                new Vector3(Objective.GetComponent<RectTransform>().position.x, -381.4f / 10, Objective.GetComponent<RectTransform>().position.z);
            startTime = Time.frameCount;
            timeUntilDisapear = 300;
    }

    public void showNpcDialog(int scene, int text_to_show, int font_to_set = 50)
    {
        NPC_Dialog.SetActive(true);
        Dialog.SetActive(true);
        Dialog_text.GetComponent<Text>().text = NPC_Dialog.transform.GetChild(scene).GetChild(text_to_show).gameObject.GetComponent<Text>().text;
        Dialog_text.GetComponent<Text>().fontSize = font_to_set;
        NPC_Dialog.SetActive(false);
        DialogPos = new Vector3((Screen.width * 12) / 20, (Screen.height * 17) / 20, Dialog.GetComponent<RectTransform>().position.z);
        Dialog.GetComponent<RectTransform>().position = DialogPos;
        startTime = Time.frameCount;
        timeUntilDisapear = 200;
    }

    public void IntroduceEnemy(GameObject enemy)
    {
        bool found = false;
        for (int i = 0; enemiesSeen.Count > i; i++)
        {
            if (enemiesSeen[i].enemy == enemy) found = true;
        }
        if (!found)
        {
            enemiesSeen.Add(new SeenEnemy(enemy, Instantiate(triangle, new Vector3(0, 0, 0), Quaternion.identity) as GameObject));
            enemiesSeen[enemiesSeen.Count - 1].triangle.transform.SetParent(GameObject.FindGameObjectWithTag("canvas").transform, false);
            enemiesSeen[enemiesSeen.Count - 1].triangle.transform.position = Camera.main.WorldToScreenPoint(enemy.transform.GetChild(3).transform.position);
        }

    }

    private void ShowEnemies()
    {
        if (enemiesSeen != null)
        {
            for (int i = 0; enemiesSeen.Count > i; i++)
            {
                if (Vector3.Angle(Camera.main.transform.forward, enemiesSeen[i].enemy.transform.position - gameObject.transform.position) < 90)
                {
                    enemiesSeen[i].triangle.transform.position = Camera.main.WorldToScreenPoint(enemiesSeen[i].enemy.transform.GetChild(3).transform.position);
                    UpdateState(enemiesSeen[i]);
                    UpdateAlfa(enemiesSeen[i]);
                }
                else
                {
                    Color tempColor = enemiesSeen[i].triangle.GetComponent<Image>().color;
                    tempColor.a = 0.0f;
                    enemiesSeen[i].triangle.GetComponent<Image>().color = tempColor;
                }
            }
        }
    }
    private void UpdateState(SeenEnemy enemy)
    {
        switch (enemy.enemy.GetComponent<csAreaVision>().actualState)
        {
            case csAreaVision.enemyState.PATROLLING:
                enemy.triangle.GetComponent<Image>().sprite = GreenT;
                break;
            case csAreaVision.enemyState.DETECTING:
                enemy.triangle.GetComponent<Image>().sprite = OrangeT;
                break;
            case csAreaVision.enemyState.FIGHTING:
                enemy.triangle.GetComponent<Image>().sprite = RedT;
                break;
            case csAreaVision.enemyState.SEARCHING:
                enemy.triangle.GetComponent<Image>().sprite = YellT;
                break;
            case csAreaVision.enemyState.LEAVING:
                enemy.triangle.GetComponent<Image>().sprite = BlueT;
                break;
            default:
                enemy.triangle.GetComponent<Image>().sprite = null;
                break;
        }
    }
    private void UpdateAlfa(SeenEnemy enemy)
    {
        float lerpValue = Mathf.Lerp(1, 0, Mathf.InverseLerp(0, DetectionDistance, Vector3.Distance(transform.position, enemy.enemy.transform.position)));
        Color tempColor = enemy.triangle.GetComponent<Image>().color;
        tempColor.a = lerpValue;
        enemy.triangle.GetComponent<Image>().color = tempColor;
    }

    private void UpdateAlfa(Mission missionPoint)
    {
        float lerpValue = Mathf.Lerp(1, 0, Mathf.InverseLerp(0, DetectionDistance, Vector3.Distance(transform.position, missionPoint.mission.transform.position)));
        Color tempColor = missionPoint.triangle.GetComponent<Image>().color;
        tempColor.a = lerpValue;
        missionPoint.triangle.GetComponent<Image>().color = tempColor;
    }

    private void MissionHUD()
    {
        if (misions.Instance.ActualMision == misions.Misions.NONE && SceneManager.GetActiveScene().name == "city")
        {
            Missions.Add(new Mission((misions.Instance.PrincipalMision.pointObject), Instantiate(missionSprite, new Vector3(0, 0, 0), Quaternion.identity) as GameObject));
            Missions[Missions.Count - 1].triangle.transform.SetParent(GameObject.FindGameObjectWithTag("canvas").transform, false);
            Missions[Missions.Count - 1].triangle.transform.position = Camera.main.WorldToScreenPoint(misions.Instance.PrincipalMision.pointObject.transform.position);
        }
    }

    private void ShowMission()
    {
        for (int i = 0; Missions.Count > i; i++)
        {
            if (Vector3.Angle(Camera.main.transform.forward, misions.Instance.PrincipalMision.pointObject.transform.position - gameObject.transform.position) < 90)
            {
                UpdateAlfa(Missions[i]);
                Missions[i].triangle.transform.position = Camera.main.WorldToScreenPoint(Missions[i].mission.transform.position);
            }
            else
            {
                Color tempColor = Missions[i].triangle.GetComponent<Image>().color;
                tempColor.a = 0.0f;
                Missions[i].triangle.GetComponent<Image>().color = tempColor;
            }
        }
    }

    private void loadIntreractSprite()
    {
        InteractSprite = Instantiate(InteractSprite, new Vector3(0, 0, 0), Quaternion.identity);
       InteractSprite.transform.SetParent(GameObject.FindGameObjectWithTag("canvas").transform, false);
        interact = Resources.Load<Sprite>("Sprites/Default");
        InteractSprite.GetComponent<Image>().sprite = interact;
        Color tempColor = InteractSprite.GetComponent<Image>().color;
        tempColor.a = 0.0f;
        InteractSprite.GetComponent<Image>().color = tempColor;
    }

    public void showInteractSprite(Vector3 worldPoint, Sprite _sprite)
    {
        if (Vector3.Angle(Camera.main.transform.forward, worldPoint - gameObject.transform.position) < 90)
        {
            InteractSprite.GetComponent<Image>().sprite = _sprite;
            Color tempColor = InteractSprite.GetComponent<Image>().color;
            tempColor.a = 1.0f;
            InteractSprite.GetComponent<Image>().color = tempColor;
            InteractSprite.transform.position = Camera.main.WorldToScreenPoint(worldPoint);
        }
        else
        {
            Color tempColor = InteractSprite.GetComponent<Image>().color;
            tempColor.a = 0.0f;
            InteractSprite.GetComponent<Image>().color = tempColor;
        }      
    }
    public void hideInteractSprite()
    {
        InteractSprite.GetComponent<Image>().sprite = interact;
        Color tempColor = InteractSprite.GetComponent<Image>().color;
        tempColor.a = 0.0f;
        InteractSprite.GetComponent<Image>().color = tempColor;
    }
}
