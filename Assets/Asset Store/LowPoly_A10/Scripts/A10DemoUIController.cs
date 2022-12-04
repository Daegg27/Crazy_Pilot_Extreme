using UnityEngine;
using UnityEngine.UI;

namespace A10 {
	public class A10DemoUIController : MonoBehaviour {
		public A10Controller a10Controller;
		public Button changeCameraPosButton;
		public Button toggleWheelsButton;
		public Slider sliderLeftAileron;
		public Slider sliderRightAileron;
		public Slider sliderLeftRudder;
		public Slider sliderRightRudder;
		public Slider sliderLeftHorzStablizer;
		public Slider sliderRightHorzStablizer;
		private bool wheels = false;
		private Transform cameraPositions;
		private Transform cam;
		private int cameraIndex = 0;
		

		void Start() {
			changeCameraPosButton.onClick.AddListener(ChangeCameraPosition);
			toggleWheelsButton.onClick.AddListener(ToggleWheels);

			sliderLeftAileron.onValueChanged.AddListener((value) => {
				a10Controller.SetLeftAileronAngle(value);
			});

			sliderRightAileron.onValueChanged.AddListener((value) => {
				a10Controller.SetRightAileronAngle(value);
			});

			sliderLeftRudder.onValueChanged.AddListener((value) => {
				a10Controller.SetLeftRudderAngle(value);
			});

			sliderRightRudder.onValueChanged.AddListener((value) => {
				a10Controller.SetRightRudderAngle(value);
			});

			sliderLeftHorzStablizer.onValueChanged.AddListener((value) => {
				a10Controller.SetLeftHorizontalStablizerAngle(value);
			});

			sliderRightHorzStablizer.onValueChanged.AddListener((value) => {
				a10Controller.SetRightHorizontalStablizerAngle(value);
			});

			cameraPositions = GameObject.Find("CameraSpots").transform;
			cam = Camera.main.transform;
			SetCameraPosition();
		}

		void SetCameraPosition() {
			Transform spot = cameraPositions.GetChild(cameraIndex);
			cam.position = spot.position;
			cam.rotation = spot.rotation;
		}

		void ChangeCameraPosition() {
			cameraIndex++;

			if(cameraIndex >= cameraPositions.childCount) {
				cameraIndex = 0;
			}

			SetCameraPosition();
		}

		void ToggleWheels() {
			wheels = !wheels;

			if(wheels) {
				a10Controller.WheelsOut();
			}
			else {
				a10Controller.WheelsIn();
			}
		}
	}
}