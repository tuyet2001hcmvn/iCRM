using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.Constant
{
    public class ConstTaskStatus
    {
        //Chưa hoàn thành
        public static string Incomplete = "Incomplete";
        //Cần làm
        public static string Todo = "Todo";
        //Đang thực hiện
        public static string Processing = "Processing";
        //Đã hoàn thành
        public static string Completed = "Completed";
        //Đã hoàn thành đúng hạn
        public static string CompletedOnTime = "CompletedOnTime";
        //Đã hoàn thành quá hạn
        public static string CompletedExpire = "CompletedExpire";
        //Đã quá hạn
        public static string Expired = "Expired";


        //Đang theo dõi
        public static string Follow = "Follow";
        //Chưa được phân công
        public static string Unassign = "Unassign";
        //Tất cả
        public static string All = "All";

    }
}
