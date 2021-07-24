namespace WindowResizer
{
    public enum MatchOrder
    {
        FullMatch,
        PrefixMatch,
        SuffixMatch,
        WildcardMatch
    }

    public class MatchWindowSize
    {
        public WindowSize FullMatch { get; set; }

        public WindowSize PrefixMatch { get; set; }

        public WindowSize SuffixMatch { get; set; }

        public WindowSize WildcardMatch { get; set; }

        public bool NoMatch
        {
            get
            {
                return FullMatch == null
                       && PrefixMatch == null
                       && SuffixMatch == null
                       && WildcardMatch == null;
            }
        }
    }
}
