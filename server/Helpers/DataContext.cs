using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using WebApi.Entities;
using WebApi.Entities.Identity;

namespace WebApi.Helpers
{
  //public class DataContext : IdentityDbContext<User, Role, int>
  public class DataContext : DbContext
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
      Database.SetCommandTimeout(60);
    }

    #region TABLES

    public DbSet<Application> Application { get; set; }
    public DbSet<AppUser> AppUser { get; set; }
    public DbSet<AppRole> AppRole { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<MenuItem> MenuItem { get; set; }
    public DbSet<Permission> Permission { get; set; }
    public DbSet<RoleMenu> RoleMenu { get; set; }
    public DbSet<MenuPermission> MenuPermission { get; set; }
    public DbSet<LoginProvider> LoginProvider { get; set; }
    public DbSet<OneTimePassword> OneTimePassword { get; set; }

    // Cyprus Tables

    public virtual DbSet<ActivityLog> ActivityLog { get; set; }
    public virtual DbSet<Addresses> Addresses { get; set; }
    public virtual DbSet<AffiliationPeriods> AffiliationPeriods { get; set; }
    public virtual DbSet<Agencies> Agencies { get; set; }
    public virtual DbSet<Alianzas> Alianzas { get; set; }
    public virtual DbSet<Beneficiaries> Beneficiaries { get; set; }
    public virtual DbSet<BonaFides> BonaFides { get; set; }
    public virtual DbSet<CallReasons> CallReasons { get; set; }
    public virtual DbSet<Campaigns> Campaigns { get; set; }
    public virtual DbSet<CanceledCategories> CanceledCategories { get; set; }
    public virtual DbSet<CanceledReasons> CanceledReasons { get; set; }
    public virtual DbSet<CanceledSubcategories> CanceledSubcategories { get; set; }
    public virtual DbSet<ChapterClient> ChapterClient { get; set; }
    public virtual DbSet<Chapters> Chapters { get; set; }
    public virtual DbSet<Cities> Cities { get; set; }
    public virtual DbSet<ClientCommunicationMethod> ClientCommunicationMethod { get; set; }
    public virtual DbSet<ClientDocumentType> ClientDocumentType { get; set; }
    public virtual DbSet<ClientProduct> ClientProduct { get; set; }
    public virtual DbSet<ClientUser> ClientUser { get; set; }
    public virtual DbSet<Clients> Clients { get; set; }
    public virtual DbSet<CommunicationMethods> CommunicationMethods { get; set; }
    public virtual DbSet<Countries> Countries { get; set; }
    public virtual DbSet<Covers> Covers { get; set; }
    public virtual DbSet<CsvDatas> CsvDatas { get; set; }
    public virtual DbSet<Dependents> Dependents { get; set; }
    public virtual DbSet<DocumentCategories> DocumentCategories { get; set; }
    public virtual DbSet<DocumentTypes> DocumentTypes { get; set; }
    public virtual DbSet<Files> Files { get; set; }
    public virtual DbSet<HealthPlans> HealthPlans { get; set; }
    //public virtual DbSet<Migrations> Migrations { get; set; }
    public virtual DbSet<PasswordResets> PasswordResets { get; set; }
    public virtual DbSet<Products> Products { get; set; }
    public virtual DbSet<Prospects> Prospects { get; set; }
    public virtual DbSet<QualifyingEvents> QualifyingEvents { get; set; }
    public virtual DbSet<Regions> Regions { get; set; }
    public virtual DbSet<Retirements> Retirements { get; set; }
    public virtual DbSet<Roles> Roles { get; set; }
    public virtual DbSet<States> States { get; set; }
    public virtual DbSet<Tutors> Tutors { get; set; }
    public virtual DbSet<Users> Users { get; set; }
    public virtual DbSet<Zipcodes> Zipcodes { get; set; }
    public DbSet<InsuranceBenefitType> InsuranceBenefitType { get; set; }
    public DbSet<InsurancePlanBenefit> InsurancePlanBenefit { get; set; }
    public DbSet<InsuranceRate> InsuranceRate { get; set; }
    //public DbSet<InsuranceClient> InsuranceClient { get; set; }
    public DbSet<InsuranceAddOns> InsuranceAddOns { get; set; }
    public DbSet<InsurancePlanAddOns> InsurancePlanAddOns { get; set; }
    public DbSet<InsuranceAddOnsRateAge> InsuranceAddOnsRateAge { get; set; }

    public virtual DbSet<TypeOfRelationship> TypeOfRelationship { get; set; }
    public virtual DbSet<AffType> AffType { get; set; }
    public virtual DbSet<AlianzaAddOns> AlianzaAddOns { get; set; }

    #endregion

    #region FUNCTIONS

    //[DbFunction("IsValidIdentity", "dbo")]
    //public static int? IsValidIdentity(string firstName, string ssno, string type)
    //{
    //    throw new NotImplementedException();
    //}

    //[DbFunction("IsValidIdentity")]
    //public static int IsValidIdentity(string firstName, string ssno, string type) => throw new Exception();

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

      base.OnModelCreating(modelBuilder);

      #region Base Identity Tables

      new ApplicationMap(modelBuilder.Entity<Application>());
      new UserMap(modelBuilder.Entity<AppUser>());
      new RoleMap(modelBuilder.Entity<AppRole>());
      new MenuPermissionMap(modelBuilder.Entity<MenuPermission>());
      new UserRoleMap(modelBuilder.Entity<UserRole>());
      new MenuItemMap(modelBuilder.Entity<MenuItem>());
      new RoleMenuMap(modelBuilder.Entity<RoleMenu>());
      new PermissionMap(modelBuilder.Entity<Permission>());
      new LoginProviderMap(modelBuilder.Entity<LoginProvider>());
      new OneTimePasswordMap(modelBuilder.Entity<OneTimePassword>());
      new TypeOfRelationshipMap(modelBuilder.Entity<TypeOfRelationship>());
      new AffTypeMap(modelBuilder.Entity<AffType>());


      new InsurancePlanAddOnsMap(modelBuilder.Entity<InsurancePlanAddOns>());


      #endregion

      #region Cyprus Tables


      modelBuilder.Entity<InsuranceBenefitType>(entity =>
      {
        entity.ToTable("insurance_benefit_type");

        entity.Property(e => e.Id).HasColumnName("id");
        entity.HasKey(t => t.Id);

        entity.Property(e => e.ParentBenefitTypeID).HasColumnName("parent_benefit_type_id");

        entity.Property(e => e.BenefitType)
          .IsRequired()
          .HasColumnName("benefit_type")
          .HasMaxLength(255);

        entity.Property(e => e.RowOrder).HasColumnName("row_order");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");


        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");


      });

      modelBuilder.Entity<InsuranceAddOns>(entity =>
      {
        entity.ToTable("insurance_addOns");

        entity.Property(e => e.Id).HasColumnName("id");
        entity.HasKey(t => t.Id);

        entity.Property(e => e.HealthPlanId).HasColumnName("health_plan_id");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.IndividualRate).HasColumnName("individual_rate");

        entity.Property(e => e.CoverageSingleRate).HasColumnName("coverage_single_rate");

        entity.Property(e => e.CoverageCoupleRate).HasColumnName("coverage_couple_rate");

        entity.Property(e => e.CoverageFamilyRate).HasColumnName("coverage_family_rate");

        entity.Property(e => e.MinimumEE).HasColumnName("minimum_EE");

        entity.Property(e => e.TypeCalculate).HasColumnName("type_calculate");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");


        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");


        entity.HasOne(d => d.HealthPlans)
          .WithMany(p => p.InsuranceAddOns)
          .HasForeignKey(d => d.HealthPlanId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("insurance_addOns_health_plan_id_foreign");

      });

      modelBuilder.Entity<InsuranceAddOnsRateAge>(entity =>
      {
        entity.ToTable("insurance_addOns_rate_age");

        entity.Property(e => e.Id).HasColumnName("id");
        entity.HasKey(t => t.Id);

        entity.Property(e => e.InsuranceAddOnsId).HasColumnName("insurance_addOns_id");

        entity.Property(e => e.Age).HasColumnName("age");

        entity.Property(e => e.Rate).HasColumnName("rate");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");


        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");


        entity.HasOne(d => d.InsuranceAddOns)
          .WithMany(p => p.RatesByAge)
          .HasForeignKey(d => d.InsuranceAddOnsId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("rates_by_age_insurance_addOns_id_foreign");

      });

      modelBuilder.Entity<InsurancePlanBenefit>(entity =>
      {
        entity.ToTable("insurance_plan_benefit");


        entity.Property(e => e.Id).HasColumnName("id");
        entity.HasKey(t => t.Id);

        entity.Property(e => e.CoverId)
          .IsRequired().HasColumnName("cover_id");

        entity.Property(e => e.InsuranceBenefitTypeId)
          .IsRequired()
          .HasColumnName("insurance_benefit_type_id");

        entity.Property(e => e.Value)
          .HasColumnName("value")
          .HasMaxLength(255);

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");


        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.Covers)
          .WithMany(p => p.BenefitTypes)
          .HasForeignKey(d => d.CoverId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("cover_insurance_plan_benefit_foreign");

      });

      modelBuilder.Entity<InsuranceRate>(entity =>
      {
        entity.ToTable("insurance_rate");

        entity.Property(e => e.Id).HasColumnName("id");
        entity.HasKey(t => t.Id);

        entity.Property(e => e.CoverId)
          .IsRequired().HasColumnName("cover_id");

        entity.Property(e => e.Age).HasColumnName("age");

        entity.Property(e => e.RateEffectiveDate)
          .HasColumnName("rate_effective_date")
          .HasColumnType("date");

        entity.Property(e => e.RateExpirationDate)
          .HasColumnName("rate_expiration_date")
          .HasColumnType("date");

        entity.Property(e => e.IndividualRate).HasColumnName("individual_rate");

        entity.Property(e => e.IndividualTobaccoRate).HasColumnName("individual_tobacco_rate");

        entity.Property(e => e.PolicyYear).HasColumnName("policy_year");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");


        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");


        entity.HasOne(d => d.Covers)
          .WithMany(p => p.Rate)
          .HasForeignKey(d => d.CoverId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("cover_insurance_rate_foreign");

      });

      modelBuilder.Entity<ActivityLog>(entity =>
      {
        entity.ToTable("activity_log");

        entity.HasIndex(e => e.LogName)
          .HasName("activity_log_log_name_index");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CauserId).HasColumnName("causer_id");

        entity.Property(e => e.CauserType)
          .HasColumnName("causer_type")
          .HasMaxLength(255);

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Description)
          .IsRequired()
          .HasColumnName("description");

        entity.Property(e => e.LogName)
          .HasColumnName("log_name")
          .HasMaxLength(255);

        entity.Property(e => e.Properties).HasColumnName("properties");

        entity.Property(e => e.SubjectId).HasColumnName("subject_id");

        entity.Property(e => e.SubjectType)
          .HasColumnName("subject_type")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<Addresses>(entity =>
      {
        entity.ToTable("addresses");

        entity.HasIndex(e => new { e.Line1, e.Line2, e.City, e.Zipcode, e.ClientId, e.Type })
          .HasName("ix_cyprus_address_client_id_type");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.City)
          .HasColumnName("city")
          .HasMaxLength(255);

        entity.Property(e => e.ClientId).HasColumnName("client_id");

        entity.Property(e => e.Country)
          .HasColumnName("country")
          .HasMaxLength(255);

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Line1)
          .HasColumnName("line_1")
          .HasMaxLength(255);

        entity.Property(e => e.Line2)
          .HasColumnName("line_2")
          .HasMaxLength(255);

        entity.Property(e => e.State)
          .HasColumnName("state")
          .HasMaxLength(255);

        entity.Property(e => e.Type).HasColumnName("type");

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Zip4)
          .HasColumnName("zip_4")
          .HasMaxLength(255);

        entity.Property(e => e.Zipcode)
          .HasColumnName("zipcode")
          .HasMaxLength(255);
      });

      modelBuilder.Entity<AffiliationPeriods>(entity =>
      {
        entity.ToTable("affiliation_periods");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.EndDate)
          .HasColumnName("end_date")
          .HasColumnType("date");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.StatDate)
          .HasColumnName("stat_date")
          .HasColumnType("date");

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<Agencies>(entity =>
      {
        entity.ToTable("agencies");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<Alianzas>(entity =>
      {
        entity.ToTable("alianzas");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.AffFlag)
          .HasColumnName("aff_flag")
          .HasMaxLength(255);

        entity.Property(e => e.AffStatus).HasColumnName("aff_status");

        entity.Property(e => e.AffType).HasColumnName("aff_type");

        entity.Property(e => e.ClientProductId).HasColumnName("client_product_id");

        entity.Property(e => e.Coordination).HasColumnName("coordination");

        entity.Property(e => e.CoverAmount).HasColumnName("cover_amount");

        entity.Property(e => e.CoverId).HasColumnName("cover_id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.ElegibleDate)
          .HasColumnName("elegible_date")
          .HasColumnType("date");

        entity.Property(e => e.EndDate)
          .HasColumnName("end_date")
          .HasColumnType("date");

        entity.Property(e => e.EndReason)
          .HasColumnName("end_reason")
          .HasMaxLength(255);

        entity.Property(e => e.Joint).HasColumnName("joint");

        entity.Property(e => e.LifeInsurance).HasColumnName("life_insurance");

        entity.Property(e => e.LifeInsuranceAmount).HasColumnName("life_insurance_amount");

        entity.Property(e => e.MajorMedical).HasColumnName("major_medical");

        entity.Property(e => e.MajorMedicalAmount).HasColumnName("major_medical_amount");

        entity.Property(e => e.Prima).HasColumnName("prima");

        entity.Property(e => e.QualifyingEventId).HasColumnName("qualifying_event_id");

        entity.Property(e => e.StartDate)
          .HasColumnName("start_date")
          .HasColumnType("date");

        entity.Property(e => e.SubTotal).HasColumnName("sub_total");

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.ClientProduct)
          .WithMany(p => p.Alianzas)
          .HasForeignKey(d => d.ClientProductId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("alianzas_client_product_id_foreign");

        entity.HasOne(d => d.Cover)
          .WithMany(p => p.Alianzas)
          .HasForeignKey(d => d.CoverId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("alianzas_cover_id_foreign");

        entity.HasOne(d => d.QualifyingEvent)
          .WithMany(p => p.Alianzas)
          .HasForeignKey(d => d.QualifyingEventId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("alianzas_qualifying_event_id_foreign");
      });

      modelBuilder.Entity<Beneficiaries>(entity =>
      {
        entity.ToTable("beneficiaries");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.AlianzaId).HasColumnName("alianza_id");

        entity.Property(e => e.BirthDate)
          .HasColumnName("birth_date")
          .HasColumnType("date");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Gender)
          .IsRequired()
          .HasColumnName("gender")
          .HasMaxLength(255);

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.Percent)
          .IsRequired()
          .HasColumnName("percent")
          .HasMaxLength(255);

        entity.Property(e => e.Relationship)
          .IsRequired()
          .HasColumnName("relationship")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Ssn)
          .HasColumnName("ssn").
          HasMaxLength(255);

        entity.HasOne(d => d.Alianza)
          .WithMany(p => p.Beneficiaries)
          .HasForeignKey(d => d.AlianzaId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("beneficiaries_alianza_id_foreign");

        entity.HasOne(d => d.MultiAssists)
          .WithMany(p => p.Beneficiaries)
          .HasForeignKey(d => d.MultiAssistId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("beneficiaries_multi_assists_id_foreign");
      });

      modelBuilder.Entity<BonaFides>(entity =>
      {
        entity.ToTable("bona_fides");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.Benefits)
          .HasColumnName("benefits")
          .HasMaxLength(255);

        entity.Property(e => e.Code)
          .HasColumnName("code")
          .HasMaxLength(255);

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Disclaimer)
          .HasColumnName("disclaimer")
          .HasMaxLength(255);

        entity.Property(e => e.Email)
          .HasColumnName("email")
          .HasMaxLength(255);

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.Phone)
          .HasColumnName("phone")
          .HasMaxLength(255);

        entity.Property(e => e.Siglas)
          .HasColumnName("siglas")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<CallReasons>(entity =>
      {
        entity.ToTable("call_reasons");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<Campaigns>(entity =>
      {
        entity.ToTable("campaigns");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.Origin).HasColumnName("origin");

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<CanceledCategories>(entity =>
      {
        entity.ToTable("canceled_categories");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CanceledReasonsId).HasColumnName("canceled_reasons_id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.CanceledReasons)
          .WithMany(p => p.CanceledCategories)
          .HasForeignKey(d => d.CanceledReasonsId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("canceled_categories_canceled_reasons_id_foreign");
      });

      modelBuilder.Entity<CanceledReasons>(entity =>
      {
        entity.ToTable("canceled_reasons");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<CanceledSubcategories>(entity =>
      {
        entity.ToTable("canceled_subcategories");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CanceledCategoriesId).HasColumnName("canceled_categories_id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.CanceledCategories)
          .WithMany(p => p.CanceledSubcategories)
          .HasForeignKey(d => d.CanceledCategoriesId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("canceled_subcategories_canceled_categories_id_foreign");
      });

      modelBuilder.Entity<ChapterClient>(entity =>
      {
        entity.ToTable("chapter_client");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.ChapterId).HasColumnName("chapter_id");

        entity.Property(e => e.ClientId).HasColumnName("client_id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.NewRegistration).HasColumnName("new_registration");

        entity.Property(e => e.Primary).HasColumnName("primary");

        entity.Property(e => e.RegistrationDate)
          .HasColumnName("registration_date")
          .HasColumnType("date");

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.Chapter)
          .WithMany(p => p.ChapterClient)
          .HasForeignKey(d => d.ChapterId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("chapter_client_chapter_id_foreign");

        entity.HasOne(d => d.Client)
          .WithMany(p => p.ChapterClient)
          .HasForeignKey(d => d.ClientId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("chapter_client_client_id_foreign");
      });

      modelBuilder.Entity<Chapters>(entity =>
      {
        entity.ToTable("chapters");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.BonaFideId).HasColumnName("bona_fide_id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.Quota).HasColumnName("quota");

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.BonaFide)
          .WithMany(p => p.Chapters)
          .HasForeignKey(d => d.BonaFideId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("chapters_bona_fide_id_foreign");
      });

      modelBuilder.Entity<Cities>(entity =>
      {
        entity.ToTable("cities");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CountryId).HasColumnName("country_id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.RegionId).HasColumnName("region_id");

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.Country)
          .WithMany(p => p.Cities)
          .HasForeignKey(d => d.CountryId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("cities_country_id_foreign");

        entity.HasOne(d => d.Region)
          .WithMany(p => p.Cities)
          .HasForeignKey(d => d.RegionId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("cities_region_id_foreign");
      });

      modelBuilder.Entity<ClientCommunicationMethod>(entity =>
      {
        entity.ToTable("client_communication_method");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.ClientId).HasColumnName("client_id");

        entity.Property(e => e.CommunicationMethodId).HasColumnName("communication_method_id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.Client)
          .WithMany(p => p.ClientCommunicationMethod)
          .HasForeignKey(d => d.ClientId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("client_communication_method_client_id_foreign");

        entity.HasOne(d => d.CommunicationMethod)
          .WithMany(p => p.ClientCommunicationMethod)
          .HasForeignKey(d => d.CommunicationMethodId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("client_communication_method_communication_method_id_foreign");
      });

      modelBuilder.Entity<ClientDocumentType>(entity =>
      {
        entity.ToTable("client_document_type");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.ClientId).HasColumnName("client_id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DocumentTypeId).HasColumnName("document_type_id");

        entity.Property(e => e.SobImgUrl)
          .HasColumnName("sob_img_url")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Url)
          .IsRequired()
          .HasColumnName("url")
          .HasMaxLength(255);

        entity.HasOne(d => d.Client)
          .WithMany(p => p.ClientDocumentType)
          .HasForeignKey(d => d.ClientId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("client_document_type_client_id_foreign");

        entity.HasOne(d => d.DocumentType)
          .WithMany(p => p.ClientDocumentType)
          .HasForeignKey(d => d.DocumentTypeId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("client_document_type_document_type_id_foreign");
      });

      modelBuilder.Entity<ClientProduct>(entity =>
      {
        entity.ToTable("client_product");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.ClientId).HasColumnName("client_id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.ProductId).HasColumnName("product_id");

        entity.Property(e => e.Status).HasColumnName("status");

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.Client)
          .WithMany(p => p.ClientProduct)
          .HasForeignKey(d => d.ClientId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("client_product_client_id_foreign");

        entity.HasOne(d => d.Product)
          .WithMany(p => p.ClientProduct)
          .HasForeignKey(d => d.ProductId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("client_product_product_id_foreign");
      });

      modelBuilder.Entity<ClientUser>(entity =>
      {
        entity.ToTable("client_user");

        entity.HasIndex(e => new { e.AlianzaId, e.UserId })
          .HasName("client_user_user_id");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.AlianzaId).HasColumnName("alianza_id");

        entity.Property(e => e.CallType).HasColumnName("call_type");

        entity.Property(e => e.ClientId).HasColumnName("client_id");

        entity.Property(e => e.Comments).HasColumnName("comments");

        entity.Property(e => e.ConfirmationNumber)
          .IsRequired()
          .HasColumnName("confirmation_number")
          .HasMaxLength(255);

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.HasOne(d => d.Alianza)
          .WithMany(p => p.ClientUser)
          .HasForeignKey(d => d.AlianzaId)
          .HasConstraintName("client_user_alianza_id_foreign");

        entity.HasOne(d => d.Client)
          .WithMany(p => p.ClientUser)
          .HasForeignKey(d => d.ClientId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("client_user_client_id_foreign");

        entity.HasOne(d => d.User)
          .WithMany(p => p.ClientUser)
          .HasForeignKey(d => d.UserId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("client_user_user_id_foreign");
      });

      modelBuilder.Entity<Clients>(entity =>
      {
        entity.ToTable("clients");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.AgencyId).HasColumnName("agency_id");

        entity.Property(e => e.BirthDate)
          .HasColumnName("birth_date")
          .HasColumnType("date");

        entity.Property(e => e.CampaignId).HasColumnName("campaign_id");

        entity.Property(e => e.ContractNumber)
          .HasColumnName("contract_number")
          .HasMaxLength(255);

        entity.Property(e => e.Contribution).HasColumnName("contribution");

        entity.Property(e => e.CoverId).HasColumnName("cover_id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.EffectiveDate)
          .HasColumnName("effective_date")
          .HasColumnType("date");

        entity.Property(e => e.Email)
          .HasColumnName("email")
          .HasMaxLength(255);

        entity.Property(e => e.Gender).HasColumnName("gender");

        entity.Property(e => e.Initial)
          .HasColumnName("initial")
          .HasMaxLength(255);

        entity.Property(e => e.LastName1)
          .IsRequired()
          .HasColumnName("last_name_1")
          .HasMaxLength(255);

        entity.Property(e => e.LastName2)
          .HasColumnName("last_name_2")
          .HasMaxLength(255);

        entity.Property(e => e.MaritalStatus).HasColumnName("marital_status");

        entity.Property(e => e.MedicareA).HasColumnName("medicare_a");

        entity.Property(e => e.MedicareB).HasColumnName("medicare_b");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.Phone1)
          .IsRequired()
          .HasColumnName("phone_1")
          .HasMaxLength(255);

        entity.Property(e => e.Phone2)
          .HasColumnName("phone_2")
          .HasMaxLength(255);

        entity.Property(e => e.PreRegister)
          .HasColumnName("pre_register")
          .HasDefaultValueSql("((0))");

        entity.Property(e => e.Principal).HasColumnName("principal");

        entity.Property(e => e.RetirementId).HasColumnName("retirement_id");

        entity.Property(e => e.Ssn)
          .HasColumnName("ssn")
          .HasMaxLength(255);

        entity.Property(e => e.Status).HasColumnName("status");

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.Agency)
          .WithMany(p => p.Clients)
          .HasForeignKey(d => d.AgencyId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("clients_agency_id_foreign");

        entity.HasOne(d => d.Campaign)
          .WithMany(p => p.Clients)
          .HasForeignKey(d => d.CampaignId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("clients_campaign_id_foreign");

        entity.HasOne(d => d.Cover)
          .WithMany(p => p.Clients)
          .HasForeignKey(d => d.CoverId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("clients_cover_id_foreign");

        entity.HasOne(d => d.Retirement)
          .WithMany(p => p.Clients)
          .HasForeignKey(d => d.RetirementId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("clients_retirement_id_foreign");
      });

      modelBuilder.Entity<CommunicationMethods>(entity =>
      {
        entity.ToTable("communication_methods");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<Countries>(entity =>
      {
        entity.ToTable("countries");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.Code)
          .IsRequired()
          .HasColumnName("code")
          .HasMaxLength(255);

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<Covers>(entity =>
      {
        entity.ToTable("covers");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.Alianza)
          .IsRequired()
          .HasColumnName("alianza")
          .HasDefaultValueSql("('0')");

        entity.Property(e => e.Code)
          .HasColumnName("code")
          .HasMaxLength(255);

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.HealthPlanId).HasColumnName("health_plan_id");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.Sob)
          .HasColumnName("sob")
          .HasMaxLength(255);

        entity.Property(e => e.SobImg)
          .HasColumnName("sob_img")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Type)
          .HasColumnName("type")
          .HasMaxLength(255);


        entity.Property(e => e.Beneficiary)
          .IsRequired()
          .HasColumnName("beneficiary")
          .HasDefaultValueSql("('0')");

        entity.HasOne(d => d.HealthPlan)
          .WithMany(p => p.Covers)
          .HasForeignKey(d => d.HealthPlanId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("covers_health_plan_id_foreign");


        entity.Ignore(t => t.AddOnsAlt);

      });

      modelBuilder.Entity<CsvDatas>(entity =>
      {
        entity.ToTable("csv_datas");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.CsvData)
          .IsRequired()
          .HasColumnName("csv_data");

        entity.Property(e => e.CsvFilename)
          .IsRequired()
          .HasColumnName("csv_filename")
          .HasMaxLength(255);

        entity.Property(e => e.CsvHeader)
          .IsRequired()
          .HasColumnName("csv_header")
          .HasDefaultValueSql("('0')");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<Dependents>(entity =>
      {
        entity.ToTable("dependents");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.AgencyId).HasColumnName("agency_id");

        entity.Property(e => e.BirthDate)
          .HasColumnName("birth_date")
          .HasColumnType("date");

        entity.Property(e => e.CityId).HasColumnName("city_id");

        entity.Property(e => e.ClientId).HasColumnName("client_id");

        entity.Property(e => e.ContractNumber)
          .HasColumnName("contract_number")
          .HasMaxLength(255);

        entity.Property(e => e.CoverId).HasColumnName("cover_id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.EffectiveDate)
          .HasColumnName("effective_date")
          .HasColumnType("date");

        entity.Property(e => e.Email)
          .HasColumnName("email")
          .HasMaxLength(255);

        entity.Property(e => e.Gender).HasColumnName("gender");

        entity.Property(e => e.Initial)
          .HasColumnName("initial")
          .HasMaxLength(255);

        entity.Property(e => e.LastName1)
          .HasColumnName("last_name_1")
          .HasMaxLength(255);

        entity.Property(e => e.LastName2)
          .HasColumnName("last_name_2")
          .HasMaxLength(255);

        entity.Property(e => e.Name)
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.Phone1)
          .HasColumnName("phone_1")
          .HasMaxLength(255);

        entity.Property(e => e.Phone2)
          .HasColumnName("phone_2")
          .HasMaxLength(255);

        entity.Property(e => e.Relationship).HasColumnName("relationship");

        entity.Property(e => e.Ssn)
          .HasColumnName("ssn")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.Agency)
          .WithMany(p => p.Dependents)
          .HasForeignKey(d => d.AgencyId)
          .HasConstraintName("dependents_agency_id_foreign");

        entity.HasOne(d => d.City)
          .WithMany(p => p.Dependents)
          .HasForeignKey(d => d.CityId)
          .HasConstraintName("dependents_city_id_foreign");

        entity.HasOne(d => d.Client)
          .WithMany(p => p.Dependents)
          .HasForeignKey(d => d.ClientId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("dependents_client_id_foreign");

        entity.HasOne(d => d.Cover)
          .WithMany(p => p.Dependents)
          .HasForeignKey(d => d.CoverId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("dependents_cover_id_foreign");

      });

      modelBuilder.Entity<DocumentCategories>(entity =>
      {
        entity.ToTable("document_categories");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<DocumentTypes>(entity =>
      {
        entity.ToTable("document_types");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DocumentCategoryId).HasColumnName("document_category_id");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.DocumentCategory)
          .WithMany(p => p.DocumentTypes)
          .HasForeignKey(d => d.DocumentCategoryId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("document_types_document_category_id_foreign");
      });

      modelBuilder.Entity<Files>(entity =>
      {
        entity.ToTable("files");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.Path)
          .IsRequired()
          .HasColumnName("path")
          .HasMaxLength(255);

        entity.Property(e => e.Size)
          .IsRequired()
          .HasColumnName("size")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<HealthPlans>(entity =>
      {
        entity.ToTable("health_plans");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Url)
          .IsRequired()
          .HasColumnName("url")
          .HasMaxLength(255);
      });

      //modelBuilder.Entity<Migrations>(entity =>
      //{
      //    entity.ToTable("migrations");

      //    entity.Property(e => e.Id).HasColumnName("id");

      //    entity.Property(e => e.Batch).HasColumnName("batch");

      //    entity.Property(e => e.Migration)
      //        .IsRequired()
      //        .HasColumnName("migration")
      //        .HasMaxLength(255);
      //});

      modelBuilder.Entity<PasswordResets>(entity =>
      {
        entity.HasNoKey();

        entity.ToTable("password_resets");

        entity.HasIndex(e => e.Email)
          .HasName("password_resets_email_index");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Email)
          .IsRequired()
          .HasColumnName("email")
          .HasMaxLength(255);

        entity.Property(e => e.Token)
          .IsRequired()
          .HasColumnName("token")
          .HasMaxLength(255);
      });

      modelBuilder.Entity<Products>(entity =>
      {
        entity.ToTable("products");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<Prospects>(entity =>
      {
        entity.ToTable("prospects");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.Address1)
          .IsRequired()
          .HasColumnName("address_1")
          .HasMaxLength(255);

        entity.Property(e => e.Address2)
          .IsRequired()
          .HasColumnName("address_2")
          .HasMaxLength(255);

        entity.Property(e => e.BirthDate)
          .HasColumnName("birth_date")
          .HasColumnType("date");

        entity.Property(e => e.CityId).HasColumnName("city_id");

        entity.Property(e => e.Country)
          .IsRequired()
          .HasColumnName("country")
          .HasMaxLength(255);

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.LastName1)
          .IsRequired()
          .HasColumnName("last_name1")
          .HasMaxLength(255);

        entity.Property(e => e.LastName2)
          .IsRequired()
          .HasColumnName("last_name2")
          .HasMaxLength(255);

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.Phone)
          .IsRequired()
          .HasColumnName("phone")
          .HasMaxLength(255);

        entity.Property(e => e.Ssn)
          .IsRequired()
          .HasColumnName("ssn")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Zip4)
          .IsRequired()
          .HasColumnName("zip+4")
          .HasMaxLength(255);

        entity.Property(e => e.ZipcodeId).HasColumnName("zipcode_id");

        entity.HasOne(d => d.City)
          .WithMany(p => p.Prospects)
          .HasForeignKey(d => d.CityId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("prospects_city_id_foreign");

        entity.HasOne(d => d.Zipcode)
          .WithMany(p => p.Prospects)
          .HasForeignKey(d => d.ZipcodeId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("prospects_zipcode_id_foreign");
      });

      modelBuilder.Entity<QualifyingEvents>(entity =>
      {
        entity.ToTable("qualifying_events");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.Requirements)
          .IsRequired()
          .HasColumnName("requirements")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<Regions>(entity =>
      {
        entity.ToTable("regions");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<Retirements>(entity =>
      {
        entity.ToTable("retirements");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.Code)
          .HasColumnName("code")
          .HasMaxLength(255);

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<Roles>(entity =>
      {
        entity.ToTable("roles");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");
      });

      modelBuilder.Entity<States>(entity =>
      {
        entity.ToTable("states");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.Code)
          .HasColumnName("code")
          .HasMaxLength(255);

        entity.Property(e => e.CountryId).HasColumnName("country_id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.Country)
          .WithMany(p => p.States)
          .HasForeignKey(d => d.CountryId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("states_country_id_foreign");
      });

      modelBuilder.Entity<Tutors>(entity =>
      {
        entity.ToTable("tutors");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.ClientId).HasColumnName("client_id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.LastName)
          .IsRequired()
          .HasColumnName("last_name")
          .HasMaxLength(255);

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.PhiFileUrl)
          .HasColumnName("phi_file_url")
          .HasMaxLength(255);

        entity.Property(e => e.Phone)
          .IsRequired()
          .HasColumnName("phone")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.Client)
          .WithMany(p => p.Tutors)
          .HasForeignKey(d => d.ClientId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("tutors_client_id_foreign");
      });

      modelBuilder.Entity<Users>(entity =>
      {
        entity.ToTable("users");

        entity.HasIndex(e => e.Email)
          .HasName("users_email_unique")
          .IsUnique();

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.Code)
          .IsRequired()
          .HasColumnName("code")
          .HasMaxLength(255);

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Email)
          .IsRequired()
          .HasColumnName("email")
          .HasMaxLength(255);

        entity.Property(e => e.LastName)
          .IsRequired()
          .HasColumnName("last_name")
          .HasMaxLength(255);

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.Password)
          .IsRequired()
          .HasColumnName("password")
          .HasMaxLength(255);

        entity.Property(e => e.Phone)
          .HasColumnName("phone")
          .HasMaxLength(255);

        entity.Property(e => e.RememberToken)
          .HasColumnName("remember_token")
          .HasMaxLength(100);

        entity.Property(e => e.RoleId).HasColumnName("role_id");

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.Role)
          .WithMany(p => p.Users)
          .HasForeignKey(d => d.RoleId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("users_role_id_foreign");
      });

      modelBuilder.Entity<Zipcodes>(entity =>
      {
        entity.ToTable("zipcodes");

        entity.Property(e => e.Id).HasColumnName("id");

        entity.Property(e => e.CityId).HasColumnName("city_id");

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.Property(e => e.Name)
          .IsRequired()
          .HasColumnName("name")
          .HasMaxLength(255);

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.City)
          .WithMany(p => p.Zipcodes)
          .HasForeignKey(d => d.CityId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("zipcodes_city_id_foreign");
      });

      modelBuilder.Entity<MultiAssists>(entity =>
      {
        entity.ToTable("multi_assists");

        entity.HasKey(t => t.Id);
        entity.Property(e => e.Id)
        .HasColumnName("id")
        .ValueGeneratedOnAdd();

        entity.Property(e => e.ClientProductId)
        .HasColumnName("client_product_id")
        .IsRequired();

        entity.Property(e => e.CoverId).HasColumnName("cover_id")
        .IsRequired();

        entity.Property(e => e.EffectiveDate)
          .HasColumnName("efective_date")
          .HasColumnType("datetime");

        entity.Property(e => e.EligibleWaitingPeriodDate)
          .HasColumnName("eligible_waiting_period_date")
          .HasColumnType("datetime");

        entity.Property(e => e.EndDate)
          .HasColumnName("end_date")
          .HasColumnType("datetime");

        entity.Property(e => e.SentDate)
          .HasColumnName("sent_date")
          .HasColumnType("datetime");

        entity.Property(e => e.Cost).HasColumnName("cost");

        entity.Property(e => e.StatusId)
        .HasColumnName("status_id")
        .HasColumnType("VARCHAR")
        .HasMaxLength(10);

        entity.Property(e => e.Ref1)
          .HasColumnName("ref1")
          .HasColumnType("VARCHAR")
          .HasMaxLength(30);

        entity.Property(e => e.Ref2)
          .HasColumnName("ref2")
          .HasColumnType("VARCHAR")
          .HasMaxLength(30);

        entity.Property(e => e.Ref3)
          .HasColumnName("ref3")
          .HasColumnType("VARCHAR")
          .HasMaxLength(30);

        entity.Property(e => e.AccountType)
        .HasColumnName("account_type")
        .HasColumnType("VARCHAR")
        .HasMaxLength(4);

        entity.Property(e => e.BankName)
        .HasColumnName("bank_name")
        .HasColumnType("VARCHAR")
        .HasMaxLength(60);

        entity.Property(e => e.AccountHolderName)
        .HasColumnName("account_holder_name")
        .HasColumnType("VARCHAR")
        .HasMaxLength(60);

        entity.Property(e => e.RoutingNum)
        .HasColumnName("routing_num")
        .HasColumnType("VARCHAR")
        .HasMaxLength(9);

        entity.Property(e => e.AccountNum)
        .HasColumnName("account_num")
        .HasColumnType("VARCHAR")
        .HasMaxLength(12);

        entity.Property(e => e.ExpDate)
        .HasColumnName("exp_date")
        .HasColumnType("VARCHAR")
        .HasMaxLength(4);

        entity.Property(e => e.DebDay)
        .HasColumnName("deb_day");

        entity.Property(e => e.DebRecurringType)
        .HasColumnName("deb_recurring_type")
        .HasColumnType("VARCHAR")
        .HasMaxLength(10);

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.ClientProduct)
          .WithMany(p => p.MultiAssists)
          .HasForeignKey(d => d.ClientProductId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("multi_assists_client_product_id_foreign");

        entity.HasOne(d => d.Cover)
          .WithMany(p => p.MultiAssists)
          .HasForeignKey(d => d.CoverId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("multi_assists_cover_id_foreign");
      });

      modelBuilder.Entity<MultiAssistsVehicle>(entity =>
      {
        entity.ToTable("multi_assists_vehicle");

        entity.Property(e => e.Id).HasColumnName("id")
        .ValueGeneratedOnAdd();
        entity.HasKey(t => t.Id);

        entity.Property(e => e.MultiAssistId).HasColumnName("multi_assist_id")
        .IsRequired();

        entity.Property(e => e.Make)
          .IsRequired()
          .HasColumnName("make")
          .HasMaxLength(30);

        entity.Property(e => e.Model)
          .IsRequired()
          .HasColumnName("model")
          .HasMaxLength(30);

        entity.Property(e => e.Year)
          .IsRequired()
          .HasColumnName("year");

        entity.Property(e => e.Vin)
          .IsRequired()
          .HasColumnName("vin")
          .HasMaxLength(17);

        entity.Property(e => e.CreatedAt)
          .HasColumnName("created_at")
          .HasColumnType("datetime");

        entity.Property(e => e.UpdatedAt)
          .HasColumnName("updated_at")
          .HasColumnType("datetime");

        entity.Property(e => e.DeletedAt)
          .HasColumnName("deleted_at")
          .HasColumnType("datetime");

        entity.HasOne(d => d.MultiAssists)
          .WithMany(p => p.MultiAssistsVehicle)
          .HasForeignKey(d => d.MultiAssistId)
          .OnDelete(DeleteBehavior.ClientSetNull)
          .HasConstraintName("multi_assists_vehicle_multi_assists_id_foreign");
      });

      modelBuilder.Entity<AlianzaAddOns>(entity =>
      {
        entity.ToTable("alianza_addons");

        entity.Property(e => e.AlianzaId).HasColumnName("alianza_id").IsRequired();
        entity.Property(e => e.InsuranceAddOnId).HasColumnName("insurance_addon_id").IsRequired();
        entity.HasKey(t => new { t.AlianzaId, t.InsuranceAddOnId });

        //FK
        entity.HasOne(d => d.Alianza)
    .WithMany(p => p.AlianzaAddOns)
    .HasForeignKey(d => d.AlianzaId)
    .OnDelete(DeleteBehavior.ClientSetNull)
    .HasConstraintName("alianza_addons_alianza_id_foreign");

        //FK
        entity.HasOne(d => d.InsuranceAddOn)
    .WithMany(p => p.AlianzaAddOns)
    .HasForeignKey(d => d.InsuranceAddOnId)
    .OnDelete(DeleteBehavior.ClientSetNull)
    .HasConstraintName("alianza_addons_insurance_addons_id_foreign");

      });

      #endregion

    }
  }
}