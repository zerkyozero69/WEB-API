﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Jwt.Models.นับจำนวนกิจกรรม
{
    public class count_NumberActivityName
    {
        public string ActivityName { get; set; }
        public string Count { get; set; }
    }
    public class count_NumberSendSeed
    {
        public string OrganizeName { get; set; }
        public string Count { get; set; }
    }
    public class Titile_Group
    {
        public Titile_Group()
        {
            Status_List = new List<Status_count>();
        }

        public string GroupName { get; set; }
        public List<Status_count> Status_List { get; set; }
    }
    public class Status_count
    {
        public string ActivityName { get; set; }
        public string CountActivityName { get; set; }
    }

    //PhonesList.Add(new PhoneGroup("", new[]{ new Phone
    //            {
    //                Title = "หน้าหลัก",
    //                Price = 50000,
    //                ImageUrl = "home.png",
    //                PageView = "HomePage",
    //                HasBadges = false,
    //                BadgeCount = 0
    //            },
    //            new Phone
    //            {
    //                Title = "สืบค้นเอกสาร",
    //                Price = 38000,
    //                ImageUrl = "find.png",
    //                PageView = "SearcherPage",
    //                HasBadges = false,
    //                BadgeCount = 0
    //            },
    //            new Phone
    //            {
    //                Title = "รายการขอทำสำเนา",
    //                Price = 38000,
    //                ImageUrl = "cart.png",
    //                PageView = "OrderListPage",
    //                HasBadges = true,
    //                BadgeCount = 10
    //            },
    //            new Phone
    //            {
    //                Title = "ข้อมูลการขอทำสำเนา",
    //                Price = 38000,
    //                ImageUrl = "checklist.png",
    //                PageView = "OrderHistoryPage",
    //                HasBadges = true,
    //                BadgeCount = 1
    //            }
    //        })); 

}