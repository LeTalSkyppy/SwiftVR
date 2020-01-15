using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public float time = 1f;
    public int i = 5;
    protected float x = 20f;
    protected float y = 0f;
    protected RectTransform trans;
    protected Text txt;
    protected Color transparent = new Color(1, 1, 1, 0);

    private void Start ()
    {
        trans = GetComponent<RectTransform>();
        txt = GetComponent<Text>();
        txt.text = ""+i;
        txt.color = transparent;
    }

    private void Update ()
    {
        time -= Time.deltaTime;

        if (time < 0f && i > 0)
        {
            time = 1f;
            i--;
            x = 20f;
            y = 0f;
            txt.text = ""+i;
            txt.color = transparent;
            trans.localScale = new Vector3(x, x, 1);
        }

        x = Mathf.Lerp(x, 0.7f, Time.deltaTime * 20f);
        y = Mathf.Lerp(y, 1f, Time.deltaTime * 10f);

        trans.localScale = new Vector3(x, x, 1);
        txt.color = Color.Lerp(transparent, Color.white, y);
    }
}
