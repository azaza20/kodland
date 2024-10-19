using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
public class Dialog : MonoBehaviour
{
    [SerializeField] GameObject dialogue;
    public DialogueNode[] node;
    [SerializeField] TMP_Text npc;
    [SerializeField] TMP_Text[] textButtons;
    [SerializeField] GameObject[] buttons;
    public int currentNode;
    public bool ShowDialogue = false;
    [SerializeField] public List<GameObject> answerButtons = new List<GameObject>();
    [SerializeField] GameObject colorTarget; // здесь хранится наш префаб
    [System.NonSerialized] public GameObject target; // объект, в котором будет хранится наш префаб пока мы не выполним задание
    bool isInstantiate; // обозначаем активирован ли наш префаб

    public static Action<int> Click; //событие
    public void Awake()
    {
        npc = GameObject.FindGameObjectWithTag("NPCtext").GetComponent<TMP_Text>();
        dialogue = GameObject.FindGameObjectWithTag("Dialogue");
        //buttons = GameObject.FindGameObjectsWithTag("QuestButton");
        //textButtons = new TMP_Text[buttons.Length];
        //for (int i = 0; i < buttons.Length; i++)
        //{
        //    textButtons[i] = buttons[i].transform.GetChild(0).GetComponent<TMP_Text>();
        //}
    }
    private void Start()
    {
        dialogue.SetActive(false);
    }
    public static Action _color; //событие
    public void AnswerClickedEvent(int button)
    {
        if (_color != null)
        {
            _color.Invoke();
        }
        Debug.Log(button);
        Debug.Log(node[currentNode].PlayerAnswer.Length);
        if (node[currentNode].PlayerAnswer[button].SpeakEnd)
        {
            dialogue.SetActive(false);
        }
        if (node[currentNode].PlayerAnswer[button].questValue > 0)
        {
            PlayerPrefs.SetInt(node[currentNode].PlayerAnswer[button].questName,
            node[currentNode].PlayerAnswer[button].questValue);
        }
        if (node[currentNode].PlayerAnswer[button].GetMoney != 0)
        {
            FindObjectOfType<PlayerController>().AddCoin(node[currentNode].PlayerAnswer[button].GetMoney);
        }
        if (node[currentNode].PlayerAnswer[button].target != null)
        {
            if (!isInstantiate)
            {
                target = Instantiate(colorTarget);
                isInstantiate = true;
            }
            target.transform.position = node[currentNode].PlayerAnswer[button].target.transform.position;
        }
        if (node[currentNode].PlayerAnswer[button].destroyTarget)
        {
            Destroy(target);
        }
        currentNode = node[currentNode].PlayerAnswer[button].ToNode;
        Refresh();
    }
    public void AnswerClicked(int button)
    {
        Click.Invoke(button); //запуск события
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogue.SetActive(true);
            Click += AnswerClickedEvent; //подписка на событие
            currentNode = 0;
            Refresh();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogue.SetActive(false);
            Click -= AnswerClickedEvent; //отписка
        }
    }
    public void Refresh()
    {
        //отключаем все кнопки из списка при вызове функции
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
        //удаляем все данные из списка
        answerButtons.Clear();
        npc.text = node[currentNode].NpcText;
        for (int i = 0; i < node[currentNode].PlayerAnswer.Length; i++)
        {
            //включаем кнопку, если в поле questName нет записей
            if (node[currentNode].PlayerAnswer[i].questName == "" ||
                    //включаем кнопку, если поле needQuestValue совпадает с требуемым
                    node[currentNode].PlayerAnswer[i].needQuestValue ==
                        PlayerPrefs.GetInt(node[currentNode].PlayerAnswer[i].questName))
            {
                //добавляем данную кнопку в список
                textButtons[answerButtons.Count].text = node[currentNode].PlayerAnswer[i].Text;
                answerButtons.Add(buttons[answerButtons.Count]);
                
                
            }
        }
        //включаем кнопки из списка      
        for (int i = 0; i < answerButtons.Count; i++)
        {
            answerButtons[i].SetActive(true);
        }
    }
}

[System.Serializable]
public class DialogueNode
{
    public string NpcText;
    public Answer[] PlayerAnswer;
}

[System.Serializable]
public class Answer
{
    public int questValue; // Значение, которое будет присваиваться состоянию квеста
    public int needQuestValue; // Значение, при котором будет активироваться кнопка ответа
    public string questName; // Название квеста
    public string Text;
    public int ToNode;
    public int GetMoney;
    public bool destroyTarget; // если true, то уничтожим particle
    public GameObject target; // ссылка на объект к которому переместится Particle
    public bool SpeakEnd;
}