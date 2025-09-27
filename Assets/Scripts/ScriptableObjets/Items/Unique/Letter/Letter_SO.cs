using UnityEngine;

[CreateAssetMenu(fileName = "Letter", menuName = "Scriptable Objects/Items/Letter")]
public class Letter_SO : UniqueItem_SO
{
    [TextArea] public string message;
}