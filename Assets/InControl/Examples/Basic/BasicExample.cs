//namespace BasicExample
//{
//	using InControl;
//	using UnityEngine;
//    using XInputDotNetPure;
//
//
//	public class BasicExample : MonoBehaviour
//	{
//
//        public string[] joystickNames;
//        XInputDotNetPure.PlayerIndex index;
//
//        XInputDotNetPure.PlayerIndex controllerOne;
//        XInputDotNetPure.PlayerIndex controllerTwo;
//        XInputDotNetPure.PlayerIndex controllerThree;
//        XInputDotNetPure.PlayerIndex controllerFour;
//
//        void Start ()
//        {
//            //Handheld.Vibrate();
//            //XInputDotNetPure.GamePad.SetVibration(0, .1f, .1f);
//
//            //joystickNames = Input.GetJoystickNames();
//          /*  for (int i = 0; i < joystickNames.Length; i++)
//            {
//                if (i == 0)
//                {
//                    index = PlayerIndex.One;
//                }
//
//                if (i == 1)
//                {
//                    index = PlayerIndex.Two;
//                }
//
//                if (i == 2)
//                {
//                    index = PlayerIndex.Three;
//                }
//
//                if (i == 3)
//                {
//                    index = PlayerIndex.Four;
//                }
//
//
//                if (joystickNames[i] == "Controller (Xbox One For Windows)")
//                {
//                    XInputDotNetPure.GamePad.SetVibration(index, 1f, 1f);
//                    print("One controller detected");
//                }
//
//                if (joystickNames[i] == "Controller (XBOX 360 For Windows)")
//                {
//                    XInputDotNetPure.GamePad.SetVibration(index, .1f, .1f);
//                    print("360 controller detected");
//                }
//
//                if (joystickNames[i] == "Wireless Controller") //PS4
//                {
//                    XInputDotNetPure.GamePad.SetVibration(index, .5f, .5f);
//                    print("PS4 controller detected");
//                }
//
//                else return;
//            }
//            */
//
//        }
//        void Update()
//        {
//
//            
//
//            // Use last device which provided input.
//            var inputDevice = InputManager.ActiveDevice;
//
//            //XInputDotNetPure.GamePad.SetVibration(0, .075f, .075f);
//        
//
//            // Rotate target object with left stick.
//            transform.Rotate(Vector3.down, 500.0f * Time.deltaTime * inputDevice.LeftStickX, Space.World);
//            transform.Rotate(Vector3.right, 500.0f * Time.deltaTime * inputDevice.LeftStickY, Space.World);
//
//            // Get two colors based on two action buttons.
//            var color1 = inputDevice.Action1.IsPressed ? Color.red : Color.white;
//            var color2 = inputDevice.Action2.IsPressed ? Color.green : Color.white;
//
//            // Blend the two colors together to color the object.
//            GetComponent<Renderer>().material.color = Color.Lerp(color1, color2, 0.5f);
//        }
//	}
//}
//
