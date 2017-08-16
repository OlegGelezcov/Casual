using UnityEngine;

namespace Casual.UI {
    public interface IColored {
        Color GetObjectColor();
        void SetObjectColor(Color value);
    }

    public class ColoredObject : GameBehaviour {

        private IColored m_IColored = null;

        private IColored colored {
            get {
                if(m_IColored == null ) {
                    foreach(Component component in gameObject.GetComponents(typeof(Component))) {
                        if(component is IColored) {
                            m_IColored = component as IColored;
                        }
                    }
                }
                return m_IColored;
            }
        }

        public Color GetObjectColor() {
            return colored?.GetObjectColor() ?? Color.white;
        }

        public void SetObjectColor(Color value) {
            if(colored != null ) {
                colored.SetObjectColor(value);
            }
        }

        public Color color {
            get {
                return GetObjectColor();
            }
            set {
                SetObjectColor(value);
            }
        }
    }
}
