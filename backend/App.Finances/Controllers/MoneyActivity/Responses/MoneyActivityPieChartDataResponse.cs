using System;
using System.Collections.Generic;

namespace MyFinances.Controllers.MoneyActivity.Responses
{
    public class MoneyActivityPieChartDataResponse
    {
        public List<MoneyActivityResponse> MoneyActivities { get; set; } = new List<MoneyActivityResponse>();

        public class MoneyActivityResponse
        {
            public string Amount { get; set; }
            public string Category { get; set; }
        }
    }

    public class MoneyActivityXyChartDataResponse
    {
        public List<MoneyActivityResponse> MoneyActivities { get; set; } = new List<MoneyActivityResponse>();

        public class MoneyActivityResponse
        {
            public string Amount { get; set; }
            public string Category { get; set; }
        }
    }
}
