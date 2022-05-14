using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartGameL1 : MonoBehaviour
{
    private MissionUI missionUI;
    public RectTransform canvas;
    private Player player;

    private void Awake()
    {
        missionUI = FindObjectOfType<MissionUI>();
        List<string> missionList = new List<string> { "- Steal confidential document", "- Turn Off camera", "- Collect 4 document", "- Open the safe", "- Steal SD card ", "- Validate SD card with a server", "Escape safely"};

        missionUI.SetMission(missionList);

        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        player.enabled = false;
        StartCoroutine(DeleteStartUI());
    }

    IEnumerator DeleteStartUI()
    {
        yield return new WaitForSeconds(2f);

        canvas.DOLocalMoveY(Screen.height, 1.5f);
        yield return new WaitForSeconds(1.5f);

        player.enabled = true;
        Destroy(canvas.parent.gameObject);

    }
}
