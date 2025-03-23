﻿namespace CesiZen.Domain.DataTransfertObject;

public class PagedResultDto<T>
{
    public List<T> Data { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
