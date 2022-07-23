using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DPL.EF
{
    public partial class CashFlowDbContext : DbContext
    {
        public CashFlowDbContext()
        {
        }

        public CashFlowDbContext(DbContextOptions<CashFlowDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnswerQuestion> AnswerQuestions { get; set; } = null!;
        public virtual DbSet<Asset> Assets { get; set; } = null!;
        public virtual DbSet<AssetCategory> AssetCategories { get; set; } = null!;
        public virtual DbSet<Card> Cards { get; set; } = null!;
        public virtual DbSet<CardEffect> CardEffects { get; set; } = null!;
        public virtual DbSet<CashFlow> CashFlows { get; set; } = null!;
        public virtual DbSet<CashFlowCategory> CashFlowCategories { get; set; } = null!;
        public virtual DbSet<EffectTable> EffectTables { get; set; } = null!;
        public virtual DbSet<Function> Functions { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<QustionEffect> QustionEffects { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<RoleFunction> RoleFunctions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserBoard> UserBoards { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<AnswerQuestion>(entity =>
            {
                entity.Property(e => e.Id).HasComment("使用者問卷答案流水號");

                entity.Property(e => e.Answer).HasComment("答案:[\"1個\"]");

                entity.Property(e => e.QusetionId).HasComment("問卷流水號 ( 外鍵 )");

                entity.Property(e => e.UserId).HasComment("使用者流水號 ( 外鍵 )");
            });

            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AssetCategoryId).HasComment("資產類別流水號 ( 外鍵 )");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasComment("資產流水號");

                entity.Property(e => e.Name).HasComment("資產名稱");

                entity.Property(e => e.Status).HasComment("狀態");

                entity.Property(e => e.Value)
                    .HasColumnType("money")
                    .HasComment("資產價值 ( 台幣 )");
            });

            modelBuilder.Entity<AssetCategory>(entity =>
            {
                entity.Property(e => e.Id).HasComment("資產類別流水號");

                entity.Property(e => e.Name).HasComment("資產類別名稱");

                entity.Property(e => e.ParentId).HasComment("父類別流水號");

                entity.Property(e => e.Status).HasComment("狀態 0. 停用 1. 啟用 2. 刪除");
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.Property(e => e.Id).HasComment("卡片流水號");

                entity.Property(e => e.Name).HasComment("卡片名稱");

                entity.Property(e => e.Status).HasComment("狀態 0. 停用 1. 啟用 2. 刪除");
            });

            modelBuilder.Entity<CardEffect>(entity =>
            {
                entity.Property(e => e.Id).HasComment("卡片影響類別流水號");

                entity.Property(e => e.CardId).HasComment("卡片流水號 ( 外鍵 )");

                entity.Property(e => e.Description).HasComment("影響效果");

                entity.Property(e => e.EffectTableId).HasComment("影響資料表流水號 ( 外鍵 )");

                entity.Property(e => e.TableId).HasComment("影響類別內流水號 ( 外鍵 )");
            });

            modelBuilder.Entity<CashFlow>(entity =>
            {
                entity.Property(e => e.Id).HasComment("現金流流水號");

                entity.Property(e => e.CashFlowCategoryId).HasComment("現金流類別流水號");

                entity.Property(e => e.Name).HasComment("現金流名稱");

                entity.Property(e => e.Status).HasComment("狀態 0. 停用 1. 啟用 2. 刪除");

                entity.Property(e => e.Value)
                    .HasColumnType("money")
                    .HasComment("現金流價值");
            });

            modelBuilder.Entity<CashFlowCategory>(entity =>
            {
                entity.Property(e => e.Id).HasComment("現金流類別流水號");

                entity.Property(e => e.Name).HasComment("現金流類別名稱");

                entity.Property(e => e.ParentId).HasComment("父類別流水號");

                entity.Property(e => e.Status).HasComment("狀態 0. 停用 1. 啟用 2. 刪除");

                entity.Property(e => e.Time).HasComment("所花費小時");

                entity.Property(e => e.Type).HasComment("現金流類型 1. 主動 2. 被動");
            });

            modelBuilder.Entity<EffectTable>(entity =>
            {
                entity.Property(e => e.Id).HasComment("影響資料表流水號");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasComment("受影響資料表名稱");

                entity.Property(e => e.Status).HasComment("狀態 0. 停用 1. 啟用 2. 刪除");
            });

            modelBuilder.Entity<Function>(entity =>
            {
                entity.Property(e => e.Id).HasComment("功能流水號");

                entity.Property(e => e.Description).HasComment("功能解釋:封鎖帳號...");

                entity.Property(e => e.Status).HasComment("狀態 0. 停用 1. 啟用 2. 刪除");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.Id).HasComment("日誌流水號");

                entity.Property(e => e.Action).HasComment("動作 1 新增 2 修改 3 刪除");

                entity.Property(e => e.ActionDate)
                    .HasColumnType("datetime")
                    .HasComment("執行動作時間");

                entity.Property(e => e.TableId).HasComment("資料表內流水號");

                entity.Property(e => e.TableName)
                    .HasMaxLength(50)
                    .HasComment("資料表名稱");

                entity.Property(e => e.UserId).HasComment("使用者流水號 ( 外鍵 )");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasComment("使用者名稱");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Id).HasComment("問卷流水號");

                entity.Property(e => e.Answer).HasComment("選項答案 :[\"1個,2個\"]");

                entity.Property(e => e.Name).HasComment("題目名稱 ,Ex.生幾個小孩");

                entity.Property(e => e.Status).HasComment("狀態 0. 停用 1. 啟用 2. 刪除");

                entity.Property(e => e.Type).HasComment("問卷類型 1. 單選 2. 多選 3. 自由填文字 4. 數字");
            });

            modelBuilder.Entity<QustionEffect>(entity =>
            {
                entity.Property(e => e.Id).HasComment("問卷影響類別流水號");

                entity.Property(e => e.Description).HasComment("影響效果");

                entity.Property(e => e.EffectTableId).HasComment("影響資料表流水號 ( 外鍵 )");

                entity.Property(e => e.QuestionId).HasComment("問卷流水號 ( 外鍵 )");

                entity.Property(e => e.TableId).HasComment("影響類別內流水號");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasComment("權限角色流水號");

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .HasComment("權限角色名稱 管理員、玩家");

                entity.Property(e => e.Status).HasComment("狀態 0. 停用 1. 啟用 2. 刪除");
            });

            modelBuilder.Entity<RoleFunction>(entity =>
            {
                entity.Property(e => e.Id).HasComment("權限功能流水號");

                entity.Property(e => e.FunctionId).HasComment("功能流水號 ( 外鍵 )");

                entity.Property(e => e.RoleId).HasComment("權限角色流水號 ( 外鍵 )");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasComment("使用者流水號");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasComment("信箱");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasComment("姓名");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("密碼 hash");

                entity.Property(e => e.RoleId).HasComment("權限角色流水號 ( 外鍵 )");

                entity.Property(e => e.Status).HasComment("狀態 0. 停用 1. 啟用 2. 刪除");
            });

            modelBuilder.Entity<UserBoard>(entity =>
            {
                entity.Property(e => e.Id).HasComment("排行榜流水號");

                entity.Property(e => e.Debt)
                    .HasColumnType("money")
                    .HasComment("半小時內最高負債");

                entity.Property(e => e.NetProfit)
                    .HasColumnType("money")
                    .HasComment("半小時內最高淨收入");

                entity.Property(e => e.Revenue)
                    .HasColumnType("money")
                    .HasComment("半小時內最高收入");

                entity.Property(e => e.TotoalNetProfit)
                    .HasColumnType("money")
                    .HasComment("總時長淨收入");

                entity.Property(e => e.UserId).HasComment("使用者流水號 ( 外鍵 )");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
