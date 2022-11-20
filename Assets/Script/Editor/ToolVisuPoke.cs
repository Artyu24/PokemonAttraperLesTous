using System;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.Common.Serialization;
using Object.Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static Object.Data.BaseData;
using static UnityEditor.Progress;
using static UnityEditor.Rendering.FilterWindow;

public class ToolVisuPoke : EditorWindow
{
    private List<PokeData> allPoke;
    private PokeDataBase pokeDataBase;

    private List<AttackData> allAttack;
    private AttackDatabase attackDataBase;

    private VisualElement leftPane;
    private VisualElement rightPane;

    private ListView BoxLBot;

    [Header("Modif")] 
    private TextField pokeName;
    private TextField pokeDesc;
    private TextField pokeType;
    private Image pokeImage;
    private int ID;

    [Header("Attacks")] 
    private PopupField<string>[] attackPopupFields;
    private TextElement[] attackDamage;
    private TextElement[] attackPP;
    private TextElement[] attackType;
    private TextElement[] attackDesc;

    [Header("stats")] 
    private TextField dmgStat;
    private TextField hpStat;
    private TextField defStat;
    private TextField speedStat;

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

        attackPopupFields = new PopupField<string>[4];
        attackDamage = new TextElement[4];
        attackPP = new TextElement[4];
        attackType = new TextElement[4];
        attackDesc = new TextElement[4];

        for (int i = 0; i < 4; i++)
        {
            attackPopupFields[i] = new PopupField<string>();
            attackDamage[i] = new TextElement();
            attackPP[i] = new TextElement();
            attackType[i] = new TextElement();
            attackDesc[i] = new TextElement();
        }
        dmgStat = new TextField();
        hpStat = new TextField();
        defStat = new TextField();
        speedStat = new TextField();

        }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (attackDesc[i].text != allAttack[attackPopupFields[i].index].desc)
            {
                UpdateAttack(i, attackPopupFields[i].index);
            }
        }
    }

    void CreateGUI()
    {
        pokeDataBase = (PokeDataBase)AssetDatabase.LoadAssetAtPath("Assets/Script/Data/DataBase/PokeDataBase.asset", typeof(PokeDataBase));
        attackDataBase = (AttackDatabase)AssetDatabase.LoadAssetAtPath("Assets/Script/Data/DataBase/AttackDatabase.asset", typeof(AttackDatabase));

        allPoke = pokeDataBase.PokeData;
        allAttack = attackDataBase.AttackData;

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

        Button save = new Button();
        save.text = "SAVE CHANGES";
        save.clicked += SaveOnclicked;
        BoxLTop.Add(save);

        Button updatePokeList = new Button();
        updatePokeList.text = "UPDATE POKE LIST";
        updatePokeList.clicked += UpdatePokeList;
        BoxLTop.Add(updatePokeList);

        Button createNewPoke = new Button();
        createNewPoke.text = "CREATE A NEW POKE";
        createNewPoke.clicked += CreateNewPoke;
        BoxLTop.Add(createNewPoke);

        Button deletePoke = new Button();
        deletePoke.text = "DELETE SELECTED POKE";
        deletePoke.clicked += DeletePoke;
        BoxLTop.Add(deletePoke);



        BoxLBot = new ListView();
        //BoxLBot.style.backgroundColor = Color.magenta;
        BoxLBot.style.width = new Length(100, LengthUnit.Percent);
        BoxLBot.style.height = new Length(66, LengthUnit.Percent);
        leftPane.Add(BoxLBot);
        BoxLBot.headerTitle = "List of Pokémon";

        // Initialize the list view with all sprites' names
        PrintPokeList(BoxLBot);

        // React to the user's selection
        BoxLBot.onSelectionChange += OnSpriteSelectionChange;
        #endregion

        SetUpRightPart(splitView);

    }

    private void UpdatePokeList()
    {
        PrintPokeList(BoxLBot);
    }

    private void CreateNewPoke()
    {
        allPoke.Add(new PokeData("", allPoke.Count));
        UpdatePokeList();
    }

    private void DeletePoke()
    {
        allPoke.RemoveAt(ID);
        UpdatePokeList();
    }

    private void SaveOnclicked()
    {
        allPoke[ID].name = pokeName.value;
        allPoke[ID].desc = pokeDesc.value;
        PokeType currentPokeType;
        if (Enum.TryParse<PokeType>(pokeType.value, out currentPokeType))
            allPoke[ID].TYPE = currentPokeType;
        for (int i = 0; i < 4; i++)
            allPoke[ID].attackIDlist[i] = attackPopupFields[i].index;
        int p;
        if(int.TryParse(dmgStat.value, out p))
            allPoke[ID].dmg = p;
        if (int.TryParse(hpStat.value, out p))
            allPoke[ID].hpMax = p;
        if (int.TryParse(defStat.value, out p))
            allPoke[ID].def = p;
        if (int.TryParse(speedStat.value, out p))
            allPoke[ID].speed = p;

        UpdatePokeList();
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
        BoxRTop.style.height = new Length(45, LengthUnit.Percent);
        BoxRTop.style.top = new Length(0, LengthUnit.Percent);
        BoxRTop.style.flexWrap = new StyleEnum<Wrap>(Wrap.Wrap);
        BoxRTop.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
        //BoxRTop.style.paddingTop = 0;
        BoxRTop.style.alignItems = new StyleEnum<Align>(Align.FlexStart);


        var BoxRTopTL = new VisualElement();//Image du poke
        BoxRTop.Add(BoxRTopTL);
        BoxRTopTL.style.width = new Length(25, LengthUnit.Percent);
        BoxRTopTL.style.height = new Length(75, LengthUnit.Percent);
        BoxRTopTL.style.top = new Length(0, LengthUnit.Percent);
        BoxRTopTL.style.left = new Length(0, LengthUnit.Percent);
        CreateBorder(BoxRTopTL, 3);
        
        BoxRTopTL.Add(pokeImage);
        pokeImage.scaleMode = ScaleMode.ScaleToFit;


        var BoxRTopBL = new VisualElement();//PokeType
        BoxRTop.Add(BoxRTopBL);
        BoxRTopBL.style.width = new Length(25, LengthUnit.Percent);
        BoxRTopBL.style.height = new Length(25, LengthUnit.Percent);
        BoxRTopBL.style.bottom = new Length(0, LengthUnit.Percent);
        BoxRTopBL.style.left = new Length(0, LengthUnit.Percent);
        CreateBorder(BoxRTopBL, 3);

        BoxRTopBL.Add(pokeType);
        pokeType.style.width = new Length(100, LengthUnit.Percent);
        pokeType.style.height = new Length(100, LengthUnit.Percent);
        pokeType.value = "PokeType";

        var BoxRTopTR = new VisualElement();//Name
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


        var BoxRTopBR = new VisualElement();//desc
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
        //BoxRMid.style.backgroundColor = Color.green;
        BoxRMid.style.width = new Length(100, LengthUnit.Percent);
        BoxRMid.style.height = new Length(35, LengthUnit.Percent);
        BoxRMid.style.flexWrap = new StyleEnum<Wrap>(Wrap.Wrap);
        BoxRMid.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
        BoxRMid.style.alignContent = new StyleEnum<Align>(Align.Center);
        BoxRMid.style.left = new Length(5, LengthUnit.Percent);

        var BoxRMidTR = CreateAttackBox(BoxRMid,0, attackPopupFields[0], attackDamage[0], attackPP[0], attackType[0], attackDesc[0]);
        var BoxRMidTL = CreateAttackBox(BoxRMid,1, attackPopupFields[1], attackDamage[1], attackPP[1], attackType[1], attackDesc[1]);
        var BoxRMidBL = CreateAttackBox(BoxRMid,2, attackPopupFields[2], attackDamage[2], attackPP[2], attackType[2], attackDesc[2]);
        var BoxRMidBR = CreateAttackBox(BoxRMid,3, attackPopupFields[3], attackDamage[3], attackPP[3], attackType[3], attackDesc[3]);

        #endregion

        #region Bot
        var BoxRBot = new VisualElement();
        rightPane.Add(BoxRBot);
        //BoxRBot.style.backgroundColor = Color.blue;
        BoxRBot.style.width = new Length(100, LengthUnit.Percent);
        BoxRBot.style.height = new Length(20, LengthUnit.Percent);
        BoxRBot.style.alignContent = new StyleEnum<Align>(Align.Center);
        BoxRBot.style.alignItems = new StyleEnum<Align>(Align.FlexStart);

        var boxDmg = new VisualElement();
        dmgStat.value = 99999.ToString();
        dmgStat.label = "Damage Stat : ";
        BoxRBot.Add(boxDmg);
        boxDmg.Add(dmgStat);
        boxDmg.style.height = new Length(25, LengthUnit.Percent);
        boxDmg.style.left = new Length(40, LengthUnit.Percent);


        var boxHp = new VisualElement();
        hpStat.value = 99999.ToString();
        hpStat.label = "HP Stat : ";
        BoxRBot.Add(boxHp);
        boxHp.Add(hpStat);
        boxHp.style.height = new Length(25, LengthUnit.Percent);
        boxHp.style.left = new Length(40, LengthUnit.Percent);


        var boxDef = new VisualElement();
        defStat.value = 99999.ToString();
        defStat.label = "Defense Stat : ";
        BoxRBot.Add(boxDef);
        boxDef.Add(defStat);
        boxDef.style.height = new Length(25, LengthUnit.Percent);
        boxDef.style.left = new Length(40, LengthUnit.Percent);

        var boxSpeed = new VisualElement();
        speedStat.value = 99999.ToString();
        speedStat.label = "Speed Stat : ";
        BoxRBot.Add(boxSpeed);
        boxSpeed.Add(speedStat);
        boxSpeed.style.height = new Length(25, LengthUnit.Percent);
        boxSpeed.style.left = new Length(40, LengthUnit.Percent);

        #endregion

        #endregion

    }


    private VisualElement CreateAttackBox(VisualElement _boxToAttach, int _attackNumber, PopupField<string> _attackPopupField, TextElement _attackDamage, TextElement _attackPP, TextElement _attackType, TextElement _attackDesc)
    {
        var _box = new VisualElement();
        _boxToAttach.Add(_box);
        _box.style.width = new Length(45, LengthUnit.Percent);
        _box.style.height = new Length(45, LengthUnit.Percent);

        _attackPopupField.choices = GetAllAttackName();
        _attackPopupField.label = "Attack No " + (_attackNumber + 1) + " :";
        _attackPopupField.index = _attackNumber;
        _box.Add(_attackPopupField);

        _attackDamage.text = "Damage : " + allAttack[_attackNumber].dmg;
        _box.Add(_attackDamage);
        
        _attackPP.text = "PP : " + allAttack[_attackNumber].pp;
        _box.Add(_attackPP);

        _attackType.text = "PokeType : " + allAttack[_attackNumber].TYPE;
        _box.Add(_attackType);

        _attackDesc.text = allAttack[_attackNumber].desc;
        _box.Add(_attackDesc);

        CreateBorder(_box, 2);
        return _box;
    }
    private List<string> GetAllAttackName()
    {
        int i = 0;
        List<string> attackName = new List<string>();
        foreach (var attack in allAttack)
        {
            attackName.Add(allAttack[i].name);
            i++;
        }
        return attackName;
    }

    private void UpdateAttack(int popupField, int attackID)
    {
        attackDamage[popupField].text = "Damage : " + allAttack[attackID].dmg.ToString();
        attackPP[popupField].text = "PP : " + allAttack[attackID].pp.ToString();
        attackType[popupField].text = "PokeType : " + allAttack[attackID].TYPE.ToString();
        attackDesc[popupField].text = allAttack[attackID].desc;
    }

    void PrintPokeList(ListView Element)
    {
        //leftPane.Clear();

        Element.makeItem = () => new Label();
        Element.bindItem = (item, index) => 
        { 
            (item as Label).text = index + " - " + allPoke[index].name;
            allPoke[index].ID = index;
        };
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

        ID = allPoke.Find(x => x == pokeData).ID;

        #region Top
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
        #endregion

        #region Mid
        for (int i = 0; i < 4; i++)
        {
            int selectedAttack = allPoke.Find(x => x == pokeData).attackIDlist[i];
            attackPopupFields[i].value = allAttack[selectedAttack].name; 
            attackDamage[i].text = allAttack[selectedAttack].dmg.ToString();
            attackPP[i].text = allAttack[selectedAttack].pp.ToString();
            attackType[i].text = allAttack[selectedAttack].TYPE.ToString();
            attackDesc[i].text = allAttack[selectedAttack].desc;
        }
        #endregion

        #region Bot
        var selectedDamage = allPoke.Find(x => x == pokeData).dmg;
        dmgStat.value = selectedDamage.ToString();

        var selectedHp = allPoke.Find(x => x == pokeData).hpMax;
        hpStat.value = selectedHp.ToString();

        var selectedDef = allPoke.Find(x => x == pokeData).def;
        defStat.value = selectedDef.ToString();

        var selectedSpeed = allPoke.Find(x => x == pokeData).speed;
        speedStat.value = selectedSpeed.ToString();
        #endregion
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
