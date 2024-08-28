using Microsoft.EntityFrameworkCore;
using SpreadSheet.Models;

namespace SpreadSheet.Data;

public partial class SpreadSheetDbContext : DbContext
{
    public SpreadSheetDbContext()
    {

    }

    public SpreadSheetDbContext(DbContextOptions<SpreadSheetDbContext> options) : base(options)
    {

    }

    public virtual DbSet<Cell> Cells { get; set; }
}
