using Bone.Convenience;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bone.UISpace
{
    public class UISpace : MonoSingleton<UISpace>
    {
        public Camera SourceCamera;
        public RectTransform TargetCanvas;
        public class Reference
        {
            public UISpaceRegister Register;

            public Reference(UISpace parent, string id, Vector3 pos)
            {
                var go = new GameObject();
                go.transform.parent = parent.TargetCanvas;
                go.name = id;
                var im = go.AddComponent<Image>();
                var Reg = new UISpaceRegister();
                Reg.UIReference = im;

                Register = Reg;
                parent.Lookup.Add(id, this);
            }
            public Reference Fades(bool sw)
            {
                if(Register.ShouldFade != sw)
                    Register.ShouldFade = sw;
                return this;
            }
            public Reference WithSprite(Sprite s)
            {
                if (Register.Sprite != s)
                    Register.SetSprite(s);
                return this;
            }
            public Reference Colored(Color c)
            {
                if (Register.OriginalColor != c)
                    Register.SetColor(c);
                return this;
            }
            public Reference WithDuration(float f)
            {
                if(f != Register.Duration)
                    Register.Duration = f;
                return this;
            }
            public Reference Sized(float f)
            {
                if(Register.SpriteSize.HasValue == false||(f != Register.SpriteSize.Value.x || f != Register.SpriteSize.Value.y))
                {
                    Register.SetSize(new Vector2(f, f));
                }
                return this;
            }
            public Reference NativeSized()
            {
                if (Register.SpriteSize.HasValue)
                {
                    Register.SetSize(null);
                }
                return this;
            }
            public Reference UnevenSized(Vector2 s)
            {
                if(Register.SpriteSize.HasValue == false || Register.SpriteSize.Value != s)
                {
                    Register.SpriteSize = s;
                }
                return this;
            }
            public Reference DestroyWhenExpired()
            {
                Register.ShouldSweep = true;
                return this;
            }
        }
        public class UISpaceRegister
        {
            public bool ShouldSweep = false;
            public Color OriginalColor { get; set; }
            private Color _trueColor { get; set; }
            public Vector2? SpriteSize { get; set; }
            public float TimeSinceLastRequest { get; set; }
            public Sprite Sprite { get; set; }
            public Vector3 TargetPosition { get; set; }
            public Image UIReference { get; set; }
            public bool ShouldFade { get; set; }
            public float Duration { get; set; }

            private bool IsActive = true;

            public void EnableRegister()
            {
                if (IsActive)
                    return;
                if(UIReference != null)
                {
                    UIReference.gameObject.SetActive(true);
                }
                IsActive = true;
            }
            public void DisableRegister()
            {
                if (!IsActive)
                    return;
                if(UIReference != null)
                {
                    UIReference.gameObject.SetActive(false);
                }
                IsActive = false;
            }

            public void SetSprite(Sprite s)
            {
                Sprite = s;
                if(UIReference != null)
                {
                    UIReference.color = OriginalColor;
                    UIReference.sprite = s;
                    if (SpriteSize.HasValue)
                    {
                        UIReference.rectTransform.sizeDelta = SpriteSize.Value;
                    }
                    else
                    {
                        UIReference.SetNativeSize();
                    }
                }
            }
            public void SetColor(Color c)
            {
                OriginalColor = c;
                if(UIReference != null)
                {
                    UIReference.color = c;
                }
            }
            public void SetAlpha(float o)
            {
                if(UIReference != null)
                {
                    var oc = OriginalColor;
                    UIReference.color = new Color(oc.r, oc.g, oc.b, OriginalColor.a * o);
                }
            }
            public void SetSize(Vector2? im)
            {
                SpriteSize = im;
                if(UIReference != null)
                {
                    if (SpriteSize.HasValue)
                    {
                        UIReference.rectTransform.sizeDelta = SpriteSize.Value;
                    }
                    else
                    {
                        UIReference.SetNativeSize();
                    }
                }
            }
        }
        private Dictionary<string, Reference> Lookup = new Dictionary<string, Reference>();
        public Reference Request(string id, Vector3 pos)
        {
            var r = GetOrCreate(id);
            r.Register.TargetPosition = pos;
            r.Register.TimeSinceLastRequest = 0f;
            return r;
        }
        protected Reference GetOrCreate(string ident)
        {
            if (Lookup.ContainsKey(ident))
            {
                return Lookup[ident];
            }
            else
            {
                var re = new Reference(this, ident, Vector3.zero);
                return re;
            }
        }
        private void OnEnable()
        {
            if(SourceCamera == null)
            {
                SourceCamera = Camera.main;
            }
            if(TargetCanvas == null)
            {
                TargetCanvas = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
            }
        }
        private void FixedUpdate()
        {
            foreach(var refer in Lookup.Values)
            {
                var reg = refer.Register;
                if(reg.TimeSinceLastRequest > reg.Duration)
                {
                    reg.DisableRegister();
                    continue;
                }
                else
                {
                    reg.EnableRegister();
                }
                
                if (reg.ShouldFade && reg.TimeSinceLastRequest != 0f && reg.Duration > 0f)
                {
                    var alph = (reg.Duration - reg.TimeSinceLastRequest) / reg.Duration;
                    reg.SetAlpha(alph);
                }
                else
                {
                    reg.SetAlpha(1f);
                }
                Vector2 vp = SourceCamera.WorldToViewportPoint(reg.TargetPosition);
                var proper = new Vector2(
                    (vp.x * TargetCanvas.sizeDelta.x) - TargetCanvas.sizeDelta.x / 2f,
                    (vp.y * TargetCanvas.sizeDelta.y) - TargetCanvas.sizeDelta.y / 2f
                    );
                reg.UIReference.rectTransform.anchoredPosition = proper;
                reg.TimeSinceLastRequest += Time.deltaTime;
            }
        }
    }
}

