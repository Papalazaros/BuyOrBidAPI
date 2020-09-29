using System.Collections;
using System.Collections.Generic;

namespace BuyOrBid.Models
{
    public enum RuleType
    {
        Range,
        Required,
        Number
    }

    public class RangeRule : Rule
    {
        public RangeRule(object valueFrom, object valueTo)
        {
            Type = RuleType.Range;
            ValueFrom = valueFrom;
            ValueTo = valueTo;
        }

        public object ValueFrom { get; set; }
        public object ValueTo { get; set; }
    }

    public class Rule
    {
        public RuleType? Type { get; set; }
    }

    public class FilterDescriptor
    {
        public string? PropertyName { get; set; }
        public string? PropertyType { get; set; }
        public string? PropertySubType { get; set; }
        public string? PropertyArgument { get; set; }
        public IEnumerable? AvailableValues { get; set; }
        public IEnumerable<RangeRule>? Rules { get; set; }
    }
}
