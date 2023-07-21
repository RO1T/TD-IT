using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private int price;
    [SerializeField]
    private Text priceTxt;

    public GameObject TowerPrefab { get => towerPrefab; }
    public Sprite Sprite { get => sprite; }
    public int Price { get => price; }

    private void Start()
    {
        PriceCheck();
        priceTxt.text = Price.ToString();
        GameManager.Instance.Changed += new CurrencyChanged(PriceCheck);
    }
    private void PriceCheck()
    {
        if (price <= GameManager.Instance.Currency)
        {
            GetComponent<Image>().color = Color.white;
            priceTxt.color = Color.white;
        }
        else
        {
            GetComponent<Image>().color = Color.grey;
            priceTxt.color = Color.grey;
        }
    }
    public void ShowInfo(string type)
    {
        string tooltip = string.Empty;
        var tower = towerPrefab.GetComponentInChildren<Tower>();
        switch (type)
        {
            case "Tower":
                tooltip = string.Format("<color=lime><size=20>Name: Tower</size></color>\nDamage: {0}\nAttack speed: {1}", tower.Damage, tower.AttackCooldown);
                break;
            case "Anti-Bug":
                tooltip = string.Format("<color=lime><size=20>Name: Anti-Bug</size></color>\nDamage: {0}\nAttack speed: {1}", tower.Damage, tower.AttackCooldown);
                break;
            case "Pig Thrower":
                tooltip = string.Format("<color=lime><size=20>Name: Pig Thrower</size></color>\nDamage: {0}\nAttack speed: {1}", tower.Damage, tower.AttackCooldown);
                break;
            case "Tesla":
                tooltip = string.Format("<color=lime><size=20>Name: Tesla</size></color>\nDamage: {0}\nAttack speed: {1}", tower.Damage, tower.AttackCooldown);
                break;
            case "Key Logger":
                tooltip = string.Format("<color=lime><size=20>Name: Key Logger</size></color>\nDamage: {0}\nAttack speed: {1}", tower.Damage, tower.AttackCooldown);
                break;
            default:
                break;
        }
        GameManager.Instance.SetTooltipExit(tooltip);
        GameManager.Instance.ShowStats();
    }
}
