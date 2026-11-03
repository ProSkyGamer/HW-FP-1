#region

using Runtime.Gameplay.Meta.Wallet;
using UnityEngine;

#endregion

[CreateAssetMenu(fileName = "StatisticsConfig", menuName = "Config/Statistics/StatisticsConfig", order = 0)]
public class StatisticsConfig : ScriptableObject
{
    [field: SerializeField] public CurrencyData StatisticsResetPrice;
}