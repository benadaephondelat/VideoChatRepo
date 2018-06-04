//namespace VideoChatWebApp.Data
//{
//    using global::TestMakerFreeWebApp.Data;

//    using Microsoft.AspNetCore.Identity;
//    using System;
//    using System.Collections.Generic;
//    using System.ComponentModel.DataAnnotations;

//    namespace TestMakerFreeWebApp.Data
//    {
//        public class ApplicationUser : IdentityUser
//        {
//            public ApplicationUser()
//            {

//            }
            
//            public string DisplayName { get; set; }

//            public string Notes { get; set; }

//            [Required]
//            public int Type { get; set; }

//            [Required]
//            public int Flags { get; set; }

//            [Required]
//            public DateTime CreatedDate { get; set; }

//            [Required]
//            public DateTime LastModifiedDate { get; set; }

//            public virtual List<Token> Tokens { get; set; }
//        }
//    }
//}