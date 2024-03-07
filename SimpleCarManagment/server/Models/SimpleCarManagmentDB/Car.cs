using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleCarManagment.Models.SimpleCarManagmentDb
{
  [Table("Cars", Schema = "dbo")]
  public partial class Car
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
      get;
      set;
    }

    public ICollection<CarService> CarServices { get; set; }
    public string CustomName
    {
      get;
      set;
    }
    public string Name
    {
      get;
      set;
    }
    public string Image
    {
      get;
      set;
    }
    public DateTime Production
    {
      get;
      set;
    }
    public string Vin
    {
      get;
      set;
    }
    public double Mileage
    {
      get;
      set;
    }
    public string UserId
    {
      get;
      set;
    }
    public string Registration
    {
      get;
      set;
    }
  }
}
