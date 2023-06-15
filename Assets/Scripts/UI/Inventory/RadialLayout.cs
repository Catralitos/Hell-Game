using UnityEngine;
using UnityEngine.UI;

/*
Radial Layout Group by Just a Pixel (Danny Goodayle) - http://www.justapixel.co.uk
Copyright (c) 2015
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
namespace UI.Inventory
{
    public class RadialLayout : LayoutGroup {
        public float fDistance = 2.0f;
        [Range(0f, 360f)] 
        private float MinAngle = 0.0f;
        [Range(0f, 360f)] 
        private float MaxAngle = 315.0f; 
        [Range(0f, 360f)] 
        private float StartAngle = 90.0f;

        private int _numItems = 1;
        
        protected override void OnEnable() { base.OnEnable(); CalculateRadial(); }
        public override void SetLayoutHorizontal()
        {
        }
        public override void SetLayoutVertical()
        {
        }
        public override void CalculateLayoutInputVertical()
        {
            CalculateRadial();
        }
        public override void CalculateLayoutInputHorizontal()
        { 
            CalculateRadial();
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            _numItems = transform.childCount;
            MaxAngle = 360 - 360.0f / _numItems;
            CalculateRadial();
        }
#endif
        protected override void Start()
        {
            base.Start();
            _numItems = transform.childCount;
            MinAngle = 0.0f;
            MaxAngle = 360 - 360.0f / _numItems;
            StartAngle = 90.0f;
            CalculateRadial();
        }

        public void AddItem()
        {
            _numItems++;
            MaxAngle = 360 - 360.0f / _numItems;
            CalculateRadial();
        }
        
        public void RemoveItem()
        {
            _numItems--;
            MaxAngle = 360 - 360.0f / _numItems;
            CalculateRadial();
        }
        
        void CalculateRadial()
        {
            m_Tracker.Clear();
            if (transform.childCount == 0)
                return;
            float fOffsetAngle = ((MaxAngle - MinAngle)) / (transform.childCount -1);
        
            float fAngle = StartAngle;
            for (int i = 0; i < transform.childCount; i++)
            {
                RectTransform child = (RectTransform)transform.GetChild(i);
                if (child != null)
                {
                    //Adding the elements to the tracker stops the user from modifiying their positions via the editor.
                    m_Tracker.Add(this, child, 
                        DrivenTransformProperties.Anchors |
                        DrivenTransformProperties.AnchoredPosition |
                        DrivenTransformProperties.Pivot);
                    Vector3 vPos = new Vector3(Mathf.Cos(fAngle * Mathf.Deg2Rad), Mathf.Sin(fAngle * Mathf.Deg2Rad), 0);
                    child.localPosition = vPos * fDistance;
                    //Force objects to be center aligned, this can be changed however I'd suggest you keep all of the objects with the same anchor points.
                    child.anchorMin = child.anchorMax = child.pivot = new Vector2(0.5f, 0.5f);
                    fAngle += fOffsetAngle;
                }
            }

        }
    }
}