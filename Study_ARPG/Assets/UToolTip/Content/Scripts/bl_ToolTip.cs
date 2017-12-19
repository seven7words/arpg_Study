using UnityEngine;
using UnityEngine.UI;

public class bl_ToolTip : MonoBehaviour
{
    /// <summary>
    /// Root ToolTip UI
    /// </summary>
    [Header("Main")]
    [SerializeField]private RectTransform m_Rect = null;
    /// <summary>
    /// Custom position of tooltip to mouse point
    /// </summary>
    [SerializeField]private Text TooltipText = null;
    [SerializeField]private Image ToolTipImage = null;
    [Header("Settings")]
    [SerializeField,Range(0,15)]private float PivotLerp = 12;
    [SerializeField,Range(0,10)]private float AxisLerp = 3;
    [SerializeField,Range(0.01f,5)]private float FadeSpeed = 1;
    [Space(5)]
    public References m_References;

    private bool m_Show = false;
    private UTTPlaceH H;
    private UTTPlaceV V;
    private Vector2 ContentPosition;
    private Vector2 currentPivot;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate()
    {
        if (!m_Show)
            return;

        m_Rect.position = GetMouseRectPosition();
        AnchorPosition();
        LerpPosition();
    }

    /// <summary>
    /// Show tooltip event
    /// </summary>
    /// <param name="b">show?</param>
    public void ShowToolTip(bool b,float wait = 0.0f, Sprite s = null,string t = "")
    {
        //cache information 
        m_Show = b;
        CacheSprite = s;
        CacheText = t;
        //Wait for show if it is needed.
        if (b)
        {
            Invoke("ToolTip", wait);           
        }
        else
        {
            CancelInvoke("ToolTip");
            m_Show = b; AnimatedState(false);
        }

    }

    private Sprite CacheSprite;
    private string CacheText;
    void ToolTip()
    {
        //if this have a sprite
        if (CacheSprite != null)
        {
            ToolTipImage.gameObject.SetActive(true);
            ToolTipImage.sprite = CacheSprite;
            if(m_References.Separators != null) { foreach (GameObject g in m_References.Separators){ g.SetActive(true); } }
        }
        else//if this dont have a sprite
        {
            ToolTipImage.gameObject.SetActive(false);
            if (m_References.Separators != null) { foreach (GameObject g in m_References.Separators) { g.SetActive(false); } }
        }
        //refresh sprite
        if (!m_Show) { ToolTipImage.sprite = null; }
        //get values  
        AnimatedState(m_Show);
        TooltipText.text = CacheText;

        LayoutElement[] element = GetComponentsInChildren<LayoutElement>();
        if (element.Length > 1)
        {
            for (int i = 0; i < element.Length; i++)
            {
                element[i].CalculateLayoutInputVertical();
                element[i].CalculateLayoutInputHorizontal();
            }
        }
        m_References.ContentLayout.CalculateLayoutInputVertical();
    }

    /// <summary>
    /// 
    /// </summary>
    Vector2 Cpos;
    void LerpPosition()
    {
        m_Rect.pivot = Vector2.Lerp(m_Rect.pivot, currentPivot,PivotLerp * Time.deltaTime);
        Cpos = Vector2.Lerp(Cpos, ContentPosition, AxisLerp * Time.deltaTime);
        m_Rect.GetChild(0).GetComponent<RectTransform>().anchoredPosition = Cpos;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="b"></param>
    public void AnimatedState(bool b)
    {
        //Play Fade animation
        Animator a = this.GetComponent<Animator>();
        a.speed = FadeSpeed;
        a.SetBool("Fade", b);
    }

    /// <summary>
    /// Determine if the tooltip side in screen.
    /// </summary>
    void AnchorPosition()
    {
        Vector3 v = GetMouseRectPosition();
        if (v.x  <= (Screen.width / 2))
        {
            H = UTTPlaceH.Left;
        }
        else
        {
            H = UTTPlaceH.Right;
        }

        if (v.y  > (Screen.height / 2))
        {
            V = UTTPlaceV.Top;
        }
        else
        {
            V = UTTPlaceV.Botton;
        }

        if (V == UTTPlaceV.Top && H == UTTPlaceH.Left)
        {
            m_References.ToolTipBg.sprite = m_References.LeftTopBg;
            SetPivot(0, 1);
        }
        else if (V == UTTPlaceV.Top && H == UTTPlaceH.Right)
        {
            m_References.ToolTipBg.sprite = m_References.RightTopBg;
            SetPivot(1, 1);
        }
        else if (V == UTTPlaceV.Botton && H == UTTPlaceH.Left)
        {
            m_References.ToolTipBg.sprite = m_References.LeftBottonBg;
            SetPivot(0, 0);
        }
        else if (V == UTTPlaceV.Botton && H == UTTPlaceH.Right)
        {
            m_References.ToolTipBg.sprite = m_References.RightBottonBg;
            SetPivot(1, 0);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pivot"></param>
    void SetPivot(float x,float y)
    {
        currentPivot = new Vector2(x, y);
        float fix = (y > 0.5f) ? 18 : -18;
        ContentPosition = new Vector2(0, fix);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_Canvas"></param>
    /// <param name="_Cam"></param>
    /// <returns></returns>
    public Vector3 GetMouseRectPosition()
    {
        Canvas _Canvas = transform.root.GetComponent<Canvas>();
        Camera _Cam = Camera.main;
       Vector3 Return = Vector3.zero;

        if (_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            Vector3 mousePos = Input.mousePosition;

            Return = mousePos;
        }
        else if (_Canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector2 tempVector = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_Canvas.transform as RectTransform, Input.mousePosition, _Cam, out tempVector);
            Vector3 point = _Canvas.transform.TransformPoint(tempVector);
            Return = point;
        }

        return Return;
    }

    // Standard Singleton Access
    private static bl_ToolTip instance;
        public static bl_ToolTip Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<bl_ToolTip>();
                return instance;
            }
        }

    [System.Serializable]
    public class References
    {
        public LayoutElement ContentLayout;
        public Image ToolTipBg = null;
        public GameObject[] Separators = null;
        public Sprite LeftTopBg;
        public Sprite LeftBottonBg;
        public Sprite RightTopBg;
        public Sprite RightBottonBg;
    }
}