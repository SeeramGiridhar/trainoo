using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneSetup : MonoBehaviour
{
    [ContextMenu("Setup Game Scene")]
    public void SetupGameScene()
    {
        Debug.Log("Starting game scene setup...");
        GameObject canvasGO = CreateCanvas();
        Transform canvasTransform = canvasGO.transform;
        CreateTextElements(canvasTransform);
        CreateButtons(canvasTransform);
        CreateListContainers(canvasTransform);
        CreateGameManager();
        CreateUIManager(canvasTransform);
        CreateGameMapRenderer();
        Debug.Log("Game scene setup complete!");
    }

    private GameObject CreateCanvas()
    {
        Canvas existingCanvas = FindObjectOfType<Canvas>();
        if (existingCanvas != null) return existingCanvas.gameObject;
        
        GameObject canvasGO = new GameObject("Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        canvasGO.AddComponent<GraphicRaycaster>();
        return canvasGO;
    }

    private void CreateTextElements(Transform canvasTransform)
    {
        CreateTextElement("BalanceText", canvasTransform, new Vector2(-400, -50), TextAlignmentOptions.TopLeft, "$50,000", 36);
        CreateTextElement("DayText", canvasTransform, new Vector2(0, -50), TextAlignmentOptions.Top, "Day 1", 36);
        CreateTextElement("TrainCountText", canvasTransform, new Vector2(300, -50), TextAlignmentOptions.TopRight, "Trains: 0", 24);
        CreateTextElement("StationCountText", canvasTransform, new Vector2(300, -100), TextAlignmentOptions.TopRight, "Stations: 0", 24);
        CreateTextElement("InfoText", canvasTransform, Vector2.zero, TextAlignmentOptions.Center, "Welcome to Trainoo!", 28);
    }

    private GameObject CreateTextElement(string name, Transform parent, Vector2 pos, TextAlignmentOptions align, string text, int size)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(400, 100);
        
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = size;
        tmp.alignment = align;
        tmp.color = Color.white;
        return go;
    }

    private void CreateButtons(Transform canvasTransform)
    {
        GameObject controlPanel = new GameObject("ControlPanel");
        controlPanel.transform.SetParent(canvasTransform, false);
        RectTransform panelRect = controlPanel.AddComponent<RectTransform>();
        panelRect.anchoredPosition = new Vector2(0, 100);
        panelRect.sizeDelta = new Vector2(600, 120);
        controlPanel.AddComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f, 0.8f);

        CreateButton("NextDayButton", controlPanel.transform, new Vector2(-250, 0), "Next Day");
        CreateButton("PauseButton", controlPanel.transform, new Vector2(-50, 0), "Pause");
        CreateButton("SpeedUpButton", controlPanel.transform, new Vector2(150, 0), "Speed Up");

        GameObject buildPanel = new GameObject("BuildPanel");
        buildPanel.transform.SetParent(canvasTransform, false);
        RectTransform buildRect = buildPanel.AddComponent<RectTransform>();
        buildRect.anchoredPosition = new Vector2(-800, 0);
        buildRect.sizeDelta = new Vector2(300, 400);
        buildPanel.AddComponent<Image>().color = new Color(0.2f, 0.2f, 0.3f, 0.9f);

        CreateButton("AddStationButton", buildPanel.transform, new Vector2(0, 100), "Add Station\n($5,000)");
        CreateButton("AddTrainButton", buildPanel.transform, new Vector2(0, 0), "Add Train\n($10,000)");
        CreateButton("BuildRouteButton", buildPanel.transform, new Vector2(0, -100), "Build Route\n($2,000/unit)");
    }

    private GameObject CreateButton(string name, Transform parent, Vector2 pos, string text)
    {
        GameObject btn = new GameObject(name);
        btn.transform.SetParent(parent, false);
        RectTransform btnRect = btn.AddComponent<RectTransform>();
        btnRect.anchoredPosition = pos;
        btnRect.sizeDelta = new Vector2(150, 80);
        
        Image btnImg = btn.AddComponent<Image>();
        btnImg.color = new Color(0.2f, 0.6f, 0.8f, 1f);
        
        Button btnComp = btn.AddComponent<Button>();
        btnComp.targetGraphic = btnImg;
        
        GameObject txtGo = new GameObject("Text");
        txtGo.transform.SetParent(btn.transform, false);
        RectTransform txtRect = txtGo.AddComponent<RectTransform>();
        txtRect.anchoredPosition = Vector2.zero;
        txtRect.sizeDelta = new Vector2(140, 70);
        
        TextMeshProUGUI txt = txtGo.AddComponent<TextMeshProUGUI>();
        txt.text = text;
        txt.fontSize = 16;
        txt.alignment = TextAlignmentOptions.Center;
        txt.color = Color.white;
        
        return btn;
    }

    private void CreateListContainers(Transform canvasTransform)
    {
        CreateScrollView("StationsPanel", canvasTransform, new Vector2(700, 300), "Stations");
        CreateScrollView("TrainsPanel", canvasTransform, new Vector2(700, -300), "Trains");
    }

    private GameObject CreateScrollView(string name, Transform parent, Vector2 pos, string title)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchoredPosition = pos;
        panelRect.sizeDelta = new Vector2(300, 300);
        panel.AddComponent<Image>().color = new Color(0.15f, 0.15f, 0.2f, 0.9f);

        GameObject titleGo = new GameObject("Title");
        titleGo.transform.SetParent(panel.transform, false);
        RectTransform titleRect = titleGo.AddComponent<RectTransform>();
        titleRect.anchoredPosition = new Vector2(0, -20);
        titleRect.sizeDelta = new Vector2(280, 40);
        TextMeshProUGUI titleTmp = titleGo.AddComponent<TextMeshProUGUI>();
        titleTmp.text = title;
        titleTmp.fontSize = 24;
        titleTmp.alignment = TextAlignmentOptions.Center;
        titleTmp.color = Color.white;

        GameObject content = new GameObject("Content");
        content.transform.SetParent(panel.transform, false);
        RectTransform contentRect = content.AddComponent<RectTransform>();
        contentRect.anchoredPosition = new Vector2(0, -100);
        contentRect.sizeDelta = new Vector2(280, 200);
        content.AddComponent<Image>().color = new Color(0.1f, 0.1f, 0.15f, 0.8f);
        
        return content;
    }

    private void CreateGameManager()
    {
        GameObject go = new GameObject("GameManager");
        go.AddComponent<GameManager>();
    }

    private void CreateUIManager(Transform canvasTransform)
    {
        GameObject go = new GameObject("UIManager");
        UIManager ui = go.AddComponent<UIManager>();
        ui.balanceText = canvasTransform.Find("BalanceText").GetComponent<TextMeshProUGUI>();
        ui.dayText = canvasTransform.Find("DayText").GetComponent<TextMeshProUGUI>();
        ui.trainCountText = canvasTransform.Find("TrainCountText").GetComponent<TextMeshProUGUI>();
        ui.stationCountText = canvasTransform.Find("StationCountText").GetComponent<TextMeshProUGUI>();
        ui.infoText = canvasTransform.Find("InfoText").GetComponent<TextMeshProUGUI>();
        ui.stationsListContainer = canvasTransform.Find("StationsPanel/Content");
        ui.trainsListContainer = canvasTransform.Find("TrainsPanel/Content");
        ui.nextDayButton = canvasTransform.Find("ControlPanel/NextDayButton").GetComponent<Button>();
        ui.pauseButton = canvasTransform.Find("ControlPanel/PauseButton").GetComponent<Button>();
        ui.speedUpButton = canvasTransform.Find("ControlPanel/SpeedUpButton").GetComponent<Button>();
        ui.addStationButton = canvasTransform.Find("BuildPanel/AddStationButton").GetComponent<Button>();
        ui.addTrainButton = canvasTransform.Find("BuildPanel/AddTrainButton").GetComponent<Button>();
        ui.buildRouteButton = canvasTransform.Find("BuildPanel/BuildRouteButton").GetComponent<Button>();
    }

    private void CreateGameMapRenderer()
    {
        GameObject go = new GameObject("GameMapRenderer");
        go.AddComponent<GameMapRenderer>();
    }
}