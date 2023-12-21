using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter {

  public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
  public class OnProgressChangedEventArgs : EventArgs {
    public float progressNormalized;
  }

  [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

  private int cuttingProgress;

  public override void Interact(Player player) {
    if (!HasKitchenObject()) {
      if (player.HasKitchenObject()) {
        if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
          player.GetKitchenObject().SetKitchenObjectParent(this);
          cuttingProgress = 0;

          CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

          OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
            progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
          });
        }
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

  public override void InteractAlternate(Player player) {
    if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
      cuttingProgress++;

      CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

      OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
      });

      if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
        KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

        GetKitchenObject().DestroySelf();

        KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
      }
    }
  }

  private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

    if (cuttingRecipeSO != null) {
      return true;
    } else {
      return false;
    }
  }

  private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

    if (cuttingRecipeSO.input == inputKitchenObjectSO) {
      return cuttingRecipeSO.output;
    } else {
      return null;
    }
  }

  private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
    foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
      if (cuttingRecipeSO.input == inputKitchenObjectSO) {
        return cuttingRecipeSO;
      }
    }
    return null;
  }

}
