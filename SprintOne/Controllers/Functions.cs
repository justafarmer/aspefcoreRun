using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SprintOne.Controllers
{
    public class Functions
    {
        public static int GetMileTime(int total, int type)
        {
            var mileTime = 0;
            switch (type)
            {
                case 1:
                    mileTime = total;
                    break;
                case 2:
                    mileTime = Convert.ToInt32(total / 3.106);
                    break;
                case 3:
                    mileTime = Convert.ToInt32(total / 6.21);
                    break;
                case 4:
                    mileTime = Convert.ToInt32(total / 13.11);
                    break;
                case 5:
                    mileTime = Convert.ToInt32(total / 26.22);
                    break;
                default:
                    mileTime = 0;
                    break;
            }
            return mileTime;
        }



    }
}
