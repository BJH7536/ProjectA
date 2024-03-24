using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    public QuestManager quest;                //��ü �гο� ���̰� state�� äũ�ؼ� additem���� ���� �����ϰ��ִ� ����Ʈ�� �����´�. 
    public Button[] QuestList;                //����Ʈ ����Ʈ 

    public TextMeshProUGUI QuestTitle;
    public TextMeshProUGUI QuestDescription;
    public ScrollRect QuestScrollRect;

    private void Start()
    {
        QuestScrollRect.normalizedPosition = new Vector2(1f, 1f);                   //������ �������� �ٲ��ֱ�

        Vector2 size = QuestScrollRect.content.sizeDelta;                   
        size.y = 5000f;
        QuestScrollRect.content.sizeDelta = size;
    }

    void addQuest(int id)
    {
        GameObject qeust= Resources.Load("QuestButton") as GameObject;

        GameObject instance = PrefabUtility.InstantiatePrefab(qeust) as GameObject;     //�������߰����ֱ�
        instance.transform.SetParent(QuestScrollRect.content.transform);
    }
}
