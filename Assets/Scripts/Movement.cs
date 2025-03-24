using System;
using UnityEngine;

class Movement {
	
	private const float pushingPower = 2000f;
	public static void MoveAwayFrom(Rigidbody2D mover, Rigidbody2D from, float force) {
		mover.AddForce((mover.position - from.position).normalized * force);
	}
	
	public static void BothMaybeMoveAway(Rigidbody2D mover, Rigidbody2D from, float force) {
		MoveAwayFrom(mover, from, force);
		MoveAwayFrom(from, mover, force);
		
	}
	public static void BothMoveAway(Rigidbody2D mover, Rigidbody2D from, float force) {
		MoveAwayFrom(mover, from, force);
		MoveAwayFrom(from, mover, force);
	}
	
	public static void BothMoveAway(Rigidbody2D mover, Rigidbody2D from) {
		BothMoveAway(mover, from, pushingPower);
	}
	
	public static void BothMoveAway(GameObject mover, GameObject from) {
		BothMoveAway(mover.GetComponent<Rigidbody2D>(), from.GetComponent<Rigidbody2D>());
	}
	
	public static void BothMaybeMoveAwayDistScaled(GameObject mover, GameObject from) {
		
		BothMaybeMoveAway(mover.GetComponent<Rigidbody2D>(), from.GetComponent<Rigidbody2D>(), pushingPower / Mathf.Pow(Vector2.Distance(mover.transform.position, from.transform.position), 1f));
	}
}