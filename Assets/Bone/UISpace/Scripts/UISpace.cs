using Bone.Convenience;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace bone.UISpace
{
    public class UISpace : MonoSingleton<UISpace>
    {
        public Camera SourceCamera;
        public RectTransform TargetCanvas;
        private class UISpaceRegister
        {
            public float TimeSinceLastRequest { get; set; }
            public Sprite Sprite { get; set; }
            public Vector3 TargetPosition { get; set; }
            public Image UIReference { get; set; }
            public bool ShouldFade { get; set; }
            public float Duration { get; set; }

            public void SetSprite(Sprite s)
            {
                Sprite = s;
                if(UIReference != null)
                {
                    UIReference.sprite = s;
                    UIReference.SetNativeSize();
                }
            }
        }
        private Dictionary<string, UISpaceRegister> Lookup = new Dictionary<string, UISpaceRegister>();
        public GameObject Request(string ident, Sprite icon, Vector3 world_position, float duration = 0f, bool fade = false)
        {
            if (Lookup.ContainsKey(ident))
            {
                var register = Lookup[ident];
                register.TimeSinceLastRequest = 0f;
                if (icon != register.Sprite)
                    register.SetSprite(icon);
                if (world_position != register.TargetPosition)
                    register.TargetPosition = world_position;
                return register.UIReference.gameObject;
            }
            else
            {
                var go = new GameObject();
                go.transform.parent = TargetCanvas;
                go.name = ident;
                var im = go.AddComponent<Image>();
                var reg = new UISpaceRegister()
                {
                    TimeSinceLastRequest = 0f,
                    Sprite = icon,
                    ShouldFade = fade,
                    Duration = duration,
                    TargetPosition = world_position,
                    UIReference = im
                };
                reg.SetSprite(icon);
                Lookup[ident] = reg;
                return reg.UIReference.gameObject;
            }
        }
        public void DestroyLookup(string ident)
        {
            if (Lookup.ContainsKey(ident))
            {
                Destroy(Lookup[ident].UIReference.gameObject);
                Lookup.Remove(ident);
            }
        }
        private void FixedUpdate()
        {
            foreach(var reg in Lookup.Values)
            {
                if(reg.TimeSinceLastRequest > reg.Duration)
                {
                    reg.UIReference.color = new Color(1, 1, 1, 0);
                    continue;
                }
                
                if (reg.ShouldFade && reg.TimeSinceLastRequest != 0f && reg.Duration > 0f)
                {
                    var alph = (reg.Duration - reg.TimeSinceLastRequest) / reg.Duration;
                    reg.UIReference.color = new Color(1, 1, 1, alph);
                }
                else
                {
                    reg.UIReference.color = new Color(1, 1, 1, 1);
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

