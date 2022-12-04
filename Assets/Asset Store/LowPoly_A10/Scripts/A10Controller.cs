using UnityEngine;

namespace A10 {
	public class A10Controller : MonoBehaviour {
		private Animator animator;
		public LocalRotator leftAileron, rightAileron;
		public LocalRotator leftRudder, rightRudder;
		public LocalRotator leftHorzStablizer, rightHorzStablizer;

		void Start() {
			animator = GetComponent<Animator>();
		}

		public void WheelsOut() {
			animator.SetTrigger("WheelsOut");
		}

		public void WheelsIn() {
			animator.SetTrigger("WheelsIn");
		}

		public void SetLeftAileronAngle(float value) {
			leftAileron.angleChanges = value;
		}

		public void SetRightAileronAngle(float value) {
			rightAileron.angleChanges = value;
		}

		public void SetLeftRudderAngle(float value) {
			leftRudder.angleChanges = value;
		}

		public void SetRightRudderAngle(float value) {
			rightRudder.angleChanges = value;
		}

		public void SetLeftHorizontalStablizerAngle(float value) {
			leftHorzStablizer.angleChanges = value;
		}

		public void SetRightHorizontalStablizerAngle(float value) {
			rightHorzStablizer.angleChanges = value;
		}
	}
}