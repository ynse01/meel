using Meel.Search;
using System;

namespace Meel.Parsing
{
    public static class SearchKeyParser
    {
        public static ISearchKey Parse(ReadOnlySpan<byte> query, uint numMessages)
        {
            return Parse(ref query, numMessages);
        }

        private static ISearchKey Parse(ref ReadOnlySpan<byte> query, uint numMessages)
        {
            ReadOnlySpan<byte> current = ReadNextToken(ref query);
            ISearchKey key;
            if (LexiConstants.IsDigit(current[0]) || current[0] == LexiConstants.Asterisk)
            {
                key = new SequenceSetSearchKey(current, numMessages);
            } else if (current[0] == LexiConstants.OpenParenthesis)
            {
                // Between parenthesis is assumed to be AND-ed together.
                var closeIndex = query.IndexOf(LexiConstants.CloseParenthesis);
                var betweenBraces = query.Slice(1, closeIndex);
                // Start the AND-ing will All messages.
                key = new AllSearchKey();
                while (!betweenBraces.IsEmpty)
                {
                    var newKey = Parse(ref betweenBraces, numMessages);
                    key = new AndSearchKey(key, newKey);
                }
            } else
            {
                if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Answered))
                {
                    key = new AnsweredSearchKey(false);
                } else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Bcc))
                {
                    var token = ReadNextToken(ref query);
                    key = new BccSearchKey(token);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Before))
                {
                    var token = ReadNextToken(ref query);
                    key = new BeforeSearchKey(token);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Body))
                {
                    var token = ReadNextToken(ref query);
                    key = new BodySearchKey(token);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Cc))
                {
                    var token = ReadNextToken(ref query);
                    key = new CcSearchKey(token);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Deleted))
                {
                    key = new DeletedSearchKey(false);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Flagged))
                {
                    key = new FlaggedSearchKey(false);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.From))
                {
                    var token = ReadNextToken(ref query);
                    key = new FromSearchKey(token);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Keyword))
                {
                    var token = ReadNextToken(ref query);
                    key = new KeywordSearchKey(token, false);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.New))
                {
                    key = new NewSearchKey();
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Old))
                {
                    key = new OldSearchKey();
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.On))
                {
                    var token = ReadNextToken(ref query);
                    key = new OnSearchKey(token);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Recent))
                {
                    key = new RecentSearchKey(false);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Seen))
                {
                    key = new SeenSearchKey(false);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Since))
                {
                    var token = ReadNextToken(ref query);
                    key = new SinceSearchKey(token);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Subject))
                {
                    var token = ReadNextToken(ref query);
                    key = new SubjectSearchKey(token);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Text))
                {
                    var token = ReadNextToken(ref query);
                    key = new TextSearchKey(token);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.To))
                {
                    var token = ReadNextToken(ref query);
                    key = new ToSearchKey(token);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.UnAnswered))
                {
                    key = new AnsweredSearchKey(true);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.UnDeleted))
                {
                    key = new DeletedSearchKey(true);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.UnFlagged))
                {
                    key = new FlaggedSearchKey(true);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.UnKeyword))
                {
                    var token = ReadNextToken(ref query);
                    key = new KeywordSearchKey(token, true);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.UnSeen))
                {
                    key = new SeenSearchKey(true);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Draft))
                {
                    key = new DraftSearchKey(false);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Header))
                {
                    var field = ReadNextToken(ref query);
                    var value = ReadNextToken(ref query);
                    key = new HeaderSearchKey(field, value);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Larger))
                {
                    var token = ReadNextToken(ref query);
                    key = new LargerSearchKey(token.AsNumber());
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Not))
                {
                    var inner = Parse(ref query, numMessages);
                    key = new NotSearchKey(inner);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Or))
                {
                    var first = Parse(ref query, numMessages);
                    var second = Parse(ref query, numMessages);
                    key = new OrSearchKey(first, second);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.SentBefore))
                {
                    var token = ReadNextToken(ref query);
                    key = new SentBeforeSearchKey(token);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.SentOn))
                {
                    var token = ReadNextToken(ref query);
                    key = new SentOnSearchKey(token);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.SentSince))
                {
                    var token = ReadNextToken(ref query);
                    key = new SentSinceSearchKey(token);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Smaller))
                {
                    var token = ReadNextToken(ref query);
                    key = new SmallerSearchKey(token.AsNumber());
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.Uid))
                {
                    var token = ReadNextToken(ref query);
                    key = new UidSearchKey(token, numMessages);
                }
                else if (AsciiComparer.CompareIgnoreCase(current, LexiConstants.UnDraft))
                {
                    key = new DraftSearchKey(true);
                }
                else
                {
                    key = new AllSearchKey();
                }
            }
            return key;
        }

        private static ReadOnlySpan<byte> ReadNextToken(ref ReadOnlySpan<byte> query)
        {
            ReadOnlySpan<byte> token;
            var partIndex = query.IndexOf(LexiConstants.Space);
            if (partIndex == -1)
            {
                // Take entire input and stop afterwards.
                token = query;
                query = ReadOnlySpan<byte>.Empty;
            }
            else
            {
                token = query.Slice(0, partIndex);
                query = query.Slice(partIndex + 1);
            }
            return token;
        }
    }
}
