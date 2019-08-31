using FundaQueries.Models;
using System.Collections.Generic;
using System.Linq;

namespace FundaQueries.Spec
{
    internal class FeedsBuilder
    {
        internal static FeedsBuilder Start => new FeedsBuilder();

        private List<MakelaarBuilderPart> _makelaarBuilderParts = new List<MakelaarBuilderPart>();
        internal class MakelaarBuilderPart
        {
            private readonly FeedsBuilder _parent;

            internal string MakelaarName { get; }
            internal int NumberOfFeeds { get; private set; }
            
            internal MakelaarBuilderPart(string makelaarName, FeedsBuilder parent)
            {
                MakelaarName = makelaarName;
                _parent = parent;
            }

            internal FeedsBuilder HasFeeds(int numberOfFeeds)
            {
                NumberOfFeeds = numberOfFeeds;
                return _parent;
            }
        }

        internal MakelaarBuilderPart Makelaar(string makelaarName)
        {
            var part = new MakelaarBuilderPart(makelaarName, this);
            _makelaarBuilderParts.Add(part);
            return part;
        }

        internal List<Feed> Build()
        {
            return _makelaarBuilderParts
                        .SelectMany(m => Enumerable.Range(1, m.NumberOfFeeds)
                                    .Select(i => new Feed
                                    {
                                        Address = $"Address{i}", MakelaarName = m.MakelaarName
                                    })).ToList();
        }
    }
}
