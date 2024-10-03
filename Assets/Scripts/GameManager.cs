using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    private GroundPiece[] allGroundPiece;
    void Start()
    {
        
    }

   private void setUpNewLevel()
   {
    allGroundPiece = FindObjectsOfType<GroundPiece>();
   }

   private void Awake()
   {
    if (singleton == null)
    {
        singleton = this;
    }else if(singleton != this)
    {
        Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
   }
   private void  OnEnable()
   {
    SceneManager.sceneLoaded += OnLevelFinishedLoading;
   }
   private void OnLevelFinishedLoading(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
   {
    setUpNewLevel();
   }

   public void checkComplete()
   {
    bool isFinished = true;
    for (int i = 0; i < allGroundPiece.Length; i++)
    {
        if (allGroundPiece[i].isColour == false)
        {
            isFinished = false;
            break;
        }
    }
    if (isFinished)
    {
        NextLevel();
    }
   }
   public void NextLevel()
   {
    if (SceneManager.GetActiveScene().buildIndex == 4)
    {
     SceneManager.LoadScene(0);
       
    }else 
    {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

   }
}
