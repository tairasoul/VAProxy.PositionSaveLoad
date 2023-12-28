using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PositionSaveLoad
{
    [BepInPlugin("tairasoul.position.saveload", "PositionSaveLoad", "1.0.0")]
    public class Plugin: BaseUnityPlugin
    {
        internal ConfigEntry<KeyCode> SaveKey;
        internal ConfigEntry<KeyCode> RestoreKey;
        internal Vector3 SavedPos;
        internal bool SavePosDown = false;
        internal bool RestorePosDown = false;
        internal Text posText;
        internal void Awake()
        {
            SaveKey = Config.Bind("Keybinds", "SavePos", KeyCode.N, "Keybind to save your position.");
            RestoreKey = Config.Bind("Keybinds", "RestorePos", KeyCode.M, "Keybind to load your position.");
            SceneManager.activeSceneChanged += SceneLoad;
        }

        internal void SceneLoad(Scene old, Scene newS)
        {
            if (newS.name != "Intro" && newS.name != "Menu")
            {
                GameObject Area = GameObject.Find("UI/ui/Area");
                GameObject Pos = GameObject.Instantiate(Area);
                Pos.name = "Pos";
                Pos.transform.SetParent(Area.transform.parent);
                Destroy(Pos.GetComponent<Locations>());
                Pos.GetComponent<RectTransform>().anchoredPosition = new Vector2(-805.1504f, -584.125f);
                posText = Pos.GetComponent<Text>();
                posText.alignment = TextAnchor.UpperLeft;
            }
        }

        internal void Update()
        {
            if (UnityInput.Current.GetKeyDown(SaveKey.Value))
            {
                if (!SavePosDown)
                {
                    SavePosDown = true;
                    Inventory inv = GameObject.FindFirstObjectByType<Inventory>();
                    GameObject obj = inv.gameObject;
                    SavedPos = obj.transform.position;
                    posText.text = $"{SavedPos.x} {SavedPos.y} {SavedPos.z}";
                }
            }
            else
            {
                SavePosDown = false;
            }
            if (UnityInput.Current.GetKeyDown(RestoreKey.Value))
            {
                if (!RestorePosDown)
                {
                    RestorePosDown = true;
                    Inventory inv = GameObject.FindFirstObjectByType<Inventory>();
                    GameObject obj = inv.gameObject;
                    obj.transform.position = SavedPos;
                }
            }
            else
            {
                RestorePosDown = false;
            }
        }
    }
}
