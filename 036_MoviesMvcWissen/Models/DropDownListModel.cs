using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _036_MoviesMvcWissen.Models
{
    public class DropDownListModel
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public DropDownListModel()
        {

        }
        public DropDownListModel(string value, string text)
        {
           
            Value = value;
            Text = text;
        }
    }
}