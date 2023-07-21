using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void CurrencyChanged();
public class GameManager : Singleton<GameManager>
{
    public event CurrencyChanged Changed;
    public TowerBtn ClickedBtn { get; set; }
    private bool gameOver = false;
    [SerializeField]
    private GameObject infoMenu;
    [SerializeField]
    private Text infoNameTxt;
    [SerializeField]
    private Text infoDescTxt;
    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject optionsMenu;
    private Tower selectedTower;
    private int currency;
    [SerializeField]
    private Text currencyTxt;
    public int Currency
    {
        get => currency;
        set
        {
            currency = value;
            currencyTxt.text = "<color=lime>cash:</color> " + value.ToString();
            OnCurrencyChanged();
        }
    }
    private int lives;
    public int Lives { get => lives; 
        set 
        {
            lives = value;
            if (lives <= 0)
            {
                this.lives = 0;
                GameOver();
            }
            livesTxt.text = "<color=lime>Health:</color> " + lives.ToString();
        } 
    }
    [SerializeField]
    private Text livesTxt;
    private int wave = 0;
    [SerializeField]
    private Text waveTxt;
    [SerializeField]
    private GameObject waveBtn;
    public bool WaveActive { get => activeMonsters.Count > 0; }

    private List<Monster> activeMonsters = new List<Monster>();
    public ObjectPool Pool { get; set; }

    [SerializeField]
    private GameObject upgradePanel;
    [SerializeField]
    private GameObject statsPanel;
    [SerializeField]
    private GameObject winPanel;
    [SerializeField]
    private Text sellTxt;
    [SerializeField]
    private Text sizeTxt;
    [SerializeField]
    private Text statsTxt;
    [SerializeField]
    private int lvlIndex;
    private bool infoIsOn;



    public void PickTower(TowerBtn towerBtn)
    {
        if (Currency >= towerBtn.Price)
        {
            this.ClickedBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite);
        }
    }

    public void BuyTower()
    {
        if (Currency >= ClickedBtn.Price)
        {
            Currency -= ClickedBtn.Price;
            Hover.Instance.Deactivate();
        }
    }
    public void OnCurrencyChanged()
    {
        if (Changed != null)
        {
            Changed();
        }
    }
    public void SelectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = tower;
        selectedTower.Select();
        upgradePanel.SetActive(true);
        sellTxt.text = "<color=lime>Sell </color>" + "\n" + (selectedTower.Price / 2).ToString();
    }
    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }
        upgradePanel.SetActive(false);
        selectedTower = null;
    }
    public void SellTower()
    {
        if (selectedTower != null)
        {
            Currency += selectedTower.Price / 2;
            selectedTower.GetComponentInParent<TileScript>().IsEmpty = true;
            Destroy(selectedTower.transform.parent.gameObject);
            DeselectTower();
        }
    }
    public void SetInfoInMenu(Monster monster)
    {
        infoNameTxt.text = monster.InfoNameTxt;
        infoDescTxt.text = monster.InfoDescTxt;
    }
    public void ShowInfoAboutMonster()
    {
        infoMenu.SetActive(!infoMenu.activeSelf);
    }
    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (selectedTower == null && !Hover.Instance.IsVisible && infoMenu.activeSelf == false)
            {
                ShowInGameMenu();

            }
            else if (Hover.Instance.IsVisible)
            {
                DropTower();
            }
            else if (selectedTower != null)
            {
                DeselectTower();
            }
            else if (infoMenu.activeSelf == true)
            {
                ShowInfoAboutMonster();
            }
        }
    }
    public void StartWave()
    {
        Currency += 5;
        wave++;
        waveTxt.text = string.Format("Wave: <color=lime>{0}</color>", wave);
        StartCoroutine(SpawnWave());
        waveBtn.SetActive(false);
        if (wave == 6)
        {
            WinLevel();
        }
    }
    private IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    private void WinLevel()
    {
        SoundManager.Instance.PlaySfx("CorrectAnswer");
        winPanel.SetActive(true);
        StartCoroutine(Wait(1f));
    }

    private IEnumerator MonsterSpawn(string monsterType, int countOfMonsters)
    {
        for (int i = 0; i < countOfMonsters; i++)
        {
            Monster monster = Pool.GetObject(monsterType).GetComponent<Monster>();
            monster.Spawn();
            activeMonsters.Add(monster);
            yield return new WaitForSeconds(0.75f);
        }
    }
    private IEnumerator SpawnWave()
    {
        if (lvlIndex == 1)
        {
            switch (wave)
            {
                case 1:
                    yield return MonsterSpawn("enemyBot1", 5);
                    break;
                case 2:
                    yield return MonsterSpawn("enemyBot1", 10);
                    break;
                case 3:
                    yield return MonsterSpawn("enemyBot1", 15);
                    break;
                case 4:
                    yield return MonsterSpawn("enemyBot1", 15);
                    break;
                case 5:
                    yield return MonsterSpawn("enemyBot1", 25);
                    break;
                default:
                    break;

            }
        }
        if (lvlIndex == 2)
        {
            switch (wave)
            {
                case 1:
                    yield return MonsterSpawn("enemyBot1", 10);
                    break;
                case 2:
                    yield return MonsterSpawn("enemyBot1", 10);
                    yield return MonsterSpawn("zhuk", 2);
                    break;
                case 3:
                    yield return MonsterSpawn("zhuk", 3);
                    break;
                case 4:
                    yield return MonsterSpawn("enemyBot1", 20);
                    yield return MonsterSpawn("zhuk", 3);
                    break;
                case 5:
                    yield return MonsterSpawn("zhuk", 10);
                    break;
                default:
                    break;

            }
        }
        if (lvlIndex == 3)
        {
            switch (wave)
            {
                case 1:
                    yield return MonsterSpawn("enemyBot1", 10);
                    yield return MonsterSpawn("zhuk", 2);
                    break;
                case 2:
                    yield return MonsterSpawn("zhuk", 4);
                    break;
                case 3:
                    yield return MonsterSpawn("worm", 10);
                    break;
                case 4:
                    yield return MonsterSpawn("enemyBot1", 15);
                    yield return MonsterSpawn("worm", 10);
                    yield return MonsterSpawn("zhuk", 6);
                    break;
                case 5:
                    yield return MonsterSpawn("enemyBot1", 12);
                    yield return MonsterSpawn("worm", 21);
                    yield return MonsterSpawn("zhuk", 5);
                    break;
                default:
                    break;

            }
        }
    }
    public void RemoveMonster(Monster monster)
    {
        activeMonsters.Remove(monster);
        if (!WaveActive && !gameOver)
        {
            waveBtn.SetActive(true);
        }
    }
    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }
    private void Update()
    {
        HandleEscape();
    }
    private void Start()
    {
        Lives = 10;
        Currency = 5;
        if (PlayerPrefs.GetInt("IsRightAnswer") == 1)
        {
            Currency += 2;
        }
    }

    public void ShowStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }
    public void SetTooltipExit(string txt)
    {
        statsTxt.text = txt;
        sizeTxt.text = txt;
    }
    public void ShowInGameMenu()
    {
        if (optionsMenu.activeSelf)
        {
            ShowMainMenu();
        }
        else
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (!pauseMenu.activeSelf)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }
    private void DropTower()
    {
        ClickedBtn = null;
        Hover.Instance.Deactivate();
    }
    public void ShowOptions()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void ShowMainMenu()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}
