using UnityEngine;

public class FloatEquals {
	private const float threshhold = 0.0001f;
	public static bool Equals(float a, float b) {
		return Mathf.Abs(a - b) <= threshhold;
	}
}