using System.Collections.Generic;
using System.Linq;
using Object.Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ToolVisuPoke : EditorWindow
{
    private List<PokeData> allPoke = new List<PokeData>();
    private PokeDataBase pokeDataBase;

    private ListView leftPane;

    private VisualElement rightPane;

    [MenuItem("Window/PokémonCreator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ToolVisuPoke window = (ToolVisuPoke)EditorWindow.GetWindow(typeof(ToolVisuPoke));
        window.titleContent = new GUIContent("Pokémon Manager");

        window.Show();
    }


    void CreateGUI()
    {
        pokeDataBase = (PokeDataBase)AssetDatabase.LoadAssetAtPath("Assets/Script/Data/DataBase/A.asset", typeof(PokeDataBase));

        allPoke = pokeDataBase.PokeData;

        // Create a two-pane view with the left pane being fixed with
        var splitView = new TwoPaneSplitView(0, 300, TwoPaneSplitViewOrientation.Horizontal);


        // Add the view to the visual tree by adding it as a child to the root element
        rootVisualElement.Add(splitView);
        // A TwoPaneSplitView always needs exactly two child elements
        leftPane = new ListView();
        splitView.Add(leftPane);
        rightPane = new VisualElement();
        splitView.Add(rightPane);
        // Initialize the list view with all sprites' names
        PrintList(leftPane);

        // React to the user's selection
        leftPane.onSelectionChange += OnSpriteSelectionChange;
        



        

    }

    void OnGUI()
    {
        if (allPoke != pokeDataBase.PokeData)
        {
            allPoke = pokeDataBase.PokeData;
            PrintList(leftPane);
        }
           

        foreach (PokeData poke in allPoke)
        {
            Debug.Log(poke.name);
        }
    }

    void PrintList(ListView leftPane)
    {
        leftPane.makeItem = () => new Label();
        leftPane.bindItem = (item, index) => { (item as Label).text = allPoke[index].name; };
        leftPane.itemsSource = allPoke;
    }

    void OnSpriteSelectionChange(IEnumerable<object> selectedItems)
    {
        // Clear all previous content from the pane
        rightPane.Clear();

        PokeData PD = (PokeData) selectedItems;

        // Get the selected sprite
        var selectedSprite = PD.sprite;
        if (selectedSprite == null)
            return;
        
        // Add a new Image control and display the sprite
        var spriteImage = new Image();
        spriteImage.scaleMode = ScaleMode.ScaleToFit;
        spriteImage.sprite = selectedSprite;

        // Add the Image control to the right-hand pane
        rightPane.Add(spriteImage);
    }
}
