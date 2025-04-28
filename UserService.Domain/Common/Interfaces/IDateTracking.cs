﻿namespace UserService.Domain.Common.Interfaces;

public interface IDateTracking
{
    DateTimeOffset? CreatedAt { get; set; }
    DateTimeOffset? LastModifiedAt { get; set; }
}
