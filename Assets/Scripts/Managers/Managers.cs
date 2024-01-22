using UnityEngine;

[DefaultExecutionOrder(0)]
public class Managers : MonoBehaviour
{
    static Managers _instance; // 유일한 인스턴스를 담을 변수.
    static Managers Instance { get { Init(); return _instance; } } // 유일한 인스턴스를 참조하는 메서드.

    private CutSceneManager _cutScene = new CutSceneManager();
    private ScriptManager _script = new ScriptManager();
    private SoundManager _sound = new SoundManager();
    private UIManager _ui = new UIManager();
    
    public static CutSceneManager CutScene => Instance._cutScene;
    public static ScriptManager Script => Instance._script;
    public static SoundManager Sound => Instance._sound;
    public static UIManager UI => Instance._ui;

    [Header("playerInfom")]
    private Player _player;
    private float maxHp = 100;



    void Awake()
    {
        Init();
        gameStart();
    }

    static void Init()
    {
        // Instance가 null일 때만 Managers를 찾아 Instance에 할당
        if (_instance != null) return;
        
        GameObject go = GameObject.Find("@Managers");
        
        if (go == null)
        {
            go = new GameObject{name = "@Managers"};
            go.AddComponent<Managers>();
        
        }
        
        DontDestroyOnLoad(go);
        _instance = go.GetComponent<Managers>();

    }


    void gameStart()
    {
        GameObject.Find("Player").GetComponent<Player>().playerHP= maxHp;
        GameObject.Find("NPC").GetComponent<Enemy>().setEnemyHP(maxHp);
        GameObject.Find("NPC").GetComponent<Enemy>().setEnemyDam(20);
        GameObject.Find("Parts").GetComponent<Parts>().setPartsHP(maxHp);
    }
}
