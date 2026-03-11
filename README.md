#  Profesyonel Kütüphane Yönetim Sistemi (API)

Bu proje, modern yazılım mimarileri ve kurumsal standartlar kullanılarak geliştirilmiş kapsamlı bir **Kütüphane Yönetim Otomasyonu**'dur. Proje, üniversite staj programı kapsamında bir web uygulamasının backend altyapısı olarak tasarlanmıştır.

##  Öne Çıkan Teknik Özellikler

* **Katmanlı Mimari (N-Tier Architecture):** Proje; Domain, Infrastructure ve API olmak üzere 3 ana katmana bölünerek sürdürülebilirlik sağlanmıştır.
* **Global Soft Delete Mekanizması:** Veriler veritabanından fiziksel olarak silinmez; `BaseEntity` üzerinden yönetilen `IsDeleted` bayrağı ile mantıksal olarak korunur.
* **Merkezi Loglama Middleware:** Gelen her HTTP isteği ve dönen cevaplar, özel bir Middleware katmanı  konsolda anlık olarak izlenir.
* **EF Core Global Query Filters:** Silinmiş verilerin listeleme sorgularına dahil edilmemesi için veritabanı seviyesinde otomatik filtreleme uygulanmıştır.
* **Code-First Migration:** Veritabanı şeması tamamen C# sınıfları üzerinden yönetilmektedir.

## Kullanılan Teknolojiler

* **Platform:** .NET 8 / ASP.NET Core Web API
* **Veritabanı:** Microsoft SQL Server
* **ORM:** Entity Framework Core (EF Core)
* **Dokümantasyon:** Swagger / OpenAPI
* **Loglama:** ILogger & Custom Middleware

##  Proje Dosya Yapısı

```text
LibraryManagement/
├── LibraryManagement.Domain/ (Öz Katman)
│   ├── Entities/
│   │   ├── BaseEntity.cs (Ortak özellikler: Id, IsDeleted)
│   │   ├── Book.cs
│   │   ├── User.cs
│   │   └── Rental.cs
├── LbraryMangement.Infrastructure/ (Altyapı Katmanı)
│   ├── Context/
│   │   └── AppDbContext.cs (EF Core Bağlantısı & Global Filtreler)
│   ├── Migrations/ (Veritabanı Geçmişi)
│   └── Services/
│       └── RentalService.cs (Kiralama İş Mantığı)
├── LibraryManagement.Api/ (Sunum Katmanı)
│   ├── Controllers/
│   │   ├── BooksController.cs
│   │   ├── UsersController.cs
│   │   └── RentalsController.cs
│   ├── Middlewares/
│   │   └── RequestResponseLoggingMiddleware.cs (Anlık Trafik Loglama)
│   └── Program.cs (Uygulama Konfigürasyonu)


##  Mimari Yapı (Architectural Layers)

Proje, sorumlulukların ayrılması (Separation of Concerns) prensibine uygun olarak üç ana katmandan oluşmaktadır:

1.  **Domain Layer:** Projenin iş modellerini (`Entities`) barındırır. Tüm varlıklar `BaseEntity` sınıfından türetilerek standart bir yapıya oturtulmuştur.
2.  **Infrastructure Layer:** Veritabanı erişimi (`DbContext`), veri modellerinin yapılandırılması (Fluent API) ve iş mantığı servislerini (`RentalService`) içerir.
3.  **API Layer:** Dış dünyaya açılan uç noktaları (Controllers), hata yönetimi ve merkezi loglama (Middleware) katmanlarını yönetir.


## İlişkisel Veritabanı Tasarımı
Entity Framework Core kullanılarak kurulan veritabanı şemasında aşağıdaki ilişkiler yönetilmektedir:
* **User - Rental:** 1-N (Bir kullanıcı birden fazla kitap kiralayabilir).
* **Book - Rental:** 1-N (Bir kitap farklı zamanlarda birçok kez kiralanabilir).
* **Data Integrity:** Foreign Key kısıtlamaları ve Fluent API konfigürasyonları ile veri bütünlüğü korunmaktadır.


API Uç Noktaları (Endpoints)

* **Books:** Listeleme, Ekleme, Güncelleme, Soft-Delete.
* **Users:** Üye yönetimi ve kayıt işlemleri.
* **Rentals:** Kitap kiralama süreci ve iade işlemleri.