using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType   //스킬 상품 정보
{
    Skill_0 = 0,
    Skill_1,
    Skill_2,
    Skill_3,
    Skill_4,
    Skill_5,
    SkCount
}

public class Game_Mgr : MonoBehaviour
{
    public Button GoLobbyBtn;
    public Text m_BestScoreTxt = null;  //최고점수 표시 UI
    public Text m_CurScoreTxt  = null;  //현재점수 표시 UI
    public Text m_UserGoldTxt  = null;  //보유골드 표시 UI
    public Text m_UserInfo_Text = null; //유저 정보 표시 UI

    int m_CurScore = 0; //이번 스테이지에서 얻은 게임점수
    int m_CurGold  = 0; //이번 스테이지에서 얻은 골드값

    //------ 캐릭터 머리위에 데미지 띄우기용 변수 선언
    GameObject  a_DmgClone;
    DamageTxt   a_DmgTxt;
    Vector3     a_StCacPos;
    [Header("--------- DamageText ---------")]
    public Transform m_HUD_Canvas  = null;
    public GameObject m_DamageRoot = null;
    //------ 캐릭터 머리위에 데미지 띄우기용 변수 선언

    //------ 환경설정 Dlg 관련 변수
    [Header("--------- ConfigBox ---------")]
    public Button m_CfgBtn;
    public GameObject m_CfgBoxObj;
    //------ 환경설정 Dlg 관련 변수

    [Header("--------- GameOver ---------")]
    public GameObject ResultPanel = null;
    public Text Result_Txt   = null;
    public Button Replay_Btn = null;
    public Button RstLobby_Btn = null;

    [Header("-------- ExpLevel --------")]
    public Text ExpLevel_Txt = null;
    int m_KillCount = 0;

    //------ Inventory Show OnOff
    [Header("-------- Inventory Show OnOff --------")]
    public Button m_Inven_Btn = null;
    public Transform m_InvenScrollTr = null;
    bool  m_Inven_ScOnOff = false;
    float m_ScSpeed = 9000.0f;
    Vector3 m_ScOnPos   = new Vector3(0.0f, 0.0f, 0.0f);
    Vector3 m_ScOffPos  = new Vector3(-1000.0f, 0.0f, 0.0f);
    Vector3 m_BtnOnPos  = new Vector3(410.0f, -247.8f, 0.0f);
    Vector3 m_BtnOffPos = new Vector3(-569.6f, -247.8f, 0.0f);

    public Transform m_ScContent;
    public GameObject m_SkSmallPrefab;
    //------ Inventory Show OnOff

    SkSmallNode[] m_SkSmallList = null;     //인벤토리 목록

    [Header("-------- Skill Timer --------")]
    public GameObject m_SkCoolNode = null;
    public Transform  m_SkillTimeTr = null;

    //------ 전역변수 모음
    public static string g_NickName = "";   //유저의 별명
    public static int g_BestScore = 0;      //최고 점수
    public static int g_UserGold = 0;       //유저의 보유골드
    public static int g_Exp = 0;            //경험치   Experience
    public static int g_Level = 0;          //레벨
    //------ 전역변수 모음

    //--- 싱글턴 패턴
    public static Game_Mgr Inst = null;

    void Awake()
    {
        Inst = this;
    }
    //--- 싱글턴 패턴

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;  //인게임으로 진입하면서 정상속도로 돌려놓기...

        LoadGameData();
        RefreshUI();

        if (m_CfgBtn != null)
            m_CfgBtn.onClick.AddListener(ConfigBoxClick);

        if (GoLobbyBtn != null)
            GoLobbyBtn.onClick.AddListener(GoLobbyBtnClick);

        if (Replay_Btn != null)
            Replay_Btn.onClick.AddListener(() => 
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("InGame");
            });

        if (RstLobby_Btn != null)
            RstLobby_Btn.onClick.AddListener(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
            });

        if(m_Inven_Btn != null)
        {
            m_Inven_Btn.onClick.AddListener(() =>
            {
                m_Inven_ScOnOff = !m_Inven_ScOnOff;
            });
        }//if(m_Inven_Btn != null)

        if(m_ScContent != null)
            m_SkSmallList = m_ScContent.GetComponentsInChildren<SkSmallNode>();

        if(m_SkSmallList != null)
        for(int ii = 0; ii < m_SkSmallList.Length; ii++)
        {
            m_SkSmallList[ii].InitState((SkillType)ii);
        }

    } //void Start()

    // Update is called once per frame
    void Update()
    {
        //--- 단축키 이용으로 스킬 사용하기...
        if(Input.GetKeyDown(KeyCode.Alpha1) ||
           Input.GetKeyDown(KeyCode.Keypad1) )
        {
            UseSkill_Key(SkillType.Skill_0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) ||
                 Input.GetKeyDown(KeyCode.Keypad2))
        {
            UseSkill_Key(SkillType.Skill_1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) ||
                 Input.GetKeyDown(KeyCode.Keypad3))
        {
            UseSkill_Key(SkillType.Skill_2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) ||
                 Input.GetKeyDown(KeyCode.Keypad4))
        {
            UseSkill_Key(SkillType.Skill_3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) ||
                Input.GetKeyDown(KeyCode.Keypad5))
        {
            UseSkill_Key(SkillType.Skill_4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) ||
                Input.GetKeyDown(KeyCode.Keypad6))
        {
            UseSkill_Key(SkillType.Skill_5);
        }
        //--- 단축키 이용으로 스킬 사용하기...

        if (Input.GetKeyDown(KeyCode.K) == true) //치트키
        { //데이터 초기화
            m_CurScore = 0; //이번 스테이지에서 얻은 게임점수
            m_CurGold = 0;  //이번 스테이지에서 얻은 골드값
            PlayerPrefs.DeleteAll(); //로컬에 저장되어 있던 값 모두 제거
            LoadGameData();
            RefreshUI();
        }//if(Input.GetKeyDown(KeyCode.K) == true)

        ScrollViewOnOff_Update();

    }//void Update()

    void GoLobbyBtnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
    }

    public void DamageTxt(float a_Value, Vector3 a_Pos, Color a_Color)
    {
        if (m_DamageRoot == null || m_HUD_Canvas == null)
            return;

        a_DmgClone = (GameObject)Instantiate(m_DamageRoot);
        a_DmgClone.transform.SetParent(m_HUD_Canvas);
        a_DmgTxt = a_DmgClone.GetComponent<DamageTxt>();
        if (a_DmgTxt != null)
            a_DmgTxt.InitDamage(a_Value, a_Color);
        a_StCacPos = new Vector3(a_Pos.x, a_Pos.y + 1.15f, 0.0f);
        a_DmgClone.transform.position = a_StCacPos;
    }

    public void AddScore(int Value = 10)
    {
        m_CurScore += Value;
        if (g_BestScore < m_CurScore)
        {
            g_BestScore = m_CurScore;
            m_BestScoreTxt.text = "최고점수(" + g_BestScore + ")";
            PlayerPrefs.SetInt("BestScore", g_BestScore);
        }

        m_CurScoreTxt.text = "현재점수(" + m_CurScore + ")";
    }

    public void AddGold(int Value = 10)
    {
        m_CurGold += Value;
        g_UserGold += Value;

        int a_MaxValue = int.MaxValue - 10;
        if (a_MaxValue < g_UserGold)
            g_UserGold = a_MaxValue;

        m_UserGoldTxt.text = "보유골드(" + g_UserGold + ")";
        PlayerPrefs.SetInt("UserGold", g_UserGold);
    }

    void ConfigBoxClick()
    {
        if (m_CfgBoxObj == null)
            return;

        m_CfgBoxObj.SetActive(true);
        Time.timeScale = 0.0f;
    }

    void RefreshUI()
    {
        if(m_BestScoreTxt != null)
            m_BestScoreTxt.text = "최고점수(" + g_BestScore + ")";
        if (m_CurScoreTxt != null)
            m_CurScoreTxt.text = "현재점수(" + m_CurScore + ")";
        if (m_UserGoldTxt != null)
            m_UserGoldTxt.text = "보유골드(" + g_UserGold + ")";
        if (m_UserInfo_Text != null)
            m_UserInfo_Text.text = "내정보 : 별명(" + g_NickName + ")";

        RefreshExpLevel();
    }

    public static void LoadGameData()
    {
        g_NickName  = PlayerPrefs.GetString("NickName", "중급전사");
        g_BestScore = PlayerPrefs.GetInt("BestScore", 0);
        g_UserGold  = PlayerPrefs.GetInt("UserGold", 0);
        g_Exp       = PlayerPrefs.GetInt("UserExp", 0);
    }

    public void GameOverFunc()
    {
        ResultPanel.SetActive(true);

        Result_Txt.text = "NickName\n" + g_NickName + "\n\n" +
                          "획득 점수\n" + m_CurScore + "\n\n" +
                          "획득 골드\n" + m_CurGold;
    }

    public void AddExpLevel()
    {
        m_KillCount++;
        if(10 < m_KillCount)
        {
            g_Exp++;    //경험치 Experience
            PlayerPrefs.SetInt("UserExp", g_Exp);
            RefreshExpLevel();

            m_KillCount = 0;
        }
    } //public void AddExpLevel()

    public void RefreshExpLevel()
    {
        int a_CurLv = (int)Mathf.Sqrt((float)g_Exp);    //루트(근 √)
        int a_CurExp = 0;
        int a_TargetExp = 1;
        if(a_CurLv <= 0)  //a_CurLv = 0 일때
        {
            a_CurLv = 0;
        }
        else
        {
            int a_BaseExp = (int)Mathf.Pow(a_CurLv, 2); //제곱
            int a_NextExp = (int)Mathf.Pow((a_CurLv + 1), 2);
            a_CurExp = g_Exp - a_BaseExp;
            a_TargetExp = a_NextExp - a_BaseExp;
        }

        if (ExpLevel_Txt == null)
            return;

        ExpLevel_Txt.text = "경험치(" + a_CurExp + " / " + a_TargetExp + ") " +
                            "레벨(" + a_CurLv + ")";

    }//public void RefreshExpLevel()

    void UseSkill_Key(SkillType a_SkType)
    {
        if (m_SkSmallList == null)
            return;

        if (m_SkSmallList[(int)a_SkType].m_CurSkCount <= 0)
        {  //스킬 소진으로 사용할 수 없음
            return;
        }

        HeroCtrl a_Hero = GameObject.FindObjectOfType<HeroCtrl>();
        if (a_Hero != null)
        {
            bool a_IsOk = a_Hero.UseSkill(a_SkType);
            if (a_IsOk == true)
                m_SkSmallList[(int)a_SkType].m_CurSkCount--;
        }

        m_SkSmallList[(int)a_SkType].Refresh_UI(a_SkType); //UI 갱신
    }

    void ScrollViewOnOff_Update()
    {
        if (m_InvenScrollTr == null)
            return;

        if(Input.GetKeyDown(KeyCode.R))
        {
            m_Inven_ScOnOff = !m_Inven_ScOnOff;
        }

        //------- Menu Scroll 연출
        if(m_Inven_ScOnOff == false)
        {
            if(m_InvenScrollTr.localPosition.x > m_ScOffPos.x)
            {
                m_InvenScrollTr.localPosition =
                    Vector3.MoveTowards(m_InvenScrollTr.localPosition,
                                m_ScOffPos, m_ScSpeed * Time.deltaTime);
            }

            if(m_Inven_Btn.transform.localPosition.x > m_BtnOffPos.x)
            {
                m_Inven_Btn.transform.localPosition =
                    Vector3.MoveTowards(m_Inven_Btn.transform.localPosition,
                                m_BtnOffPos, m_ScSpeed * Time.deltaTime);
            }
        }
        else //if(m_Inven_ScOnOff == true)
        {
            if(m_ScOnPos.x > m_InvenScrollTr.localPosition.x)
            {
                m_InvenScrollTr.localPosition =
                    Vector3.MoveTowards(m_InvenScrollTr.localPosition,
                                    m_ScOnPos, m_ScSpeed * Time.deltaTime);
            }

            if(m_BtnOnPos.x > m_Inven_Btn.transform.localPosition.x)
            {
                m_Inven_Btn.transform.localPosition =
                    Vector3.MoveTowards(m_Inven_Btn.transform.localPosition,
                                    m_BtnOnPos, m_ScSpeed * Time.deltaTime);
            }
        } //else //if(m_Inven_ScOnOff == true)

    }//void ScrollViewOnOff_Update()

    public void SkillTimeFunc(SkillType a_SkType, float a_Time, float a_Dealy)
    {
        GameObject obj = Instantiate(m_SkCoolNode) as GameObject;
        obj.transform.SetParent(m_SkillTimeTr, false);
        SkillCoolCtrl a_SCtrl = obj.GetComponent<SkillCoolCtrl>();
        a_SCtrl.InitState(a_SkType, a_Time, a_Dealy);
    }

}//public class Game_Mgr : MonoBehaviour
