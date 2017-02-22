using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour {

    private const float m_space = 0.5f;

    public GameObject[][] m_stage;      //ステージデータ
    public Vector2[] m_moveRoute;      //移動ルート
    public float m_speed;               //移動速度
    public bool m_move;                 //移動判定

    private int m_routeCount;           //移動ルート番号

    private MapManager m_MMClass;

    //初期化
    private void Start()
    {
        m_routeCount = 0;

        m_MMClass = GameObject.Find("Stage").GetComponent<MapManager>();

        m_stage = new GameObject[m_MMClass.m_MapList.Count][];
        for (int i = 0; i < m_MMClass.m_MapList.Count; i++)
        {
            m_stage[i] = m_MMClass.m_MapList[i].ToArray();
        }
    }

	//移動
	public void Move()
    {
        if (!m_move) return;
        MoveLenge();
        if (m_moveRoute.Length != m_routeCount) return;
        m_move = false;
        m_moveRoute = null;
        m_routeCount = 0;
        switch(gameObject.tag)
        {
            case "Player":
                gameObject.GetComponent<CharaControl>().m_anime.SetBool("walk", false);
                break;
            case "Enemy":
                gameObject.GetComponent<CPAI>().m_anime.SetBool("walk", false);
                break;
        }
        MoveArea.ResetMoveArea();
	}

    //移動距離を出して移動
    private void MoveLenge()
    {
        Vector2 movePoint;          //移動地点
        Vector2 moveLouteLenge;     //移動ルートまでの距離
        Vector3 stagePosition;      //ステージポジション
        int stageX;                 //ステージX
        int stageY;                 //ステージY
        float height;               //高さ
        float routeLenge;           //移動ルートまでの直線距離
        float speed;                //直線移動の移動速度
        float radian;               //ラジアン角
        if (m_moveRoute.Length == m_routeCount) return;
        stageX = (int)m_moveRoute[m_routeCount].x;
        stageY = (int)m_moveRoute[m_routeCount].y;
        stagePosition = new Vector3
        (
            m_stage[stageY][stageX].transform.position.x + m_space,
            m_stage[stageY][stageX].transform.position.y + m_space,
            m_stage[stageY][stageX].transform.position.z + m_space
        );
        //バグ回避
        if (gameObject.transform.position == stagePosition)
        {
            m_routeCount++;
            return;
        }
        moveLouteLenge = new Vector2
        (
         gameObject.transform.position.x - stagePosition.x,
         gameObject.transform.position.z - stagePosition.z
        );
        radian = Mathf.Atan2(moveLouteLenge.y, moveLouteLenge.x);
        routeLenge = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(moveLouteLenge.y, 2) + Mathf.Pow(moveLouteLenge.x, 2)));
        //自分の座標と移動座標の差が移動速度未満だったら
        if (routeLenge < m_speed) speed = routeLenge;
        else speed = m_speed;
        movePoint.x = - Mathf.Cos(radian) * speed;
        movePoint.y = - Mathf.Sin(radian) * speed;
        if (movePoint.x > 0) gameObject.GetComponent<Status>().DIRECTION = Direction.right;
        else if (movePoint.x < 0) gameObject.GetComponent<Status>().DIRECTION = Direction.left;
        if (movePoint.x > 0) gameObject.GetComponent<Status>().DIRECTION = Direction.top;
        else if (movePoint.y < 0) gameObject.GetComponent<Status>().DIRECTION = Direction.bottom;
        movePoint.x += gameObject.transform.position.x;
        movePoint.y += gameObject.transform.position.z;
        height = m_stage[Mathf.RoundToInt(movePoint.y - m_space)][Mathf.RoundToInt(movePoint.x - m_space)].transform.position.y + m_space;
        gameObject.transform.position = new Vector3
        (
         movePoint.x,
         height,
         movePoint.y
        );
        if (gameObject.transform.position != stagePosition) return;
        m_routeCount++;
    }

    //移動ルートを検索する
    public bool SelectMovePoiont(GameObject movePoint)
    {
        m_moveRoute = SarchRange.SarchMoveRoute(m_stage, gameObject, movePoint, GetComponent<Status>().MOV, false);
        if (m_moveRoute.Length == 0) return false;
        m_move = true;
        return true;
    }

}
