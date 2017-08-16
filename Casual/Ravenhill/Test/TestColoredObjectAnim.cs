using Casual.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual.Ravenhill.Test {
    public class TestColoredObjectAnim : GameBehaviour {

        public override void Update() {
            base.Update();
            if(Input.GetKeyUp(KeyCode.A)) {
                var anim = FindObjectOfType<ColoredObjectAnim>();
                anim.StartAnim(new ColorAnimData {
                    duration = 2,
                    end = new Color(0, 0, 0, 0),
                    endAction = () => { Debug.Log("OK"); },
                    start = new Color(1, 1, 1, 1)
                });
            }

            if(Input.GetKeyUp(KeyCode.B)) {
                var anim = FindObjectOfType<ColoredObjectAnim>();
                anim.StartAnim(new ColorAnimData {
                    duration = 2,
                    end = new Color(1, 1, 1, 1),
                    endAction = () => { Debug.Log("OK2"); },
                    start = new Color(0, 0, 0, 0)
                });
            }
        }
    }
}
