using System.Collections.Generic;

namespace WindowResizer.Configuration;

public class MatchWindowSize
{
    public WindowSize? FullMatch { get; set; }

    public WindowSize? PrefixMatch { get; set; }

    public WindowSize? SuffixMatch { get; set; }

    public WindowSize? WildcardMatch { get; set; }

    public bool NoMatch =>
        FullMatch == null
        && PrefixMatch == null
        && SuffixMatch == null
        && WildcardMatch == null;

    public List<WindowSize?> All => new()
    {
        FullMatch, PrefixMatch, SuffixMatch, WildcardMatch
    };
}
