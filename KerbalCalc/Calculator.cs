// This file is part of KerbalCalc.
// 
// KerbalCalc is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// KerbalCalc is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with KerbalCalc. If not, see <http://www.gnu.org/licenses/>.

using UnityEngine;

namespace KerbalCalc
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class Calculator : MonoBehaviour
    {
        private const float CalcWidth = 300;
        private ApplicationLauncherButton _button;
        private bool _visible;
        private Rect _screenRect = new Rect(0, 0, CalcWidth, 0);
        private bool _hasBeenCentred;
        private readonly Calc _calculator = new Calc();

        public void AddButton()
        {
            if (_button == null)
                _button = ApplicationLauncher.Instance.AddModApplication(Show, Hide, null, null, null, null,
                    ApplicationLauncher.AppScenes.ALWAYS,
                    GameDatabase.Instance.GetTexture("KerbalCalc/Textures/Icon", false));
        }

        public void Awake()
        {
            GameEvents.onGUIApplicationLauncherReady.Add(AddButton);
        }

        public void OnDestroy()
        {
            GameEvents.onGUIApplicationLauncherReady.Remove(AddButton);

            if (_button != null)
                ApplicationLauncher.Instance.RemoveModApplication(_button);
        }

        private void Show()
        {
            _visible = true;
        }

        private void Hide()
        {
            _visible = false;
        }

        public void OnGUI()
        {
            if (!_visible) return;

            _screenRect = GUILayout.Window(GetInstanceID(), _screenRect, CreateWindow, "Kerbal Calculator",
                HighLogic.Skin.window, GUILayout.ExpandWidth(false));
            if (HighLogic.LoadedScene == GameScenes.SPACECENTER ||
                _hasBeenCentred ||
                !(_screenRect.width > 0.0f) ||
                !(_screenRect.height > 0.0f))
                return;

            _hasBeenCentred = true;
            _screenRect.center = new Vector2(Screen.width*0.5f, Screen.height*0.5f);
        }

        private void CreateWindow(int windowId)
        {
            GUI.skin = HighLogic.Skin;
            GUILayout.BeginVertical();
            _calculator.Screen = GUILayout.TextField(_calculator.Screen, GUILayout.Width(CalcWidth));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("ln")) _calculator.NaturalLog();
            if (GUILayout.Button("x^2")) _calculator.Square();
            if (GUILayout.Button("x^3")) _calculator.Cube();
            if (GUILayout.Button("x^y")) _calculator.StagePower();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("7")) _calculator.AddDigit(7);
            if (GUILayout.Button("8")) _calculator.AddDigit(8);
            if (GUILayout.Button("9")) _calculator.AddDigit(9);
            if (GUILayout.Button("/")) _calculator.StageDivide();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("4")) _calculator.AddDigit(4);
            if (GUILayout.Button("5")) _calculator.AddDigit(5);
            if (GUILayout.Button("6")) _calculator.AddDigit(6);
            if (GUILayout.Button("*")) _calculator.StageMultiply();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("1")) _calculator.AddDigit(1);
            if (GUILayout.Button("2")) _calculator.AddDigit(2);
            if (GUILayout.Button("3")) _calculator.AddDigit(3);
            if (GUILayout.Button("-")) _calculator.StageSubtract();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("0")) _calculator.AddDigit(0);
            if (GUILayout.Button(".")) _calculator.AddDecimal();
            if (GUILayout.Button("=")) _calculator.Calculate();
            if (GUILayout.Button("+")) _calculator.StageAdd();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUI.DragWindow();
            GUI.skin = null;
        }
    }
}