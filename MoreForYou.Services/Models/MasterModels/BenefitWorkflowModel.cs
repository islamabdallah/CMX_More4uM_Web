using MoreForYou.Models.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreForYou.Services.Models.MasterModels
{
    public class BenefitWorkflowModel
    {
        [Required]
        public BenefitModel BenefitModel { get; set; }

        [Required]
        public long BenefitId { get; set; }

        [Required]
        public string RoleId { get; set; }

        [Required]
        public List<RoleModel> RoleModels { get; set; }

        [Required]
        public int Order { get; set; }

        public bool IsDelted { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public List<RoleOrder> RolesOrder { get; set; }

        public string RoleName { get; set; }
    }

   }
