using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour 
{
    public static SaveManager instance;
    [SerializeField] private string fileName;
    private GameData gameData;
    private List<ISaveManager> saveManagers = new List<ISaveManager>();
    private FileDataHandler dataHandler;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance.gameObject);
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }

    private void Start()
    {
        
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();
        if(this.gameData == null)
        {
            Debug.Log("No Data");
            NewGame();
        }
        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
        Debug.Log("Loaded soul " + gameData.soul);
    }

    public void SaveGame()
    {
        foreach(ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManager = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManager);
    }
}
