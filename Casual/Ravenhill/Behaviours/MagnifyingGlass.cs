using System.Collections;
using UnityEngine;

namespace Casual.Ravenhill.Behaviours {
    public class MagnifyingGlass : GameBehaviour {

        public Material m_Mat = null;
        private float m_InitialAmount = 0.5f;
        private float m_InitialRadiusX = 0.1f;
        private float m_InitialRadiusY = 0.1f;
        private float m_InitialComplicatedRadiusInner = 0.3f;
        private float m_InitialComplicatedRadiusOuter = 0.6f;

        private float[] m_Amount = new float[8];
        private float[] m_RadiusX = new float[8];
        private float[] m_RadiusY = new float[8];
        private float[] m_RadiusInner = new float[8];
        private float[] m_RadiusOuter = new float[8];
        private float m_MouseX = 0f;
        private float m_MouseY = 0f;
        private bool m_TraceMouse = false;
        private Rect[] m_GUIRects = new Rect[9];
        private bool m_UseComplicated = false;
        private bool m_UseMultiple = false;
        private bool m_InvertScale = false;
        private int m_GlassIndex = 0;

        private float m_TargetAmount;
        private float m_CurrentAmount;
        private float m_AmountVelocity;

        public void Setup(Material mat, float amount, Vector2 radius, Vector2 radiusInnerOuter, Vector3 worldPosition) {
            m_UseComplicated = true;
            m_Mat = mat;
            m_InitialAmount = 0;
            m_CurrentAmount = 0;
            m_AmountVelocity = 0;
            m_TargetAmount = amount;
            m_InitialRadiusX = radius.x;
            m_InitialRadiusY = radius.y;
            m_InitialComplicatedRadiusInner = radiusInnerOuter.x;
            m_InitialComplicatedRadiusOuter = radiusInnerOuter.y;
            m_UseComplicated = true;

            CheckSettings();
            ResetData();
            Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
            m_MouseX = screenPos.x / Screen.width;

#if UNITY_EDITOR
#if UNITY_ANDROID
        m_MouseY = 1.0f - screenPos.y / Screen.height;
#else
        m_MouseY = screenPos.y / Screen.height;
#endif
#else
#if UNITY_IOS
            m_MouseY = screenPos.y / Screen.height;
#else
        m_MouseY = 1.0f - screenPos.y / Screen.height;
#endif
#endif
        }

        public void RemoveSelf() {
            StartCoroutine(CorRemoveSelf());
        }

        private IEnumerator CorRemoveSelf() {
            m_TargetAmount = 0.0f;
            yield return new WaitForSeconds(0.3f);
            Destroy(this);
        }

        void ResetData() {
            // reset array values
            for (int i = 0; i < 8; i++) {
                m_Amount[i] = m_InitialAmount;
                m_RadiusX[i] = m_InitialRadiusX;
                m_RadiusY[i] = m_InitialRadiusY;
                m_RadiusInner[i] = m_InitialComplicatedRadiusInner;
                m_RadiusOuter[i] = m_InitialComplicatedRadiusOuter;
            }
            // reset material parameters
            m_Mat.SetVector("_SimpleCenterRadial1", new Vector4(0.3f, 0.2f, m_RadiusX[0], m_RadiusY[0]));
            m_Mat.SetVector("_SimpleCenterRadial2", new Vector4(0.3f, 0.4f, m_RadiusX[1], m_RadiusY[1]));
            m_Mat.SetVector("_SimpleCenterRadial3", new Vector4(0.3f, 0.6f, m_RadiusX[2], m_RadiusY[2]));
            m_Mat.SetVector("_SimpleCenterRadial4", new Vector4(0.3f, 0.8f, m_RadiusX[3], m_RadiusY[3]));
            m_Mat.SetVector("_SimpleCenterRadial5", new Vector4(0.6f, 0.2f, m_RadiusX[4], m_RadiusY[4]));
            m_Mat.SetVector("_SimpleCenterRadial6", new Vector4(0.6f, 0.4f, m_RadiusX[5], m_RadiusY[5]));
            m_Mat.SetVector("_SimpleCenterRadial7", new Vector4(0.6f, 0.6f, m_RadiusX[6], m_RadiusY[6]));
            m_Mat.SetVector("_SimpleCenterRadial8", new Vector4(0.6f, 0.8f, m_RadiusX[7], m_RadiusY[7]));
            m_Mat.SetFloat("_SimpleAmount1", m_Amount[0]);
            m_Mat.SetFloat("_SimpleAmount2", m_Amount[1]);
            m_Mat.SetFloat("_SimpleAmount3", m_Amount[2]);
            m_Mat.SetFloat("_SimpleAmount4", m_Amount[3]);
            m_Mat.SetFloat("_SimpleAmount5", m_Amount[4]);
            m_Mat.SetFloat("_SimpleAmount6", m_Amount[5]);
            m_Mat.SetFloat("_SimpleAmount7", m_Amount[6]);
            m_Mat.SetFloat("_SimpleAmount8", m_Amount[7]);

            m_Mat.SetVector("_ComplicatedCenterRadial1", new Vector4(0.3f, 0.2f, m_RadiusX[0], m_RadiusY[0]));
            m_Mat.SetVector("_ComplicatedCenterRadial2", new Vector4(0.3f, 0.4f, m_RadiusX[1], m_RadiusY[1]));
            m_Mat.SetVector("_ComplicatedCenterRadial3", new Vector4(0.3f, 0.6f, m_RadiusX[2], m_RadiusY[2]));
            m_Mat.SetVector("_ComplicatedCenterRadial4", new Vector4(0.3f, 0.8f, m_RadiusX[3], m_RadiusY[3]));
            m_Mat.SetVector("_ComplicatedCenterRadial5", new Vector4(0.6f, 0.2f, m_RadiusX[4], m_RadiusY[4]));
            m_Mat.SetVector("_ComplicatedCenterRadial6", new Vector4(0.6f, 0.4f, m_RadiusX[5], m_RadiusY[5]));
            m_Mat.SetVector("_ComplicatedCenterRadial7", new Vector4(0.6f, 0.6f, m_RadiusX[6], m_RadiusY[6]));
            m_Mat.SetVector("_ComplicatedCenterRadial8", new Vector4(0.6f, 0.8f, m_RadiusX[7], m_RadiusY[7]));
            m_Mat.SetFloat("_ComplicatedAmount1", m_Amount[0]);
            m_Mat.SetFloat("_ComplicatedAmount2", m_Amount[1]);
            m_Mat.SetFloat("_ComplicatedAmount3", m_Amount[2]);
            m_Mat.SetFloat("_ComplicatedAmount4", m_Amount[3]);
            m_Mat.SetFloat("_ComplicatedAmount5", m_Amount[4]);
            m_Mat.SetFloat("_ComplicatedAmount6", m_Amount[5]);
            m_Mat.SetFloat("_ComplicatedAmount7", m_Amount[6]);
            m_Mat.SetFloat("_ComplicatedAmount8", m_Amount[7]);
            m_Mat.SetFloat("_ComplicatedRadiusInner1", m_RadiusInner[0]);
            m_Mat.SetFloat("_ComplicatedRadiusInner2", m_RadiusInner[1]);
            m_Mat.SetFloat("_ComplicatedRadiusInner3", m_RadiusInner[2]);
            m_Mat.SetFloat("_ComplicatedRadiusInner4", m_RadiusInner[3]);
            m_Mat.SetFloat("_ComplicatedRadiusInner5", m_RadiusInner[4]);
            m_Mat.SetFloat("_ComplicatedRadiusInner6", m_RadiusInner[5]);
            m_Mat.SetFloat("_ComplicatedRadiusInner7", m_RadiusInner[6]);
            m_Mat.SetFloat("_ComplicatedRadiusInner8", m_RadiusInner[7]);
            m_Mat.SetFloat("_ComplicatedRadiusOuter1", m_RadiusOuter[0]);
            m_Mat.SetFloat("_ComplicatedRadiusOuter2", m_RadiusOuter[1]);
            m_Mat.SetFloat("_ComplicatedRadiusOuter3", m_RadiusOuter[2]);
            m_Mat.SetFloat("_ComplicatedRadiusOuter4", m_RadiusOuter[3]);
            m_Mat.SetFloat("_ComplicatedRadiusOuter5", m_RadiusOuter[4]);
            m_Mat.SetFloat("_ComplicatedRadiusOuter6", m_RadiusOuter[5]);
            m_Mat.SetFloat("_ComplicatedRadiusOuter7", m_RadiusOuter[6]);
            m_Mat.SetFloat("_ComplicatedRadiusOuter8", m_RadiusOuter[7]);
        }

        private void CheckSettings() {
            if (!SystemInfo.supportsImageEffects)
                enabled = false;

            QualitySettings.antiAliasing = 8;
        }

        public override void Update() {
            base.Update();
            m_CurrentAmount = Mathf.SmoothDamp(m_CurrentAmount, m_TargetAmount, ref m_AmountVelocity, 0.3f);
            for (int i = 0; i < m_Amount.Length; i++) {
                m_Amount[i] = m_CurrentAmount;
            }
        }

        void OnRenderImage(RenderTexture sourceTexture, RenderTexture destTexture) {
            if (m_Mat == null) {
                return;
            }

            // select which pass should we use
            int pass = 0;
            if (m_UseComplicated) {
                if (m_UseMultiple)
                    pass = 3;
                else
                    pass = 2;
            } else {
                if (m_UseMultiple)
                    pass = 1;
                else
                    pass = 0;
            }

            // fill material parameters
            int ind = m_GlassIndex;
            string simpleAmount = "_SimpleAmount" + (ind + 1);
            m_Mat.SetFloat(simpleAmount, m_Amount[ind]);
            string simpleCenterRadial = "_SimpleCenterRadial" + (ind + 1);
            m_Mat.SetVector(simpleCenterRadial, new Vector4(m_MouseX, m_MouseY, m_RadiusX[ind], m_RadiusY[ind]));

            string complicatedAmount = "_ComplicatedAmount" + (ind + 1);
            m_Mat.SetFloat(complicatedAmount, m_Amount[ind]);
            string complicatedCenterRadial = "_ComplicatedCenterRadial" + (ind + 1);
            m_Mat.SetVector(complicatedCenterRadial, new Vector4(m_MouseX, m_MouseY, m_RadiusX[ind], m_RadiusY[ind]));
            string complicatedRadiusInner = "_ComplicatedRadiusInner" + (ind + 1);
            m_Mat.SetFloat(complicatedRadiusInner, m_RadiusInner[ind]);
            string complicatedRadiusOuter = "_ComplicatedRadiusOuter" + (ind + 1);
            m_Mat.SetFloat(complicatedRadiusOuter, m_RadiusOuter[ind]);

            // let's draw it
            Graphics.Blit(sourceTexture, destTexture, m_Mat, pass);
        }
    }
}
