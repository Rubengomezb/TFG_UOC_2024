﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TFG_UOC_2024.DB.Components.Enums;

namespace TFG_UOC_2024.CORE.Models.DTOs
{
    public class BaseSearchDTO
    {
        public int Page { get; set; }
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
    }

    public class BaseSearchInput
    {
        public string Keyword { get; set; } = "";
        public SearchPaginationInput Pagination { get; set; }
        public SearchOrderInput? OrderBy { get; set; }
        public List<SearchDateInput> Dates { get; set; } = new List<SearchDateInput>();
    }

    public class SearchOrderInput
    {
        public string Id { get; set; }
        public bool Desc { get; set; }
    }

    public class SearchPaginationInput
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
    }

    public class SearchDateInput
    {
        public string Field { get; set; } // createdOn, updatedOn, publishStartOn, publishEndOn
        public DateTime? CompareDate { get; set; } = DateTime.UtcNow;
        public Comparison Comparison { get; set; }
    }
}
