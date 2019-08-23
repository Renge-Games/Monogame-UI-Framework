using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UIFramework {
    public class Interface {
        public Interface(string name) {
            Name = name;
            backElements = new List<Element>();
            frontElements = new List<Element>();
        }

        List<Element> backElements;
        List<Element> frontElements;

        public Element this[int i, int interf] {
            get {
                if (interf == 0)
                    return backElements[i];
                else
                    return frontElements[i];
            }
            set {
                if (interf == 0)
                    backElements[i] = value;
                else
                    frontElements[i] = value;
            }
        }

        public int BackCount { get { return backElements.Count; } }
        public int FrontCount { get { return frontElements.Count; } }
        public string Name { get; set; }

        public void AddElement(Element element, bool toFront) {
            if (toFront)
                frontElements.Add(element);
            else
                backElements.Add(element);
        }

        public bool BEContains(Element element) {
            if (backElements.Contains(element))
                return true;
            return false;
        }

        public bool FEContains(Element element) {
            if (frontElements.Contains(element))
                return true;
            return false;
        }

        public void Transfer(Element element, bool toFront) {
            if (toFront) {
                backElements.Remove(element);
                frontElements.Add(element);
            } else {
                frontElements.Remove(element);
                backElements.Add(element);
            }
        }

        public void RemoveAt(int index, bool front) {
            if (front)
                frontElements.RemoveAt(index);
            else
                backElements.RemoveAt(index);
        }

        public void Remove(Element element) {
            if (frontElements.Contains(element))
                frontElements.Remove(element);
            else
                backElements.Remove(element);
        }

        public void PushToFront(Element element) {
            if (frontElements.Contains(element)) {
                int i = frontElements.IndexOf(element);
                frontElements.Insert(FrontCount, frontElements[i]);
                frontElements.RemoveAt(i);
            } else if (backElements.Contains(element)) {
                int i = backElements.IndexOf(element);
                backElements.Insert(BackCount, backElements[i]);
                backElements.RemoveAt(i);
            }
        }

        public void PushToBack(Element element) {
            if (frontElements.Contains(element)) {
                frontElements.Reverse();
                int i = frontElements.IndexOf(element);
                frontElements.Insert(FrontCount, frontElements[i]);
                frontElements.RemoveAt(i);
                frontElements.Reverse();
            } else if (backElements.Contains(element)) {
                backElements.Reverse();
                int i = backElements.IndexOf(element);
                backElements.Insert(BackCount, backElements[i]);
                backElements.RemoveAt(i);
                backElements.Reverse();
            }
        }

        public bool IntersectingMouse() {
            for (int i = FrontCount - 1; i >= 0; i--) {
                if (frontElements[i].IntersectingMouse())
                    return true;
            }
            for (int i = BackCount - 1; i >= 0; i--) {
                if (backElements[i].IntersectingMouse())
                    return true;
            }
            return false;
        }

        public void Update() {
            bool eventFired = false;

            for (int i = FrontCount - 1; i >= 0; i--) {
                if (!eventFired && frontElements[i].EventsEnabled && frontElements[i].PollEvents() && frontElements[i].IntersectingMouse())
                    eventFired = true;
                frontElements[i].Update();
            }
            for (int i = BackCount - 1; i >= 0; i--) {
                if (!eventFired && backElements[i].EventsEnabled && backElements[i].PollEvents() && backElements[i].IntersectingMouse())
                    eventFired = true;
                backElements[i].Update();
            }
        }

        public void Draw(SpriteBatch sb) {
            for (int i = 0; i < backElements.Count; i++)
                if (backElements[i].DrawEnabled)
                    backElements[i].Draw(sb);
            for (int i = 0; i < frontElements.Count; i++)
                if (frontElements[i].DrawEnabled)
                    frontElements[i].Draw(sb);
        }
    }
}