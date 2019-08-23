using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityFramework;

namespace UIFramework {
    public static class UI {
        static List<Interface> interfaces = new List<Interface>();
        internal static int windowCount;

        static Vector2 MinimizedWindowSize { get; set; }
        public static TextFont Font { get; set; }
        public static Color DisabledColor { get; set; }
        public static Color BGColor { get; set; }
        public static Color BGHoverColor { get; set; }
        public static Color BGPressedColor { get; set; }
        public static Color SurfaceColor { get; set; }
        public static Color SurfaceHoverColor { get; set; }
        public static Color SurfacePressedColor { get; set; }
        public static Color HighlightColor { get; set; }
        public static Color BlueHighlightColor { get; set; }
        public static int BGOffset { get; set; }

        public static int BackCount { get { return interfaces[CurrentInterfaceIndex].BackCount; } }
        public static int FrontCount { get { return interfaces[CurrentInterfaceIndex].FrontCount; } }
        public static Vector2 ScreenSize { get; set; }
        public static Interface CurrentInterface { get { return interfaces[CurrentInterfaceIndex]; } }
        public static int CurrentInterfaceIndex { get; set; }


        public static void Init(Vector2 screenSize, TextFont font) {
            ScreenSize = screenSize;
            MinimizedWindowSize = new Vector2(25, 100);
            Font = font;
            DisabledColor = new Color(200, 200, 200);
            BGColor = new Color(220, 220, 220);
            BGHoverColor = new Color(200, 200, 200);
            BGPressedColor = new Color(150, 150, 150);
            SurfaceColor = new Color(150, 150, 150);
            SurfaceHoverColor = new Color(100, 100, 100);
            SurfacePressedColor = new Color(50, 50, 50);
            HighlightColor = new Color(200, 200, 200);
            BlueHighlightColor = new Color(100, 150, 255);
            BGOffset = 1;
            CurrentInterfaceIndex = 0;
            KeyList.Init();
        }

        public static Element GetElementAt(int index, bool front) {
            return CurrentInterface[index, front ? 1 : 0];
        }

        public static bool BEContains(Element element) {
            if (CurrentInterface.BEContains(element))
                return true;
            return false;
        }

        public static bool FEContains(Element element) {
            if (CurrentInterface.BEContains(element))
                return true;
            return false;
        }

        public static void AddBackElement(Element element, Interface intf) {
            if (intf != null)
                intf.AddElement(element, false);
            else
                CurrentInterface.AddElement(element, false);
        }

        public static void AddFrontElement(Element element, Interface intf) {
            if (intf != null)
                intf.AddElement(element, true);
            else
                CurrentInterface.AddElement(element, true);
        }

        public static void TransferElementToFront(Element element) {
            CurrentInterface.Transfer(element, true);
        }

        public static void TransferElementToBack(Element element) {
            CurrentInterface.Transfer(element, false);
        }

        public static void TransferElementToOtherInterface(Element element, Interface interfaceOrigin, Interface interfaceGoal) {
            bool front = false;
            if (interfaceOrigin.FEContains(element))
                front = true;

            interfaceOrigin.Remove(element);
            interfaceGoal.AddElement(element, front);
        }

        public static void RemoveElementAt(int index, bool front) {
            CurrentInterface.RemoveAt(index, front);
        }

        public static void RemoveElement(Element element) {
            CurrentInterface.Remove(element);
        }

        public static void SetInterfaceName(string name) {
            CurrentInterface.Name = name;
        }

        public static void SetCurrentInterface(Interface intf) {
            CurrentInterfaceIndex = interfaces.IndexOf(intf);
        }

        public static Interface GetInterface(string name) {
            for (int i = 0; i < interfaces.Count; i++) {
                if (interfaces[i].Name == name) {
                    return interfaces[i];
                }
            }
            return null;
        }

        public static void GotoInterface(int index) {
            CurrentInterfaceIndex = index;
        }

        public static void SetCurrentInterface(string name) {
            for (int i = 0; i < interfaces.Count; i++) {
                if (interfaces[i].Name == name) {
                    CurrentInterfaceIndex = i;
                    break;
                }
            }
        }

        public static void AddInterface(string name) {
            interfaces.Add(new Interface(name));
            CurrentInterfaceIndex = interfaces.Count - 1;
        }

        public static void AddInterface(Interface intf) {
            interfaces.Add(intf);
            CurrentInterfaceIndex = interfaces.Count - 1;
        }

        public static void RemoveInterfaceAt(int index) {
            interfaces.RemoveAt(index);
        }

        public static void RemoveInterface(Interface intf) {
            interfaces.Remove(intf);
        }

        public static void PushToFront(Element element) {
            CurrentInterface.PushToFront(element);
        }

        public static void PushToBack(Element element) {
            CurrentInterface.PushToBack(element);
        }

        public static bool IntersectingMouse() {
            return CurrentInterface.IntersectingMouse();
        }

        public static void Update() {
            KeyList.Update();
            CurrentInterface.Update();
        }

        public static void Draw(SpriteBatch sb) {
            CurrentInterface.Draw(sb);
        }
    }
}