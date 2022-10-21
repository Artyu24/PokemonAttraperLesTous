using System.Collections.Generic;
using System.Linq;
using Object.Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.FilterWindow;

public class ToolVisuPoke : EditorWindow
{
    private List<PokeData> allPoke = new List<PokeData>();
    private PokeDataBase pokeDataBase;

    private VisualElement leftPane;
    private VisualElement rightPane;

    private ListView BoxLBot;

    [Header("Modif")] 
    private TextField pokeName;
    private TextField pokeDesc;
    private TextField pokeType;
    private Image pokeImage;


    [MenuItem("Window/PokémonCreator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ToolVisuPoke window = (ToolVisuPoke)EditorWindow.GetWindow(typeof(ToolVisuPoke));
        window.titleContent = new GUIContent("Pokémon Manager");


        window.Show();
    }

    private void OnEnable()
    {
        pokeName = new TextField();
        pokeDesc = new TextField();
        pokeType = new TextField();
        pokeImage = new Image();
    }

    void CreateGUI()
    {
        pokeDataBase = (PokeDataBase)AssetDatabase.LoadAssetAtPath("Assets/Script/Data/DataBase/PokeDataBase.asset", typeof(PokeDataBase));

        allPoke = pokeDataBase.PokeData;

        // Create a two-pane view with the left pane being fixed with
        var splitView = new TwoPaneSplitView(0, 300, TwoPaneSplitViewOrientation.Horizontal);


        // Add the view to the visual tree by adding it as a child to the root element
        rootVisualElement.Add(splitView);

        #region LEFT
        leftPane = new VisualElement();
        splitView.Add(leftPane);

        VisualElement BoxLTop = new VisualElement();
        //BoxLTop.style.backgroundColor = Color.yellow;
        BoxLTop.style.width = new Length(100, LengthUnit.Percent);
        BoxLTop.style.height = new Length(33, LengthUnit.Percent);
        BoxLTop.style.top = new Length(0, LengthUnit.Percent);
        CreateBorder(BoxLTop, 3);
        leftPane.Add(BoxLTop);

        BoxLBot = new ListView();
        //BoxLBot.style.backgroundColor = Color.magenta;
        BoxLBot.style.width = new Length(100, LengthUnit.Percent);
        BoxLBot.style.height = new Length(66, LengthUnit.Percent);
        leftPane.Add(BoxLBot);
        BoxLBot.headerTitle = "List of Pokémon";

        // Initialize the list view with all sprites' names
        PrintList(BoxLBot);

        // React to the user's selection
        BoxLBot.onSelectionChange += OnSpriteSelectionChange;
        #endregion

        SetUpRightPart(splitView);

    }

    void SetUpRightPart(TwoPaneSplitView splitView)
    {
        
        #region RIGHT
        rightPane = new VisualElement();
        splitView.Add(rightPane);

        #region TOP
        VisualElement BoxRTop = new VisualElement();
        rightPane.Add(BoxRTop);
        //BoxRTop.style.backgroundColor = Color.red;
        BoxRTop.style.width = new Length(100, LengthUnit.Percent);
        BoxRTop.style.height = new Length(33, LengthUnit.Percent);
        BoxRTop.style.top = new Length(0, LengthUnit.Percent);
        BoxRTop.style.flexWrap = new StyleEnum<Wrap>(Wrap.Wrap);
        BoxRTop.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
        //BoxRTop.style.paddingTop = 0;
        BoxRTop.style.alignItems = new StyleEnum<Align>(Align.FlexStart);


        var BoxRTopTL = new VisualElement();
        BoxRTop.Add(BoxRTopTL);
        BoxRTopTL.style.width = new Length(25, LengthUnit.Percent);
        BoxRTopTL.style.height = new Length(75, LengthUnit.Percent);
        BoxRTopTL.style.top = new Length(0, LengthUnit.Percent);
        BoxRTopTL.style.left = new Length(0, LengthUnit.Percent);
        CreateBorder(BoxRTopTL, 3);
        
        BoxRTopTL.Add(pokeImage);
        pokeImage.scaleMode = ScaleMode.ScaleToFit;


        var BoxRTopBL = new VisualElement();
        BoxRTop.Add(BoxRTopBL);
        BoxRTopBL.style.width = new Length(25, LengthUnit.Percent);
        BoxRTopBL.style.height = new Length(25, LengthUnit.Percent);
        BoxRTopBL.style.bottom = new Length(0, LengthUnit.Percent);
        BoxRTopBL.style.left = new Length(0, LengthUnit.Percent);
        CreateBorder(BoxRTopBL, 3);

        BoxRTopBL.Add(pokeType);
        pokeType.style.width = new Length(100, LengthUnit.Percent);
        pokeType.style.height = new Length(100, LengthUnit.Percent);
        pokeType.value = "TYPE";



        var BoxRTopTR = new VisualElement();
        BoxRTop.Add(BoxRTopTR);
        BoxRTopTR.style.width = new Length(75, LengthUnit.Percent);
        BoxRTopTR.style.height = new Length(25, LengthUnit.Percent);
        BoxRTopTR.style.top = new Length(0, LengthUnit.Percent);
        BoxRTopTR.style.right = new Length(0, LengthUnit.Percent);
        CreateBorder(BoxRTopTR, 3);

        BoxRTopTR.Add(pokeName);
        pokeName.style.width = new Length(100, LengthUnit.Percent);
        pokeName.style.height = new Length(100, LengthUnit.Percent);
        pokeName.value = "NAME";


        var BoxRTopBR = new VisualElement();
        BoxRTop.Add(BoxRTopBR);
        BoxRTopBR.style.width = new Length(75, LengthUnit.Percent);
        BoxRTopBR.style.height = new Length(75, LengthUnit.Percent);
        BoxRTopBR.style.bottom = new Length(0, LengthUnit.Percent);
        BoxRTopBR.style.right = new Length(0, LengthUnit.Percent);
        CreateBorder(BoxRTopBR, 3);

        BoxRTopBR.Add(pokeDesc);
        pokeDesc.style.width = new Length(100, LengthUnit.Percent);
        pokeDesc.style.height = new Length(100, LengthUnit.Percent);
        pokeDesc.value = "DESC";
        #endregion

        #region MID
        var BoxRMid = new VisualElement();
        rightPane.Add(BoxRMid);
        BoxRMid.style.backgroundColor = Color.green;
        BoxRMid.style.width = new Length(100, LengthUnit.Percent);
        BoxRMid.style.height = new Length(33, LengthUnit.Percent);
        #endregion

        #region Bot
        var BoxRBot = new VisualElement();
        rightPane.Add(BoxRBot);
        BoxRBot.style.backgroundColor = Color.blue;
        BoxRBot.style.width = new Length(100, LengthUnit.Percent);
        BoxRBot.style.height = new Length(33, LengthUnit.Percent);
        #endregion


        #endregion 

    }

    void PrintList(ListView Element)
    {
        //leftPane.Clear();

        Element.makeItem = () => new Label();
        Element.bindItem = (item, index) => { (item as Label).text = index + " - " + allPoke[index].name; };
        Element.itemsSource = allPoke;

    }

    void OnSpriteSelectionChange(IEnumerable<object> selectedItems)
    {
        // Clear all previous content from the pane
        //rightPane.Clear();

        PokeData pokeData = null;

        //PokeData PD = (PokeData) selectedItems;
        foreach (var VARIABLE in selectedItems)
        {
            pokeData = VARIABLE as PokeData;
        }

        // Get the selected sprite
        var selectedSprite = allPoke.Find(x => x == pokeData).sprite;
        // Add a new Image control and display the sprite
        pokeImage.scaleMode = ScaleMode.ScaleToFit;
        pokeImage.sprite = selectedSprite;

        var selectedName = allPoke.Find(x => x == pokeData).name;
        pokeName.value = selectedName;

        var selectedDesc = allPoke.Find(x => x == pokeData).desc;
        pokeDesc.value = selectedDesc;

        var selectedType = allPoke.Find(x => x == pokeData).TYPE.ToString();
        pokeType.value = selectedType;

    }

    void CreateBorder(VisualElement elem, int borderSize)
    {
        elem.style.borderBottomColor = Color.black;
        elem.style.borderLeftColor = Color.black;
        elem.style.borderTopColor = Color.black;
        elem.style.borderRightColor = Color.black;


        elem.style.borderBottomWidth = borderSize;
        elem.style.borderRightWidth = borderSize;
        elem.style.borderTopWidth = borderSize;
        elem.style.borderLeftWidth = borderSize;
    }

}
