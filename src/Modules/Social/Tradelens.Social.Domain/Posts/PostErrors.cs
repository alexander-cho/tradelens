using ApriumResearch.TidyResult;

namespace Tradelens.Social.Domain.Posts;

public static class PostErrors
{
    public static Error EmptyContent =>
        new("Error.EmptyContent", "Post content cannot be empty");

    public static Error MaxCashtagsPerPostLimitExceeded =>
        new("Error.MaxCashtagsPerPostLimitExceeded", "Post content cannot be empty");

    public static Error MinimumContentLengthNotMet =>
        new("Error.MinimumContentLengthNotMet", "Post content cannot be empty");

    public static Error CannotDeletePastTime =>
        new("Error.CannotDeletePastTime", "Post content cannot be empty");
}