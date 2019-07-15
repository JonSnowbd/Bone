using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bone.BGUIUtil
{
    public abstract class BGUIGroup
    {
        protected List<IBGUIDrawRequest> Requests;
        /// <summary>
        /// Registers a component to be drawn within this group. You do not need to use this for
        /// normal usage of BGUI, but if you want to load your own component this is what you'd use
        /// instead of `Example.Label("thing");`, you'd `Example.RegisterDrawRequest(yourCustomComponent);`.
        /// </summary>
        /// <param name="request">Your custom component</param>
        public virtual void RegisterDrawRequest(IBGUIDrawRequest request)
        {
            if (Requests == null)
                Requests = new List<IBGUIDrawRequest>();
            Requests.Add(request);
        }
        /// <summary>
        /// Requests that the group supply a rect for where a component of prefferedWidth and preferredHeight
        /// should be placed on the screen(and in the group.)
        /// </summary>
        /// <param name="preferredWidth">How wide the component would like to be</param>
        /// <param name="preferredHeight">How tall the component would like to be</param>
        /// <returns>A rect of where this group would place the component with given height and width.</returns>
        public abstract Rect RequestNextRect(float preferredWidth, float preferredHeight);
        /// <summary>
        /// Tells the group to finish all its calculations and output the registered requests to the screen.
        /// </summary>
        /// <returns>A rect that represents the final volume this group used on the screen.</returns>
        public abstract Rect Commit();
        /// <summary>
        /// Request that the group sums up what it believes it will be sized if it gets Committed.
        /// </summary>
        /// <returns>The Group's Size it will be if it gets Committed.</returns>
        public abstract Rect GetBoundingRect();
        /// <summary>
        /// Starts a new label inside of the current group
        /// </summary>
        /// <param name="s">What you want the label to say. You are free to use escape codes.</param>
        /// <returns>Returns the group for easy method chaining.</returns>
        public BGUIGroup Label(string s)
        {
            RegisterDrawRequest(new BGUILabelDrawRequest(this, s));
            return this;
        }
        /// <summary>
        /// Starts a new slider inside of the current group
        /// </summary>
        /// <param name="del">The delegate function that takes a float. Looking like `void YourFunction(float val);`</param>
        /// <param name="current_value">What the value currently is</param>
        /// <param name="left">If the knob was at the farthest left, what would you want that value to be</param>
        /// <param name="right">If the knob was at the farthest right, what would you want that value to be</param>
        /// <param name="width">How wide you want the slider to be</param>
        /// <returns>Returns the group for easy method chaining.</returns>
        public BGUIGroup Slider(BGUIValueDelegate<float> del, float current_value, float left, float right, float width)
        {
            RegisterDrawRequest(new BGUISliderDrawRequest(this, width, del, current_value, left, right));
            return this;
        }
        /// <summary>
        /// Starts a new button inside of the current group
        /// </summary>
        /// <param name="del">The delegate function that takes no parameters. Looking like `void YourFunction();`</param>
        /// <param name="s">What you want the label inside the button to say.</param>
        /// <returns>Returns the group for easy method chaining.</returns>
        public BGUIGroup Button(BGUIButtonDelegate del, string s)
        {
            RegisterDrawRequest(new BGUIButtonDrawRequest(this, del, s));
            return this;
        }
        /// <summary>
        /// Requests that the group draw its background. NOTE: This should ALWAYS be before `Commit()`! Otherwise all your content will be
        /// below the background due to limitations in `GUI`
        /// </summary>
        /// <returns>Returns the group for easy method chaining.</returns>
        public virtual BGUIGroup Background()
        {
            var bb = GetBoundingRect();
            GUI.Box(bb, new GUIContent(""));
            return this;
        }
    }
}
