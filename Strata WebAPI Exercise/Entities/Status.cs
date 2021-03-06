﻿using System.ComponentModel.DataAnnotations;

namespace Strata_WebAPI_Exercise.Entities
{
    public enum Status
    {
        [Display(Name="Awaiting Dispatch")]
        AwaitingDispatch,
        Dispatched,
        Delivered
    }
}