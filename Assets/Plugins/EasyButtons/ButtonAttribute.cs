﻿using System;

namespace EasyButtons
{
    public enum ButtonMode
    {
        AlwaysEnabled,
        EnabledInPlayMode,
        DisabledInPlayMode
    }
    /// <summary>
    /// Attribute to create a button in the inspector for calling the method it is attached to.
    /// The method must be public and have no arguments.
    /// </summary>
    /// <example>
    /// [Button]
    /// public void MyMethod()
    /// {
    ///     Debug.Log("Clicked!");
    /// }
    /// </example>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class InspectorButton : Attribute
    {
        public ButtonMode mode;
        public InspectorButton(ButtonMode mode = ButtonMode.AlwaysEnabled)
        {
            this.mode = mode;
        }
    }
}
