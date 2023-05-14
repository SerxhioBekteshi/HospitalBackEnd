namespace Shared.Types;

public enum LookupFilterOperation
{
    Contains,
    StartsWith,
    EndsWith,
    Equals,
    Less,
    LessOrEquals,
    More,
    MoreOrEquals,
    RangeDate,
    NotEqual
}

public enum LookupSortingDirection
{
    Asc,
    Desc
}

public enum AgentBalanceStatusEnum
{
    Pending,
    Confirmed,
    Declined
}

public enum ReservationStatusEnum
{
    Pending,
    Succeded,
    Postponed,
    Canceled,
}
