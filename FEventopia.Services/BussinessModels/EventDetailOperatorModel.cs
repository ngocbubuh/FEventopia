﻿using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class EventDetailOperatorModel
    {
        public string? Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TicketForSaleInventory { get; set; }
        public int? StallForSaleInventory { get; set; } = 0;
        public double? TicketPrice { get; set; } = 0;
        public double? EstimateCost { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public LocationModel? Location { get; set; }
    }
}
