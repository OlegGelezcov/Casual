using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Casual {
    public class TransformBezierMovable : GameBehaviour {

        public class Data {
            public PositionObject[] positions;
            public MovingFuncType movingType;
            public float speed;
            public System.Action onComplete;

            public Vector3 StartPosition {
                get {
                    if (positions.Length > 0) {
                        return positions[0].Position;
                    } else {
                        throw new ArgumentOutOfRangeException("positions");
                    }
                }
            }

            public Vector3 Mid1Position {
                get {
                    if(positions.Length > 1) {
                        return positions[1].Position;
                    } else {
                        throw new ArgumentOutOfRangeException("positions");
                    }
                }
            }

            public Vector3 Mid2Position {
                get {
                    if (positions.Length > 2) {
                        return positions[2].Position;
                    } else {
                        throw new ArgumentOutOfRangeException("positions");
                    }
                }
            }

            public bool IsEndPositionValid {
                get {
                    switch(movingType) {
                        case MovingFuncType.Bezier1: {
                                if(positions.Length > 1) {
                                    return positions[1].IsValid;
                                }
                                return false;
                            }
                        case MovingFuncType.Bezier2: {
                                if(positions.Length > 2) {
                                    return positions[2].IsValid;
                                }
                                return false;
                            }
                        case MovingFuncType.Bezier3: {
                                if(positions.Length > 3) {
                                    return positions[3].IsValid;
                                }
                                return false;
                            }
                        default: {
                                return false;
                            }
                    }
                }
            }

            public Vector3 EndPosition {
                get {
                    switch(movingType) {
                        case MovingFuncType.Bezier1: {
                                if(positions.Length > 1) {
                                    return positions[1].Position;
                                } else {
                                    throw new ArgumentOutOfRangeException("positions");
                                }
                            }
                        case MovingFuncType.Bezier2: {
                                if(positions.Length > 2) {
                                    return positions[2].Position;
                                } else {
                                    throw new ArgumentOutOfRangeException("positions");
                                }
                            }
                        case MovingFuncType.Bezier3: {
                                if(positions.Length > 3 ) {
                                    return positions[3].Position;
                                } else {
                                    throw new ArgumentOutOfRangeException("positions");
                                }
                            }
                        default: {
                                throw new InvalidOperationException("movingType");
                            }
                    }
                }
            }
        }

        private Data data = null;
        private bool isStarted = false;
        private float t = 0.0f;

        public void Move(Data data) {
            this.data = data;
            t = 0.0f;
            isStarted = true;
        }

        public void Stop() {
            isStarted = false;
        }

        private bool IsValid => data != null;

        private bool CheckState() {
            if (isStarted) {

                if (!IsValid) {
                    isStarted = false;
                    return false; ;
                }

                if (IsValid && !data.IsEndPositionValid) {
                    isStarted = false;
                    isStarted = false;
                    data.onComplete?.Invoke();
                    return false; ;
                }
            }
            return true;
        }

        public override void Update() {
            base.Update();
            if (CheckState()) {

                if (isStarted && IsValid && data.IsEndPositionValid) {
                    t += Time.deltaTime * data.speed;
                    float sourcet = t;
                    t = Mathf.Clamp01(t);

                    Vector3 currentPoint = Vector3.zero;
                    switch (data.movingType) {
                        case MovingFuncType.Bezier1: {
                                currentPoint = Utility.Bezier1(t, data.StartPosition, data.EndPosition);
                            }
                            break;
                        case MovingFuncType.Bezier2: {
                                currentPoint = Utility.Bezier2(t, data.StartPosition, data.Mid1Position, data.EndPosition);
                            }
                            break;
                        case MovingFuncType.Bezier3: {
                                currentPoint = Utility.Bezier3(t, data.StartPosition, data.Mid1Position, data.Mid2Position, data.EndPosition);
                            }
                            break;
                    }
                    transform.position = currentPoint;

                    if (sourcet >= 1.0f) {
                        isStarted = false;
                        data.onComplete?.Invoke();
                    }
                }
            }
        }
    }

    
}
