using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleCarManagment.Models.SimpleCarManagmentDb
{
  [Table("Services", Schema = "dbo")]
  public partial class Service
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
      get;
      set;
    }

    public ICollection<CarService> CarServices { get; set; }
    public string Name
    {
      get;
      set;
    }
    public double Km
    {
      get;
      set;
    }
    public int Years
    {
      get;
      set;
    }
  }
}
