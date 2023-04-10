using Items.Scriptable;
using System.Collections.Generic;
using UnityEngine;

namespace Items.Storage
{
    [CreateAssetMenu(fileName = "ItemsStorage", menuName = "ItemsSystem/ItemsStorage")]
    public class ItemsStorage : ScriptableObject
    {
        [field: SerializeField] public List<BaseItemScriptable> ItemScriptables { get; private set; }
    }
}
