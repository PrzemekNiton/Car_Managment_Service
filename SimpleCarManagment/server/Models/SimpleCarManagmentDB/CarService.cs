using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleCarManagment.Models.SimpleCarManagmentDb
{
  [Table("CarServices", Schema = "dbo")]
  public partial class CarService
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
      get;
      set;
    }
    public decimal Cost
    {
      get;
      set;
    }
    public DateTime Date
    {
      get;
      set;
    }
    public int ServiceId
    {
      get;
      set;
    }
    public Service Service { get; set; }
    public int? CarId
    {
      get;
      set;
    }
    public Car Car { get; set; }
    public string Document
    {
      get;
      set;
    }
    public double Mileage
    {
      get;
      set;
    }
  }
}
