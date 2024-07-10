using Microsoft.EntityFrameworkCore;
using System.Data;
using WebKimCuong.Models;

namespace thuongmaidientus1.Models
{
    public class DBContexts : DbContext
    {
        public DBContexts(DbContextOptions options) : base(options) { }

        #region DBSet
        public DbSet<UserRegMst> UserRegMsts { get; set; }
        public DbSet<CatMst> CatMsts { get; set; }
        public DbSet<ProdMst> ProdMsts { get; set; }
        public DbSet<CertifyMst> CertifyMsts { get; set; }
        public DbSet<DimMst> DimMsts { get; set; }
        public DbSet<ItemMst> ItemMsts { get; set; }
        public DbSet<Roles> roles { get; set; }
        public DbSet<Tokens> tokens { get; set; }
        public DbSet<Inquiry> Inquirys { get; set; }
        public DbSet<StoneMst> StoneMsts { get; set; }
        public DbSet<DimQltyMst> DimQltyMst { get; set; }
        public DbSet<GoldKrtMst> GoldKrtMst { get; set; }
        public DbSet<JewelTypeMst> JewelTypeMst { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<Merchant> merchants { get; set; }
        public DbSet<PaymentTransaction> paymentTransactions { get; set; }
        public DbSet<PaymentDescription> paymentDescriptions { get; set; }
        public DbSet<PaymentNotification> paymentNotifications { get; set; }
        public DbSet<PaymentSignature> paymentSignatures { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<CommentDescription> commentDescriptions { get; set; }
        public DbSet<Danhgia> danhGias { get; set; }
        public DbSet<ChungnhanMst> chungNhans { get; set; }
        public DbSet<DimInfoMst> DimInfoMsts { get; set; }
        public DbSet<DimQltySubMst> DimQltySubMsts { get; set; }
        public DbSet<Motoi> Motois { get; set; }
        public DbSet<StoneQltyMst> StoneQltyMsts { get; set; }
        public DbSet<ThuonghieuMst> ThuonghieuMsts { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=LAPTOP-D5OKMO3H\\SQLEXPRESS;Initial Catalog=shopkimcuongs;Persist Security Info=True;User ID=sa;Password=1234;Encrypt=True;Trust Server Certificate=True")
                         .EnableSensitiveDataLogging(); // Bật chế độ ghi log chi tiết
        }
    }
}
