using Hoverfly.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hoverfly.Core.Dsl
{
    public class AbstractDelaySettingsBuilder
    {
        public AbstractDelaySettingsBuilder(int delay)
        {
            Delay = delay;
        }

        ///<summary> Convert RequestFieldMatcher list to url/method pattens of <see cref="DelaySettings"/>.</summary>
        ///<returns></returns>
        protected string ToPattern(IList<RequestFieldMatcher> matchers)
        {
            var firstMatch = matchers.Where(m => m.Matcher == MatcherType.Exact || m.Matcher == MatcherType.RegEx).FirstOrDefault();

            if (firstMatch == null)
                throw new ArgumentException("Can't find a specified Exact or Regex matcher.");

            return firstMatch.Value;
        }

        protected int Delay { get; private set; }

        protected bool IsValid()
        {
            return Delay > 0;
        }
    }
}
