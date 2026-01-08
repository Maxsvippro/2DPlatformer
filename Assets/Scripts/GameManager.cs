using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;
    public bool fruitHaveRandomLook;
    public int fruitCollected;
    private void Awake()
    {   
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddFruit()
    {
        fruitCollected++;
        Debug.Log("Fruits Collected: " + fruitCollected);
    }
    public bool FruitHaveRandomLook() => fruitHaveRandomLook;
}
