using System.Collections.Generic;

namespace Phasma
{
    public class StringData
    {
        public string Key { get; set; } 
        public string Value { get; set; } 
    }

    public class IntData
    {
        public string Key { get; set; } 
        public int Value { get; set; } 
    }

    public class FloatData
    {
        public string Key { get; set; } 
        public float Value { get; set; } 
    }

    public class BoolData
    {
        public string Key { get; set; } 
        public bool Value { get; set; } 
    }

    public class SaveData
    {
        public List<StringData> StringData { get; set; } 
        public List<IntData> IntData { get; set; } 
        public List<FloatData> FloatData { get; set; } 
        public List<BoolData> BoolData { get; set; } 
    }
}