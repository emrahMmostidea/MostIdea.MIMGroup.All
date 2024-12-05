using System;
using System.Collections.Generic;
using System.Text;

namespace MostIdea.MIMGroup.B2B
{ 
    public enum OrderStatusEnum 
    {
        NewOrder = 1,

        Processing = 2,

        Completed = 3,

        Cancelled = 4,

        HasBeenInvoiced = 5,
    }

    public enum OrderItemStatusEnum
    {
        Added = 1,
        
        Used = 2,

        Returned = 3,
    }

    
    
}
