using System;
using System.Collections.Generic;

namespace UIGenerator
{
    /// <summary>
    /// Example of a context used to pass data to a template page
    /// The context passed can be any serializable object.
    /// 
    /// In this example some simple properties and a reference
    /// to the parent Windows Forms object are passed via this
    /// context reference
    /// 
    /// Context objects are useful for passing application specific
    /// logic to the template. You can also subclass the template
    /// base class and add your own strongly typed objects directly
    /// as properties to the template (visible as this.Property in
    /// template code)
    /// </summary>
    [Serializable]
    public class CustomContext
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }

        public List<CustomContext> Items;

        public CustomContext()
        {
            Name = string.Empty;
            Type = string.Empty;
            Format = string.Empty;
            Items = new List<CustomContext>();
        }
    }
}
