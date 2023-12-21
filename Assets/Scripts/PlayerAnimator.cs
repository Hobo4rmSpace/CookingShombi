using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

  private const string kIsWalking = "IsWalking";

  [SerializeField] private Player player;

  private Animator animator;

  private void Awake() {
    animator = GetComponent<Animator>();
  }

  private void Update() {
    animator.SetBool(kIsWalking, player.IsWalking());
  }
}
