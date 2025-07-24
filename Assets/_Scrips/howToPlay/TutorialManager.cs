using System.Collections;
using UnityEngine;
using TMPro; // Nếu dùng TextMeshPro

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;

    private int step = 0;
    private bool stepDone = false;

    void Start()
    {
        tutorialPanel.SetActive(true);
        StartCoroutine(TutorialFlow());
    }

    IEnumerator TutorialFlow()
    {
        tutorialText.text = "Chào mừng bạn đến với game!\nĐây là bảng hướng dẫn.";
        yield return new WaitForSeconds(3f);

        step = 1;
        stepDone = false;
        tutorialText.text = "Sử dụng phím <b>A</b> để sang trái, <b>D</b> để sang phải.";
        yield return new WaitUntil(() => stepDone);

        step = 2;
        stepDone = false;
        tutorialText.text = "Nhấn <b>Space</b> để nhảy.";
        yield return new WaitUntil(() => stepDone);

        step = 3;
        stepDone = false;
        tutorialText.text = "Nhấn <b>J</b> để tấn công.";
        yield return new WaitUntil(() => stepDone);

        tutorialText.text = "khi bạn hạ gục quái vật, bạn sẽ nhận được điểm và có tỉ lệ rơi vật phẩm";
        yield return new WaitForSeconds(3f);

        tutorialText.text = "Bạn đã hoàn thành hướng dẫn!\nĐi săn quỷ nào!";
        yield return new WaitForSeconds(3f);
        
        tutorialPanel.SetActive(false);
    }

    void Update()
    {
        if (step == 1 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
            stepDone = true;
        else if (step == 2 && Input.GetKeyDown(KeyCode.Space))
            stepDone = true;
        else if (step == 3 && Input.GetKeyDown(KeyCode.J))
            stepDone = true;
    }
}