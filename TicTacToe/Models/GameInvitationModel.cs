using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToe.Models
{
    public class GameInvitationModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Email To")]
        public string EmailTo { get; set; }

        public string InvitedBy { get; set; }

        public bool IsConfirmed { get; set; }

        public DateTime ConfirmationDate { get; set; }
    }
}
