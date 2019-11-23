using System;
using System.Collections.Generic;

namespace JazzOpsApp.Models
{
    public interface ISensor
    {
        Measurements Measurements { get; set; }
        IEnumerable<Measurements> GetData();
        
    }
}