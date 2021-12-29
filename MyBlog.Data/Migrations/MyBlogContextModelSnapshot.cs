﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyBlog.Data.Concrete.EntityFramework.Contexts;

namespace MyBlog.Data.Migrations
{
    [DbContext(typeof(MyBlogContext))]
    partial class MyBlogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0-preview.7.20365.15");

            modelBuilder.Entity("MyBlog.Entities.Concrete.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("CommentCount")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("NVARCHAR(MAX)");

                    b.Property<string>("CreatedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("SeoAuthor")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SeoDescription")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("SeoTags")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("Thumbnail")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ViewCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Articles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryId = 1,
                            CommentCount = 1,
                            Content = "Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı 1500'lerden beri endüstri standardı sahte metinler olarak kullanılmıştır. Beşyüz yıl boyunca varlığını sürdürmekle kalmamış, aynı zamanda pek değişmeden elektronik dizgiye de sıçramıştır. 1960'larda Lorem Ipsum pasajları da içeren Letraset yapraklarının yayınlanması ile ve yakın zamanda Aldus PageMaker gibi Lorem Ipsum sürümleri içeren masaüstü yayıncılık yazılımları ile popüler olmuştur.",
                            CreatedByName = "Emre Tomruk",
                            CreatedDate = new DateTime(2021, 12, 28, 12, 46, 31, 457, DateTimeKind.Local).AddTicks(7944),
                            Date = new DateTime(2021, 12, 28, 12, 46, 31, 457, DateTimeKind.Local).AddTicks(6607),
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "InitialCreate",
                            ModifiedDate = new DateTime(2021, 12, 28, 12, 46, 31, 457, DateTimeKind.Local).AddTicks(9074),
                            Note = "C# 10.0 ve .NET 6 Yenilikleri",
                            SeoAuthor = "Emre Tomruk",
                            SeoDescription = "C# 10.0 ve .NET 6 Yenilikleri",
                            SeoTags = "C#, C# 10.0, .NET 6,  .NET Framework",
                            Thumbnail = "Default.jpg",
                            Title = "C# 10.0 ve .NET 6 Yenilikleri",
                            UserId = 1,
                            ViewCount = 100
                        },
                        new
                        {
                            Id = 2,
                            CategoryId = 2,
                            CommentCount = 2,
                            Content = "Yinelenen bir sayfa içeriğinin okuyucunun dikkatini dağıttığı bilinen bir gerçektir. Lorem Ipsum kullanmanın amacı, sürekli 'buraya metin gelecek, buraya metin gelecek' yazmaya kıyasla daha dengeli bir harf dağılımı sağlayarak okunurluğu artırmasıdır. Şu anda birçok masaüstü yayıncılık paketi ve web sayfa düzenleyicisi, varsayılan mıgır metinler olarak Lorem Ipsum kullanmaktadır. Ayrıca arama motorlarında anahtar sözcükleri ile arama yapıldığında henüz tasarım aşamasında olan çok sayıda site listelenir. Yıllar içinde, bazen kazara, bazen bilinçli olarak (örneğin mizah katılarak), çeşitli sürümleri geliştirilmiştir.",
                            CreatedByName = "Emre Tomruk",
                            CreatedDate = new DateTime(2021, 12, 28, 12, 46, 31, 458, DateTimeKind.Local).AddTicks(2634),
                            Date = new DateTime(2021, 12, 28, 12, 46, 31, 458, DateTimeKind.Local).AddTicks(2631),
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "InitialCreate",
                            ModifiedDate = new DateTime(2021, 12, 28, 12, 46, 31, 458, DateTimeKind.Local).AddTicks(2637),
                            Note = "JavaScript ve Yenilikleri",
                            SeoAuthor = "Emre Tomruk",
                            SeoDescription = "JavaScript ES2020 ve ES2020 Yenilikleri",
                            SeoTags = "JavaScript, JS, ES2020",
                            Thumbnail = "Default.jpg",
                            Title = "JavaScript ES2020 ve ES2020 Yenilikleri",
                            UserId = 1,
                            ViewCount = 150
                        },
                        new
                        {
                            Id = 3,
                            CategoryId = 3,
                            CommentCount = 3,
                            Content = "Yaygın inancın tersine, Lorem Ipsum rastgele sözcüklerden oluşmaz. Kökleri M.Ö. 45 tarihinden bu yana klasik Latin edebiyatına kadar uzanan 2000 yıllık bir geçmişi vardır. Virginia'daki Hampden-Sydney College'dan Latince profesörü Richard McClintock, bir Lorem Ipsum pasajında geçen ve anlaşılması en güç sözcüklerden biri olan 'consectetur' sözcüğünün klasik edebiyattaki örneklerini incelediğinde kesin bir kaynağa ulaşmıştır. Lorm Ipsum, Çiçero tarafından M.Ö. 45 tarihinde kaleme alınan \"de Finibus Bonorum et Malorum\" (İyi ve Kötünün Uç Sınırları) eserinin 1.10.32 ve 1.10.33 sayılı bölümlerinden gelmektedir. Bu kitap, ahlak kuramı üzerine bir tezdir ve Rönesans döneminde çok popüler olmuştur. Lorem Ipsum pasajının ilk satırı olan \"Lorem ipsum dolor sit amet\" 1.10.32 sayılı bölümdeki bir satırdan gelmektedir. 1500'lerden beri kullanılmakta olan standard Lorem Ipsum metinleri ilgilenenler için yeniden üretilmiştir. Çiçero tarafından yazılan 1.10.32 ve 1.10.33 bölümleri de 1914 H. Rackham çevirisinden alınan İngilizce sürümleri eşliğinde özgün biçiminden yeniden üretilmiştir.",
                            CreatedByName = "Emre Tomruk",
                            CreatedDate = new DateTime(2021, 12, 28, 12, 46, 31, 458, DateTimeKind.Local).AddTicks(2649),
                            Date = new DateTime(2021, 12, 28, 12, 46, 31, 458, DateTimeKind.Local).AddTicks(2647),
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "InitialCreate",
                            ModifiedDate = new DateTime(2021, 12, 28, 12, 46, 31, 458, DateTimeKind.Local).AddTicks(2652),
                            Note = "MS SQL Server 2019 ve Yenilikler",
                            SeoAuthor = "Emre Tomruk",
                            SeoDescription = "MS SQL Server 2019 ve Yenilikler",
                            SeoTags = "T-Sql, MS Sql, MS SQL Server 2019",
                            Thumbnail = "Default.jpg",
                            Title = "MS SQL Server 2019 ve Yenilikler",
                            UserId = 1,
                            ViewCount = 90
                        });
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("Note")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedByName = "InitialCreate",
                            CreatedDate = new DateTime(2021, 12, 28, 12, 46, 31, 464, DateTimeKind.Local).AddTicks(6492),
                            Description = "C# Programlama Dili ile ilgili güncellemeler",
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "InitialCreate",
                            ModifiedDate = new DateTime(2021, 12, 28, 12, 46, 31, 464, DateTimeKind.Local).AddTicks(6527),
                            Name = "C#",
                            Note = "C# Blog Kategorisi"
                        },
                        new
                        {
                            Id = 2,
                            CreatedByName = "InitialCreate",
                            CreatedDate = new DateTime(2021, 12, 28, 12, 46, 31, 464, DateTimeKind.Local).AddTicks(6555),
                            Description = "JavaScript Programlama Dili ile ilgili güncellemeler",
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "InitialCreate",
                            ModifiedDate = new DateTime(2021, 12, 28, 12, 46, 31, 464, DateTimeKind.Local).AddTicks(6558),
                            Name = "JavaScript",
                            Note = "JavaScript Blog Kategorisi"
                        },
                        new
                        {
                            Id = 3,
                            CreatedByName = "InitialCreate",
                            CreatedDate = new DateTime(2021, 12, 28, 12, 46, 31, 464, DateTimeKind.Local).AddTicks(6568),
                            Description = "MS SQL Server 2019 ile ilgili güncellemeler",
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "InitialCreate",
                            ModifiedDate = new DateTime(2021, 12, 28, 12, 46, 31, 464, DateTimeKind.Local).AddTicks(6570),
                            Name = "MS SQL Server",
                            Note = "MS SQL Server 2019 Blog Kategorisi"
                        });
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("Comments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ArticleId = 1,
                            CreatedByName = "Emre Tomruk",
                            CreatedDate = new DateTime(2021, 12, 28, 12, 46, 31, 468, DateTimeKind.Local).AddTicks(9629),
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "InitialCreate",
                            ModifiedDate = new DateTime(2021, 12, 28, 12, 46, 31, 468, DateTimeKind.Local).AddTicks(9649),
                            Note = "C# Makale Yorumu",
                            Text = "But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?"
                        },
                        new
                        {
                            Id = 2,
                            ArticleId = 2,
                            CreatedByName = "Emre Tomruk",
                            CreatedDate = new DateTime(2021, 12, 28, 12, 46, 31, 468, DateTimeKind.Local).AddTicks(9682),
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "InitialCreate",
                            ModifiedDate = new DateTime(2021, 12, 28, 12, 46, 31, 468, DateTimeKind.Local).AddTicks(9685),
                            Note = "JavaScript Makale Yorumu",
                            Text = "But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?"
                        },
                        new
                        {
                            Id = 3,
                            ArticleId = 3,
                            CreatedByName = "Emre Tomruk",
                            CreatedDate = new DateTime(2021, 12, 28, 12, 46, 31, 468, DateTimeKind.Local).AddTicks(9690),
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "InitialCreate",
                            ModifiedDate = new DateTime(2021, 12, 28, 12, 46, 31, 468, DateTimeKind.Local).AddTicks(9692),
                            Note = "MS SQL Server 2019 Makale Yorumu",
                            Text = "But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?"
                        });
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Note")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedByName = "InitialCreate",
                            CreatedDate = new DateTime(2021, 12, 28, 12, 46, 31, 471, DateTimeKind.Local).AddTicks(3673),
                            Description = "Admin rolü tüm haklara sahiptir.",
                            IsActive = true,
                            IsDeleted = false,
                            ModifiedByName = "InitialCreate",
                            ModifiedDate = new DateTime(2021, 12, 28, 12, 46, 31, 471, DateTimeKind.Local).AddTicks(3698),
                            Name = "Admin",
                            Note = "Admin Rolü"
                        });
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CreatedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("ModifiedByName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("VARBINARY(500)");

                    b.Property<string>("Picture")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedByName = "InitialCreate",
                            CreatedDate = new DateTime(2021, 12, 28, 12, 46, 31, 480, DateTimeKind.Local).AddTicks(3655),
                            Description = "İlk Admin Kullanıcı",
                            Email = "emretmrk39@gmail.com",
                            FirstName = "Emre",
                            IsActive = true,
                            IsDeleted = false,
                            LastName = "Tomruk",
                            ModifiedByName = "InitialCreate",
                            ModifiedDate = new DateTime(2021, 12, 28, 12, 46, 31, 480, DateTimeKind.Local).AddTicks(3678),
                            Note = "kullanıcı",
                            PasswordHash = new byte[] { 99, 101, 57, 51, 98, 49, 49, 56, 53, 100, 50, 102, 48, 52, 51, 52, 98, 54, 97, 51, 52, 97, 48, 102, 102, 52, 100, 50, 102, 52, 57, 99 },
                            Picture = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcSX4wVGjMQ37PaO4PdUVEAliSLi8-c2gJ1zvQ&usqp=CAU",
                            RoleId = 1,
                            Username = "W@nted"
                        });
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.Article", b =>
                {
                    b.HasOne("MyBlog.Entities.Concrete.Category", "Category")
                        .WithMany("Articles")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBlog.Entities.Concrete.User", "User")
                        .WithMany("Articles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.Comment", b =>
                {
                    b.HasOne("MyBlog.Entities.Concrete.Article", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyBlog.Entities.Concrete.User", b =>
                {
                    b.HasOne("MyBlog.Entities.Concrete.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
