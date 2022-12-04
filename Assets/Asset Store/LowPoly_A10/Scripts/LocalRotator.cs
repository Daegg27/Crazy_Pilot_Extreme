using UnityEngine;

namespace A10 {
	public class LocalRotator : MonoBehaviour {
		public Vector3 localAxis = new Vector3(0, 1, 0);
		public float angleChanges = 0f;
		private Quaternion initRot;

		void Start() {
			initRot = transform.localRotation;
		}

		void Update() {
			Quaternion change = Quaternion.AngleAxis(angleChanges, localAxis);
			transform.localRotation = initRot * change;
		}
	}
}