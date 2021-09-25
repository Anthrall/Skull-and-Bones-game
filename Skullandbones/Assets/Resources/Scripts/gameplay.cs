using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class gameplay : MonoBehaviour
{
    public int player_score = 0, computer_score = 0; //счет игроков

    public int[,] player_board; //доска игрока
    public int[,] computer_board; //доска компьютера

    public List<int> deck, computer_cards, player_cards; //списки колоды и видов у компа
    public GameObject allcards, shirt, shirt_pref, manager, rival, help;
    public Text query_text, text_down;
    public Color[] win_lose; //цвет карт при сборе
    public Color player_answer, computer_answer; //цвета текста
    public Sprite shirt_sprite, normal_board, win_board;
    private string lang;
    public bool gameready = false, query = false, player_ans, isCoroutineExecuting, end, sound;
    //старт игры        блок карт      кто спрашивает       корутины

    private string[] query_text_variable_rus = { "У тебя есть ", "Есть ", "Может, у тебя есть ", "" };
    private string[] answer_no_rus = { "Нет", "Извини, нет", "Бери карту", "Неа", "Пожалуй, тебе стоит взять карту" };
    private string[] answer_yes_rus = { "Да, забирай", "Держи", "Эх... бери", "Есть, держи" };

    private string[] query_text_variable_eng = { "Do you have ", "Maybe you have ", "" };
    private string[] answer_no_eng = { "No", "Sorry, no", "Take a card", "Nope", "Perhaps you should take a card" };
    private string[] answer_yes_eng = { "Yes, take it", "Take it", "Eh ... take it" };

    private string[] vowels = {"E","Y","U","I","O","A"};
    private string[] consonants = {"B", "C", "D", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "V", "W", "X", "Y", "Z"};

    private string[] query_text_variable, answer_no, answer_yes;

    private void Awake()
    {
        lang = PlayerPrefs.GetString("Language");
        Invoke("GetPlayerTurn", 0.5f);
    }

    private void Start()
    {
        lang = PlayerPrefs.GetString("Language");
        if(lang=="RUS")
        {
            query_text_variable = query_text_variable_rus;
            answer_no = answer_no_rus;
            answer_yes = answer_yes_rus;

            help.transform.GetChild(0).GetComponent<Text>().text =
                "<b><size=20>ГЕЙМПЛЕЙ:</size></b>" +
                "\nВ игре есть 10 наборов по три однаковой карты. <color=yellow>Ваша задача - собрать таких наборов больше, чем противник.</color>" +
                "\nНажимайте на имеющиеся у вас карты, чтобы запросить подобные у противника.Если у него будут карты из этого набора - он отдаст их вам.Если нет -вы берете из колоды, и ход переходит противнику." +
                "\n<color=yellow> Если вы забрали карту у противника, вам будет дано право ходить еще раз.И так до тех пор, пока у противника не окажется нужной карты.</color>" +
                "\nПротивник ходит аналогично. Внимательно смотрите, что он запрашивает. Только так вы можете понять, какие у него карты. Удачи!";
        }
        else
        {
            query_text_variable = query_text_variable_eng;
            answer_no = answer_no_eng;
            answer_yes = answer_yes_eng;

            help.transform.GetChild(0).GetComponent<Text>().text =
                "<b><size=20>GAMEPLAY:</size></b>" +
                "\nThe game has 10 sets of three of the same card. <color=yellow>Your task is to collect more sets than the computer.</color>" +
                "\nClick on the cards you want to request similar ones from the enemy. If he has cards from this set, he will give them to you. If not, you take from the deck, and computer make a move." +
                "\n<color=yellow>If you took a card from computer, you will be given the right make to move again, and so on until the computer hasn't the required card.</color>" +
                "\nThe computer make moves in the same way. Take a close look at what he asks for. This is the only way you can understand what cards he has. Good luck!";
        }
        help.SetActive(false);
    }

    void Update()
    {
       /* if(gameready)
        {
            if(Input.GetKeyDown(KeyCode.Alpha5))
            {
                Debug.Log("Player board");
                PrintBoard(player_board);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                Debug.Log("Computer board");
                PrintBoard(computer_board);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("Deck");
                PrintList(deck);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.Log("Comp cards");
                PrintList(computer_cards);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("Player cards");
                UpdateListComputer(true);
                PrintList(player_cards);
            }
        }*/
    }

    public string CheckFirstLetter(string name)
    {
        foreach(string letter in vowels)
        {
            if(name[0].ToString() == letter)
            {
                return "an";
            }
        }
        foreach(string letter in consonants)
        {
            if (name[0].ToString() == letter)
            {
                return "a";
            }
        }
        return "";
    }

    public void StartGame()
    {
        if(gameready)
        {
            query_text.text = "";

            if(lang == "RUS")
                text_down.text = "Выберите карту";
            if(lang == "ENG")
                text_down.text = "Select card";

            GiveFirstCards(player_board, deck, true);
            GiveFirstCards(computer_board, deck, false);
            UpdateListComputer(false);
            StartCoroutine(PlaySound("CardOnDeck", 0.2f));
            Invoke("GetPlayerTurn", 0.3f);
            //FindObjectOfType<AudioManager>().Play("CardOnDeck");


        }
    }

    public void GiveFirstCards(int[,] board, List<int> deck, bool isplayer)
    {
        for (int i = 0; i < 5; i++)
        {
            int x = UnityEngine.Random.Range(0, deck.Count);
            int index = deck[x];
            var card = allcards.transform.GetChild(index);
            if (isplayer)
            {
                card.gameObject.SetActive(true);
                //Debug.Log("pl"+i+": "+index);
            }
            int type = card.GetComponent<card_settings>().id_type;
            int id = card.GetComponent<card_settings>().id_card;
            board[type, id] = 1;
            deck.Remove(index);
            if (!isplayer)
            {
                if (!CheckType(computer_cards, type))
                {
                    computer_cards.Add(type);
                }
                card.transform.position = new Vector3(rival.transform.position.x, rival.transform.position.y, -6);
                card.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                card.transform.position = card.GetComponent<card_settings>().pos_on_board;
                card.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        
    }

    public bool CheckType(List<int> list, int type) //проверяет типы карт у компьютера
    {
        foreach (int x in list)
        {
            if (x == type)
            {
                return true;
            }
        }
        return false;
    }

    public void QueryCard(int type, bool isplayer)
    {
        int setofcards = manager.GetComponent<spawn_cards>().typecards;
        string facename;
        if (lang == "RUS")
            facename = manager.GetComponent<spawn_cards>().cards[setofcards].faces[type].name_ru;
        else
            facename = manager.GetComponent<spawn_cards>().cards[setofcards].faces[type].name;
        query = true;
        int que = UnityEngine.Random.Range(0, query_text_variable.Length);
        if(lang == "ENG" && que != query_text_variable.Length-1)
            query_text.text = query_text_variable[que]+ CheckFirstLetter(facename)+" " + facename + "?";
        else
            query_text.text = query_text_variable[que] + facename + "?";
        

        if (isplayer)
        {
            FindObjectOfType<AudioManager>().Play("PlayerVoice");
            
            Debug.Log("<color=yellow>player query: " + facename+"</color>");
            query_text.color = player_answer;
            FindCards(computer_board, type, true, player_board);
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("ComputerVoice");
            //PlaySound("ComputerVoice", 0.1f);
            Debug.Log("<color=red>comp query: " + facename + "</color>");
            query_text.color = computer_answer;
            FindCards(player_board, type, false, computer_board);
        }
    }

    public void FindCards(int[,] board, int type, bool isplayer, int[,] myboard)
    {
        if (board[type, 0] == 1 || board[type, 1] == 1 || board[type, 2] == 1) //взять у игрока
        {
            if (isplayer)
            {
                player_ans = false;
                Invoke("GetAnswerYes", 0.6f);
                //PlaySound("PlayerVoice", 0.6f);
                //FindObjectOfType<AudioManager>().Play("PlayerVoice");
                //GetAnswerYes();
                Giveaway(board, myboard, type, true);
                StartCoroutine(WaittoUpdate(myboard, true));
                //StartCoroutine(WaittoUpdate(board, false));
                //UpdateCards(myboard, 1);
                if(!end)
                    TurnPlayer();
                if (deck.Count == 0)
                {
                    shirt.SetActive(false);
                }
            }
            else
            {
                player_ans = true;
                Invoke("GetAnswerYes", 0.6f);
                //PlaySound("ComputerVoice", 0.6f);
                //FindObjectOfType<AudioManager>().Play("ComputerVoice");
                //GetAnswerYes();
                Giveaway(board, myboard, type, false);
                StartCoroutine(WaittoUpdate(myboard, false));
                //StartCoroutine(WaittoUpdate(board, true));
                //UpdateCards(myboard, 2);
                if (!end)
                    Invoke("TurnAI", 3f);
                
            }
        }
        else
        {
            if (board[type, 0] == 0 && board[type, 1] == 0 && board[type, 2] == 0) //взять из колоды
            {
                if (isplayer)
                {
                    player_ans = false;
                    Invoke("GetAnswerNo", 0.6f);
                    //PlaySound("PlayerVoice", 0.6f);
                    //FindObjectOfType<AudioManager>().Play("PlayerVoice");
                    //GetAnswerNo();
                    GiveCardtoDeck(myboard, deck, true);
                    StartCoroutine(WaittoUpdate(myboard, true));
                    //StartCoroutine(WaittoUpdate(board, false));
                    //UpdateCards(myboard, 1);
                    if (!end)
                        Invoke("TurnAI", 3f);
                }
                else
                {
                    player_ans = true;
                    Invoke("GetAnswerNo", 0.6f);
                    //PlaySound("ComputerVoice", 0.6f);
                    //FindObjectOfType<AudioManager>().Play("ComputerVoice");
                    //GetAnswerNo();
                    GiveCardtoDeck(myboard, deck, false);
                    StartCoroutine(WaittoUpdate(myboard, false));
                    //StartCoroutine(WaittoUpdate(board, true));
                    //UpdateCards(myboard, 2);
                    if (!end)
                        TurnPlayer();
                }
            }
        }
    }

    public void GetAnswerNo()
    {
        int ans = UnityEngine.Random.Range(0, answer_no.Length);
        query_text.text = answer_no[ans];
        if (player_ans)
        {
            query_text.color = player_answer;
            FindObjectOfType<AudioManager>().Play("PlayerVoice");
            Debug.Log("<color=yellow>player answer: NO</color>");
        }
        else
        {
            query_text.color = computer_answer;
            FindObjectOfType<AudioManager>().Play("ComputerVoice");
            Debug.Log("<color=red>comp answer: NO</color>");
        }
    }

    public void GetAnswerYes()
    {
        int ans = UnityEngine.Random.Range(0, answer_yes.Length);
        query_text.text = answer_yes[ans];
        if (player_ans)
        {
            query_text.color = player_answer;
            FindObjectOfType<AudioManager>().Play("PlayerVoice");
            Debug.Log("<color=yellow>player answer: YES</color>");
        }
        else
        {
            query_text.color = computer_answer;
            FindObjectOfType<AudioManager>().Play("ComputerVoice");
            Debug.Log("<color=red>comp answer: YES</color>");
        }
    }

    public void Giveaway(int[,] board_from, int[,] board_to, int type, bool isplayer)
    {
        
        for (int i = 0; i < board_from.GetLength(1); i++)
        {

            if (board_from[type, i] == 1 && board_to[type, i] == 0)
            {
                board_to[type, i] = 1;
                board_from[type, i] = 0;

                int id = FindID(type, i);
                var card = allcards.transform.GetChild(id);
                if (isplayer)
                {
                    Debug.Log("<color=yellow>PLAYER: id card away " + id+"</color>");
                    AnimCardRival(card.GetComponent<card_settings>().id, true);
                    //Vector3 normal = card.GetComponent<card_settings>().pos_on_board;
                    //card.transform.position = new Vector3(normal.x, normal.y, -6f);
                    
                    //Vector3 tmp = new Vector3(0, 0, 0);
                    //card.transform.rotation = Quaternion.Euler(tmp);
                    
                }
                else
                {
                    Debug.Log("<color=red>COMP: id card away " + id + "</color>");
                    AnimCardRival(card.GetComponent<card_settings>().id, false);
                    //card.transform.position = new Vector3(rival.transform.position.x, rival.transform.position.y, -6);
                    //Vector3 tmp = new Vector3(0, 0, 0);
                    //card.transform.rotation = Quaternion.Euler(tmp);

                }

            }
        }
    }

    public int FindID(int type, int id_card)
    {
        for (int i = 0; i < allcards.transform.childCount; i++)
        {
            var card = allcards.transform.GetChild(i);
            if (card.GetComponent<card_settings>().id_type == type && card.GetComponent<card_settings>().id_card == id_card)
            {
                return card.GetComponent<card_settings>().id;

            }
        }
        return 0;
    }

    public void GiveCardtoDeck(int[,] myboard, List<int> deck, bool isplayer)
    {
        
        int index = UnityEngine.Random.Range(0, deck.Count);
        //int index = deck[x];
        //Debug.Log("id card deck " + index);
        var card = allcards.transform.GetChild(deck[index]);
        int type = card.GetComponent<card_settings>().id_type;
        int col = card.GetComponent<card_settings>().id_card;

        myboard[type, col] = 1;
        

        if (isplayer)
        {
            Debug.Log("<color=yellow>PLAYER: index card "+index+"\nid card deck " + deck[index] + "</color>");
            AnimCardDeck(card.GetComponent<card_settings>().id, true);
            //Vector3 normal = card.GetComponent<card_settings>().pos_on_board;
            //card.transform.position = new Vector3(normal.x, normal.y, -6f);
            //Vector3 tmp = new Vector3(0, 0, 0);
            //card.transform.rotation = Quaternion.Euler(tmp);

        }
        else
        {
            Debug.Log("<color=red>COMP: index card " + index + "\nid card deck " + deck[index] + "</color>");
            AnimCardDeck(card.GetComponent<card_settings>().id, false);
            //card.transform.position = new Vector3(rival.transform.position.x, rival.transform.position.y, -6);
            //Vector3 tmp = new Vector3(0, 0, 0);
            //card.transform.rotation = Quaternion.Euler(tmp);

        }
        deck.RemoveAt(index);
        if (deck.Count == 0)
        {
            shirt.SetActive(false);
        }
    }

    public void GetPlayerTurn()
    {
        query = false;
    }

    public void TurnAI()
    {
        query = true;
        //StartCoroutine(WaittoUpdate(computer_board, false));
        UpdateListComputer(false);

        if (computer_cards.Count > 0)
        {
            int x = UnityEngine.Random.Range(0, computer_cards.Count);
            //Debug.Log(x);
            int index = computer_cards[x];
            QueryCard(index, false);
        }
        else
        {
            if(deck.Count >= 1)
            {
                GiveCardtoDeck(computer_board, deck, false);
                UpdateListComputer(false);
                query_text.color = computer_answer;
                FindObjectOfType<AudioManager>().Play("ComputerVoice");
                if(lang == "RUS")
                    query_text.text = "У меня не осталось карт. Беру из колоды";
                else
                    query_text.text = "I have no cards left. I take from the deck";

                TurnPlayer();
            }
            else
            {
                end = true;
                //Debug.Log("END GAME");
                Invoke("EndGame", 5.3f);
            }
        }

        Invoke("GetPlayerTurn", 1.5f);
    }

    public void TurnPlayer()
    {
        //StartCoroutine(WaittoUpdate(player_board, true));
        UpdateListComputer(false);
        UpdateListComputer(true);
        
        if (player_cards.Count > 0)
        {
            //query = false;
            Invoke("GetPlayerTurn", 1.5f);
        }
        else
        {
            if (deck.Count >= 1)
            {

                GiveCardtoDeck(player_board, deck, true);
                
                query_text.color = player_answer;
                FindObjectOfType<AudioManager>().Play("PlayerVoice");

                if (lang == "RUS")
                    query_text.text = "У меня не осталось карт. Беру из колоды";
                else
                    query_text.text = "I have no cards left. I take from the deck";

                Invoke("TurnAI", 3f);
            }
            else
            {
                end = true;
                
                Invoke("EndGame", 5.3f);
            }
        }
        //Debug.Log("Now your turn");
    }

    public void EndGame()
    {
        Debug.Log("END GAME");
        UpdateCards(computer_board, 2);
        UpdateCards(player_board, 1);

        for (int i=0; i<player_board.GetLength(0); i++)
        {
            if(computer_board[i,0] == 1 && computer_board[i, 1] == 1 && computer_board[i, 2] == 1)
            {
                computer_score += 1;
            }
            if(player_board[i, 0] == 1 && player_board[i, 1] == 1 && player_board[i, 2] == 1)
            {
                player_score += 1;
            }
        }
        FindObjectOfType<AudioManager>().Stop("Theme");
        if (player_score > computer_score)
        {
            query = true;
            
            FindObjectOfType<AudioManager>().Play("Win");

            if (lang == "RUS")
                query_text.text = "<b>ТЫ ВЫИГРАЛ!</b>";
            else
                query_text.text = "<b>YOU WIN!</b>";


            query_text.color = player_answer;

        }
        else
        {
            if (player_score < computer_score)
            {
                query = true;
                
                FindObjectOfType<AudioManager>().Play("Lose");

                if (lang == "RUS")
                    query_text.text = "<b>ТЫ ПРОИГРАЛ!</b>";
                else
                    query_text.text = "<b>YOU LOSE!</b>";

                query_text.color = computer_answer;

            }
            else
            {
                if (player_score == computer_score)
                {
                    query = true;
                    
                    FindObjectOfType<AudioManager>().Play("NeutralEnd");

                    if (lang == "RUS")
                        query_text.text = "<b>НИЧЬЯ! ХОЧЕШЬ ПОВТОРИТЬ?</b>";
                    else
                        query_text.text = "<b>DRAW! DO YOU WANT TO REPEAT?</b>";

                    query_text.color = computer_answer;

                }
            }
        }

        if (lang == "RUS")
            text_down.text = "Игра окончена! Для повтора игры нажмите кнопку РЕСТАРТ";
        else
            text_down.text = "The game is over! To replay the game, press the RESTART button";

    }

    IEnumerator WaittoUpdate(int[,] board, bool isplayer)
    {
        if (isCoroutineExecuting)
            yield break;
        isCoroutineExecuting = true;
        yield return new WaitForSeconds(4.85f);

        if (isplayer)
        {
            UpdateCards(board, 1);
        }
        else
        {
            UpdateCards(board, 2);
        }
        

        isCoroutineExecuting = false;
    }

    IEnumerator PlaySound(string name, float delay)
    {
        if (sound)
            yield break;
        sound = true;
        yield return new WaitForSeconds(delay);

        FindObjectOfType<AudioManager>().Play(name);

        sound = false;
    }

    public void UpdateCards(int[,] board, int mode)
    {
        bool soundplay = false;
        
        for (int i = 0; i < player_board.GetLength(0); i++)
        {
            if(board[i, 0] == 1 && board[i, 1] == 1 && board[i, 2] == 1)
            {
                for(int j=0; j < board.GetLength(1); j++)
                {
                    int id = FindID(i, j);
                    var card = allcards.transform.GetChild(id);
                    var setting = allcards.transform.GetChild(id).GetComponent<card_settings>();

                    setting.collect_type = mode;
                    if (mode == 1)
                    {
                        if(!setting.collect)
                        {
                            card.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
                            if (!soundplay)
                            {
                                FindObjectOfType<AudioManager>().Play("PlayerCollect");
                                soundplay = true;
                            }
                        }
                        card.GetComponent<SpriteRenderer>().sprite = win_board;
                        card.transform.position = new Vector3(setting.pos_on_board.x, setting.pos_on_board.y, -6f);
                        card.transform.rotation = Quaternion.Euler(0, 0, 0);
                        card.transform.localScale = setting.scale;
                        card.transform.GetChild(0).GetComponent<SpriteRenderer>().color = win_lose[1];
                        card.transform.GetChild(1).GetComponent<SpriteRenderer>().color = win_lose[1];
                        //card.transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
                        setting.collect = true;
                    }
                    if (mode == 2)
                    {
                        if (!setting.collect)
                        {
                            card.transform.GetChild(3).GetComponent<ParticleSystem>().Play();
                            if (!soundplay)
                            {
                                FindObjectOfType<AudioManager>().Play("ComputerCollect");
                                soundplay = true;
                            }
                        }

                        //setting.collect_type = mode;
                        card.GetComponent<SpriteRenderer>().sprite = normal_board;
                        card.transform.position = new Vector3(setting.pos_on_board.x, setting.pos_on_board.y, -6f);
                        card.transform.rotation = Quaternion.Euler(0, 0, 0);
                        card.transform.localScale = setting.scale;
                        card.GetComponent<SpriteRenderer>().color = win_lose[0];
                        card.transform.GetChild(0).GetComponent<SpriteRenderer>().color = win_lose[0];
                        card.transform.GetChild(1).GetComponent<SpriteRenderer>().color = win_lose[0];
                        setting.collect = true;
                        
                    }
                }
            }
        }
    }

    public void UpdateListComputer(bool isplayer)
    {
        if (isplayer)
        {
            player_cards = new List<int>();
            for (int i = 0; i < player_board.GetLength(0); i++)
            {
                for (int j = 0; j < player_board.GetLength(1); j++)
                {
                    if (player_board[i, j] == 1)
                    {
                        player_cards.Add(i);
                        break;

                    }
                }

                if (player_board[i, 0] == 1 && player_board[i, 1] == 1 && player_board[i, 2] == 1)
                {
                    if (CheckType(player_cards, i))
                        player_cards.Remove(i);
                }
                
            }
            //PrintList(player_cards);
        }
        else
        {
            computer_cards = new List<int>();
            for (int i = 0; i < computer_board.GetLength(0); i++)
            {
                for (int j = 0; j < computer_board.GetLength(1); j++)
                {
                    if (computer_board[i, j] == 1)
                    {
                        computer_cards.Add(i);
                        break;

                    }
                }

                if (computer_board[i, 0] == 1 && computer_board[i, 1] == 1 && computer_board[i, 2] == 1)
                {
                    if (CheckType(computer_cards, i))
                        computer_cards.Remove(i);
                }
                
            }
            PrintList(computer_cards);
        }
    }

    public void AnimCardDeck(int id_card, bool isplayer)
    {
        var Seq = DOTween.Sequence();
        var card = allcards.transform.GetChild(id_card);
        var setting = allcards.transform.GetChild(id_card).GetComponent<card_settings>();

        card.gameObject.SetActive(true);
        //card.transform.position = new Vector3(shirt.transform.position.x, shirt.transform.position.y, -6);
        //Vector3 tmp = shirt.transform.eulerAngles;
        //card.transform.rotation = Quaternion.Euler(tmp);
        
        if (isplayer)
        {
            card.GetComponent<SpriteRenderer>().sprite = normal_board;
            Seq.AppendInterval(1f);
            StartCoroutine(PlaySound("CardOnDeck", 1f));
            Seq.Append(card.transform.DOMoveY(3.85f, 0.2f));
            Seq.AppendInterval(0.2f);
            Seq.Append(card.transform.DOMove(setting.pos_on_board, 0.2f));
            Vector3 tmp1 = new Vector3(0, 0, 0);
            Seq.Join(card.transform.DORotate(tmp1,0.3f));
            
        }
        else
        {
            card.GetComponent<SpriteRenderer>().sprite = shirt_sprite;
            Seq.AppendInterval(1f);
            StartCoroutine(PlaySound("CardOnDeck", 1f));
            Seq.Append(card.transform.DOMoveX(-5.7f, 0.2f));
            Seq.AppendInterval(0.2f);
            Seq.Append(card.transform.DOMove(new Vector3(rival.transform.position.x, rival.transform.position.y, -6), 0.5f));
            Vector3 tmp1 = rival.transform.eulerAngles;
            Seq.Join(card.transform.DORotate(tmp1, 0.3f));

        }
    }

    public void AnimCardRival(int id_card, bool isplayer)
    {
        var Seq = DOTween.Sequence();
        var card = allcards.transform.GetChild(id_card);
        var setting = allcards.transform.GetChild(id_card).GetComponent<card_settings>();

        
        if (isplayer)
        {
            card.GetComponent<SpriteRenderer>().sprite = normal_board;
            card.gameObject.SetActive(true);
            card.transform.position = new Vector3(rival.transform.position.x, rival.transform.position.y, -6);
            Vector3 tmp = rival.transform.eulerAngles;
            card.transform.rotation = Quaternion.Euler(tmp);

            Seq.AppendInterval(1f);
            StartCoroutine(PlaySound("CardAway", 1f));
            Seq.Append(card.transform.DOMoveY(4.45f, 0.2f));
            Seq.AppendInterval(0.2f);
            Seq.Append(card.transform.DOMove(setting.pos_on_board, 0.2f));
            Vector3 tmp1 = new Vector3(0, 0, 0);
            Seq.Join(card.transform.DORotate(tmp1,0.3f));
        }
        else
        {
            card.GetComponent<SpriteRenderer>().sprite = normal_board;
            Seq.AppendInterval(1f);
            StartCoroutine(PlaySound("CardAway", 1f));
            Seq.Append(card.transform.DOMove(new Vector3(rival.transform.position.x, rival.transform.position.y-2.09f, rival.transform.position.z-1), 0.2f));
            Seq.AppendInterval(0.2f);
            Seq.Append(card.transform.DOMoveY(rival.transform.position.y, 0.2f));
        }
    }

    public void PrintList(List<int> list)
    {
        string log = "";
        foreach(int index in list)
        {
            log += index + " ";
        }
        Debug.Log("List count: "+list.Count+"  Elements: "+log);
    }

    public void PrintBoard(int[,] board)
    {
        Debug.Log("    0  1  2  ");
        for (int i=0; i < board.GetLength(0); i++)
        {
            string line = "    ";
            for (int j=0; j < board.GetLength(1); j++)
            {
                line += board[i, j].ToString() + "  ";
            }
            Debug.Log(i + ": " + line);
        }
        Debug.Log("");
    }
}
