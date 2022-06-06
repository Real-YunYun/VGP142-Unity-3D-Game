using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [Header("Camera Parameters")]
    public Vector3 Posistion = new Vector3(0, 10, -8);
    public float SmoothTime = 0.25f;
    public Transform Player;
    private Vector3 Velocity = Vector3.zero;

    private bool ExitingGame = false;

    [Header("Interface Parameters")]
    private Canvas UI_Canvas;
    private GameObject UI_HUD_Canvas;
    private GameObject UI_Menu_Canvas;
    private GameObject UI_Load_Canvas;
    private GameObject UI_Exit_Canvas;
    [HideInInspector] public GameObject InteractText;

    [Header("Cursor Settings")]
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        //Getting UI Children
        UI_Canvas = transform.Find("UI Canvas").GetComponent<Canvas>();
        InteractText = transform.Find("UI Canvas/InteractText").gameObject;
        UI_HUD_Canvas = transform.Find("UI Canvas/HUD Canvas").gameObject;
        UI_Menu_Canvas = transform.Find("UI Canvas/Menu Canvas").gameObject;
        UI_Load_Canvas = transform.Find("UI Canvas/Menu Canvas/Load Game Canvas").gameObject;
        UI_Exit_Canvas = transform.Find("UI Canvas/Menu Canvas/Exit Canvas").gameObject;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        //Cursor.SetCursor(cursorTexture, Vector2.zero, cursorMode);
        transform.position = Vector3.SmoothDamp(transform.position, Player.position + Posistion, ref Velocity, SmoothTime);
        HandleUI();
    }

    public void HandleUI(GameState gameState = GameState.Title)
    {
        if (GameManager.Instance.CurrentGameState == GameState.Title)
        {
            UI_HUD_Canvas.SetActive(false);
            UI_Menu_Canvas.SetActive(false);
            UI_Load_Canvas.SetActive(false);
            UI_Exit_Canvas.SetActive(false);
        }
        if (GameManager.Instance.CurrentGameState == GameState.Pause)
        {
            UI_HUD_Canvas.SetActive(false);
            UI_Menu_Canvas.SetActive(true);
            UI_Load_Canvas.SetActive(false);
            UI_Exit_Canvas.SetActive(false);

            //Handling UI Buttons
            transform.Find("UI Canvas/Menu Canvas/Parent/Save Game Button").GetComponent<Button>().interactable = GameManager.Instance.CurrentLevel == "HUB";
            transform.Find("UI Canvas/Menu Canvas/Parent/Exit to Lobby Button").GetComponent<Button>().interactable = GameManager.Instance.CurrentLevel != "HUB";
        }
        if (GameManager.Instance.CurrentGameState == GameState.Load)
        {
            UI_HUD_Canvas.SetActive(false);
            UI_Menu_Canvas.SetActive(true);
            UI_Load_Canvas.SetActive(true);
            UI_Exit_Canvas.SetActive(false);
        }
        if (GameManager.Instance.CurrentGameState == GameState.Playing)
        {
            UI_HUD_Canvas.SetActive(true);
            UI_Menu_Canvas.SetActive(false);
            UI_Load_Canvas.SetActive(false);
            UI_Exit_Canvas.SetActive(false);

            //Handling Health Bar
            string Health = "";
            for (int i = 1; i <= Player.gameObject.GetComponent<PlayerController>().Health; i++) Health += "/";
            UI_HUD_Canvas.transform.Find("Bars/Health Bar").GetComponent<Text>().text = Health;

            //Handling Energy Bar
            string Energy = "";
            for (int i = 1; i <= Player.gameObject.GetComponent<PlayerController>().Energy; i++) Energy += "/";
            UI_HUD_Canvas.transform.Find("Bars/Energy Bar").GetComponent<Text>().text = Energy;

            UI_HUD_Canvas.transform.Find("Bits/Text").GetComponent<Text>().text = "Bits: " +GameManager.Instance.Data.Bits.ToString();
        }
        if (GameManager.Instance.CurrentGameState == GameState.Exiting)
        {
            UI_HUD_Canvas.SetActive(false);
            UI_Menu_Canvas.SetActive(true);
            UI_Load_Canvas.SetActive(false);
            UI_Exit_Canvas.SetActive(true);
        }
    }

    public void Resume()
    {
        GameManager.Instance.PauseGame();
    }

    //Saving Handle
    public void SaveGame()
    {
        GameManager.Instance.SaveGame();
    }

    //Loading Handles
    public void PromptLoadGame()
    {
        GameManager.Instance.CurrentGameState = GameState.Load;
    }

    public void LoadGame(bool state)
    {
        if (state) GameManager.Instance.LoadGame();
        else GameManager.Instance.CurrentGameState = GameState.Pause;
    }

    //Exiting Game Handles
    public void PromptExit(bool ExitGame = true)
    {
        if (ExitGame) ExitingGame = true;
        else ExitingGame = false;
        GameManager.Instance.CurrentGameState = GameState.Exiting;
    }

    public void ExitTo(bool state)
    {
        if (state && !ExitingGame)
        {
            GameManager.Instance.PauseGame();
            GameManager.Instance.LoadGame();
            GameManager.Instance.CurrentGameState = GameState.Playing;
            SceneManager.LoadScene("HUB");
        }
        else if (state && ExitingGame) GameManager.Instance.ExitGame();
        else GameManager.Instance.CurrentGameState = GameState.Pause;
    }
}
