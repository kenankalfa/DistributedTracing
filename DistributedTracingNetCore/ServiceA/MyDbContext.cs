using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ServiceA
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {

        }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        public DbSet<tb_Person> tb_Person { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<tb_Person>(builder =>
            //{
            //    builder.HasKey(q => q.PersonRef);
            //    builder.ToTable("tb_Person", "Organization");
            //});

            modelBuilder.ApplyConfiguration(new tb_Person_Configuration());

            base.OnModelCreating(modelBuilder);
        }
    }

    public class tb_Person
    {
        public tb_Person()
        {
            //tb_PersonEmploymentStatus = new HashSet<tb_PersonEmploymentStatus>();
            //tb_StaffAppointment = new HashSet<tb_StaffAppointment>();
        }

        public int PersonRef { get; set; }
        public string FirstName { get; set; }
        //public int RegisterNumber { get; set; }
        //public int WorkSpaceRef { get; set; }
        //public string LastName { get; set; }
        //public string NickName { get; set; }
        //public string MaidenName { get; set; }
        //public string Email { get; set; }
        //public bool? IsEmptyEmail { get; set; }
        //public string BirthPlace { get; set; }
        //public DateTime BirthDate { get; set; }
        //public int? GenderRef { get; set; }
        //public int MaritalStatusRef { get; set; }
        //public int? BloodTypeRef { get; set; }
        //public int? MilitaryServiceStatusTypeRef { get; set; }
        //public DateTime? MilitaryServiceProbationDateEnd { get; set; }
        //public string Mobile { get; set; }
        //public string IdentificationNumber { get; set; }
        //public int FirmRef { get; set; }
        //public int LocationRef { get; set; }
        //public string MotherName { get; set; }
        //public string FatherName { get; set; }
        //public bool? HasDrivingLicence { get; set; }
        //public string DrivingLicenceType { get; set; }
        //public bool? HasMedicalProblem { get; set; }
        //public string MedicalProblemExplanation { get; set; }
        //public bool? HasTravelRestriction { get; set; }
        //public int? TravelRestrictionPercentage { get; set; }
        //public bool? HasCriminalRecord { get; set; }
        //public string CriminalRecordExplanation { get; set; }
        //public int? IntegrationPersonRef { get; set; }
        //public int? ImportedPersonRef { get; set; }
        //public int? PositionRef { get; set; }
        //public int? OrganizationUnitRef { get; set; }
        //public int? ManagerRef { get; set; }
        //public int StatusRef { get; set; }
        //public bool? IsFieldPersonnel { get; set; }
        //public string CompanyMobilePhoneNumber { get; set; }
        //public string CompanyFctcode { get; set; }
        //public int? PreferedLanguageRef { get; set; }
        //public string SignatureField { get; set; }
        //public byte[] RowVersion { get; set; }
        //public int CreatedUserRef { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public int ModifiedUserRef { get; set; }
        //public DateTime ModifiedDate { get; set; }
        //public Guid? SessionRef { get; set; }
        //public bool? IsDeleted { get; set; }
        //public string UserDomainName { get; set; }
        //public tb_Location LocationRefNavigation { get; set; }
        //public ICollection<tb_PersonEmploymentStatus> tb_PersonEmploymentStatus { get; set; }
        //public ICollection<tb_StaffAppointment> tb_StaffAppointment { get; set; }
    }

    public class tb_Person_Configuration : IEntityTypeConfiguration<tb_Person>
    {
        public void Configure(EntityTypeBuilder<tb_Person> entity)
        {
            entity.HasKey(e => e.PersonRef);

            entity.ToTable("tb_Person", "Organization");

            //entity.HasIndex(e => e.FirstName)
            //    .HasName("ix_Person_FirsName");

            //entity.HasIndex(e => e.LastName)
            //    .HasName("ix_Person_LastName");

            //entity.HasIndex(e => new { e.RegisterNumber, e.FirstName, e.LastName, e.IdentificationNumber, e.IsDeleted, e.PersonRef })
            //    .HasName("ix_Person_FistNameLastNameByisdeletedpersonref");

            //entity.Property(e => e.BirthDate).HasColumnType("datetime");

            //entity.Property(e => e.BirthPlace)
            //    .IsRequired()
            //    .HasMaxLength(150);

            //entity.Property(e => e.CompanyFctcode)
            //    .HasColumnName("CompanyFCTCode")
            //    .HasMaxLength(10);

            //entity.Property(e => e.CompanyMobilePhoneNumber).HasMaxLength(50);

            //entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            //entity.Property(e => e.CriminalRecordExplanation).HasMaxLength(250);

            //entity.Property(e => e.DrivingLicenceType).HasMaxLength(50);

            //entity.Property(e => e.Email).HasMaxLength(150);

            //entity.Property(e => e.FatherName).HasMaxLength(150);

            //entity.Property(e => e.FirstName)
            //    .IsRequired()
            //    .HasMaxLength(150);

            //entity.Property(e => e.GenderRef).HasDefaultValueSql("((0))");

            //entity.Property(e => e.HasCriminalRecord).HasDefaultValueSql("((0))");

            //entity.Property(e => e.HasDrivingLicence).HasDefaultValueSql("((0))");

            //entity.Property(e => e.HasMedicalProblem).HasDefaultValueSql("((0))");

            //entity.Property(e => e.HasTravelRestriction).HasDefaultValueSql("((0))");

            //entity.Property(e => e.IdentificationNumber).HasMaxLength(20);

            //entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

            //entity.Property(e => e.IsEmptyEmail).HasDefaultValueSql("((0))");

            //entity.Property(e => e.IsFieldPersonnel).HasDefaultValueSql("((0))");

            //entity.Property(e => e.LastName)
            //    .IsRequired()
            //    .HasMaxLength(150);

            //entity.Property(e => e.MaidenName).HasMaxLength(150);

            //entity.Property(e => e.MedicalProblemExplanation).HasMaxLength(250);

            //entity.Property(e => e.MilitaryServiceProbationDateEnd).HasColumnType("datetime");

            //entity.Property(e => e.Mobile).HasMaxLength(20);

            //entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

            //entity.Property(e => e.MotherName).HasMaxLength(150);

            //entity.Property(e => e.NickName).HasMaxLength(150);

            //entity.Property(e => e.PreferedLanguageRef).HasDefaultValueSql("((1))");

            //entity.Property(e => e.RowVersion).IsRowVersion();

            //entity.Property(e => e.SignatureField).HasMaxLength(200);

            //entity.Property(e => e.UserDomainName).HasMaxLength(250);

            //entity.HasOne(d => d.LocationRefNavigation)
            //    .WithMany(p => p.tb_Person)
            //    .HasForeignKey(d => d.LocationRef)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_tb_Person_tb_Location");
        }
    }
}
