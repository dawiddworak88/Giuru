﻿using System;

namespace Analytics.Api.ServicesModels.SalesAnalytics
{
    public class DailySalesServiceModel
    {
        public int DayOfWeek { get; set; }
        public int Day { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double Quantity { get; set; }
    }
}