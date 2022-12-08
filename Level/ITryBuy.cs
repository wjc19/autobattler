using UnityEngine;

public interface ITryBuy 
{
    bool TryBuyCharacter(Character character, Player player);
    bool TryBuyItem(Item item, Player player);
}