using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {

  // Transform and GameObject are the same thing.
  [SerializeField] private KitchenObjectSO kitchenObjectSO;

  public override void Interact(Player player) {
    if (!HasKitchenObject()) {
      if (player.HasKitchenObject()) {
        player.GetKitchenObject().SetKitchenObjectParent(this);
      } else {
        // Player not carrying anything
      }
    } else {
      // There is a KitchenObject here
      if (player.HasKitchenObject()) {
        // Player is carrying something
      } else {
        // Player is not carrying anything
        GetKitchenObject().SetKitchenObjectParent(player);
      }
    }
  }

}
