using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace SprintOne.Models.ViewModels
{
    public class RaceRecordViewModel
    {
        public RaceRecord Record { get; set; }

        [Range(1,5, ErrorMessage = "Race type is required.")]
        public int RaceType { get; set; }

        [Range(0, 23, ErrorMessage = "Value for Hours must be between {1} and {2}.")]
        public int RaceTimeHours { get; set; }
        [Range(0, 59, ErrorMessage = "Value for Minutes must be between {1} and {2}.")]
        public int RaceTimeMinutes { get; set; }
        [Range(0, 59, ErrorMessage = "Value for Seconds must be between {1} and {2}.")]
        public int RaceTimeSeconds { get; set; }


        public List<SelectListItem> RaceTypeList = new List<SelectListItem>()
        {
        new SelectListItem() { Text="One Mile", Value="1"},
        new SelectListItem() { Text="5 Kilometers", Value="2"},
        new SelectListItem() { Text="10 Kilometers", Value="3"},
        new SelectListItem() { Text="Half-Marathon", Value="4"},
        new SelectListItem() { Text="Full-Marathon", Value="5"}
        };
    }
}
